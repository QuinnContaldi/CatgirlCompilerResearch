using System;
using Meowra.Data;
using Meowra.Experiment;
using UnityEngine;
using UnityEngine.UI;

namespace Meowra.UI
{
    // Three separate pages share presentation helpers, but do not decide study order.
    public sealed class FinalMeasuresView : MonoBehaviour
    {
        private Font font;
        private Color textColor;
        private Button submit;
        private readonly int[] ratings = new int[ApiItems.Count];
        private Toggle[] options;
        private InputField reason;
        private int selectedCondition = -1;
        public bool ApiComplete => Array.TrueForAll(ratings, value => value > 0);
        public int[] CopyRatings() => (int[])ratings.Clone();
        public bool HasPreference => selectedCondition >= 0;
        public FeedbackCondition PreferredCondition => (FeedbackCondition)selectedCondition;
        public string Reason => reason.text;

        public static FinalMeasuresView Create(GameObject template, string name, Action onSubmit)
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
            view.font = text.font;
            view.textColor = text.color;
            view.submit = Instantiate(template.GetComponentInChildren<Button>(true), page.transform);
            view.submit.name = "SubmitButton";
            view.submit.gameObject.SetActive(true);
            view.submit.onClick = new Button.ButtonClickedEvent();
            view.submit.onClick.AddListener(() => onSubmit());
            view.submit.GetComponentInChildren<Text>(true).text = "Submit and continue";
            Place((RectTransform)view.submit.transform, .35f, .02f, .65f, .095f);
            return view;
        }

        public void BuildApi(string scaleLabels)
        {
            Label(transform, "Title", "Dr. Meowra — API items", 28, 0, .91f, 1, .99f);
            Label(transform, "Instructions", "Please rate each statement about Dr. Meowra. Select one response per row.\n" + scaleLabels, 18, .01f, .81f, .99f, .91f);
            Label(transform, "Engaging", "Engaging (5 items)", 20, .02f, .755f, .98f, .81f);
            Label(transform, "Credible", "Credible (5 items)", 20, .02f, .425f, .98f, .48f);
            for (int item = 0; item < ApiItems.Count; item++)
            {
                int index = item;
                float top = item < 5 ? .755f - item * .055f : .425f - (item - 5) * .055f;
                Label(transform, "ItemText" + item, ApiItems.Wording(item), 19, .025f, top - .05f, .55f, top, TextAnchor.MiddleLeft);
                var row = new GameObject("Item" + (item + 1), typeof(RectTransform), typeof(ToggleGroup));
                row.transform.SetParent(transform, false);
                Place((RectTransform)row.transform, .56f, top - .05f, .97f, top);
                var group = row.GetComponent<ToggleGroup>();
                group.allowSwitchOff = true;
                for (int value = 1; value <= 5; value++)
                {
                    int rating = value;
                    Choice(row.transform, group, "Rating" + value, value.ToString(), (value - 1) / 5f, 0, value / 5f - .01f, 1, on =>
                    {
                        if (on) ratings[index] = rating;
                        else if (ratings[index] == rating) ratings[index] = 0;
                        submit.interactable = ApiComplete;
                    });
                }
            }
            options = GetComponentsInChildren<Toggle>(true);
        }

        public void BuildPreference()
        {
            Label(transform, "Title", "Final preferred condition", 28, 0, .82f, 1, .97f);
            Label(transform, "Prompt", "If you could choose one of these feedback styles for your programming\nenvironment, which would you choose? Select one.", 22, .03f, .67f, .97f, .82f);
            var group = gameObject.AddComponent<ToggleGroup>();
            group.allowSwitchOff = true;
            string[] labels = { "Raw", "Neutral", "Dr. Meowra" };
            for (int i = 0; i < labels.Length; i++)
            {
                int index = i;
                Choice(transform, group, "Condition" + i, labels[i], .2f, .51f - i * .13f, .8f, .61f - i * .13f, on =>
                {
                    if (on) selectedCondition = index;
                    else if (selectedCondition == index) selectedCondition = -1;
                    submit.interactable = HasPreference;
                });
            }
            options = GetComponentsInChildren<Toggle>(true);
        }

        public void BuildReason()
        {
            Label(transform, "Title", "Why did you prefer that style?", 28, .02f, .8f, .98f, .96f);
            var box = new GameObject("ReasonInput", typeof(RectTransform), typeof(Image), typeof(InputField));
            box.transform.SetParent(transform, false);
            Place((RectTransform)box.transform, .07f, .2f, .93f, .75f);
            box.GetComponent<Image>().color = Color.white;
            var text = Label(box.transform, "Text", "", 22, .025f, .04f, .975f, .96f, TextAnchor.UpperLeft);
            text.supportRichText = false;
            reason = box.GetComponent<InputField>();
            reason.targetGraphic = box.GetComponent<Image>();
            reason.textComponent = text;
            reason.lineType = InputField.LineType.MultiLineNewline;
            reason.characterLimit = 0;
            // Only the preference is specified as forced choice; submitting blank prose is allowed.
        }

        public void Begin()
        {
            Array.Clear(ratings, 0, ratings.Length);
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
