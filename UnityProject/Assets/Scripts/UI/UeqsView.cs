using Meowra.Data;
using UnityEngine;
using UnityEngine.UI;

namespace Meowra.UI
{
    // Builds one reusable questionnaire on the existing evaluation panel.
    public sealed class UeqsView : MonoBehaviour
    {
        private readonly int[] positions = new int[UeqsItems.Count];
        private readonly Toggle[,] choices = new Toggle[UeqsItems.Count, 7];
        private Button submit;
        private Font font;
        private Color textColor;

        public bool IsComplete
        {
            get
            {
                foreach (int position in positions) if (position == 0) return false;
                return true;
            }
        }
        public int[] CopyPositions() => (int[])positions.Clone();

        public void Build(System.Action onSubmit)
        {
            var existingText = GetComponentInChildren<Text>(true);
            font = existingText.font;
            textColor = existingText.color;
            submit = GetComponentInChildren<Button>(true);
            foreach (Transform child in transform) child.gameObject.SetActive(child.gameObject == submit.gameObject);
            submit.onClick = new Button.ButtonClickedEvent();
            submit.onClick.AddListener(() => onSubmit());
            submit.GetComponentInChildren<Text>(true).text = "Submit and continue";
            Place((RectTransform)submit.transform, .35f, .02f, .65f, .095f);
            Label("Title", "UEQ-S", 28, 0, .89f, 1, .98f);
            Label("Instructions", "Please rate the feedback style from the two tasks you just completed.\nSelect one position between each pair of words.", 20, 0, .79f, 1, .9f);
            Label("PragmaticQuality", "Pragmatic Quality", 20, 0, .735f, 1, .79f);
            Label("HedonicQuality", "Hedonic Quality", 20, 0, .395f, 1, .45f);
            for (int item = 0; item < UeqsItems.Count; item++)
            {
                float top = item < 4 ? .735f - item * .07f : .395f - (item - 4) * .07f;
                Label("Left" + item, (item + 1) + ". " + UeqsItems.Left(item), 20, .02f, top - .065f, .27f, top, TextAnchor.MiddleRight);
                Label("Right" + item, UeqsItems.Right(item), 20, .73f, top - .065f, .98f, top, TextAnchor.MiddleLeft);
                var row = new GameObject("Item" + (item + 1), typeof(RectTransform), typeof(ToggleGroup));
                row.transform.SetParent(transform, false);
                Place((RectTransform)row.transform, .29f, top - .065f, .71f, top);
                var group = row.GetComponent<ToggleGroup>();
                group.allowSwitchOff = true;
                for (int option = 0; option < 7; option++)
                {
                    int index = item;
                    int position = option + 1;
                    var cell = new GameObject("Position" + position, typeof(RectTransform), typeof(Image), typeof(Toggle));
                    cell.transform.SetParent(row.transform, false);
                    Place((RectTransform)cell.transform, option / 7f + .008f, .12f, (option + 1) / 7f - .008f, .88f);
                    cell.GetComponent<Image>().color = new Color(.86f, .9f, .94f);
                    var mark = new GameObject("Selected", typeof(RectTransform), typeof(Image));
                    mark.transform.SetParent(cell.transform, false);
                    Place((RectTransform)mark.transform, .18f, .18f, .82f, .82f);
                    mark.GetComponent<Image>().color = new Color(.12f, .35f, .58f);
                    mark.GetComponent<Image>().raycastTarget = false;
                    var toggle = cell.GetComponent<Toggle>();
                    toggle.targetGraphic = cell.GetComponent<Image>();
                    toggle.graphic = mark.GetComponent<Image>();
                    toggle.SetIsOnWithoutNotify(false);
                    toggle.group = group;
                    toggle.onValueChanged.AddListener(on =>
                    {
                        if (on) positions[index] = position;
                        else if (positions[index] == position) positions[index] = 0;
                        submit.interactable = IsComplete;
                    });
                    choices[item, option] = toggle;
                }
            }
        }

        public void Begin()
        {
            System.Array.Clear(positions, 0, positions.Length);
            foreach (var choice in choices) choice.SetIsOnWithoutNotify(false);
            submit.interactable = false;
        }

        private void Label(string name, string content, int size, float left, float bottom, float right, float top, TextAnchor alignment = TextAnchor.MiddleCenter)
        {
            var obj = new GameObject(name, typeof(RectTransform), typeof(Text));
            obj.transform.SetParent(transform, false);
            var label = obj.GetComponent<Text>();
            label.font = font;
            label.color = textColor;
            label.fontSize = size;
            label.text = content;
            label.alignment = alignment;
            label.raycastTarget = false;
            Place(label.rectTransform, left, bottom, right, top);
        }

        private static void Place(RectTransform rect, float left, float bottom, float right, float top)
        {
            rect.anchorMin = new Vector2(left, bottom);
            rect.anchorMax = new Vector2(right, top);
            rect.offsetMin = rect.offsetMax = Vector2.zero;
        }
    }
}
