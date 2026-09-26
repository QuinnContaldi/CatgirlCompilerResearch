using Meowra.Data;
using UnityEditor;

[CustomEditor(typeof(ScenarioData))]
public sealed class ScenarioDataEditor : Editor
{
    public override void OnInspectorGUI()
    {
        EditorGUILayout.HelpBox("Imported runtime copy. Edit Stimuli/SetA or SetB JSON, then Tools > Experiment > Import Study Content. Tasks are shared across conditions.", MessageType.Info);
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
        EditorGUILayout.HelpBox("Assign eight tasks: Set A in slots 1–4 and Set B in slots 5–8. The researcher selects one of four crossover cells. Review draft content before enabling scored sessions.", MessageType.Info);
        EditorGUILayout.HelpBox("Edit consent in Consent_Text.md and host/shared copy in study-content/*.md, then Tools > Experiment > Import Study Content. Inspector text is an imported copy.", MessageType.Info);
        DrawDefaultInspector();
        string error = ((StudyDefinition)target).GetValidationError();
        EditorGUILayout.HelpBox(error ?? "Study content is ready.", error == null ? MessageType.Info : MessageType.Warning);
    }
}
