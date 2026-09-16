using System;
using System.Collections.Generic;
using Meowra.Data;

namespace Meowra.Experiment
{
    public sealed class TrialAssignment
    {
        public ScenarioData Scenario { get; }
        public FeedbackCondition Condition { get; }
        public int BlockIndex { get; }

        public TrialAssignment(ScenarioData scenario, FeedbackCondition condition, int blockIndex)
        {
            Scenario = scenario;
            Condition = condition;
            BlockIndex = blockIndex;
        }
    }

    public static class Counterbalancing
    {
        public static FeedbackCondition[] GetOrder(int orderNumber)
        {
            switch (orderNumber)
            {
                case 1: return new[] { FeedbackCondition.Raw, FeedbackCondition.Neutral, FeedbackCondition.Meowra };
                case 2: return new[] { FeedbackCondition.Raw, FeedbackCondition.Meowra, FeedbackCondition.Neutral };
                case 3: return new[] { FeedbackCondition.Neutral, FeedbackCondition.Raw, FeedbackCondition.Meowra };
                case 4: return new[] { FeedbackCondition.Neutral, FeedbackCondition.Meowra, FeedbackCondition.Raw };
                case 5: return new[] { FeedbackCondition.Meowra, FeedbackCondition.Raw, FeedbackCondition.Neutral };
                case 6: return new[] { FeedbackCondition.Meowra, FeedbackCondition.Neutral, FeedbackCondition.Raw };
                default: throw new ArgumentOutOfRangeException(nameof(orderNumber));
            }
        }

        public static string GetOrderLabel(int orderNumber)
        {
            var order = GetOrder(orderNumber);
            return $"{orderNumber}: {order[0]} > {order[1]} > {order[2]}";
        }

        public static List<TrialAssignment> BuildSchedule(StudyDefinition study, int orderNumber, int stimulusSet)
        {
            if (study == null) throw new ArgumentNullException(nameof(study));
            string error = study.GetValidationError(true);
            if (error != null) throw new ArgumentException(error, nameof(study));
            if (stimulusSet < 1 || stimulusSet > 3) throw new ArgumentOutOfRangeException(nameof(stimulusSet));
            var order = GetOrder(orderNumber);
            var schedule = new List<TrialAssignment>(6);
            for (int block = 0; block < order.Length; block++)
            {
                // Rotation is independent of block order: each pair appears in each
                // condition across the three sets, once per participant session.
                int pair = ((int)order[block] + stimulusSet - 1) % 3;
                for (int offset = 0; offset < 2; offset++)
                    schedule.Add(new TrialAssignment(study.scenarios[pair * 2 + offset], order[block], block));
            }
            return schedule;
        }
    }
}
