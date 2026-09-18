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
        [SerializeField] private double responseTimeSeconds;

        public string ScenarioId => scenarioId;
        public FeedbackCondition Condition => condition;
        public AnswerChoice SelectedAnswer => selectedAnswer;
        public bool Scored => scored;
        public bool Correct => correct;
        public double ResponseTimeSeconds => responseTimeSeconds;

        public TrialResponse(ScenarioData scenario, FeedbackCondition feedbackCondition, AnswerChoice answer, bool preview, double elapsedSeconds = 0)
        {
            if (scenario == null) throw new ArgumentNullException(nameof(scenario));
            if ((int)answer < 0 || (int)answer > 3) throw new ArgumentOutOfRangeException(nameof(answer));
            if (double.IsNaN(elapsedSeconds) || double.IsInfinity(elapsedSeconds) || elapsedSeconds < 0)
                throw new ArgumentOutOfRangeException(nameof(elapsedSeconds));
            responseTimeSeconds = elapsedSeconds;
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
        [SerializeField] private int schemaVersion = 3;
        [SerializeField] private string participantId;
        [SerializeField] private string startedUtc;
        [SerializeField] private string timingDefinition = "Scenario displayed to Submit; includes time away from app; seconds.";
        [SerializeField] private int orderNumber;
        [SerializeField] private int stimulusSet;
        [SerializeField] private bool preview;
        [SerializeField] private bool completed;
        [SerializeField] private List<string> plannedScenarioIds = new List<string>();
        [SerializeField] private List<FeedbackCondition> plannedConditions = new List<FeedbackCondition>();
        [SerializeField] private List<TrialResponse> responses = new List<TrialResponse>();
        [SerializeField] private List<UeqsResponse> ueqsResponses = new List<UeqsResponse>();
        [SerializeField] private ApiResponse apiResponse;
        [SerializeField] private bool apiSubmitted;
        [SerializeField] private FeedbackCondition preferredCondition;
        [SerializeField] private bool preferenceSubmitted;
        [SerializeField] private string preferenceSubmittedUtc;
        [SerializeField] private string preferenceReason;
        [SerializeField] private bool reasonSubmitted;
        [SerializeField] private string reasonSubmittedUtc;
        public ApiResponse ApiResponse => apiResponse;
        public bool ApiSubmitted => apiSubmitted;
        public FeedbackCondition PreferredCondition => preferredCondition;
        public bool PreferenceSubmitted => preferenceSubmitted;
        public string PreferenceReason => preferenceReason;
        public bool ReasonSubmitted => reasonSubmitted;
        public IReadOnlyList<UeqsResponse> UeqsResponses => ueqsResponses;
        [SerializeField] private int correctCount;
        [SerializeField] private int scoredCount;

        public string ParticipantId => participantId;
        public int OrderNumber => orderNumber;
        public int StimulusSet => stimulusSet;
        public bool IsPreview => preview;
        public bool Completed => completed;
        public int CorrectCount => correctCount;
        public int ScoredCount => scoredCount;
        public IReadOnlyList<TrialResponse> Responses => responses;

        public ParticipantSession(int order, int set, bool isPreview, IReadOnlyList<TrialAssignment> schedule)
        {
            participantId = Guid.NewGuid().ToString("N");
            startedUtc = DateTime.UtcNow.ToString("O");
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
            if (response == null || completed || index >= plannedScenarioIds.Count || index / 2 != ueqsResponses.Count ||
                response.ScenarioId != plannedScenarioIds[index] || response.Condition != plannedConditions[index])
                return false;
            responses.Add(response);
            if (response.Scored) scoredCount++;
            if (response.Correct) correctCount++;
            return true;
        }

        public bool Record(UeqsResponse response)
        {
            int block = ueqsResponses.Count + 1;
            if (response == null || completed || block > 3 || responses.Count != block * 2 ||
                response.BlockNumber != block || response.Condition != plannedConditions[block * 2 - 1])
                return false;
            ueqsResponses.Add(response);
            return true;
        }

        public bool Record(ApiResponse response)
        {
            if (response == null || completed || apiSubmitted || ueqsResponses.Count != 3) return false;
            apiResponse = response;
            apiSubmitted = true;
            return true;
        }

        public bool RecordPreference(FeedbackCondition condition)
        {
            if (completed || !apiSubmitted || preferenceSubmitted || !Enum.IsDefined(typeof(FeedbackCondition), condition)) return false;
            preferredCondition = condition;
            preferenceSubmitted = true;
            preferenceSubmittedUtc = DateTime.UtcNow.ToString("O");
            return true;
        }

        public bool RecordReason(string reason)
        {
            if (completed || !preferenceSubmitted || reasonSubmitted || reason == null) return false;
            preferenceReason = reason;
            reasonSubmitted = true;
            reasonSubmittedUtc = DateTime.UtcNow.ToString("O");
            return true;
        }

        public void Complete()
        {
            if (responses.Count != plannedScenarioIds.Count || ueqsResponses.Count != 3 || !apiSubmitted || !preferenceSubmitted || !reasonSubmitted)
                throw new InvalidOperationException("Cannot complete a session with unsubmitted trials, evaluations, or final measures.");
            completed = true;
        }
    }
}
