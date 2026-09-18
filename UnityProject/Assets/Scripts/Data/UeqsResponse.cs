using System;
using System.Collections.Generic;
using UnityEngine;
using Meowra.Experiment;

namespace Meowra.Data
{
    // Fixed item order and wording supplied by the researcher. Positions run left to right.
    public static class UeqsItems
    {
        public const int Count = 8;
        public static string Left(int item) => left[item];
        public static string Right(int item) => right[item];
        private static readonly string[] left = { "obstructive", "complicated", "inefficient", "confusing", "boring", "not interesting", "conventional", "usual" };
        private static readonly string[] right = { "supportive", "easy", "efficient", "clear", "exciting", "interesting", "inventive", "leading edge" };
    }

    [Serializable]
    public sealed class UeqsResponse
    {
        [SerializeField] private int blockNumber;
        [SerializeField] private FeedbackCondition condition;
        [SerializeField] private int[] positions;
        [SerializeField] private string submittedUtc;
        public int BlockNumber => blockNumber;
        public FeedbackCondition Condition => condition;
        public IReadOnlyList<int> Positions => Array.AsReadOnly(positions);

        public UeqsResponse(int block, FeedbackCondition feedbackCondition, int[] answers)
        {
            if (block < 1 || block > 3) throw new ArgumentOutOfRangeException(nameof(block));
            if (answers == null || answers.Length != UeqsItems.Count) throw new ArgumentException("All eight UEQ-S items are required.");
            foreach (int answer in answers)
                if (answer < 1 || answer > 7) throw new ArgumentOutOfRangeException(nameof(answers));
            blockNumber = block;
            condition = feedbackCondition;
            positions = (int[])answers.Clone();
            submittedUtc = DateTime.UtcNow.ToString("O");
        }
    }
}
