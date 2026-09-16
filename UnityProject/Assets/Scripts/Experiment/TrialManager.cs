using System;
using Meowra.Data;
using Meowra.UI;
using UnityEngine;

namespace Meowra.Experiment
{
    public sealed class TrialManager : MonoBehaviour
    {
        [SerializeField] private TrialView view;
        private TrialAssignment current;
        private AnswerChoice selected = AnswerChoice.Unassigned;
        private bool preview;
        private bool submitted;
        public event Action<TrialResponse> Submitted;

        private void Awake()
        {
            view.AnswerSelected += SelectAnswer;
            view.SubmitRequested += Submit;
        }

        private void OnDestroy()
        {
            if (view == null) return;
            view.AnswerSelected -= SelectAnswer;
            view.SubmitRequested -= Submit;
        }

        public void Begin(TrialAssignment trial, bool isPreview, Sprite portrait, int number, int total)
        {
            current = trial;
            preview = isPreview;
            selected = AnswerChoice.Unassigned;
            submitted = false;
            view.Show(trial.Scenario, trial.Condition, portrait, number, total);
        }

        public void SelectAnswer(AnswerChoice answer)
        {
            if (current == null || submitted || (int)answer < -1 || (int)answer > 3) return;
            selected = answer;
            view.AllowSubmit(answer != AnswerChoice.Unassigned);
        }

        public void Submit()
        {
            if (current == null || submitted || selected == AnswerChoice.Unassigned) return;
            submitted = true;
            view.Lock();
            Submitted?.Invoke(new TrialResponse(current.Scenario, current.Condition, selected, preview));
        }
    }
}
