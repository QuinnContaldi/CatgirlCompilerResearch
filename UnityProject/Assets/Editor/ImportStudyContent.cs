using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using Meowra.Data;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

// Markdown/JSON is the authoring source; Unity assets are reproducible runtime copies.
[InitializeOnLoad]
public sealed class ImportStudyContent : IPreprocessBuildWithReport
{
    public int callbackOrder => 0;
    static string Root => Path.GetFullPath(Path.Combine(Application.dataPath, "../.."));
    static ImportStudyContent()
    {
        EditorApplication.playModeStateChanged += state =>
        {
            if (state != PlayModeStateChange.ExitingEditMode) return;
            try { Check(); }
            catch (Exception e) { Debug.LogError(e.Message); EditorApplication.isPlaying = false; }
        };
    }
    public void OnPreprocessBuild(BuildReport report) => Check();

    [MenuItem("Tools/Experiment/Import Study Content")]
    public static void Run() => Process(true);
    [MenuItem("Tools/Experiment/Check Study Content")]
    public static void Check() => Process(false);

    public static void RunBatch()
    {
        try { Run(); Check(); EditorApplication.Exit(0); }
        catch (Exception e) { Debug.LogException(e); EditorApplication.Exit(1); }
    }

    static void Load(string json, UnityEngine.Object target, HashSet<string> seen)
    {
        foreach (Match match in Regex.Matches(json, "\"([A-Za-z][A-Za-z0-9]*)\"\\s*:"))
        {
            string key = match.Groups[1].Value;
            var field = target.GetType().GetField(key);
            if (field == null || (field.FieldType != typeof(string) && field.FieldType != typeof(string[]) && field.FieldType != typeof(AnswerChoice)))
                throw new InvalidDataException("Unsupported content field: " + key);
            if (!seen.Add(key)) throw new InvalidDataException("Duplicate content field: " + key);
        }
        JsonUtility.FromJsonOverwrite(json, target);
    }

    static void Process(bool write)
    {
        if (EditorApplication.isPlaying) throw new InvalidOperationException("Exit Play mode before importing content.");
        var assets = new List<UnityEngine.Object>();
        var drafts = new List<UnityEngine.Object>();
        try
        {
            var study = AssetDatabase.LoadAssetAtPath<StudyDefinition>("Assets/Data/StudyDefinition.asset");
            var draft = UnityEngine.Object.Instantiate(study);
            draft.name = study.name;
            assets.Add(study); drafts.Add(draft);
            foreach (var field in typeof(StudyDefinition).GetFields(BindingFlags.Public | BindingFlags.Instance))
                if (field.FieldType == typeof(string) && field.Name != "studyVersion") field.SetValue(draft, null);
                else if (field.FieldType == typeof(string[])) field.SetValue(draft, null);
            var seen = new HashSet<string>();
            foreach (string name in new[] { "Neutral_Host", "Meowra_Host", "Study_Text" })
            {
                string text = File.ReadAllText(Path.Combine(Root, "study-content", name + ".md"));
                var match = Regex.Match(text, @"```json\s*\n([\s\S]*?)\n```");
                if (!match.Success) throw new InvalidDataException("Missing JSON content block: " + name);
                Load(match.Groups[1].Value, draft, seen);
            }
            string consent = File.ReadAllText(Path.Combine(Root, "Consent_Text.md")).Replace("\r\n", "\n");
            const string marker = "## Screen text\n";
            int start = consent.IndexOf(marker, StringComparison.Ordinal);
            if (start < 0) throw new InvalidDataException("Missing consent Screen text section.");
            draft.consentText = consent.Substring(start + marker.Length).Trim();
            draft.scenarios = new ScenarioData[8];
            for (int i = 0; i < 8; i++)
            {
                string set = i < 4 ? "A" : "B";
                string id = set + (i % 4 + 1);
                var task = AssetDatabase.LoadAssetAtPath<ScenarioData>("Assets/Data/Tasks/" + id + ".asset");
                if (task == null) throw new InvalidDataException("Missing runtime task: " + id);
                var copy = ScriptableObject.CreateInstance<ScenarioData>();
                copy.name = task.name;
                assets.Add(task); drafts.Add(copy);
                Load(File.ReadAllText(Path.Combine(Root, "Stimuli", "Set" + set, id + ".json")), copy, new HashSet<string>());
                string error = copy.GetValidationError();
                if (error != null || copy.scenarioId != id || copy.taskSet != set || string.IsNullOrWhiteSpace(copy.matchedPairId))
                    throw new InvalidDataException(id + ": " + (error ?? "Check task ID, set and matched pair."));
                draft.scenarios[i] = copy;
                if (i >= 4 && copy.matchedPairId != draft.scenarios[i - 4].matchedPairId)
                    throw new InvalidDataException("Matched pair IDs differ: " + id);
            }
            string validation = draft.GetValidationError(true);
            if (validation != null) throw new InvalidDataException(validation);
            // Compare references to the existing runtime objects, not transient validation copies.
            for (int i = 0; i < 8; i++) draft.scenarios[i] = (ScenarioData)assets[i + 1];
            bool changed = false;
            for (int i = 0; i < assets.Count; i++)
            {
                if (EditorJsonUtility.ToJson(assets[i]) == EditorJsonUtility.ToJson(drafts[i])) continue;
                if (!write) throw new InvalidDataException("Study content differs from runtime asset " + assets[i].name + ". Use Tools → Experiment → Import Study Content.");
                changed = true;
            }
            if (write && changed)
            {
                draft.contentReviewed = false;
                Undo.RecordObjects(assets.ToArray(), "Import study content");
                for (int i = 0; i < assets.Count; i++)
                {
                    EditorUtility.CopySerialized(drafts[i], assets[i]);
                    EditorUtility.SetDirty(assets[i]);
                }
                AssetDatabase.SaveAssets();
            }
            Debug.Log("STUDY_CONTENT_OK: source content validated" + (changed ? " and imported; content review reset." : "; runtime assets match."));
        }
        finally { foreach (var draft in drafts) UnityEngine.Object.DestroyImmediate(draft); }
    }
}
