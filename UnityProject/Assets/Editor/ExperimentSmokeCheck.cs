using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Meowra.Data;
using Meowra.Experiment;
using Meowra.UI;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Object = UnityEngine.Object;

// Synthetic fixtures exist in memory only. Never writes question content to assets.
public static class ExperimentSmokeCheck
{
    public static IEnumerator Run()
    {
        var manager = Object.FindAnyObjectByType<ExperimentManager>();
        Require(manager != null && manager.Stage == ExperimentStage.Setup, "Startup must show researcher setup.");
        var pages = GameObject.Find("Canvas/Pages").transform;
        var navigation = Object.FindAnyObjectByType<PageManager>();
        for (int i = 0; i < pages.childCount; i++)
        {
            navigation.ShowPage(i);
            AssertOnePage(pages);
        }
        navigation.ShowPage(0);
        navigation.PreviousPage();
        Require(navigation.CurrentPageIndex == 0, "PageManager must not wrap backwards.");
        navigation.ShowPage(pages.childCount - 1);
        navigation.NextPage();
        Require(navigation.CurrentPageIndex == pages.childCount - 1, "PageManager must not wrap forwards.");
        navigation.ShowPage(0);

        var original = Field<StudyDefinition>(manager, "study");
        string originalJson = JsonUtility.ToJson(original);
        var study = ScriptableObject.CreateInstance<StudyDefinition>();
        var texture = new Texture2D(16, 4);
        var sprites = new List<Sprite>();
        try
        {
            for (int i = 0; i < 6; i++)
            {
                var scenario = ScriptableObject.CreateInstance<ScenarioData>();
                scenario.scenarioId = "test-" + i;
                study.scenarios[i] = scenario;
            }
            SetStudy(manager, study);
            manager.StartSession();
            Require(manager.Stage == ExperimentStage.Setup, "Incomplete assets must block scored sessions.");
            Require(!GameObject.Find("Canvas/Pages/ResearcherSetupPage/StartSessionButton").GetComponent<Button>().interactable,
                "The start button must explain invalid content through disabled state.");
            foreach (var item in WalkSession(manager, true, 5, 2)) yield return item;
            Require(manager.Session.ScoredCount == 0 && manager.Session.CorrectCount == 0, "Blank preview must be unscored.");

            for (int i = 0; i < 6; i++)
            {
                var scenario = study.scenarios[i];
                var sprite = Sprite.Create(texture, new Rect(i * 2, 0, 2, 4), new Vector2(.5f, .5f));
                sprites.Add(sprite);
                scenario.codeImage = sprite;
                scenario.question = "test prompt " + i;
                scenario.answerA = "test A"; scenario.answerB = "test B";
                scenario.answerC = "test C"; scenario.answerD = "test D";
                scenario.rawFeedback = "raw fixture " + i;
                scenario.neutralFeedback = "neutral fixture " + i;
                scenario.meowraFeedback = "meowra fixture " + i;
                Require(scenario.GetValidationError() != null, "An explicit answer key is mandatory.");
                var missingKey = new TrialResponse(scenario, FeedbackCondition.Raw, AnswerChoice.A, false);
                Require(!missingKey.Scored && !missingKey.Correct, "Missing keys must never score as A.");
                scenario.correctAnswer = (AnswerChoice)(i % 4);
            }
            study.meowraPortrait = sprites[0];
            study.meowraIntroduction = "test introduction";
            Require(study.GetValidationError() == null, "Complete synthetic authoring data must validate.");
            string savedId = study.scenarios[1].scenarioId;
            study.scenarios[1].scenarioId = study.scenarios[0].scenarioId;
            Require(study.GetValidationError() != null, "Duplicate scenario IDs must be rejected.");
            study.scenarios[1].scenarioId = savedId;
            CheckOrders(study);

            // Each saved menu option drives a complete run using the actual UI events.
            for (int order = 1; order <= 6; order++)
                for (int set = 1; set <= 3; set++)
                    foreach (var item in WalkSession(manager, false, order, set)) yield return item;
            Require(JsonUtility.ToJson(original) == originalJson, "Tests must not mutate authoring assets.");
        }
        finally
        {
            SetStudy(manager, original);
            foreach (var scenario in study.scenarios) if (scenario != null) Object.Destroy(scenario);
            foreach (var sprite in sprites) Object.Destroy(sprite);
            Object.Destroy(texture);
            Object.Destroy(study);
        }
    }

