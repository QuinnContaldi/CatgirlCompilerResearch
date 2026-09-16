using System.Collections.Generic;
using Meowra.Data;
using Meowra.UI;
using UnityEngine;

namespace Meowra.Experiment
{
    public enum ExperimentStage { Setup, Welcome, Instructions, Introduction, Trial, Evaluation, Complete }

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

        public ExperimentStage Stage => stage;
        public ParticipantSession Session => session;
        public TrialAssignment CurrentTrial => schedule != null && trialIndex < schedule.Count ? schedule[trialIndex] : null;

        private void Start()
        {
            trials.Submitted += OnTrialSubmitted;
            var orders = new List<string>();
            for (int i = 1; i <= 6; i++) orders.Add(Counterbalancing.GetOrderLabel(i));
            view.Configure(orders);
            ShowSetup();
        }

        private void OnDestroy()
        {
            if (trials != null) trials.Submitted -= OnTrialSubmitted;
        }

        public void StartSession() => BeginSession(false);
        public void PreviewLayout() => BeginSession(true);

        private void BeginSession(bool preview)
        {
            if (stage != ExperimentStage.Setup) return;
            if (study == null || study.GetValidationError(preview) != null)
            {
                view.ShowSetup(study);
                return;
            }
            schedule = Counterbalancing.BuildSchedule(study, view.OrderNumber, view.StimulusSet);
            session = new ParticipantSession(view.OrderNumber, view.StimulusSet, preview, schedule);
            trialIndex = 0;
            view.ShowSession(preview);
            Navigate(ExperimentStage.Welcome, welcomePage);
        }

        public void ContinueWelcome()
        {
            if (stage == ExperimentStage.Welcome) Navigate(ExperimentStage.Instructions, instructionsPage);
        }

        public void ContinueInstructions()
        {
            if (stage == ExperimentStage.Instructions) BeginBlock();
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
            if (stage == ExperimentStage.Introduction) ShowTrial();
        }

        private void ShowTrial()
        {
            Navigate(ExperimentStage.Trial, trialPage);
            trials.Begin(CurrentTrial, session.IsPreview, study.meowraPortrait, trialIndex + 1, schedule.Count);
        }

        private void OnTrialSubmitted(TrialResponse response)
        {
            if (stage != ExperimentStage.Trial || !session.Record(response)) return;
            trialIndex++;
            // Preserve an insertion point for a later condition evaluation screen.
            if (trialIndex % 2 == 0) Navigate(ExperimentStage.Evaluation, evaluationPage);
            else ShowTrial();
        }

        public void ContinueEvaluation()
        {
            if (stage != ExperimentStage.Evaluation) return;
            if (trialIndex < schedule.Count) BeginBlock();
            else
            {
                session.Complete();
                view.ShowCompletion(session.IsPreview);
                Navigate(ExperimentStage.Complete, completionPage);
            }
        }

        public void ReturnToSetup()
        {
            if (stage == ExperimentStage.Complete || stage == ExperimentStage.Welcome) ShowSetup();
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
