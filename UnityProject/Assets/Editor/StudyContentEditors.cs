using Meowra.Data;
using UnityEditor;

[CustomEditor(typeof(ScenarioData))]
public sealed class ScenarioDataEditor : Editor
{
    public override void OnInspectorGUI()
    {
        EditorGUILayout.HelpBox("Author content here. Paste plain C source into Code Text, enter all four choices, and explicitly choose the answer key. The same code, prompt and answers are used in all conditions.", MessageType.Info);
        DrawDefaultInspector();
        string error = ((ScenarioData)target).GetValidationError();
        EditorGUILayout.HelpBox(error ?? "This scenario is ready.", error == null ? MessageType.Info : MessageType.Warning);
    }
}

[CustomEditor(typeof(StudyDefinition))]
public sealed class StudyDefinitionEditor : Editor
{
    public override void OnInspectorGUI()
    {
        EditorGUILayout.HelpBox("Assign six Scenario assets. Pairs are slots 1–2, 3–4, 5–6.\nSet 1: Raw = pair 1, Neutral = pair 2, Meowra = pair 3.\nSet 2: Raw = pair 2, Neutral = pair 3, Meowra = pair 1.\nSet 3: Raw = pair 3, Neutral = pair 1, Meowra = pair 2.\nCondition order is chosen separately on the researcher menu.", MessageType.Info);
        EditorGUILayout.HelpBox("Enter participant information and risks in Consent Text. Participants read it and click Accept; the displayed wording and acceptance time are saved in session.json.", MessageType.Info);
        DrawDefaultInspector();
        string error = ((StudyDefinition)target).GetValidationError();
        EditorGUILayout.HelpBox(error ?? "Study content is ready.", error == null ? MessageType.Info : MessageType.Warning);
    }
}
