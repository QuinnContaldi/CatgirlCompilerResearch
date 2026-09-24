using System.Collections.Generic;
using UnityEngine;

namespace Meowra.Data
{
    [CreateAssetMenu(menuName = "Meowra/Study Definition", fileName = "StudyDefinition")]
    public sealed class StudyDefinition : ScriptableObject
    {
        [Tooltip("Six distinct scenarios. Slots 1–2, 3–4 and 5–6 form the three rotating pairs.")]
        public ScenarioData[] scenarios = new ScenarioData[6];
        [Header("Participant consent")]
        [Tooltip("Enter the consent information participants should read, including risks and other study details.")]
        [TextArea(10, 30)] public string consentText;

        [Header("Meowra as study host")]
        [TextArea(2, 5)] public string consentQuote = "Nya! This page explains what taking part involves and the risks associated with the experiment. Please read it before deciding whether to participate.";
        [TextArea(4, 12)] public string hostIntroduction = "Hi, I'm Dr. Meowra, your catgirl assistant! I'm here to guide you through this coding assignment, nya. Don't worry about getting everything right—we'll take it one step at a time. I'll stay with you throughout the study, and in my feedback block I'll help explain the coding errors. Paws ready?";
        [TextArea(2, 5)] public string hostQuote = "Paws ready! The researcher can choose the session settings here.";
        [TextArea(2, 5)] public string surveyQuote = "That's the first pair of tasks finished. How did that feedback style feel to use?";
        [TextArea(2, 5)] public string completionQuote = "Meowtastic effort! Thank you for taking part, purrr!";
        [Tooltip("Shown while answering; rotates by question number, independently of correctness and condition.")]
        public string[] answerEncouragements = {
            "One question at a time, nya. Take the time you need!",
            "I'm here with you, nya. Give this question your best try!",
            "Keep going at your own pace. You've got this, meow!",
            "Paws up for sticking with it! Take a moment to think.",
            "No rush, nya. Give yourself time to work through this question.",
            "One last coding question, nya. Keep giving it your best!"
        };
        [TextArea(2, 5)] public string hostIntroductionQuote = "A little hello before we begin, nya. Let me introduce myself!";
        [TextArea(2, 5)] public string instructionsQuote = "Here is how the activity works. I'll be nearby as you get familiar with it.";
        [TextArea(2, 5)] public string blockIntroductionQuote = "My explanation block is coming up, meow. Here's what to expect.";
        [TextArea(2, 5)] public string secondEvaluationQuote = "Two blocks down, nya. Please rate the style you just experienced.";
        [TextArea(2, 5)] public string thirdEvaluationQuote = "You've tried all three styles. These ratings are for the most recent block.";
        [TextArea(2, 5)] public string personaMeasuresQuote = "This page asks about your experience with me. Candid answers are welcome, nya.";
        [TextArea(2, 5)] public string preferenceQuote = "Looking back at the three styles, which would you choose to use?";
        [TextArea(2, 5)] public string openResponseQuote = "I'd like to hear your reasons in your own words. Share whatever stood out to you.";
        public Sprite meowraPortrait;
        [TextArea(4, 12)] public string meowraIntroduction;

        public string GetValidationError(bool preview = false)
        {
            if (string.IsNullOrWhiteSpace(hostIntroduction) || string.IsNullOrWhiteSpace(consentQuote) ||
                string.IsNullOrWhiteSpace(hostQuote) || string.IsNullOrWhiteSpace(surveyQuote) || string.IsNullOrWhiteSpace(completionQuote))
                return "Enter all Meowra host messages.";
            string[] pageQuotes = { hostQuote, consentQuote, hostIntroductionQuote, instructionsQuote,
                blockIntroductionQuote, surveyQuote, secondEvaluationQuote, thirdEvaluationQuote,
                personaMeasuresQuote, preferenceQuote, openResponseQuote, completionQuote };
            var dialogue = new HashSet<string>(System.StringComparer.OrdinalIgnoreCase);
            foreach (string quote in pageQuotes)
                if (string.IsNullOrWhiteSpace(quote) || !dialogue.Add(quote.Trim()))
                    return "Give every Meowra page a distinct, nonempty host message.";
            if (answerEncouragements == null || answerEncouragements.Length < 6 ||
                System.Array.Exists(answerEncouragements, string.IsNullOrWhiteSpace))
                return "Enter at least six nonempty answer encouragements.";
            foreach (string quote in answerEncouragements)
                if (!dialogue.Add(quote.Trim()))
                    return "Give every question a distinct encouragement, different from the other host messages.";
            if (!preview && string.IsNullOrWhiteSpace(consentText)) return "Enter Consent Text in the study asset.";
            if (scenarios == null || scenarios.Length != 6) return "Assign exactly six scenarios.";
            var ids = new HashSet<string>();
            for (int i = 0; i < scenarios.Length; i++)
            {
                var scenario = scenarios[i];
                if (scenario == null) return $"Assign scenario slot {i + 1}.";
                if (string.IsNullOrWhiteSpace(scenario.scenarioId) || !ids.Add(scenario.scenarioId.Trim()))
                    return $"Scenario slot {i + 1} needs a unique, nonempty ID.";
                if (!preview)
                {
                    string error = scenario.GetValidationError();
                    if (error != null) return $"{scenario.name}: {error}";
                }
            }
            if (!preview && string.IsNullOrWhiteSpace(meowraIntroduction)) return "Enter the Meowra Introduction in the study asset.";
            if (!preview && meowraPortrait == null) return "Assign the Meowra Portrait in the study asset.";
            return null;
        }
    }
}
