using System;
using System.Collections.Generic;
using Meowra.Experiment;
using UnityEngine;

namespace Meowra.Data
{
    [Serializable]
    public sealed class TrialResponse
    {
        [SerializeField] private string scenarioId;
        [SerializeField] private FeedbackCondition condition;
        [SerializeField] private AnswerChoice selectedAnswer;
        [SerializeField] private AnswerChoice correctAnswer;
        [SerializeField] private bool scored;
        [SerializeField] private bool correct;

        public string ScenarioId => scenarioId;
        public FeedbackCondition Condition => condition;
        public AnswerChoice SelectedAnswer => selectedAnswer;
        public bool Scored => scored;
        public bool Correct => correct;

        public TrialResponse(ScenarioData scenario, FeedbackCondition feedbackCondition, AnswerChoice answer, bool preview)
        {
            if (scenario == null) throw new ArgumentNullException(nameof(scenario));
            if ((int)answer < 0 || (int)answer > 3) throw new ArgumentOutOfRangeException(nameof(answer));
            scenarioId = scenario.scenarioId;
            condition = feedbackCondition;
            selectedAnswer = answer;
            correctAnswer = preview ? AnswerChoice.Unassigned : scenario.correctAnswer;
            scored = (int)correctAnswer >= 0 && (int)correctAnswer <= 3;
            correct = scored && answer == correctAnswer;
        }
    }

    // Runtime session data, never written back to the reusable authoring assets.
    [Serializable]
    public sealed class ParticipantSession
    {
        [SerializeField] private int orderNumber;
        [SerializeField] private int stimulusSet;
        [SerializeField] private bool preview;
        [SerializeField] private bool completed;
        [SerializeField] private List<string> plannedScenarioIds = new List<string>();
        [SerializeField] private List<FeedbackCondition> plannedConditions = new List<FeedbackCondition>();
        [SerializeField] private List<TrialResponse> responses = new List<TrialResponse>();
        [SerializeField] private int correctCount;
        [SerializeField] private int scoredCount;

        public int OrderNumber => orderNumber;
        public int StimulusSet => stimulusSet;
        public bool IsPreview => preview;
        public bool Completed => completed;
        public int CorrectCount => correctCount;
        public int ScoredCount => scoredCount;
        public IReadOnlyList<TrialResponse> Responses => responses;

        public ParticipantSession(int order, int set, bool isPreview, IReadOnlyList<TrialAssignment> schedule)
        {
            orderNumber = order;
            stimulusSet = set;
            preview = isPreview;
            foreach (var trial in schedule)
            {
                plannedScenarioIds.Add(trial.Scenario.scenarioId);
                plannedConditions.Add(trial.Condition);
            }
        }

        public bool Record(TrialResponse response)
        {
            int index = responses.Count;
            if (response == null || completed || index >= plannedScenarioIds.Count ||
                response.ScenarioId != plannedScenarioIds[index] || response.Condition != plannedConditions[index])
                return false;
            responses.Add(response);
            if (response.Scored) scoredCount++;
            if (response.Correct) correctCount++;
            return true;
        }

        public void Complete()
        {
            if (responses.Count != plannedScenarioIds.Count)
                throw new InvalidOperationException("Cannot complete a session with unanswered trials.");
            completed = true;
        }
    }
}
