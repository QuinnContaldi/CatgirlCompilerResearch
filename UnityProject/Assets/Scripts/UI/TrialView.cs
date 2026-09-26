using System;
using Meowra.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Meowra.UI
{
    public sealed class TrialView : MonoBehaviour
    {
        [SerializeField] private Text progress;
        [SerializeField] private TMP_Text codeText;
        [SerializeField] private TMP_Text explanation;
        [SerializeField] private Text question;
        [SerializeField] private Image portrait;
        [SerializeField] private Toggle[] answers;
        [SerializeField] private Text[] answerLabels;
        [SerializeField] private Button submitButton;
        [SerializeField] private ScrollRect scroll;
        [SerializeField] private Color answerBackground = new Color(.9f, .93f, .97f);
        [SerializeField] private Color selectedAnswerBackground = new Color(.72f, .84f, .94f);
        public event Action<AnswerChoice> AnswerSelected;
        public event Action SubmitRequested;

        private void Awake()
        {
            for (int i = 0; i < answers.Length; i++)
            {
                int index = i;
                answers[i].onValueChanged.AddListener(selected =>
                {
                    UpdateAnswerAppearance(index);
                    if (selected) AnswerSelected?.Invoke((AnswerChoice)index);
                    else if (!answers[index].group.AnyTogglesOn())
                        AnswerSelected?.Invoke(AnswerChoice.Unassigned);
                });
            }
            submitButton.onClick.AddListener(() => SubmitRequested?.Invoke());
        }

        public void Show(ScenarioData scenario, FeedbackCondition condition, StudyDefinition study, int number, int total)
        {
            submitButton.GetComponentInChildren<Text>().text = study.taskSubmitLabel;
            progress.text = string.Format(study.taskProgressFormat, number, total);
            codeText.text = StudyTextFormatter.Code(scenario.codeText);
            // This protocol tests interface framing, without technical feedback or hints.
            explanation.transform.parent.gameObject.SetActive(false);
            portrait.transform.parent.gameObject.SetActive(false);
            question.text = scenario.question;
            portrait.sprite = study.meowraPortrait;
            // The shared host controls portrait visibility within the Meowra block.
            portrait.gameObject.SetActive(false);
            var group = answers[0].group;
            group.allowSwitchOff = true;
            for (int i = 0; i < answers.Length; i++)
            {
                answers[i].SetIsOnWithoutNotify(false);
                UpdateAnswerAppearance(i);
                answers[i].interactable = true;
                answerLabels[i].text = $"{(AnswerChoice)i}. {scenario.GetAnswer((AnswerChoice)i)}";
            }
            submitButton.interactable = false;
            Canvas.ForceUpdateCanvases();
            scroll.verticalNormalizedPosition = 1;
        }

        public void AllowSubmit(bool allowed) => submitButton.interactable = allowed;

        private void UpdateAnswerAppearance(int index)
        {
            // Retain the toggle marker as well as color to make selection identifiable.
            answers[index].targetGraphic.color = answers[index].isOn
                ? selectedAnswerBackground
                : answerBackground;
        }

        public void Lock()
        {
            submitButton.interactable = false;
            foreach (var answer in answers) answer.interactable = false;
        }

    }
}
