using System;
using System.Collections;
using Meowra.UI;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

// Editor-only integration check of the saved scene and its actual button events.
// No test framework or test code is included in the player.
[InitializeOnLoad]
public static class NavigationSmokeCheck
{
    private const string RunningKey = "Meowra.NavigationCheck.Running";
    private const string ResultKey = "Meowra.NavigationCheck.Result";
    private const string DeadlineKey = "Meowra.NavigationCheck.Deadline";
    private const string ErrorKey = "Meowra.NavigationCheck.Error";
    private static int playFrames;
    private static IEnumerator checks;

    static NavigationSmokeCheck()
    {
        EditorApplication.update += Update;
        Application.logMessageReceived += RecordError;
    }

    [MenuItem("Tools/Experiment/Run Smoke Check")]
    public static void Run()
    {
        // Let Editor startup callbacks finish before entering Play mode. Unity's
        // search index cannot be created for the first time while playing.
        EditorApplication.delayCall += Begin;
    }

    private static void Begin()
    {
        if (EditorApplication.isPlayingOrWillChangePlaymode)
            throw new InvalidOperationException("Exit Play mode before running the check.");

        if (!Application.isBatchMode && !EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            return;

        EditorSceneManager.OpenScene("Assets/Scenes/Experiment.unity");
        SessionState.SetBool(RunningKey, true);
        SessionState.SetInt(ResultKey, -1);
        SessionState.SetBool(ErrorKey, false);
        SessionState.SetFloat(DeadlineKey, (float)EditorApplication.timeSinceStartup + 90);
        playFrames = 0;
        checks = null;
        EditorApplication.EnterPlaymode();
    }

    private static void RecordError(string message, string stackTrace, LogType type)
    {
        if (SessionState.GetBool(RunningKey, false) &&
            (type == LogType.Error || type == LogType.Exception || type == LogType.Assert))
            SessionState.SetBool(ErrorKey, true);
    }

    private static void Update()
    {
        if (!SessionState.GetBool(RunningKey, false))
            return;

        int result = SessionState.GetInt(ResultKey, -1);
        if (result >= 0 && !EditorApplication.isPlayingOrWillChangePlaymode)
        {
            SessionState.SetBool(RunningKey, false);
            if (Application.isBatchMode)
                EditorApplication.Exit(result);
            return;
        }

        if (EditorApplication.timeSinceStartup > SessionState.GetFloat(DeadlineKey, 0))
        {
            Finish(false, "Timed out waiting for Play mode.");
            return;
        }

        if (result >= 0 || !EditorApplication.isPlaying || EditorApplication.isCompiling || ++playFrames < 3)
            return;

        try
        {
            if (checks == null) checks = ExperimentSmokeCheck.Run();
            if (checks.MoveNext()) return;
            Require(!SessionState.GetBool(ErrorKey, false), "Unity reported a runtime error.");
            Finish(true, "Navigation, authoring validation, all 18 assignments, trial presentation and scoring passed.");
        }
        catch (Exception error)
        {
            Finish(false, error.ToString());
        }
    }

    private static void Require(bool condition, string message)
    {
        if (!condition)
            throw new InvalidOperationException(message);
    }

    private static void Finish(bool success, string message)
    {
        SessionState.SetInt(ResultKey, success ? 0 : 1);
        if (success)
            Debug.Log("NAVIGATION_CHECK_OK: " + message);
        else
            Debug.LogError("NAVIGATION_CHECK_FAILED: " + message);
        EditorApplication.ExitPlaymode();
    }
}
