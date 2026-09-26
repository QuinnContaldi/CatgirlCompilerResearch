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
        public const int TasksPerBlock = 4;
        public const int CellCount = 4;

        public static FeedbackCondition[] GetOrder(int cell)
        {
            if (cell < 1 || cell > CellCount) throw new ArgumentOutOfRangeException(nameof(cell));
            return cell % 2 == 1
                ? new[] { FeedbackCondition.Neutral, FeedbackCondition.Meowra }
                : new[] { FeedbackCondition.Meowra, FeedbackCondition.Neutral };
        }

        public static string GetOrderLabel(int cell)
        {
            var order = GetOrder(cell);
            return $"{cell}: {order[0]} + Set {(cell <= 2 ? "A" : "B")} > {order[1]} + Set {(cell <= 2 ? "B" : "A")}";
        }

        public static List<TrialAssignment> BuildSchedule(StudyDefinition study, int cell)
        {
            if (study == null) throw new ArgumentNullException(nameof(study));
            string error = study.GetValidationError(true);
            if (error != null) throw new ArgumentException(error, nameof(study));
            var order = GetOrder(cell);
            var schedule = new List<TrialAssignment>(8);
            for (int block = 0; block < 2; block++)
            {
                int set = (cell <= 2 ? block : 1 - block);
                for (int offset = 0; offset < TasksPerBlock; offset++)
                    schedule.Add(new TrialAssignment(study.scenarios[set * TasksPerBlock + offset], order[block], block));
            }
            return schedule;
        }
    }
}
