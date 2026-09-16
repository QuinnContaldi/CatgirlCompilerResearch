using System;
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

    static NavigationSmokeCheck()
    {
        EditorApplication.update += Update;
        Application.logMessageReceived += RecordError;
    }

    [MenuItem("Tools/Navigation/Run Smoke Check")]
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
            CheckNavigation();
            Require(!SessionState.GetBool(ErrorKey, false), "Unity reported a runtime error.");
            Finish(true, "Startup, page visibility, wired buttons, boundaries and direct selection passed.");
        }
        catch (Exception error)
        {
            Finish(false, error.ToString());
        }
    }

    private static void CheckNavigation()
    {
        var manager = UnityEngine.Object.FindFirstObjectByType<PageManager>();
        Require(manager != null && manager.enabled, "An enabled PageManager is required.");
        var pages = GameObject.Find("Canvas/Pages").transform;
        var back = GameObject.Find("Canvas/Navigation/PreviousButton").GetComponent<Button>();
        var next = GameObject.Find("Canvas/Navigation/NextButton").GetComponent<Button>();
        var input = UnityEngine.Object.FindFirstObjectByType<InputSystemUIInputModule>();
        Require(input != null && input.isActiveAndEnabled && input.actionsAsset != null,
            "The EventSystem needs a configured input module.");
        Require(pages.childCount == 3, "Expected the three initial pages.");
        Require(back.onClick.GetPersistentEventCount() == 1 && next.onClick.GetPersistentEventCount() == 1,
            "Both buttons need a saved On Click connection.");

        AssertPage(manager, pages, back, next, 0);
        manager.PreviousPage();
        Click(back);
        AssertPage(manager, pages, back, next, 0);
        Click(next);
        AssertPage(manager, pages, back, next, 1);
        Click(next);
        AssertPage(manager, pages, back, next, 2);
        manager.NextPage();
        Click(next);
        AssertPage(manager, pages, back, next, 2);
        Click(back);
        AssertPage(manager, pages, back, next, 1);
        Click(back);
        AssertPage(manager, pages, back, next, 0);
        manager.ShowPage(2);
        AssertPage(manager, pages, back, next, 2);
        manager.ShowPage(-1);
        manager.ShowPage(3);
        AssertPage(manager, pages, back, next, 2);
        manager.ShowPage(0);
        AssertPage(manager, pages, back, next, 0);
    }

    private static void Click(Button button)
    {
        var pointer = new PointerEventData(EventSystem.current) { button = PointerEventData.InputButton.Left };
        ExecuteEvents.Execute(button.gameObject, pointer, ExecuteEvents.pointerClickHandler);
    }

    private static void AssertPage(PageManager manager, Transform pages, Button back, Button next, int index)
    {
        Require(manager.CurrentPageIndex == index, $"Expected page index {index}.");
        for (int i = 0; i < pages.childCount; i++)
            Require(pages.GetChild(i).gameObject.activeInHierarchy == (i == index), $"Incorrect visibility for page {i}.");
        Require(back.interactable == (index > 0), "Incorrect Back availability.");
        Require(next.interactable == (index < pages.childCount - 1), "Incorrect Next availability.");
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
