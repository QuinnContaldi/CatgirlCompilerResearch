using System;
using System.Collections.Generic;
using Meowra.Data;
using UnityEngine;
using UnityEngine.UI;

namespace Meowra.UI
{
    public sealed class StudyShellView : MonoBehaviour
    {
        [SerializeField] private Dropdown orderDropdown;
        [SerializeField] private Dropdown stimulusSetDropdown;
        [SerializeField] private Text setupStatus;
        [SerializeField] private Button startButton;
        [SerializeField] private Button previewButton;
        [SerializeField] private GameObject previewBanner;
        [SerializeField] private Text introduction;
        [SerializeField] private Image introductionPortrait;
        [SerializeField] private Text completion;

        private GameObject saveErrorPanel;
        public event Action SaveRetryRequested;

        public int OrderNumber => orderDropdown.value + 1;
        public int StimulusSet => stimulusSetDropdown.value + 1;

        public void Configure(List<string> orders)
        {
            orderDropdown.ClearOptions();
            orderDropdown.AddOptions(orders);
            stimulusSetDropdown.ClearOptions();
            stimulusSetDropdown.AddOptions(new List<string> { "1: Original pairs", "2: Rotate pairs once", "3: Rotate pairs twice" });
        }

        public void ShowSetup(StudyDefinition study)
        {
            string error = study == null ? "Assign a Study Definition on ExperimentManager." : study.GetValidationError();
            startButton.interactable = error == null;
            previewButton.interactable = study != null && study.GetValidationError(true) == null;
            setupStatus.text = error == null
                ? "Trial content is configured. Responses are saved locally after each submission."
                : $"Content needs attention: {error}\nUse Preview Layout to inspect the blank templates.";
            previewBanner.SetActive(false);
        }

        public void ShowSaveError()
        {
            if (saveErrorPanel == null)
            {
                var canvas = setupStatus.canvas.rootCanvas;
                saveErrorPanel = new GameObject("SaveErrorOverlay", typeof(RectTransform), typeof(Image));
                saveErrorPanel.transform.SetParent(canvas.transform, false);
                var rect = (RectTransform)saveErrorPanel.transform;
                rect.anchorMin = Vector2.zero;
                rect.anchorMax = Vector2.one;
                rect.offsetMin = rect.offsetMax = Vector2.zero;
                saveErrorPanel.GetComponent<Image>().color = new Color(.08f, .08f, .08f, 1);
                var messageObject = new GameObject("Message", typeof(RectTransform), typeof(Text));
                messageObject.transform.SetParent(rect, false);
                var message = messageObject.GetComponent<Text>();
                message.font = setupStatus.font;
                message.fontSize = 24;
                message.alignment = TextAnchor.MiddleCenter;
                message.text = "Unable to save. Please notify the researcher.\nYour response is held in memory. Keep this app open.\nRetry after restoring access to the save folder.";
                message.rectTransform.sizeDelta = new Vector2(760, 200);
                message.rectTransform.anchoredPosition = new Vector2(0, 70);
                var button = Instantiate(startButton, rect);
                button.name = "RetrySaveButton";
                button.onClick = new Button.ButtonClickedEvent();
                button.onClick.AddListener(() => SaveRetryRequested?.Invoke());
                button.interactable = true;
                button.GetComponentInChildren<Text>().text = "Retry saving";
                var buttonRect = (RectTransform)button.transform;
                buttonRect.anchorMin = buttonRect.anchorMax = new Vector2(.5f, .5f);
                buttonRect.pivot = new Vector2(.5f, .5f);
                buttonRect.sizeDelta = new Vector2(260, 60);
                buttonRect.anchoredPosition = new Vector2(0, -100);
            }
            saveErrorPanel.SetActive(true);
            saveErrorPanel.transform.SetAsLastSibling();
            UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null);
        }

        public void HideSaveError()
        {
            if (saveErrorPanel != null) saveErrorPanel.SetActive(false);
        }

        public void ShowSession(bool preview) => previewBanner.SetActive(preview);

        public void ShowIntroduction(StudyDefinition study)
        {
            introduction.text = string.IsNullOrWhiteSpace(study.meowraIntroduction)
                ? "[Your Dr. Meowra introduction text]" : study.meowraIntroduction;
            introductionPortrait.sprite = study.meowraPortrait;
            introductionPortrait.gameObject.SetActive(study.meowraPortrait != null);
        }

        public void ShowCompletion(bool preview)
        {
            completion.text = preview
                ? "Layout preview complete. Preview answers are not scored."
                : "Study complete. Thank you! Please notify the researcher.";
        }
    }
}
