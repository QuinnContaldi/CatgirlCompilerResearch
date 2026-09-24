using System;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.TextCore.LowLevel;
using UnityEngine.UI;
using Meowra.UI;

// Editor migration: the resulting font and prefab are ordinary saved assets.
public static class ConfigureStudyText
{
    [MenuItem("Tools/Experiment/Configure Text Panels")]
    public static void Run()
    {
        if (TMP_Settings.LoadDefaultSettings() == null)
            throw new InvalidOperationException("Restore Assets/TextMesh Pro (the bundled TMP essential resources) before configuring text panels.");

        const string fontPath = "Assets/Fonts/StudyMono.asset";
        var font = AssetDatabase.LoadAssetAtPath<TMP_FontAsset>(fontPath);
        if (font == null)
        {
            var source = AssetDatabase.LoadAssetAtPath<Font>("Assets/Fonts/LiberationMono-Regular.ttf");
            font = TMP_FontAsset.CreateFontAsset(source, 48, 5, GlyphRenderMode.SDFAA, 1024, 1024);
            font.name = "StudyMono";
            AssetDatabase.CreateAsset(font, fontPath);
            AssetDatabase.AddObjectToAsset(font.material, font);
            foreach (var atlas in font.atlasTextures) AssetDatabase.AddObjectToAsset(atlas, font);
            // Prewarm the frozen content's alphabet; retain dynamic support for authoring.
            string alphabet = "‘’—–";
            for (int i = 32; i < 127; i++) alphabet += (char)i;
            font.TryAddCharacters(alphabet);
            EditorUtility.SetDirty(font);
        }

        const string prefabPath = "Assets/Prefabs/TrialPanel.prefab";
        var root = PrefabUtility.LoadPrefabContents(prefabPath);
        try
        {
            var content = root.GetComponentInChildren<ScrollRect>(true).content;
            Remove(content.Find("CodeArea"));
            Remove(content.Find("Explanation"));
            Remove(content.Find("FeedbackImage"));
            var code = Panel(content, "CodeTextPanel", font, "[Code snippet]");
            var feedback = Panel(content, "FeedbackTextPanel", font, "[Explanation]");
            code.transform.parent.SetSiblingIndex(1);
            feedback.transform.parent.SetSiblingIndex(3);
            var view = new SerializedObject(root.GetComponent<TrialView>());
            view.FindProperty("codeText").objectReferenceValue = code;
            view.FindProperty("explanation").objectReferenceValue = feedback;
            view.ApplyModifiedPropertiesWithoutUndo();
            PrefabUtility.SaveAsPrefabAsset(root, prefabPath);
        }
        finally { PrefabUtility.UnloadPrefabContents(root); }
        AssetDatabase.SaveAssets();
        Debug.Log("STUDY_TEXT_CONFIGURED: Text panels and bundled monospace font saved.");
    }

    private static TMP_Text Panel(Transform content, string name, TMP_FontAsset font, string placeholder)
    {
        var existing = content.Find(name);
        if (existing != null) return existing.GetComponentInChildren<TMP_Text>(true);
        var panel = new GameObject(name, typeof(RectTransform), typeof(Image), typeof(VerticalLayoutGroup));
        panel.transform.SetParent(content, false);
        panel.GetComponent<Image>().color = new Color32(30, 34, 42, 255);
        var layout = panel.GetComponent<VerticalLayoutGroup>();
        layout.padding = new RectOffset(20, 20, 16, 16);
        layout.childControlWidth = true;
        layout.childControlHeight = true;
        layout.childForceExpandWidth = true;
        layout.childForceExpandHeight = false;
        var go = new GameObject("Text", typeof(RectTransform), typeof(TextMeshProUGUI));
        go.transform.SetParent(panel.transform, false);
        var text = go.GetComponent<TextMeshProUGUI>();
        text.font = font;
        text.fontSize = 24;
        text.enableAutoSizing = false;
        text.color = new Color32(232, 235, 240, 255);
        text.alignment = TextAlignmentOptions.TopLeft;
        text.richText = true;
        text.parseCtrlCharacters = false;
        text.textWrappingMode = TextWrappingModes.Normal;
        text.overflowMode = TextOverflowModes.Overflow;
        text.raycastTarget = false;
        text.text = placeholder;
        return text;
    }

    private static void Remove(Transform child)
    {
        if (child != null) UnityEngine.Object.DestroyImmediate(child.gameObject);
    }
}
