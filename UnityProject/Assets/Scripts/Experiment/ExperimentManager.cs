using System;
using System.IO;
using System.Collections.Generic;
using Meowra.Data;
using Meowra.UI;
using UnityEngine;

namespace Meowra.Experiment
{
    // Api is a reserved historical value, never entered by the current flow.
    public enum ExperimentStage { Setup, Welcome, Instructions, Introduction, Trial, Evaluation, Complete, Api, Preference, OpenResponse, HostIntroduction, Background, Transition, BlockCompletion }

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
        private ConsentView consent;
        private UeqsView evaluation;
        private FinalMeasuresView background;
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
            logger = new DataLogger(StudyResults.RootDirectory);
            view.SaveRetryRequested += RetrySave;
            trials.Submitted += OnTrialSubmitted;
            var orders = new List<string>();
            for (int i = 1; i <= Counterbalancing.CellCount; i++) orders.Add(Counterbalancing.GetOrderLabel(i));
            consent = welcomePage.AddComponent<ConsentView>();
            consent.Build(study);
            evaluation = evaluationPage.AddComponent<UeqsView>();
            evaluation.Build(ContinueEvaluation, study);
            background = FinalMeasuresView.Create(evaluationPage, "BackgroundPage", ContinueBackground, study);
            background.BuildReason(study.backgroundPrompt);
            preference = FinalMeasuresView.Create(evaluationPage, "PreferencePage", ContinuePreference, study);
            preference.BuildPreference();
            openResponse = FinalMeasuresView.Create(evaluationPage, "OpenResponsePage", ContinueOpenResponse, study);
            openResponse.BuildReason(study.reasonPrompt);
            pages.RegisterPage(background.gameObject);
            pages.RegisterPage(preference.gameObject);
            pages.RegisterPage(openResponse.gameObject);
            view.BuildHost(welcomePage.transform.parent);
            view.Configure(orders);
            view.BuildResultsAccess(() => OpenSavedData(false), () => OpenSavedData(true));
            ShowSetup();
        }

        private void OpenSavedData(bool latestParticipant)
        {
            if (stage != ExperimentStage.Setup) return;
            view.ShowResultsStatus(StudyResults.OpenSavedData(logger.RootDirectory, latestParticipant));
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
            schedule = Counterbalancing.BuildSchedule(study, view.OrderNumber);
            session = new ParticipantSession(view.OrderNumber, preview, schedule);
            session.SetStudyVersion(study.studyVersion);
            trialIndex = 0;
            view.ShowSession(preview);
            sessionDirectory = logger.GetSessionDirectory(session);
            consent.Show(study.consentText);
            SaveThen(() =>
            {
                Debug.Log($"{(preview ? "Preview" : "Live survey")} files: {sessionDirectory}");
                Navigate(ExperimentStage.Welcome, welcomePage);
            });
        }

        public void ContinueWelcome()
        {
            if (SavePending || stage != ExperimentStage.Welcome) return;
            if (!session.AcceptConsent(consent.DisplayedText)) return;
            SaveThen(() =>
            {
                view.ShowPersonaMessage(study.overview, study.overviewHeading);
                Navigate(ExperimentStage.HostIntroduction, introductionPage);
            });
        }

        public void ContinueInstructions()
        {
            if (!SavePending && stage == ExperimentStage.Instructions) ShowTrial();
        }

        public void ContinueBackground()
        {
            if (SavePending || stage != ExperimentStage.Background || !session.RecordBackground(background.Reason)) return;
            SaveThen(BeginBlock);
        }

        private void BeginBlock()
        {
            bool meowra = CurrentTrial.Condition == FeedbackCondition.Meowra;
            view.ShowPersonaMessage(meowra ? study.meowraBlockIntroduction : study.neutralIntroduction,
                meowra ? study.meowraName : study.introductionHeading);
            Navigate(ExperimentStage.Introduction, introductionPage);
        }

        public void ContinueIntroduction()
        {
            if (SavePending) return;
            if (stage == ExperimentStage.HostIntroduction)
            {
                background.Begin();
                Navigate(ExperimentStage.Background, background.gameObject);
            }
            else if (stage == ExperimentStage.Introduction)
            {
                view.ShowPersonaMessage(CurrentTrial.Condition == FeedbackCondition.Meowra ? study.meowraTutorial : study.neutralTutorial, study.tutorialHeading);
                Navigate(ExperimentStage.Instructions, introductionPage);
            }
            else if (stage == ExperimentStage.Instructions || stage == ExperimentStage.Transition) ShowTrial();
            else if (stage == ExperimentStage.BlockCompletion)
            {
                evaluation.Begin();
                Navigate(ExperimentStage.Evaluation, evaluationPage);
            }
        }

        private void ShowTrial()
        {
            Navigate(ExperimentStage.Trial, trialPage);
            trials.Begin(CurrentTrial, session.IsPreview, study, trialIndex % 4 + 1, 4);
        }

        private void OnTrialSubmitted(TrialResponse response)
        {
            if (SavePending || stage != ExperimentStage.Trial || !session.Record(response)) return;

            SaveThen(AdvanceAfterTrial);
        }

        private void AdvanceAfterTrial()
        {
            bool meowra = CurrentTrial.Condition == FeedbackCondition.Meowra;
            trialIndex++;
            bool complete = trialIndex % 4 == 0;
            string message = complete
                ? (meowra ? study.meowraCompletion : study.neutralCompletion)
                : (meowra ? study.meowraTransitions : study.neutralTransitions)[trialIndex % 4 - 1];
            view.ShowPersonaMessage(message, complete ? study.completionHeading : study.progressHeading);
            Navigate(complete ? ExperimentStage.BlockCompletion : ExperimentStage.Transition, introductionPage);
        }

        public void ContinueEvaluation()
        {
            if (SavePending || stage != ExperimentStage.Evaluation || !evaluation.IsComplete) return;
            var response = new UeqsResponse(trialIndex / 4, schedule[trialIndex - 1].Condition, evaluation.CopyPositions());
            if (!session.Record(response)) return;
            SaveThen(AdvanceAfterEvaluation);
        }

        private void AdvanceAfterEvaluation()
        {
            if (trialIndex < schedule.Count) BeginBlock();
            else
            {
                preference.Begin();
                Navigate(ExperimentStage.Preference, preference.gameObject);
            }
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
                view.ShowCompletion(session.IsPreview, study);
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
                view.ShowSaveError(study);
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
            bool treatment = nextStage == ExperimentStage.Introduction || nextStage == ExperimentStage.Instructions ||
                nextStage == ExperimentStage.Trial || nextStage == ExperimentStage.Transition || nextStage == ExperimentStage.BlockCompletion;
            int index = nextStage == ExperimentStage.Transition || nextStage == ExperimentStage.BlockCompletion ? trialIndex - 1 : trialIndex;
            bool meowra = treatment && schedule[index].Condition == FeedbackCondition.Meowra;
            string dialogue = treatment ? study.taskHeading : study.studyHeading;
            if (meowra)
            {
                switch (nextStage)
                {
                    case ExperimentStage.Introduction: dialogue = study.meowraIntroductionDialogue; break;
                    case ExperimentStage.Instructions: dialogue = study.meowraTutorialDialogue; break;
                    case ExperimentStage.Trial: dialogue = study.meowraTaskDialogue[trialIndex % 4]; break;
                    case ExperimentStage.Transition: dialogue = study.meowraTransitions[index % 4]; break;
                    case ExperimentStage.BlockCompletion: dialogue = study.meowraCompletionDialogue; break;
                }
            }
            view.ShowHost(study, dialogue, meowra);
        }
    }
}
