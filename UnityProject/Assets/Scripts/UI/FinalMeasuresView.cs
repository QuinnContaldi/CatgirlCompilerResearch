using System;
using Meowra.Data;
using Meowra.Experiment;
using UnityEngine;
using UnityEngine.UI;

namespace Meowra.UI
{
    // Neutral final-response pages share controls; the manager decides study order.
    public sealed class FinalMeasuresView : MonoBehaviour
    {
        private StudyDefinition study;
        private Font font;
        private Color textColor;
        private Button submit;
        private Toggle[] options;
        private InputField reason;
        private int selectedCondition = -1;
        public bool HasPreference => selectedCondition >= 0;
        public FeedbackCondition PreferredCondition => (FeedbackCondition)selectedCondition;
        public string Reason => reason.text;

        public static FinalMeasuresView Create(GameObject template, string name, Action onSubmit, StudyDefinition study)
        {
            var page = new GameObject(name, typeof(RectTransform), typeof(Image), typeof(FinalMeasuresView));
            page.SetActive(false);
            page.transform.SetParent(template.transform.parent, false);
            var source = (RectTransform)template.transform;
            var rect = (RectTransform)page.transform;
            rect.anchorMin = source.anchorMin;
            rect.anchorMax = source.anchorMax;
            rect.pivot = source.pivot;
            rect.sizeDelta = source.sizeDelta;
            rect.anchoredPosition = source.anchoredPosition;
            var background = template.GetComponent<Image>();
            page.GetComponent<Image>().color = background != null ? background.color : Color.white;
            var view = page.GetComponent<FinalMeasuresView>();
            var text = template.GetComponentInChildren<Text>(true);
            view.study = study;
            view.font = text.font;
            view.textColor = text.color;
            view.submit = Instantiate(template.GetComponentInChildren<Button>(true), page.transform);
            view.submit.name = "SubmitButton";
            view.submit.gameObject.SetActive(true);
            view.submit.onClick = new Button.ButtonClickedEvent();
            view.submit.onClick.AddListener(() => onSubmit());
            view.submit.GetComponentInChildren<Text>(true).text = study.submitLabel;
            Place((RectTransform)view.submit.transform, .35f, .02f, .65f, .095f);
            return view;
        }

        public void BuildPreference()
        {
            Label(transform, "Title", study.preferenceHeading, 28, 0, .82f, 1, .97f);
            Label(transform, "Prompt", study.preferencePrompt, 22, .03f, .67f, .97f, .82f);
            var group = gameObject.AddComponent<ToggleGroup>();
            group.allowSwitchOff = true;
            string[] labels = { study.neutralName, study.meowraName };
            for (int i = 0; i < labels.Length; i++)
            {
                int index = i + 1;
                Choice(transform, group, "Condition" + i, labels[i], .2f, .51f - i * .13f, .8f, .61f - i * .13f, on =>
                {
                    if (on) selectedCondition = index;
                    else if (selectedCondition == index) selectedCondition = -1;
                    submit.interactable = HasPreference;
                });
            }
            options = GetComponentsInChildren<Toggle>(true);
        }

        public void BuildReason(string prompt)
        {
            Label(transform, "Title", prompt, 24, .02f, .8f, .98f, .96f);
            var box = new GameObject("ReasonInput", typeof(RectTransform), typeof(Image), typeof(InputField));
            box.transform.SetParent(transform, false);
            Place((RectTransform)box.transform, .07f, .2f, .93f, .75f);
            box.GetComponent<Image>().color = new Color(.94f, .96f, .98f);
            var text = Label(box.transform, "Text", "", 22, .025f, .04f, .975f, .96f, TextAnchor.UpperLeft);
            text.supportRichText = false;
            reason = box.GetComponent<InputField>();
            reason.targetGraphic = box.GetComponent<Image>();
            reason.textComponent = text;
            var placeholder = Label(box.transform, "Placeholder", study.optionalPlaceholder, 22, .025f, .04f, .975f, .96f, TextAnchor.UpperLeft);
            placeholder.color = new Color(.35f, .4f, .46f);
            reason.placeholder = placeholder;
            reason.lineType = InputField.LineType.MultiLineNewline;
            reason.characterLimit = 0;
            // Only the preference is specified as forced choice; submitting blank prose is allowed.
        }

        public void Begin()
        {
            selectedCondition = -1;
            if (options != null) foreach (var option in options) option.SetIsOnWithoutNotify(false);
            if (reason != null) reason.SetTextWithoutNotify("");
            submit.interactable = reason != null;
        }

        private void Choice(Transform parent, ToggleGroup group, string name, string caption, float left, float bottom, float right, float top, Action<bool> changed)
        {
            var cell = new GameObject(name, typeof(RectTransform), typeof(Image), typeof(Toggle));
            cell.transform.SetParent(parent, false);
            Place((RectTransform)cell.transform, left, bottom, right, top);
            var background = cell.GetComponent<Image>();
            background.color = new Color(.86f, .9f, .94f);
            var mark = new GameObject("Selected", typeof(RectTransform), typeof(Image));
            mark.transform.SetParent(cell.transform, false);
            Place((RectTransform)mark.transform, 0, 0, 1, .12f);
            mark.GetComponent<Image>().color = new Color(.12f, .35f, .58f);
            mark.GetComponent<Image>().raycastTarget = false;
            Label(cell.transform, "Label", caption, 21, 0, .1f, 1, 1);
            var toggle = cell.GetComponent<Toggle>();
            toggle.targetGraphic = background;
            toggle.graphic = mark.GetComponent<Image>();
            toggle.SetIsOnWithoutNotify(false);
            toggle.group = group;
            toggle.onValueChanged.AddListener(on => changed(on));
        }

        private Text Label(Transform parent, string name, string content, int size, float left, float bottom, float right, float top, TextAnchor alignment = TextAnchor.MiddleCenter)
        {
            var obj = new GameObject(name, typeof(RectTransform), typeof(Text));
            obj.transform.SetParent(parent, false);
            var label = obj.GetComponent<Text>();
            label.font = font;
            label.color = textColor;
            label.fontSize = size;
            label.text = content;
            label.alignment = alignment;
            label.raycastTarget = false;
            Place(label.rectTransform, left, bottom, right, top);
            return label;
        }

        private static void Place(RectTransform rect, float left, float bottom, float right, float top)
        {
            rect.anchorMin = new Vector2(left, bottom);
            rect.anchorMax = new Vector2(right, top);
            rect.offsetMin = rect.offsetMax = Vector2.zero;
        }
    }
}
