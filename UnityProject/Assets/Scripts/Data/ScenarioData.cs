using UnityEngine;

namespace Meowra.Data
{
    public enum FeedbackCondition { Raw, Neutral, Meowra }
    public enum AnswerChoice { Unassigned = -1, A = 0, B = 1, C = 2, D = 3 }

    [CreateAssetMenu(menuName = "Meowra/Scenario", fileName = "Scenario")]
    public sealed class ScenarioData : ScriptableObject
    {
        public string scenarioId;
        [TextArea(6, 24)] public string codeText;

        // Retained only as references to the original stimuli; participant UI uses text.
        [HideInInspector] public Sprite codeImage;
        [HideInInspector] public Sprite rawFeedbackImage;
        [HideInInspector] public Sprite neutralFeedbackImage;
        [HideInInspector] public Sprite meowraFeedbackImage;
        [TextArea(2, 6)] public string question;
        [TextArea(2, 6)] public string answerA;
        [TextArea(2, 6)] public string answerB;
        [TextArea(2, 6)] public string answerC;
        [TextArea(2, 6)] public string answerD;
        public AnswerChoice correctAnswer = AnswerChoice.Unassigned;
        [TextArea(4, 12)] public string rawFeedback;
        [TextArea(4, 12)] public string neutralFeedback;
        [TextArea(4, 12)] public string meowraFeedback;

        public string GetAnswer(AnswerChoice choice)
        {
            switch (choice)
            {
                case AnswerChoice.A: return answerA;
                case AnswerChoice.B: return answerB;
                case AnswerChoice.C: return answerC;
                case AnswerChoice.D: return answerD;
                default: throw new System.ArgumentOutOfRangeException(nameof(choice));
            }
        }

        public Sprite GetFeedbackImage(FeedbackCondition condition)
        {
            switch (condition)
            {
                case FeedbackCondition.Raw: return rawFeedbackImage;
                case FeedbackCondition.Neutral: return neutralFeedbackImage;
                case FeedbackCondition.Meowra: return meowraFeedbackImage;
                default: throw new System.ArgumentOutOfRangeException(nameof(condition));
            }
        }

        public string GetFeedback(FeedbackCondition condition)
        {
            switch (condition)
            {
                case FeedbackCondition.Raw: return rawFeedback;
                case FeedbackCondition.Neutral: return neutralFeedback;
                case FeedbackCondition.Meowra: return meowraFeedback;
                default: throw new System.ArgumentOutOfRangeException(nameof(condition));
            }
        }

        // A missing key must never silently become answer A.
        public string GetValidationError()
        {
            if (string.IsNullOrWhiteSpace(scenarioId)) return "Assign a unique Scenario ID.";
            if (string.IsNullOrWhiteSpace(codeText)) return "Enter Code Text.";
            if (string.IsNullOrWhiteSpace(question)) return "Enter the Question.";
            for (int i = 0; i < 4; i++)
                if (string.IsNullOrWhiteSpace(GetAnswer((AnswerChoice)i))) return $"Enter answer {(AnswerChoice)i}.";
            if ((int)correctAnswer < 0 || (int)correctAnswer > 3) return "Select the Correct Answer.";
            foreach (FeedbackCondition condition in System.Enum.GetValues(typeof(FeedbackCondition)))
                if (string.IsNullOrWhiteSpace(GetFeedback(condition))) return $"Enter {condition} Feedback.";
            return null;
        }
    }
}
