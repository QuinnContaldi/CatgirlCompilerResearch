using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Reflection;
using Meowra.Data;
using Meowra.Experiment;
using Meowra.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Object = UnityEngine.Object;

// Runs actual scene controls with isolated synthetic sessions; never freezes authored drafts.
public static class ExperimentSmokeCheck
{
    public static IEnumerator Run()
    {
        var manager = Object.FindAnyObjectByType<ExperimentManager>();
        var pages = GameObject.Find("Canvas/Pages").transform;
        var original = Field<StudyDefinition>(manager, "study");
        Require(original.GetValidationError(true) == null, "Content must preview: " + original.GetValidationError(true));
        Require(!original.contentReviewed || original.GetValidationError() == null, "Enabled content must support scored sessions.");
        var start = Field<Button>(Field<StudyShellView>(manager, "view"), "startButton");
        Require(start.interactable == (original.GetValidationError() == null), "Start button must reflect the authored session readiness.");
        var fixture = Object.Instantiate(original);
        fixture.contentReviewed = false;
        Require(fixture.GetValidationError() != null, "Disabling content must still block scored sessions.");
        fixture.contentReviewed = true;
        fixture.consentText = "Synthetic test consent.";
        Require(fixture.GetValidationError() == null, "Fixture content must validate.");
        var logger = new DataLogger(Path.Combine(Path.GetTempPath(), "MeowraCrossoverCheck-" + Guid.NewGuid().ToString("N")));
        Set(manager, "logger", logger);
        Set(manager, "study", fixture);
        var dropdown = GameObject.Find("Canvas/Pages/ResearcherSetupPage/OrderDropdown").GetComponent<Dropdown>();
        Require(dropdown.options.Count == 4, "Exactly four assignment cells must be offered.");
        string previousPath = null;
        string previousJson = null;
        for (int mode = 0; mode < 2; mode++)
        for (int cell = 1; cell <= 4; cell++)
        {
            dropdown.value = cell - 1;
            var schedule = Counterbalancing.BuildSchedule(fixture, cell);
            Require(schedule.Count == 8 && schedule.Select(t => t.Scenario.scenarioId).Distinct().Count() == 8, "Eight distinct tasks required.");
            Require(schedule.Take(4).All(t => t.Condition == (cell % 2 == 1 ? FeedbackCondition.Neutral : FeedbackCondition.Meowra)), "First condition must match cell.");
            Require(schedule.Skip(4).All(t => t.Condition != schedule[0].Condition && t.Condition != FeedbackCondition.Raw), "Second condition must be the other treatment.");
            Require(schedule[0].Scenario.taskSet == (cell <= 2 ? "A" : "B") && schedule[4].Scenario.taskSet == (cell <= 2 ? "B" : "A"), "Task set must match cell.");
            if (mode == 0) manager.PreviewLayout(); else manager.StartSession();
            Require(manager.Stage == ExperimentStage.Welcome, "Start at neutral consent.");
            AssertHost(false);
            manager.ContinueWelcome();
            Require(manager.Session.ConsentAccepted, "Consent must save.");
            manager.ContinueIntroduction();
            Require(manager.Stage == ExperimentStage.Background, "Background follows overview.");
            manager.ContinueBackground();
            int trials = 0, evaluations = 0, transitions = 0, completions = 0;
            while (manager.Stage != ExperimentStage.Complete)
            {
                Require(pages.Cast<Transform>().Count(p => p.gameObject.activeInHierarchy) == 1, "Exactly one page visible.");
                if (manager.Stage == ExperimentStage.Introduction || manager.Stage == ExperimentStage.Instructions)
                {
                    AssertHost(manager.CurrentTrial.Condition == FeedbackCondition.Meowra);
                    Click(Field<GameObject>(manager, "introductionPage").GetComponentInChildren<Button>());
                }
                else if (manager.Stage == ExperimentStage.Trial)
                {
                    AssertHost(manager.CurrentTrial.Condition == FeedbackCondition.Meowra);
                    var view = Object.FindAnyObjectByType<TrialView>();
                    Require(!Field<TMPro.TMP_Text>(view, "explanation").gameObject.activeInHierarchy, "Technical feedback must be hidden.");
                    var toggles = Field<Toggle[]>(view, "answers");
                    var button = Field<Button>(view, "submitButton");
                    Require(toggles.All(t => !t.isOn) && !button.interactable, "Selection must reset.");
                    var trialManager = Object.FindAnyObjectByType<TrialManager>();
                    trialManager.Submit();
                    Require(manager.Session.Responses.Count == trials, "Empty submission rejected.");
                    toggles[0].isOn = true;
                    toggles[1].isOn = true;
                    Require(toggles.Count(t => t.isOn) == 1 && toggles[1].targetGraphic.color != toggles[0].targetGraphic.color, "Exclusive selection must highlight the selected row.");
                    int key = (int)manager.CurrentTrial.Scenario.correctAnswer;
                    toggles[key].isOn = true;
                    Time.timeScale = 0;
                    var wait = System.Diagnostics.Stopwatch.StartNew();
                    while (wait.Elapsed.TotalSeconds < .03) yield return null;
                    Time.timeScale = 1;
                    if (cell == 1 && mode == 0 && trials == 0)
                    {
                        string blocker = Path.Combine(logger.RootDirectory, "blocked-file");
                        File.WriteAllText(blocker, "not a directory");
                        Set(manager, "logger", new DataLogger(blocker));
                        Click(button);
                        Require(manager.SavePending && manager.Stage == ExperimentStage.Trial, "Save failure must block advancement.");
                        Set(manager, "logger", logger);
                        manager.RetrySave();
                    }
                    else Click(button);
                    trials++;
                    Require(manager.Session.Responses.Count == trials, "One response saved per click.");
                    var response = manager.Session.Responses[trials - 1];
                    Require(response.ResponseTimeSeconds >= .015 && response.Scored == (mode == 1), "Timing and scoring mode must be correct.");
                    Require(mode == 0 || response.Correct, "Known correct selection must score correctly.");
                    string json = File.ReadAllText(Path.Combine(manager.SessionDirectory, "session.json"));
                    Require(JsonUtility.FromJson<ParticipantSession>(json).Responses.Count == trials, "Partial JSON must persist immediately.");
                }
                else if (manager.Stage == ExperimentStage.Transition || manager.Stage == ExperimentStage.BlockCompletion)
                {
                    AssertHost(schedule[trials - 1].Condition == FeedbackCondition.Meowra);
                    if (manager.Stage == ExperimentStage.Transition && schedule[trials - 1].Condition == FeedbackCondition.Meowra)
                    {
                        int position = (trials - 1) % 4;
                        string banner = GameObject.Find("Canvas/MeowraHost/Dialogue").GetComponent<Text>().text;
                        Require(banner == fixture.meowraName + "\n" + fixture.meowraTransitionDialogue[position], "Progress must use its dedicated host banner.");
                        Require(!banner.Contains(fixture.meowraTransitions[position]), "Progress must not repeat its body in the host banner.");
                    }
                    if (manager.Stage == ExperimentStage.Transition) transitions++; else completions++;
                    double elapsed = manager.Session.Responses.Last().ResponseTimeSeconds;
                    var wait = System.Diagnostics.Stopwatch.StartNew();
                    while (wait.Elapsed.TotalSeconds < .03) yield return null;
                    Require(manager.Session.Responses.Last().ResponseTimeSeconds == elapsed, "Transition time must not alter response time.");
                    manager.ContinueIntroduction();
                }
                else if (manager.Stage == ExperimentStage.Evaluation)
                {
                    AssertHost(false);
                    var evaluation = Object.FindAnyObjectByType<UeqsView>();
                    Require(!evaluation.IsComplete, "UEQ-S must reset.");
                    manager.ContinueEvaluation();
                    Require(manager.Stage == ExperimentStage.Evaluation, "Incomplete evaluation must not advance.");
                    foreach (var group in evaluation.GetComponentsInChildren<ToggleGroup>()) group.GetComponentsInChildren<Toggle>()[3].isOn = true;
                    Require(evaluation.IsComplete, "Eight responses complete UEQ-S.");
                    Click(evaluation.GetComponentInChildren<Button>());
                    evaluations++;
                }
                else if (manager.Stage == ExperimentStage.Preference)
                {
                    AssertHost(false);
                    var preference = Object.FindAnyObjectByType<FinalMeasuresView>();
                    var options = preference.GetComponentsInChildren<Toggle>();
                    Require(options.Length == 2 && options.All(t => !t.isOn), "Only Neutral/Meowra, no default preference.");
                    manager.ContinuePreference();
                    Require(manager.Stage == ExperimentStage.Preference, "Preference requires selection.");
                    options[cell % 2].isOn = true;
                    Click(preference.GetComponentInChildren<Button>());
                    Require(!manager.Session.RecordPreference(FeedbackCondition.Raw), "Raw preference rejected.");
                }
                else if (manager.Stage == ExperimentStage.OpenResponse)
                {
                    AssertHost(false);
                    var final = Object.FindAnyObjectByType<FinalMeasuresView>();
                    final.GetComponentInChildren<InputField>().text = "Test, quoted \"reason\"\nsecond line";
                    Click(final.GetComponentInChildren<Button>());
                }
                else throw new InvalidOperationException("Unexpected stage: " + manager.Stage);
                yield return null;
            }
            Require(trials == 8 && evaluations == 2 && transitions == 6 && completions == 2, "Complete two four-task blocks with all transitions.");
            Require(manager.Session.Completed && manager.Session.UeqsResponses.Count == 2, "Session must finish after final response.");
            Require(File.ReadAllLines(Path.Combine(manager.SessionDirectory, "assignment.csv")).Length == 9, "Eight assignment CSV rows.");
            Require(File.ReadAllLines(Path.Combine(manager.SessionDirectory, "trials.csv")).Length == 9, "Eight trial CSV rows.");
            Require(File.ReadAllLines(Path.Combine(manager.SessionDirectory, "ueqs.csv")).Length == 17, "Sixteen UEQ-S CSV rows.");
            Require(!File.Exists(Path.Combine(manager.SessionDirectory, "api.csv")), "No persona questionnaire export.");
            string csv = File.ReadAllText(Path.Combine(manager.SessionDirectory, "trials.csv"));
            Require(csv.Contains("task_response_time_ms") && csv.Contains("error_category") && csv.Contains("assignment_cell"), "New analysis columns required.");
            if (previousPath != null) Require(File.ReadAllText(previousPath) == previousJson, "Earlier session files preserved.");
            previousPath = Path.Combine(manager.SessionDirectory, "session.json");
            previousJson = File.ReadAllText(previousPath);
            manager.ReturnToSetup();
        }
        Set(manager, "study", original);
        Object.Destroy(fixture);
        Debug.Log("CROSSOVER_CHECK_DIRECTORY: " + logger.RootDirectory);
    }

    private static void AssertHost(bool visible)
    {
        var portrait = GameObject.Find("Canvas/MeowraHost/Portrait").GetComponent<Image>();
        Require(portrait.enabled == visible, "Meowra visibility must follow treatment only.");
        var text = GameObject.Find("Canvas/MeowraHost/Dialogue").GetComponent<Text>().text;
        Require(text.Contains("Dr. Meowra") == visible, "Neutral pages must not contain persona framing.");
    }
    private static T Field<T>(object target, string name) => (T)target.GetType().GetField(name, BindingFlags.Instance | BindingFlags.NonPublic).GetValue(target);
    private static void Set(object target, string name, object value) => target.GetType().GetField(name, BindingFlags.Instance | BindingFlags.NonPublic).SetValue(target, value);
    private static void Click(Button button)
    {
        Require(button != null && button.interactable, "Button must be usable.");
        ExecuteEvents.Execute(button.gameObject, new PointerEventData(EventSystem.current) { button = PointerEventData.InputButton.Left }, ExecuteEvents.pointerClickHandler);
    }
    private static void Require(bool condition, string message)
    {
        if (!condition) throw new InvalidOperationException(message);
    }
}
