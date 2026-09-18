using System;
using System.IO;
using System.Collections.Generic;
using Meowra.Data;
using Meowra.UI;
using UnityEngine;

namespace Meowra.Experiment
{
    public enum ExperimentStage { Setup, Welcome, Instructions, Introduction, Trial, Evaluation, Complete, Api, Preference, OpenResponse }

    public sealed class ExperimentManager : MonoBehaviour
    {
        [SerializeField] private StudyDefinition study;
        [SerializeField] private PageManager pages;
        [SerializeField] private TrialManager trials;
        [SerializeField] private StudyShellView view;
        [Header("Pages")]
        [SerializeField] private GameObject setupPage;
        [SerializeField] private GameObject welcomePage;
        [SerializeField] private GameObject instructionsPage;
        [SerializeField] private GameObject introductionPage;
        [SerializeField] private GameObject trialPage;
        [SerializeField] private GameObject evaluationPage;
        [SerializeField] private GameObject completionPage;
        [Header("Runtime state — inspect while playing")]
        [SerializeField] private ExperimentStage stage;
        [SerializeField] private ParticipantSession session;
        [SerializeField] private int trialIndex;
        private List<TrialAssignment> schedule;
        private DataLogger logger;
        private UeqsView evaluation;
        private FinalMeasuresView api;
        private FinalMeasuresView preference;
        private FinalMeasuresView openResponse;
        private Action afterSave;
        [SerializeField] private string sessionDirectory;
        public string SessionDirectory => sessionDirectory;
        public bool SavePending => afterSave != null;

        public ExperimentStage Stage => stage;
        public ParticipantSession Session => session;
        public TrialAssignment CurrentTrial => schedule != null && trialIndex < schedule.Count ? schedule[trialIndex] : null;

        private void Start()
        {
            logger = new DataLogger(Path.Combine(Application.persistentDataPath, "StudySessions"));
            view.SaveRetryRequested += RetrySave;
            trials.Submitted += OnTrialSubmitted;
            var orders = new List<string>();
            for (int i = 1; i <= 6; i++) orders.Add(Counterbalancing.GetOrderLabel(i));
            evaluation = evaluationPage.AddComponent<UeqsView>();
            evaluation.Build(ContinueEvaluation);
            api = FinalMeasuresView.Create(evaluationPage, "ApiPage", ContinueApi);
            api.BuildApi("1 = Strongly disagree    2 = Disagree    3 = Neutral    4 = Agree    5 = Strongly agree");
            preference = FinalMeasuresView.Create(evaluationPage, "PreferencePage", ContinuePreference);
            preference.BuildPreference();
            openResponse = FinalMeasuresView.Create(evaluationPage, "OpenResponsePage", ContinueOpenResponse);
            openResponse.BuildReason();
            pages.RegisterPage(api.gameObject);
            pages.RegisterPage(preference.gameObject);
            pages.RegisterPage(openResponse.gameObject);
            view.Configure(orders);
            ShowSetup();
        }

        private void OnDestroy()
        {
            if (trials != null) trials.Submitted -= OnTrialSubmitted;
            if (view != null) view.SaveRetryRequested -= RetrySave;
        }

        public void StartSession() => BeginSession(false);
        public void PreviewLayout() => BeginSession(true);

        private void BeginSession(bool preview)
        {
            if (SavePending || stage != ExperimentStage.Setup) return;
            if (study == null || study.GetValidationError(preview) != null)
            {
                view.ShowSetup(study);
                return;
            }
            schedule = Counterbalancing.BuildSchedule(study, view.OrderNumber, view.StimulusSet);
            session = new ParticipantSession(view.OrderNumber, view.StimulusSet, preview, schedule);
            trialIndex = 0;
            view.ShowSession(preview);
            sessionDirectory = logger.GetSessionDirectory(session);
            SaveThen(() => Navigate(ExperimentStage.Welcome, welcomePage));
        }

        public void ContinueWelcome()
        {
            if (!SavePending && stage == ExperimentStage.Welcome) Navigate(ExperimentStage.Instructions, instructionsPage);
        }

