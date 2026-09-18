using System;
using System.Collections.Generic;
using UnityEngine;

namespace Meowra.Data
{
    // Researcher-supplied selected API items; not the full instrument.
    public static class ApiItems
    {
        public const int Count = 10;
        private static readonly string[] wording = {
            "Dr. Meowra was expressive.", "Dr. Meowra was enthusiastic.",
            "Dr. Meowra was entertaining.", "Dr. Meowra was motivating.",
            "Dr. Meowra was friendly.", "Dr. Meowra was knowledgeable.",
            "Dr. Meowra was intelligent.", "Dr. Meowra was useful.",
            "Dr. Meowra was helpful.", "Dr. Meowra was instructor-like."
        };
        public static string Wording(int item) => wording[item];
        public static string Dimension(int item) => item < 5 ? "Engaging" : "Credible";
    }

    [Serializable]
    public sealed class ApiResponse
    {
        [SerializeField] private int[] ratings;
        [SerializeField] private string submittedUtc;
        public IReadOnlyList<int> Ratings => Array.AsReadOnly(ratings);

        public ApiResponse(int[] answers)
        {
            if (answers == null || answers.Length != ApiItems.Count)
                throw new ArgumentException("All ten API items are required.");
            foreach (int answer in answers)
                if (answer < 1 || answer > 5) throw new ArgumentOutOfRangeException(nameof(answers));
            ratings = (int[])answers.Clone();
            submittedUtc = DateTime.UtcNow.ToString("O");
        }
    }
}
