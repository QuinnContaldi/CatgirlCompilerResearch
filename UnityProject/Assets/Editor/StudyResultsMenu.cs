using Meowra.Data;
using UnityEditor;
using UnityEngine;

public static class StudyResultsMenu
{
    [MenuItem("Tools/Experiment/Open saved data")]
    public static void OpenSavedData() => Debug.Log(StudyResults.OpenSavedData(StudyResults.RootDirectory, false));

    [MenuItem("Tools/Experiment/Open latest live CSVs")]
    public static void OpenLatestLiveCsvs() => Debug.Log(StudyResults.OpenSavedData(StudyResults.RootDirectory, true));
}
