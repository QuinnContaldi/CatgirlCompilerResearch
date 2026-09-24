using UnityEngine;
using UnityEngine.UI;

namespace Meowra.UI
{
    // Presents researcher-authored text; ExperimentManager handles acceptance and saving.
    public sealed class ConsentView : MonoBehaviour
    {
        private Text body;
        private ScrollRect scroll;
        public string DisplayedText => body.text;

        public void Build()
        {
            var title = transform.Find("Title").GetComponent<Text>();
            title.text = "Consent to participate";
            Place(title.rectTransform, .04f, .84f, .96f, .98f);
            transform.Find("ContinueButton").GetComponentInChildren<Text>().text = "Accept";
            body = transform.Find("Body").GetComponent<Text>();

            var box = new GameObject("ConsentTextBox", typeof(RectTransform), typeof(Image), typeof(ScrollRect));
            box.transform.SetParent(transform, false);
            Place((RectTransform)box.transform, .04f, .2f, .96f, .82f);
            box.GetComponent<Image>().color = Color.white;
            var viewport = new GameObject("Viewport", typeof(RectTransform), typeof(RectMask2D));
            viewport.transform.SetParent(box.transform, false);
            Place((RectTransform)viewport.transform, .025f, .03f, .975f, .97f);
            body.transform.SetParent(viewport.transform, false);
            body.alignment = TextAnchor.UpperLeft;
            body.fontSize = 24;
            body.supportRichText = false;
            body.verticalOverflow = VerticalWrapMode.Overflow;
            body.raycastTarget = true;
            var rect = body.rectTransform;
            rect.anchorMin = new Vector2(0, 1);
            rect.anchorMax = Vector2.one;
            rect.pivot = new Vector2(.5f, 1);
            rect.sizeDelta = Vector2.zero;
            rect.anchoredPosition = Vector2.zero;
            body.gameObject.AddComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.PreferredSize;
            scroll = box.GetComponent<ScrollRect>();
            scroll.viewport = (RectTransform)viewport.transform;
            scroll.content = rect;
            scroll.horizontal = false;
            scroll.movementType = ScrollRect.MovementType.Clamped;
            scroll.scrollSensitivity = 35;

            var hint = Instantiate(title, transform);
            hint.name = "ScrollHint";
            hint.text = "Scroll to read all information.";
            hint.fontSize = 18;
            Place(hint.rectTransform, .04f, .145f, .96f, .19f);
        }

        public void Show(string text)
        {
            body.text = string.IsNullOrWhiteSpace(text)
                ? "[Preview: enter your consent information in Study Definition > Consent Text.]"
                : text;
            scroll.StopMovement();
            scroll.content.anchoredPosition = Vector2.zero;
            scroll.verticalNormalizedPosition = 1;
        }

        private static void Place(RectTransform rect, float left, float bottom, float right, float top)
        {
            rect.anchorMin = new Vector2(left, bottom);
            rect.anchorMax = new Vector2(right, top);
            rect.offsetMin = rect.offsetMax = Vector2.zero;
        }
    }
}
