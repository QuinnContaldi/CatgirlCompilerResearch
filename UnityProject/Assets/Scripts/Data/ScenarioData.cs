using UnityEngine;

namespace Meowra.Data
{
    // Numeric IDs retained for compatibility with historical exports; Raw is never scheduled.
    public enum FeedbackCondition { Raw = 0, Neutral = 1, Meowra = 2 }
    public enum AnswerChoice { Unassigned = -1, A = 0, B = 1, C = 2, D = 3 }

    [CreateAssetMenu(menuName = "Meowra/Scenario", fileName = "Scenario")]
    public sealed class ScenarioData : ScriptableObject
    {
        public string scenarioId;
        public string taskSet;
        public string matchedPairId;
        public string errorCategory;
        [TextArea(6, 24)] public string codeText;

        [TextArea(2, 6)] public string question;
        [TextArea(2, 6)] public string answerA;
        [TextArea(2, 6)] public string answerB;
        [TextArea(2, 6)] public string answerC;
        [TextArea(2, 6)] public string answerD;
        public AnswerChoice correctAnswer = AnswerChoice.Unassigned;
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

        // A missing key must never silently become answer A.
        public string GetValidationError()
        {
            if (string.IsNullOrWhiteSpace(scenarioId)) return "Assign a unique Scenario ID.";
            if (string.IsNullOrWhiteSpace(codeText)) return "Enter Code Text.";
            if (string.IsNullOrWhiteSpace(question)) return "Enter the Question.";
            for (int i = 0; i < 4; i++)
                if (string.IsNullOrWhiteSpace(GetAnswer((AnswerChoice)i))) return $"Enter answer {(AnswerChoice)i}.";
            if ((int)correctAnswer < 0 || (int)correctAnswer > 3) return "Select the Correct Answer.";
            if (taskSet != "A" && taskSet != "B") return "Choose task set A or B.";
            if (string.IsNullOrWhiteSpace(errorCategory)) return "Enter the error category.";
            return null;
        }
    }
}