    private static IEnumerable WalkSession(ExperimentManager manager, bool preview, int order, int set)
    {
        var setup = GameObject.Find("Canvas/Pages/ResearcherSetupPage").transform;
        setup.Find("OrderDropdown").GetComponent<Dropdown>().value = order - 1;
        setup.Find("StimulusSetDropdown").GetComponent<Dropdown>().value = set - 1;
        // Changing the test study happens only in memory; refresh validation first.
        Object.FindAnyObjectByType<StudyShellView>().ShowSetup(Field<StudyDefinition>(manager, "study"));
        Click(setup.Find(preview ? "PreviewLayoutButton" : "StartSessionButton").gameObject);
        yield return null;
        Require(manager.Stage == ExperimentStage.Welcome, "Start must reach Welcome.");
        Click(GameObject.Find("Canvas/Pages/WelcomePage/ContinueButton"));
        Click(GameObject.Find("Canvas/Pages/InstructionsPage/ContinueButton"));
        yield return null;
        int introductions = 0;
        int evaluations = 0;
        int submitted = 0;
        int steps = 0;
        var pages = GameObject.Find("Canvas/Pages").transform;
        while (manager.Stage != ExperimentStage.Complete)
        {
            Require(++steps <= 12, "Session did not finish in the expected number of stages.");
            AssertOnePage(pages);
            if (manager.Stage == ExperimentStage.Introduction)
            {
                introductions++;
                Require(manager.CurrentTrial.Condition == FeedbackCondition.Meowra && submitted % 2 == 0,
                    "Introduction must occur immediately before the Meowra block.");
                Click(GameObject.Find("Canvas/Pages/MeowraIntroductionPage/ContinueButton"));
            }
            else if (manager.Stage == ExperimentStage.Evaluation)
            {
                evaluations++;
                Require(submitted == evaluations * 2, "Each evaluation follows exactly two trials.");
                Click(GameObject.Find("Canvas/Pages/EvaluationPlaceholderPage/ContinueButton"));
            }
            else
            {
                Require(manager.Stage == ExperimentStage.Trial, "Unexpected session stage.");
                var assignment = manager.CurrentTrial;
                var view = Object.FindAnyObjectByType<TrialView>();
                var toggles = view.GetComponentsInChildren<Toggle>();
                var submit = Field<Button>(view, "submitButton");
                Require(toggles.Length == 4 && toggles.All(t => !t.isOn), "No answer may be preselected, including after a frame.");
                Require(!submit.interactable, "Submit must wait for an explicit selection.");
                Require(Field<Image>(view, "codeImage").sprite == assignment.Scenario.codeImage, "Incorrect code image binding.");
                Require(Field<Text>(view, "explanation").text == (preview ? "[Explanation]" : assignment.Scenario.GetFeedback(assignment.Condition)),
                    "Feedback must match both scenario and assigned condition.");
                var portrait = Field<Image>(view, "portrait");
                Require(portrait.gameObject.activeSelf == (!preview && assignment.Condition == FeedbackCondition.Meowra), "Persona image leaked into another condition.");
                Click(submit.gameObject);
                Object.FindAnyObjectByType<TrialManager>().Submit();
                Require(manager.Session.Responses.Count == submitted, "An unanswered trial was recorded.");
                toggles[0].isOn = true;
                toggles[1].isOn = true;
                Require(toggles.Count(t => t.isOn) == 1, "Answers must be mutually exclusive.");
                toggles[1].isOn = false;
                Require(!submit.interactable, "Clearing a choice must disable Submit.");
                int answer = preview ? submitted % 4 : (int)assignment.Scenario.correctAnswer;
                if (!preview && submitted % 2 == 1) answer = (answer + 1) % 4;
                toggles[answer].isOn = true;
                Require(submit.interactable, "A selected answer must enable Submit.");
                Click(submit.gameObject);
                Object.FindAnyObjectByType<TrialManager>().Submit();
                submitted++;
                Require(manager.Session.Responses.Count == submitted, "A repeated submit duplicated or skipped a response.");
                var response = manager.Session.Responses[submitted - 1];
                Require(response.ScenarioId == assignment.Scenario.scenarioId && response.Condition == assignment.Condition,
                    "Response assignment was not retained.");
                Require(response.SelectedAnswer == (AnswerChoice)answer, "Wrong selected answer recorded.");
                Require(response.Scored == !preview && response.Correct == (!preview && (submitted - 1) % 2 == 0), "Incorrect answer-key scoring.");
                Require(!manager.Session.Record(response), "Session accepted a replayed response.");
            }
            yield return null;
        }
        Require(submitted == 6 && introductions == 1 && evaluations == 3, "Incorrect block structure.");
        Require(manager.Session.Completed && manager.Session.OrderNumber == order && manager.Session.StimulusSet == set,
            "Session must retain its selected assignment.");
        Require(manager.Session.CorrectCount == (preview ? 0 : 3) && manager.Session.ScoredCount == (preview ? 0 : 6), "Incorrect total score.");
        var completed = manager.Session;
        Click(GameObject.Find("Canvas/Pages/CompletionPage/SetupButton"));
        Require(manager.Session == completed && manager.Stage == ExperimentStage.Setup, "Retain results on return to setup.");
        yield return null;
    }

