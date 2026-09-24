using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Reflection;
using Meowra.Data;
using Meowra.Experiment;
using Meowra.UI;
using TMPro;
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

        string testDirectory = Path.Combine(Path.GetTempPath(), "MeowraLoggingCheck-" + Guid.NewGuid().ToString("N"));
        var logger = new DataLogger(testDirectory);
        SetLogger(manager, logger);
        Debug.Log("LOGGING_CHECK_DIRECTORY: " + testDirectory);
        var original = Field<StudyDefinition>(manager, "study");
        Require(original.GetValidationError(true) == null, "The authored study must allow preview: " + original.GetValidationError(true));
        yield return null; // Let reactivated graphics register with the canvas raycaster.
        ClickAtCenter(GameObject.Find("Canvas/Pages/ResearcherSetupPage/OpenSavedDataButton"), false);
        ClickAtCenter(GameObject.Find("Canvas/Pages/ResearcherSetupPage/OpenLatestCsvButton"), false);
        ClickAtCenter(GameObject.Find("Canvas/Pages/ResearcherSetupPage/PreviewLayoutButton"));
        Require(manager.Stage == ExperimentStage.Welcome, "The authored preview button must open consent.");
        manager.ReturnToSetup();
        var textView = Object.FindAnyObjectByType<TrialView>(FindObjectsInactive.Include);
        navigation.ShowPage(textView.transform.GetSiblingIndex());
        foreach (var scenario in original.scenarios)
            foreach (FeedbackCondition condition in Enum.GetValues(typeof(FeedbackCondition)))
            {
                textView.Show(scenario, condition, original.meowraPortrait, 1, 6);
                yield return null;
                Canvas.ForceUpdateCanvases();
                AssertRenderedText(Field<TMP_Text>(textView, "codeText"), scenario.codeText);
                AssertRenderedText(Field<TMP_Text>(textView, "explanation"), scenario.GetFeedback(condition));
                Require(textView.GetComponentsInChildren<Image>().All(image => image.sprite == null || image.sprite != scenario.codeImage),
                    "Scenario code must render as text, never its archived picture.");
            }
        var codeLabel = Field<TMP_Text>(textView, "codeText");
        foreach (string literal in new[] { "#include <stdio.h>\nif (x < 5 && x > 0) {}",
            "printf(\"<color=red></noparse> & \\\n\"); // int 42", "/* int \"text\" */\nint n = 12;" })
        {
            codeLabel.text = StudyTextFormatter.Code(literal);
            AssertRenderedText(codeLabel, literal);
        }
        navigation.ShowPage(0);
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
                scenario.codeText = "int fixture = " + i + ";";
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
            Require(study.GetValidationError() != null, "Missing consent text must block scored sessions.");
            study.consentText = string.Join("\n", Enumerable.Repeat("Synthetic consent information for testing scrolling.", 80));
            Require(study.GetValidationError() == null, "Complete synthetic authoring data must validate.");
            string savedId = study.scenarios[1].scenarioId;
            study.scenarios[1].scenarioId = study.scenarios[0].scenarioId;
            Require(study.GetValidationError() != null, "Duplicate scenario IDs must be rejected.");
            study.scenarios[1].scenarioId = savedId;
            CheckOrders(study);
            CheckPersistence(study);

            // Each saved menu option drives a complete run using the actual UI events.
            var previousFiles = new Dictionary<string, string>();
            for (int order = 1; order <= 6; order++)
                for (int set = 1; set <= 3; set++)
                {
                    SetLogger(manager, new DataLogger(testDirectory));
                    foreach (var item in WalkSession(manager, false, order, set)) yield return item;
                    foreach (var file in previousFiles)
                        Require(File.ReadAllText(file.Key) == file.Value, "A new session modified a previous participant's files.");
                    foreach (string file in Directory.GetFiles(manager.SessionDirectory))
                        previousFiles[file] = File.ReadAllText(file);
                }
            // Exercise the shipped scenarios too, without supplying participant consent
            // or changing research content in the saved asset.
            var authoredStudy = Object.Instantiate(original);
            try
            {
                authoredStudy.consentText = study.consentText;
                Require(authoredStudy.GetValidationError() == null,
                    "Authored live content must validate once consent is supplied: " + authoredStudy.GetValidationError());
                SetStudy(manager, authoredStudy);
                foreach (var item in WalkSession(manager, false, 1, 1)) yield return item;
            }
            finally
            {
                SetStudy(manager, study);
                Object.Destroy(authoredStudy);
            }
            Require(JsonUtility.ToJson(original) == originalJson, "Tests must not mutate authoring assets.");
        }
        finally
        {
            Time.timeScale = 1;
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
        yield return null;
        ClickAtCenter(setup.Find(preview ? "PreviewLayoutButton" : "StartSessionButton").gameObject);
        yield return null;
        Require(manager.Stage == ExperimentStage.Welcome, "Start must reach Welcome.");
        CheckSnapshot(manager, 0);
        Require(manager.SessionDirectory.Contains(preview ? "Previews" : "Participants"), "Preview storage must be separate.");
        Require(!manager.Session.ConsentAccepted, "Consent must start unaccepted.");
        var consent = Object.FindAnyObjectByType<ConsentView>();
        string displayedConsent = consent.DisplayedText;
        var acceptButton = GameObject.Find("Canvas/Pages/WelcomePage/ContinueButton");
        Require(acceptButton.GetComponentInChildren<Text>().text == "Accept", "Consent needs an Accept button.");
        if (!preview)
        {
            var scroll = consent.GetComponentInChildren<ScrollRect>();
            Canvas.ForceUpdateCanvases();
            Require(scroll.content.rect.height > scroll.viewport.rect.height, "Long consent must be scrollable.");
        }
        Click(acceptButton);
        Require(manager.Stage == ExperimentStage.HostIntroduction && manager.Session.ConsentAccepted,
            "Accept must record consent and immediately introduce the host.");
        Click(GameObject.Find("Canvas/Pages/MeowraIntroductionPage/ContinueButton"));
        Require(manager.Stage == ExperimentStage.Instructions, "Host introduction must lead to instructions.");
        Require(manager.Session.ConsentText == displayedConsent && !string.IsNullOrEmpty(manager.Session.ConsentAcceptedUtc),
            "Consent must retain the displayed wording and timestamp.");
        manager.ContinueWelcome();
        CheckSnapshot(manager, 0);
        Click(GameObject.Find("Canvas/Pages/InstructionsPage/ContinueButton"));
        yield return null;
        int introductions = 0;
        int evaluations = 0;
        int submitted = 0;
        int steps = 0;
        var pages = GameObject.Find("Canvas/Pages").transform;
        while (manager.Stage != ExperimentStage.Complete)
        {
            Require(++steps <= 22, "Session did not finish in the expected number of stages.");
            AssertOnePage(pages);
            var host = GameObject.Find("Canvas/MeowraHost/Portrait").GetComponent<Image>();
            Require(host.enabled == (!preview && manager.Stage == ExperimentStage.Trial),
                "Head portrait appears on questions only.");
            var large = GameObject.Find("Canvas/MeowraLargePortrait").GetComponent<Image>();
            Require(large.enabled == (!preview && manager.Stage != ExperimentStage.Trial),
                "Other forms must show the large portrait.");
            if (manager.Stage == ExperimentStage.Trial)
            {
                string expected = Field<StudyDefinition>(manager, "study").answerEncouragements[submitted];
                Require(GameObject.Find("Canvas/MeowraHost/Dialogue").GetComponent<Text>().text.EndsWith(expected),
                    "Encouragement must be visible before answering.");
            }
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
                var questionnaire = Object.FindAnyObjectByType<UeqsView>();
                var options = questionnaire.GetComponentsInChildren<Toggle>();
                Require(options.Length == 56 && options.All(t => !t.isOn), "UEQ-S must reset all eight rows.");
                manager.ContinueEvaluation();
                Require(manager.Stage == ExperimentStage.Evaluation && manager.Session.UeqsResponses.Count == evaluations - 1,
                    "Incomplete UEQ-S must not advance or record.");
                for (int item = 0; item < 8; item++)
                {
                    options[item * 7 + (item % 7)].isOn = true;
                    if (item < 7) Require(!questionnaire.IsComplete, "All eight answers are required.");
                }
                options[1].isOn = true;
                Require(!options[0].isOn, "UEQ-S choices must be exclusive within each item.");
                Click(questionnaire.GetComponentInChildren<Button>().gameObject);
                Require(manager.Session.UeqsResponses.Count == evaluations, "Each block must record one evaluation.");
                var rating = manager.Session.UeqsResponses[evaluations - 1];
                Require(rating.BlockNumber == evaluations && rating.Condition == manager.Session.Responses[submitted - 1].Condition,
                    "UEQ-S must retain the completed block's condition.");
                Require(rating.Positions[0] == 2 && rating.Positions[6] == 7 && rating.Positions[7] == 1, "UEQ-S positions changed.");
                Require(!manager.Session.Record(rating), "Duplicate evaluations must be rejected.");
                CheckSnapshot(manager, submitted);
            }
            else if (manager.Stage == ExperimentStage.Api)
            {
                Require(evaluations == 3 && !manager.Session.Completed, "API follows all three blocks before completion.");
                var questionnaire = Object.FindAnyObjectByType<FinalMeasuresView>();
                var options = questionnaire.GetComponentsInChildren<Toggle>();
                Require(options.Length == 50 && options.All(t => !t.isOn), "API must start with ten unanswered five-point items.");
                manager.ContinueApi();
                Require(!manager.Session.ApiSubmitted && manager.Stage == ExperimentStage.Api, "Incomplete API must not advance.");
                for (int item = 0; item < 10; item++)
                {
                    options[item * 5 + item % 5].isOn = true;
                    if (item < 9) Require(!questionnaire.ApiComplete, "All ten API items are required.");
                }
                options[1].isOn = true;
                Require(!options[0].isOn, "API answers must be exclusive per row.");
                Click(questionnaire.GetComponentInChildren<Button>().gameObject);
                manager.ContinueApi();
                Require(manager.Stage == ExperimentStage.Preference && manager.Session.ApiResponse.Ratings[0] == 2,
                    "API must save once and show preference.");
                Require(!manager.Session.Record(new ApiResponse(questionnaire.CopyRatings())), "Duplicate API must be rejected.");
                CheckSnapshot(manager, submitted);
            }
            else if (manager.Stage == ExperimentStage.Preference)
            {
                var questionnaire = Object.FindAnyObjectByType<FinalMeasuresView>();
                var options = questionnaire.GetComponentsInChildren<Toggle>();
                Require(options.Length == 3 && options.All(t => !t.isOn), "Preference must have three choices with no default.");
                manager.ContinuePreference();
                Require(!manager.Session.PreferenceSubmitted, "Preference requires an explicit choice.");
                options[0].isOn = true;
                int selected = order % 3;
                options[selected].isOn = true;
                Require(options.Count(t => t.isOn) == 1, "Preference must be forced single choice.");
                Click(questionnaire.GetComponentInChildren<Button>().gameObject);
                manager.ContinuePreference();
                Require(manager.Stage == ExperimentStage.OpenResponse && (int)manager.Session.PreferredCondition == selected,
                    "Preference must save before the separate explanation page.");
                Require(!manager.Session.RecordPreference(FeedbackCondition.Raw), "Duplicate preference must be rejected.");
                CheckSnapshot(manager, submitted);
            }
            else if (manager.Stage == ExperimentStage.OpenResponse)
            {
                var questionnaire = Object.FindAnyObjectByType<FinalMeasuresView>();
                var input = questionnaire.GetComponentInChildren<InputField>();
                Require(input.text == "" && !manager.Session.Completed, "Prose must reset and session must remain incomplete.");
                string reason = preview ? "" : "Clear, \"helpful\" feedback.\nNya — readable.";
                input.text = reason;
                // Exercise retry on the final save too: completion must never skip a failed write.
                var working = (DataLogger)typeof(ExperimentManager).GetField("logger", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(manager);
                bool fail = !preview && order == 1 && set == 1;
                if (fail) SetLogger(manager, new DataLogger(Path.Combine(working.RootDirectory, "blocked-by-file")));
                Click(questionnaire.GetComponentInChildren<Button>().gameObject);
                if (fail)
                {
                    Require(manager.SavePending && manager.Stage == ExperimentStage.OpenResponse, "Failed final save must block completion page.");
                    var previous = JsonUtility.FromJson<ParticipantSession>(File.ReadAllText(Path.Combine(manager.SessionDirectory, "session.json")));
                    Require(!previous.Completed && previous.PreferenceSubmitted && !previous.ReasonSubmitted, "Previous durable snapshot must remain intact.");
                    manager.ContinueOpenResponse();
                    SetLogger(manager, working);
                    manager.RetrySave();
                }
                manager.ContinueOpenResponse();
                Require(manager.Stage == ExperimentStage.Complete && manager.Session.PreferenceReason == reason, "Final prose must be preserved verbatim.");
                CheckSnapshot(manager, submitted);
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
                Require(Field<TMP_Text>(view, "codeText").text == StudyTextFormatter.Code(preview ? "[Code snippet]" : assignment.Scenario.codeText), "Incorrect code text binding.");
                Require(Field<TMP_Text>(view, "explanation").text == StudyTextFormatter.Feedback(preview ? "[Explanation]" : assignment.Scenario.GetFeedback(assignment.Condition)),
                    "Feedback must match both scenario and assigned condition.");
                var portrait = Field<Image>(view, "portrait");
                Require(!portrait.gameObject.activeSelf, "Trial should use the shared host portrait without a duplicate.");
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
                // Real elapsed time must still accumulate while Unity game time is stopped.
                float previousScale = Time.timeScale;
                Time.timeScale = 0;
                var elapsed = System.Diagnostics.Stopwatch.StartNew();
                yield return null;
                yield return null;
                double minimumSeconds = elapsed.Elapsed.TotalSeconds;
                bool testFailure = !preview && order == 1 && set == 1 && submitted == 0;
                DataLogger workingLogger = null;
                if (testFailure)
                {
                    workingLogger = (DataLogger)typeof(ExperimentManager).GetField("logger", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(manager);
                    string blocker = Path.Combine(workingLogger.RootDirectory, "blocked-by-file");
                    File.WriteAllText(blocker, "This file intentionally prevents creating a directory.");
                    SetLogger(manager, new DataLogger(blocker));
                }
                Click(submit.gameObject);
                if (testFailure)
                {
                    Require(manager.SavePending && manager.Stage == ExperimentStage.Trial, "Failed saves must block progression.");
                    Require(manager.Session.Responses.Count == 1, "Failed save must retain the response in memory.");
                    CheckSnapshot(manager, 0);
                    manager.ContinueEvaluation();
                    manager.ReturnToSetup();
                    Object.FindAnyObjectByType<TrialManager>().Submit();
                    Require(manager.Session.Responses.Count == 1, "Retry must not duplicate a response.");
                    SetLogger(manager, workingLogger);
                    manager.RetrySave();
                    Require(!manager.SavePending && manager.Stage == ExperimentStage.Trial && manager.CurrentTrial.Scenario != assignment.Scenario, "Retry must save and advance directly to the next question.");
                }
                Time.timeScale = previousScale;
                Object.FindAnyObjectByType<TrialManager>().Submit();
                submitted++;
                CheckSnapshot(manager, submitted);
                Require(manager.Session.Responses[submitted - 1].ResponseTimeSeconds >= minimumSeconds,
                    "Timing must use elapsed seconds independently of Unity time scale.");
                Require(manager.Session.Responses.Count == submitted, "A repeated submit duplicated or skipped a response.");
                var response = manager.Session.Responses[submitted - 1];
                Require(response.Encouragement == Field<StudyDefinition>(manager, "study").answerEncouragements[submitted - 1],
                    "Saved encouragement must match the message shown while answering.");
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
        CheckSnapshot(manager, 6);
        var completed = manager.Session;
        Click(GameObject.Find("Canvas/Pages/CompletionPage/SetupButton"));
        Require(manager.Session == completed && manager.Stage == ExperimentStage.Setup, "Retain results on return to setup.");
        yield return null;
    }

    private static void AssertRenderedText(TMP_Text text, string expected)
    {
        text.ForceMeshUpdate(true, true);
        Require(text.GetParsedText() == expected, "Highlighting must preserve literal characters: " + expected);
        Require(text.font != null && text.font.name == "StudyMono", "Use the bundled monospace font.");
        Require(!text.enableAutoSizing && text.fontSize == 24, "All conditions must retain the same readable font size.");
        Require(text.textInfo.characterInfo.Take(text.textInfo.characterCount).All(c => !c.isVisible || c.textElement != null),
            "Every visible character must have a font glyph.");
    }

    private static void SetLogger(ExperimentManager manager, DataLogger logger)
    {
        typeof(ExperimentManager).GetField("logger", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(manager, logger);
    }

    private static void CheckSnapshot(ExperimentManager manager, int count)
    {
        // Read the actual files, including partial sessions, rather than only checking memory.
        string json = File.ReadAllText(Path.Combine(manager.SessionDirectory, "session.json"));
        var restored = JsonUtility.FromJson<ParticipantSession>(json);
        Require(restored.ParticipantId == manager.Session.ParticipantId && restored.Responses.Count == count,
            "Saved JSON must preserve identity and submitted responses.");
        Require(restored.OrderNumber == manager.Session.OrderNumber && restored.StimulusSet == manager.Session.StimulusSet,
            "Saved assignment differs from the session.");
        if (count == manager.Session.Responses.Count)
            Require(json == JsonUtility.ToJson(manager.Session, true), "JSON must round-trip the entire current snapshot.");
        var assignment = File.ReadAllLines(Path.Combine(manager.SessionDirectory, "assignment.csv"));
        Require(assignment.Length == 7, "Even an unanswered session must export all six planned tasks.");
        string taskOrder = string.Join(" > ", restored.PlannedScenarioIds);
        string conditionOrder = string.Join(" > ", Counterbalancing.GetOrder(restored.OrderNumber));
        for (int i = 0; i < 6; i++)
        {
            string expected = restored.ParticipantId + "," + restored.OrderNumber + ",\"" + conditionOrder + "\"," +
                restored.StimulusSet + ",\"" + taskOrder + "\"," + (restored.IsPreview ? "true" : "false") + "," +
                (restored.Completed ? "true" : "false") + "," + (i + 1) + "," + (i / 2 + 1) + "," + (i % 2 + 1) +
                ",\"" + restored.PlannedScenarioIds[i] + "\"," + restored.PlannedConditions[i] + "," + (i < count ? "true" : "false");
            Require(assignment[i + 1] == expected, "Assignment CSV must preserve rotation, condition order and partial progress.");
        }
        var apiCsv = File.ReadAllLines(Path.Combine(manager.SessionDirectory, "api.csv"));
        Require(apiCsv.Length == (restored.ApiSubmitted ? 11 : 1), "API CSV must have ten rows only after submission.");
        if (restored.ApiSubmitted)
            Require(apiCsv[1].EndsWith(",1,Engaging,\"Dr. Meowra was expressive.\",2") &&
                apiCsv[10].Contains(",10,Credible,\"Dr. Meowra was instructor-like.\",5"), "API export must preserve wording, order and ratings.");
        string preferenceCsv = File.ReadAllText(Path.Combine(manager.SessionDirectory, "preference.csv"));
        if (restored.PreferenceSubmitted)
            Require(preferenceCsv.Contains(restored.ParticipantId + "," + restored.PreferredCondition + ","), "Preference CSV lost the chosen condition.");
        if (restored.ReasonSubmitted)
            Require(preferenceCsv.Contains("\"" + restored.PreferenceReason.Replace("\"", "\"\"") + "\""), "CSV must quote commas, quotes, Unicode and newlines correctly.");
        var ratings = File.ReadAllLines(Path.Combine(manager.SessionDirectory, "ueqs.csv"));
        Require(ratings.Length == restored.UeqsResponses.Count * 8 + 1, "UEQ-S CSV must preserve eight rows per saved evaluation.");
        if (restored.UeqsResponses.Count > 0)
            Require(ratings[1].EndsWith(",1,Pragmatic,obstructive,supportive,2"), "UEQ-S CSV must retain item wording and selected position.");
        var csv = File.ReadAllLines(Path.Combine(manager.SessionDirectory, "trials.csv"));
        Require(csv[0].Contains(",session_completed,") && csv[0].Contains(",task_order,block_number,trial_in_block,"),
            "Trial CSV must label completion and task order explicitly.");
        Require(csv.Length == count + 1 && csv[0].EndsWith("response_time_seconds"), "CSV must have one row per submitted response and timing units.");
        if (count > 0)
        {
            Require(restored.Responses[count - 1].ResponseTimeSeconds > 0, "Elapsed time must survive serialization.");
            Require(File.Exists(Path.Combine(manager.SessionDirectory, "session.json.bak")), "Replacement must retain a previous snapshot.");
        }
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
    private static void ClickAtCenter(GameObject target, bool execute = true)
    {
        Require(target != null && target.activeInHierarchy, "Missing active pointer target.");
        Canvas.ForceUpdateCanvases();
        var rect = (RectTransform)target.transform;
        var canvas = target.GetComponentInParent<Canvas>();
        var camera = canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera;
        var pointer = new PointerEventData(EventSystem.current) {
            button = PointerEventData.InputButton.Left,
            position = RectTransformUtility.WorldToScreenPoint(camera, rect.TransformPoint(rect.rect.center))
        };
        var hits = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointer, hits);
        Require(hits.Count > 0, "No pointer hit for " + target.name);
        var receiver = ExecuteEvents.GetEventHandler<IPointerClickHandler>(hits[0].gameObject);
        Require(receiver == target, target.name + " is blocked by " + UnityEditor.AnimationUtility.CalculateTransformPath(hits[0].gameObject.transform, null));
        if (execute) ExecuteEvents.Execute(receiver, pointer, ExecuteEvents.pointerClickHandler);
    }

    private static void CheckPersistence(StudyDefinition study)
    {
        string root = Path.Combine(Path.GetTempPath(), "MeowraPersistenceCheck-" + Guid.NewGuid().ToString("N"));
        Require(StudyResults.FindLatestParticipantDirectory(root) == null, "No results must be handled before the first session.");
        var schedule = Counterbalancing.BuildSchedule(study, 1, 1);
        var first = new ParticipantSession(1, 1, false, schedule);
        first.AcceptConsent("Synthetic persistence-check consent.");
        first.Record(new TrialResponse(schedule[0].Scenario, schedule[0].Condition, AnswerChoice.A, false, 1.25));
        var logger = new DataLogger(root);
        logger.Save(first);
        string firstDirectory = logger.GetSessionDirectory(first);
        var saved = Directory.GetFiles(firstDirectory).ToDictionary(path => path, File.ReadAllText);

        // Recreate the storage service without any reference to the previous session.
        logger = new DataLogger(root);
        var second = new ParticipantSession(1, 1, false, schedule);
        logger.Save(second);
        string secondDirectory = logger.GetSessionDirectory(second);
        Require(firstDirectory != secondDirectory, "Each participant needs a distinct storage folder.");
        File.SetLastWriteTimeUtc(Path.Combine(firstDirectory, "session.json"), DateTime.UtcNow.AddMinutes(-2));
        File.SetLastWriteTimeUtc(Path.Combine(secondDirectory, "session.json"), DateTime.UtcNow.AddMinutes(-1));
        logger.Save(new ParticipantSession(1, 1, true, schedule));
        Require(StudyResults.FindLatestParticipantDirectory(root) == secondDirectory,
            "Latest live results must be discoverable from disk, excluding newer previews.");
        foreach (var file in saved)
            Require(File.ReadAllText(file.Key) == file.Value, "Starting another session must preserve partial-session JSON and CSV.");
        var restored = JsonUtility.FromJson<ParticipantSession>(File.ReadAllText(Path.Combine(firstDirectory, "session.json")));
        Require(!restored.Completed && restored.Responses.Count == 1 && restored.Responses[0].ResponseTimeSeconds == 1.25,
            "Submitted partial data must remain readable after recreating storage.");
        Debug.Log("PERSISTENCE_CHECK_DIRECTORY: " + root);
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
