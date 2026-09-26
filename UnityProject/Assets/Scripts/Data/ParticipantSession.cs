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

        [SerializeField] private string encouragement;
        [SerializeField] private string taskSet;
        [SerializeField] private string errorCategory;
        public string TaskSet => taskSet;
        public string ErrorCategory => errorCategory;
        public AnswerChoice CorrectAnswer => correctAnswer;
        public string Encouragement => encouragement;
        public void SetEncouragement(string message) => encouragement = message;

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
            taskSet = scenario.taskSet;
            errorCategory = scenario.errorCategory;
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
        [SerializeField] private int schemaVersion = 6;
        [SerializeField] private string personaProtocol = "two-condition-v1; Meowra only during treatment; no technical feedback; neutral evaluations; untimed transitions";
        [SerializeField] private bool consentAccepted;
        [SerializeField] private string consentAcceptedUtc;
        [SerializeField] private string consentText;
        public bool ConsentAccepted => consentAccepted;
        public string ConsentText => consentText;
        public string ConsentAcceptedUtc => consentAcceptedUtc;
        [SerializeField] private string participantId;
        [SerializeField] private string startedUtc;
        [SerializeField] private string timingDefinition = "Scenario displayed to Submit; includes time away from app; seconds.";
        [SerializeField] private int orderNumber;
        [SerializeField] private int assignmentCell;
        [SerializeField] private string studyVersion;
        [SerializeField] private string unityVersion = Application.unityVersion;
        [SerializeField] private string endedUtc;
        [SerializeField] private string programmingBackground;
        [SerializeField] private bool backgroundSubmitted;
        [SerializeField] private List<string> plannedTaskSets = new List<string>();
        public IReadOnlyList<string> PlannedTaskSets => plannedTaskSets;
        public int AssignmentCell => assignmentCell;
        public void SetStudyVersion(string version) => studyVersion = version;
        public bool RecordBackground(string background)
        {
            if (!consentAccepted || backgroundSubmitted || completed) return false;
            programmingBackground = background ?? "";
            backgroundSubmitted = true;
            return true;
        }
        [SerializeField] private bool preview;
        [SerializeField] private bool completed;
        [SerializeField] private List<string> plannedScenarioIds = new List<string>();
        [SerializeField] private List<FeedbackCondition> plannedConditions = new List<FeedbackCondition>();
        [SerializeField] private List<TrialResponse> responses = new List<TrialResponse>();
        [SerializeField] private List<UeqsResponse> ueqsResponses = new List<UeqsResponse>();
        [SerializeField] private FeedbackCondition preferredCondition;
        [SerializeField] private bool preferenceSubmitted;
        [SerializeField] private string preferenceSubmittedUtc;
        [SerializeField] private string preferenceReason;
        [SerializeField] private bool reasonSubmitted;
        [SerializeField] private string reasonSubmittedUtc;
        public FeedbackCondition PreferredCondition => preferredCondition;
        public bool PreferenceSubmitted => preferenceSubmitted;
        public string PreferenceReason => preferenceReason;
        public bool ReasonSubmitted => reasonSubmitted;
        public IReadOnlyList<UeqsResponse> UeqsResponses => ueqsResponses;
        [SerializeField] private int correctCount;
        [SerializeField] private int scoredCount;

        public string ParticipantId => participantId;
        public int OrderNumber => orderNumber;
        public bool IsPreview => preview;
        public bool Completed => completed;
        public int CorrectCount => correctCount;
        public int ScoredCount => scoredCount;
        public IReadOnlyList<TrialResponse> Responses => responses;
        public IReadOnlyList<string> PlannedScenarioIds => plannedScenarioIds;
        public IReadOnlyList<FeedbackCondition> PlannedConditions => plannedConditions;

        public ParticipantSession(int order, bool isPreview, IReadOnlyList<TrialAssignment> schedule)
        {
            participantId = Guid.NewGuid().ToString("N");
            startedUtc = DateTime.UtcNow.ToString("O");
            orderNumber = order;
            assignmentCell = order;
            preview = isPreview;
            foreach (var trial in schedule)
            {
                plannedScenarioIds.Add(trial.Scenario.scenarioId);
                plannedTaskSets.Add(trial.Scenario.taskSet);
                plannedConditions.Add(trial.Condition);
            }
        }

        public bool AcceptConsent(string displayedText)
        {
            if (consentAccepted || completed || string.IsNullOrWhiteSpace(displayedText)) return false;
            consentText = displayedText;
            consentAcceptedUtc = DateTime.UtcNow.ToString("O");
            consentAccepted = true;
            return true;
        }

        public bool Record(TrialResponse response)
        {
            int index = responses.Count;
            if (response == null || completed || index >= plannedScenarioIds.Count || index / 4 != ueqsResponses.Count ||
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
            if (response == null || completed || block > 2 || responses.Count != block * 4 ||
                response.BlockNumber != block || response.Condition != plannedConditions[block * 4 - 1])
                return false;
            ueqsResponses.Add(response);
            return true;
        }

        public bool RecordPreference(FeedbackCondition condition)
        {
            if (completed || ueqsResponses.Count != 2 || preferenceSubmitted || (condition != FeedbackCondition.Neutral && condition != FeedbackCondition.Meowra)) return false;
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
            if (responses.Count != plannedScenarioIds.Count || ueqsResponses.Count != 2 || !preferenceSubmitted || !reasonSubmitted)
                throw new InvalidOperationException("Cannot complete a session with unsubmitted trials, evaluations, or final measures.");
            completed = true;
            endedUtc = DateTime.UtcNow.ToString("O");
        }
    }
}