    private static void CheckOrders(StudyDefinition study)
    {
        var expected = new[] { "Raw,Neutral,Meowra", "Raw,Meowra,Neutral", "Neutral,Raw,Meowra",
            "Neutral,Meowra,Raw", "Meowra,Raw,Neutral", "Meowra,Neutral,Raw" };
        for (int order = 1; order <= 6; order++)
        {
            Require(string.Join(",", Counterbalancing.GetOrder(order)) == expected[order-1], "Incorrect numbered condition order.");
            var coverage = new Dictionary<string, HashSet<FeedbackCondition>>();
            for (int set = 1; set <= 3; set++)
            {
                var schedule = Counterbalancing.BuildSchedule(study, order, set);
                Require(schedule.Count == 6 && schedule.Select(t => t.Scenario.scenarioId).Distinct().Count() == 6, "A schedule must use each scenario exactly once.");
                foreach (FeedbackCondition condition in Enum.GetValues(typeof(FeedbackCondition)))
                    Require(schedule.Count(t => t.Condition == condition) == 2, "Each condition needs two scenarios.");
                foreach (var trial in schedule)
                {
                    if (!coverage.ContainsKey(trial.Scenario.scenarioId)) coverage[trial.Scenario.scenarioId] = new HashSet<FeedbackCondition>();
                    coverage[trial.Scenario.scenarioId].Add(trial.Condition);
                }
            }
            Require(coverage.Values.All(c => c.Count == 3), "Each scenario must rotate across all three conditions.");
        }
    }

    private static T Field<T>(Object target, string name) where T : Object
    {
        return (T)new SerializedObject(target).FindProperty(name).objectReferenceValue;
    }
    private static void SetStudy(ExperimentManager manager, StudyDefinition study)
    {
        var serialized = new SerializedObject(manager);
        serialized.FindProperty("study").objectReferenceValue = study;
        serialized.ApplyModifiedPropertiesWithoutUndo();
    }
    private static void Click(GameObject target)
    {
        Require(target != null && target.activeInHierarchy, "Missing active UI target.");
        ExecuteEvents.Execute(target, new PointerEventData(EventSystem.current) { button = PointerEventData.InputButton.Left }, ExecuteEvents.pointerClickHandler);
    }
    private static void AssertOnePage(Transform pages)
    {
        Require(pages.Cast<Transform>().Count(p => p.gameObject.activeInHierarchy) == 1, "Exactly one page must be visible.");
    }
    private static void Require(bool condition, string message)
    {
        if (!condition) throw new InvalidOperationException(message);
    }
}