        public void ContinueInstructions()
        {
            if (!SavePending && stage == ExperimentStage.Instructions) BeginBlock();
        }

        private void BeginBlock()
        {
            if (CurrentTrial.Condition == FeedbackCondition.Meowra)
            {
                view.ShowIntroduction(study);
                Navigate(ExperimentStage.Introduction, introductionPage);
            }
            else ShowTrial();
        }

        public void ContinueIntroduction()
        {
            if (!SavePending && stage == ExperimentStage.Introduction) ShowTrial();
        }

        private void ShowTrial()
        {
            Navigate(ExperimentStage.Trial, trialPage);
            trials.Begin(CurrentTrial, session.IsPreview, study.meowraPortrait, trialIndex + 1, schedule.Count);
        }

        private void OnTrialSubmitted(TrialResponse response)
        {
            if (SavePending || stage != ExperimentStage.Trial || !session.Record(response)) return;
            SaveThen(AdvanceAfterTrial);
        }

        private void AdvanceAfterTrial()
        {
            trialIndex++;
            if (trialIndex % 2 == 0)
            {
                evaluation.Begin();
                Navigate(ExperimentStage.Evaluation, evaluationPage);
            }
            else ShowTrial();
        }

        public void ContinueEvaluation()
        {
            if (SavePending || stage != ExperimentStage.Evaluation || !evaluation.IsComplete) return;
            var response = new UeqsResponse(trialIndex / 2, schedule[trialIndex - 1].Condition, evaluation.CopyPositions());
            if (!session.Record(response)) return;
            SaveThen(AdvanceAfterEvaluation);
        }

        private void AdvanceAfterEvaluation()
        {
            if (trialIndex < schedule.Count) BeginBlock();
            else
            {
                api.Begin();
                Navigate(ExperimentStage.Api, api.gameObject);
            }
        }

        public void ContinueApi()
        {
            if (SavePending || stage != ExperimentStage.Api || !api.ApiComplete) return;
            if (!session.Record(new ApiResponse(api.CopyRatings()))) return;
            SaveThen(() =>
            {
                preference.Begin();
                Navigate(ExperimentStage.Preference, preference.gameObject);
            });
        }

        public void ContinuePreference()
        {
            if (SavePending || stage != ExperimentStage.Preference || !preference.HasPreference) return;
            if (!session.RecordPreference(preference.PreferredCondition)) return;
            SaveThen(() =>
            {
                openResponse.Begin();
                Navigate(ExperimentStage.OpenResponse, openResponse.gameObject);
            });
        }

        public void ContinueOpenResponse()
        {
            if (SavePending || stage != ExperimentStage.OpenResponse) return;
            if (!session.RecordReason(openResponse.Reason)) return;
            session.Complete();
            SaveThen(() =>
            {
                view.ShowCompletion(session.IsPreview);
                Navigate(ExperimentStage.Complete, completionPage);
            });
        }

        public void ReturnToSetup()
        {
            if (!SavePending && (stage == ExperimentStage.Complete || stage == ExperimentStage.Welcome)) ShowSetup();
        }

        private void SaveThen(Action continuation)
        {
            afterSave = continuation;
            RetrySave();
        }

        public void RetrySave()
        {
            if (!SavePending) return;
            try
            {
                logger.Save(session);
            }
            catch (Exception error) when (error is IOException || error is UnauthorizedAccessException || error is System.Security.SecurityException)
            {
                view.ShowSaveError();
                Debug.LogWarning($"Session save failed at {sessionDirectory}: {error.Message}");
                return;
            }
            view.HideSaveError();
            var continuation = afterSave;
            afterSave = null;
            continuation();
        }

        private void ShowSetup()
        {
            // Retain the last completed session for inspection until a new one starts.
            view.ShowSetup(study);
            Navigate(ExperimentStage.Setup, setupPage);
        }

        private void Navigate(ExperimentStage nextStage, GameObject page)
        {
            stage = nextStage;
            pages.ShowPage(page);
        }
    }
}
