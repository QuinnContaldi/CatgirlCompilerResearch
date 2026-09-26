using System;
using System.IO;
using Meowra.Data;
using UnityEditor;
using UnityEngine;

// Retains the original pictures for reference; participant UI now uses text.
public static class ImportStudyPictures
{
    private static readonly string[] Families = {
        "statement_termination", "name_resolution", "delimiter_matching",
        "type_use", "function_call", "operator_use"
    };

    [MenuItem("Tools/Experiment/Archive/Import Original Study Pictures")]
    public static void Run()
    {
        string sourceRoot = Path.GetFullPath(Path.Combine(Application.dataPath, "../../Stimuli/ReferenceImages"));
        for (int i = 0; i < Families.Length; i++)
        {
            string family = Families[i];
            var scenario = AssetDatabase.LoadAssetAtPath<ScenarioData>($"Assets/Data/Scenarios/Scenario{i + 1:00}.asset");
            scenario.codeImage = Import(sourceRoot, "Coding", family);
            scenario.rawFeedbackImage = Import(sourceRoot, "Raw", family);
            scenario.neutralFeedbackImage = Import(sourceRoot, "Neutral", family);
            scenario.meowraFeedbackImage = Import(sourceRoot, "Meowra", family);
            EditorUtility.SetDirty(scenario);
        }

        AssetDatabase.SaveAssets();
        Verify();
    }

    private static Sprite Import(string sourceRoot, string condition, string family)
    {
        string sourceName = family;
        if ((condition == "Raw" || condition == "Meowra") && family == "statement_termination") sourceName = "statment_termination";
        if (condition == "Meowra" && family == "delimiter_matching") sourceName = "delimitor_matching";
        if (condition == "Neutral" && family == "type_use") sourceName = "type_mismatch";
        string directory = $"Assets/Images/Study/{condition}";
        Directory.CreateDirectory(directory);
        string path = $"{directory}/{family}.png";
        File.Copy(Path.Combine(sourceRoot, condition, sourceName + ".png"), path, true);
        AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceSynchronousImport);
        var importer = (TextureImporter)AssetImporter.GetAtPath(path);
        importer.textureType = TextureImporterType.Sprite;
        importer.spriteImportMode = SpriteImportMode.Single;
        importer.mipmapEnabled = false;
        importer.textureCompression = TextureImporterCompression.Uncompressed;
        importer.maxTextureSize = 2048;
        importer.npotScale = TextureImporterNPOTScale.None;
        importer.SaveAndReimport();
        return AssetDatabase.LoadAssetAtPath<Sprite>(path);
    }

    public static void Verify()
    {
        for (int i = 0; i < Families.Length; i++)
        {
            var scenario = AssetDatabase.LoadAssetAtPath<ScenarioData>($"Assets/Data/Scenarios/Scenario{i + 1:00}.asset");
            RequirePicture(scenario.codeImage, "Coding", Families[i]);
            foreach (FeedbackCondition condition in Enum.GetValues(typeof(FeedbackCondition)))
                RequirePicture(scenario.GetFeedbackImage(condition), condition.ToString(), Families[i]);
        }
        Debug.Log("STUDY_PICTURES_OK: All 24 sprites imported and assigned to their scenario and condition.");
    }

    private static void RequirePicture(Sprite sprite, string folder, string family)
    {
        if (sprite == null || AssetDatabase.GetAssetPath(sprite) != $"Assets/Images/Study/{folder}/{family}.png")
            throw new InvalidOperationException($"Incorrect picture for {folder}/{family}.");
    }
}
