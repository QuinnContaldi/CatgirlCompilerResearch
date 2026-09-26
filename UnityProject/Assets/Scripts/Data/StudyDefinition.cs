using System.Collections.Generic;
using UnityEngine;

namespace Meowra.Data
{
    [CreateAssetMenu(menuName = "Meowra/Study Definition", fileName = "StudyDefinition")]
    public sealed class StudyDefinition : ScriptableObject
    {
        [Tooltip("Eight tasks: Set A in slots 1–4 and Set B in slots 5–8.")]
        public ScenarioData[] scenarios = new ScenarioData[8];
        [Header("Participant consent")]
        [Tooltip("Enter the consent information participants should read, including risks and other study details.")]
        [TextArea(10, 30)] public string consentText;

        public Sprite meowraPortrait;

        [Header("Two-condition protocol — draft until reviewed")]
        public bool contentReviewed;
        public string studyVersion = "two-condition-draft-v2";
        [TextArea(3, 8)] public string overview;
        [TextArea(2, 6)] public string neutralIntroduction;
        [TextArea(2, 6)] public string meowraBlockIntroduction;
        [TextArea(2, 6)] public string neutralTutorial;
        [TextArea(2, 6)] public string meowraTutorial;
        [TextArea(2, 6)] public string neutralCompletion;
        [TextArea(2, 6)] public string meowraCompletion;
        public string[] neutralTransitions;
        public string[] meowraTransitions;

        [Header("Dr. Meowra — blue-box dialogue")]
        [TextArea(2, 4)] public string meowraIntroductionDialogue;
        [TextArea(2, 4)] public string meowraTutorialDialogue;
        [Tooltip("Four messages, in question order, shared by Set A and Set B. Encouragement only: no hints or correctness feedback.")]
        [TextArea(2, 4)] public string[] meowraTaskDialogue;
        [TextArea(2, 4)] public string meowraCompletionDialogue;


        [Header("Shared participant text (imported from study-content)")]
        [TextArea(2, 8)] public string backgroundPrompt;
        [TextArea(2, 8)] public string preferencePrompt;
        [TextArea(2, 8)] public string reasonPrompt;
        [TextArea(2, 8)] public string overviewHeading;
        [TextArea(2, 8)] public string introductionHeading;
        [TextArea(2, 8)] public string tutorialHeading;
        [TextArea(2, 8)] public string completionHeading;
        [TextArea(2, 8)] public string progressHeading;
        [TextArea(2, 8)] public string taskHeading;
        [TextArea(2, 8)] public string studyHeading;
        [TextArea(2, 8)] public string submitLabel;
        [TextArea(2, 8)] public string preferenceHeading;
        [TextArea(2, 8)] public string optionalPlaceholder;
        [TextArea(2, 8)] public string previewCompletion;
        [TextArea(2, 8)] public string studyCompletion;
        [TextArea(2, 8)] public string ueqsInstructions;

        public string consentHeading;
        public string acceptLabel;
        public string scrollHint;
        public string neutralName;
        public string meowraName;
        public string ueqsHeading;
        public string pragmaticHeading;
        public string hedonicHeading;
        public string taskSubmitLabel;
        public string taskProgressFormat;
        public string saveErrorMessage;
        public string retryLabel;

        public string GetValidationError(bool preview = false)
        {
            if (!preview && !contentReviewed) return "Review and freeze the draft tasks, consent and treatment wording before scored sessions.";
            if (!preview && string.IsNullOrWhiteSpace(consentText)) return "Enter Consent Text.";
            if (scenarios == null || scenarios.Length != 8) return "Assign eight tasks: Set A in slots 1–4, Set B in slots 5–8.";
            var ids = new HashSet<string>();
            for (int i = 0; i < scenarios.Length; i++)
            {
                var scenario = scenarios[i];
                if (scenario == null) return $"Assign task slot {i + 1}.";
                if (!ids.Add(scenario.scenarioId ?? "")) return "Task IDs must be unique.";
                string error = scenario.GetValidationError();
                if (error != null) return $"{scenario.name}: {error}";
                if (scenario.taskSet != (i < 4 ? "A" : "B")) return "Slots 1–4 must be Set A; slots 5–8 must be Set B.";
                if (i >= 4 && scenario.errorCategory != scenarios[i - 4].errorCategory) return "Match error categories across A/B pairs.";
            }
            var categories = new HashSet<string>();
            for (int i = 0; i < 4; i++)
                if (!categories.Add(scenarios[i].errorCategory)) return "Use four distinct error categories per set.";
            string[] messages = { consentHeading, acceptLabel, scrollHint, neutralName, meowraName, ueqsHeading, pragmaticHeading, hedonicHeading, taskSubmitLabel, taskProgressFormat, saveErrorMessage, retryLabel, backgroundPrompt, preferencePrompt, reasonPrompt, overviewHeading, introductionHeading, tutorialHeading, completionHeading, progressHeading, taskHeading, studyHeading, submitLabel, preferenceHeading, optionalPlaceholder, previewCompletion, studyCompletion, ueqsInstructions, overview, neutralIntroduction, meowraBlockIntroduction, neutralTutorial, meowraTutorial, neutralCompletion, meowraCompletion };
            if (System.Array.Exists(messages, string.IsNullOrWhiteSpace)) return "Complete the treatment and overview text.";
            if (neutralTransitions == null || meowraTransitions == null || neutralTransitions.Length != 3 || meowraTransitions.Length != 3 ||
                System.Array.Exists(neutralTransitions, string.IsNullOrWhiteSpace) || System.Array.Exists(meowraTransitions, string.IsNullOrWhiteSpace)) return "Enter three transitions per condition.";
            if (meowraTaskDialogue == null || meowraTaskDialogue.Length != 4 ||
                System.Array.Exists(meowraTaskDialogue, string.IsNullOrWhiteSpace)) return "Enter four blue-box question messages for Dr. Meowra.";
            var dialogue = new HashSet<string>(System.StringComparer.OrdinalIgnoreCase);
            var bannerMessages = new List<string> { meowraIntroductionDialogue, meowraTutorialDialogue, meowraCompletionDialogue };
            bannerMessages.AddRange(meowraTaskDialogue);
            bannerMessages.AddRange(meowraTransitions);
            foreach (string message in bannerMessages)
            {
                if (string.IsNullOrWhiteSpace(message)) return "Complete Dr. Meowra's blue-box dialogue.";
                if (!dialogue.Add(message.Trim())) return "Use a different Dr. Meowra blue-box message for each slide.";
            }
            try { string.Format(taskProgressFormat, 1, 4); }
            catch (System.FormatException) { return "Invalid task progress format."; }
            if (!taskProgressFormat.Contains("{0}") || !taskProgressFormat.Contains("{1}")) return "Task progress must include question {0} and total {1}.";
            if (meowraPortrait == null) return "Assign the Meowra portrait.";
            return null;
        }
    }
}
