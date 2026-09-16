using System;
using Meowra.Data;
using UnityEngine;
using UnityEngine.UI;

namespace Meowra.UI
{
    public sealed class TrialView : MonoBehaviour
    {
        [SerializeField] private Text progress;
        [SerializeField] private Image codeImage;
        [SerializeField] private GameObject codePlaceholder;
        [SerializeField] private Text explanation;
        [SerializeField] private Text question;
        [SerializeField] private Image portrait;
        [SerializeField] private Toggle[] answers;
        [SerializeField] private Text[] answerLabels;
        [SerializeField] private Button submitButton;
        [SerializeField] private ScrollRect scroll;
        public event Action<AnswerChoice> AnswerSelected;
        public event Action SubmitRequested;

        private void Awake()
        {
            for (int i = 0; i < answers.Length; i++)
            {
                int index = i;
                answers[i].onValueChanged.AddListener(selected =>
                {
                    if (selected) AnswerSelected?.Invoke((AnswerChoice)index);
                    else if (!answers[index].group.AnyTogglesOn())
                        AnswerSelected?.Invoke(AnswerChoice.Unassigned);
                });
            }
            submitButton.onClick.AddListener(() => SubmitRequested?.Invoke());
        }

        public void Show(ScenarioData scenario, FeedbackCondition condition, Sprite persona, int number, int total)
        {
            progress.text = $"Question {number} of {total}";
            codeImage.sprite = scenario.codeImage;
            codeImage.preserveAspect = true;
            codeImage.enabled = scenario.codeImage != null;
            codePlaceholder.SetActive(scenario.codeImage == null);
            explanation.text = ContentOrPlaceholder(scenario.GetFeedback(condition), "[Explanation]");
            question.text = ContentOrPlaceholder(scenario.question, "[Question prompt]");
            portrait.sprite = persona;
            portrait.gameObject.SetActive(condition == FeedbackCondition.Meowra && persona != null);
            var group = answers[0].group;
            group.allowSwitchOff = true;
            for (int i = 0; i < answers.Length; i++)
            {
                answers[i].SetIsOnWithoutNotify(false);
                answers[i].interactable = true;
                answerLabels[i].text = $"{(AnswerChoice)i}. {ContentOrPlaceholder(scenario.GetAnswer((AnswerChoice)i), "[Answer text]")}";
            }
            submitButton.interactable = false;
            Canvas.ForceUpdateCanvases();
            scroll.verticalNormalizedPosition = 1;
        }

        public void AllowSubmit(bool allowed) => submitButton.interactable = allowed;

        public void Lock()
        {
            submitButton.interactable = false;
            foreach (var answer in answers) answer.interactable = false;
        }

        private static string ContentOrPlaceholder(string content, string placeholder)
        {
            return string.IsNullOrWhiteSpace(content) ? placeholder : content;
        }
    }
}
