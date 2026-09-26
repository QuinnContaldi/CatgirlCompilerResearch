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

        private Text hostDialogue;
        private Image hostPortrait;
                private RectTransform pagesRect;
        private Sprite headPortrait;
        private Sprite portraitSource;

        public void BuildHost(Transform pageContainer)
        {
            // Keep identical page geometry, whether the header includes Meowra or not.
            var canvas = setupStatus.canvas.rootCanvas.transform;
            var header = canvas.Find("Header");
            if (header != null) header.gameObject.SetActive(false);
            var pageRect = (RectTransform)pageContainer;
            pagesRect = pageRect;
            pageRect.anchorMax = new Vector2(pageRect.anchorMax.x, .77f);
            var banner = new GameObject("MeowraHost", typeof(RectTransform), typeof(Image));
            banner.transform.SetParent(canvas, false);
            var rect = (RectTransform)banner.transform;
            rect.anchorMin = new Vector2(.06f, .79f);
            rect.anchorMax = new Vector2(.94f, .94f);
            var previewRect = (RectTransform)previewBanner.transform;
            previewRect.anchorMin = new Vector2(.55f, .94f);
            previewRect.anchorMax = new Vector2(.96f, 1f);
            rect.offsetMin = rect.offsetMax = Vector2.zero;
            banner.GetComponent<Image>().color = new Color(.12f, .15f, .2f);
            var portrait = new GameObject("Portrait", typeof(RectTransform), typeof(Image));
            portrait.transform.SetParent(rect, false);
            hostPortrait = portrait.GetComponent<Image>();
            hostPortrait.preserveAspect = true;
            hostPortrait.raycastTarget = false;
            hostPortrait.rectTransform.anchorMin = Vector2.zero;
            hostPortrait.rectTransform.anchorMax = new Vector2(.18f, 1);
            hostPortrait.rectTransform.offsetMin = new Vector2(8, 8);
            hostPortrait.rectTransform.offsetMax = new Vector2(-8, -8);
            var dialogue = new GameObject("Dialogue", typeof(RectTransform), typeof(Text));
            dialogue.transform.SetParent(rect, false);
            hostDialogue = dialogue.GetComponent<Text>();
            hostDialogue.font = setupStatus.font;
            hostDialogue.fontSize = 23;
            hostDialogue.color = Color.white;
            hostDialogue.alignment = TextAnchor.MiddleLeft;
            hostDialogue.raycastTarget = false;
            hostDialogue.rectTransform.anchorMin = new Vector2(.2f, 0);
            hostDialogue.rectTransform.anchorMax = Vector2.one;
            hostDialogue.rectTransform.offsetMin = new Vector2(0, 10);
            hostDialogue.rectTransform.offsetMax = new Vector2(-16, -10);
        }

        public void ShowHost(StudyDefinition study, string quote, bool showMeowra)
        {
            var source = study == null ? null : study.meowraPortrait;
            if (source != portraitSource)
            {
                if (headPortrait != null) Destroy(headPortrait);
                portraitSource = source;
                if (source != null)
                {
                    var r = source.rect;
                    headPortrait = Sprite.Create(source.texture,
                        new Rect(r.x + r.width * .29f, r.y + r.height * .60f,
                            r.width * .41f, r.height * .40f), new Vector2(.5f, .5f));
                }
            }
            // Fixed page geometry across conditions: artwork must not change task width.
            hostPortrait.sprite = headPortrait;
            hostPortrait.enabled = showMeowra && source != null;
            pagesRect.anchorMin = new Vector2(.06f, pagesRect.anchorMin.y);
            hostDialogue.rectTransform.anchorMin = new Vector2(.2f, 0);
            hostDialogue.text = showMeowra ? study.meowraName + "\n" + quote : quote;
        }

        private void OnDestroy()
        {
            if (headPortrait != null) Destroy(headPortrait);
        }

        public void ShowPersonaMessage(string message, string heading)
        {
            introduction.text = message;
            var scroll = introduction.GetComponentInParent<ScrollRect>(true);
            var scrollRect = (RectTransform)scroll.transform;
            scrollRect.anchorMin = new Vector2(.05f, .2f);
            scrollRect.anchorMax = new Vector2(.95f, .82f);
            scroll.verticalNormalizedPosition = 1;
            var title = scroll.transform.parent.Find("Title");
            if (title != null) title.GetComponent<Text>().text = heading;
            introductionPortrait.gameObject.SetActive(false);
        }

        private GameObject saveErrorPanel;
        public event Action SaveRetryRequested;

        public int OrderNumber => orderDropdown.value + 1;

        public void BuildResultsAccess(Action openAll, Action openLatest)
        {
            // Researcher-only controls replace the old authoring hint below Start/Preview.
            var hint = startButton.transform.parent.Find("AuthoringHint");
            if (hint != null) hint.gameObject.SetActive(false);
            AddResultsButton("OpenSavedDataButton", "Open saved data", .08f, .49f, openAll);
            AddResultsButton("OpenLatestCsvButton", "Latest live CSVs", .51f, .92f, openLatest);
        }

        private void AddResultsButton(string name, string label, float left, float right, Action action)
        {
            var button = Instantiate(startButton, startButton.transform.parent);
            button.name = name;
            button.onClick = new Button.ButtonClickedEvent();
            button.onClick.AddListener(() => action());
            button.interactable = true;
            var text = button.GetComponentInChildren<Text>();
            text.text = label;
            text.fontSize = 19;
            var rect = (RectTransform)button.transform;
            rect.anchorMin = new Vector2(left, .005f);
            rect.anchorMax = new Vector2(right, .075f);
            rect.offsetMin = rect.offsetMax = Vector2.zero;
        }

        public void ShowResultsStatus(string message) => setupStatus.text = message;

        public void Configure(List<string> orders)
        {
            orderDropdown.ClearOptions();
            orderDropdown.AddOptions(orders);
            stimulusSetDropdown.gameObject.SetActive(false);
            var parent = orderDropdown.transform.parent;
            parent.Find("SetLabel").gameObject.SetActive(false);
            parent.Find("OrderLabel").GetComponent<Text>().text = "Assignment cell";

        }

        public void ShowSetup(StudyDefinition study)
        {
            string error = study == null ? "Assign a Study Definition on ExperimentManager." : study.GetValidationError();
            string previewError = study == null ? "Assign a Study Definition on ExperimentManager." : study.GetValidationError(true);
            startButton.interactable = error == null;
            previewButton.interactable = previewError == null;
            setupStatus.text = (error == null
                ? "Live survey ready. JSON and CSV save after each submission."
                : $"Live survey unavailable: {error}") + "\n" + (previewError == null
                ? "Preview available (unscored; saved separately)."
                : $"Preview unavailable: {previewError}");
            previewBanner.SetActive(false);
        }

        public void ShowSaveError(StudyDefinition study)
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
                message.text = study.saveErrorMessage;
                message.rectTransform.sizeDelta = new Vector2(760, 200);
                message.rectTransform.anchoredPosition = new Vector2(0, 70);
                var button = Instantiate(startButton, rect);
                button.name = "RetrySaveButton";
                button.onClick = new Button.ButtonClickedEvent();
                button.onClick.AddListener(() => SaveRetryRequested?.Invoke());
                button.interactable = true;
                button.GetComponentInChildren<Text>().text = study.retryLabel;
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

        public void ShowCompletion(bool preview, StudyDefinition study)
        {
            completion.text = preview
                ? study.previewCompletion
                : study.studyCompletion;
        }
    }
}
