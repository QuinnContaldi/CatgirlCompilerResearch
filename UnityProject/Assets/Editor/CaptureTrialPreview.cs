using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using Meowra.Experiment;
using Meowra.UI;
using Meowra.Data;
using System.IO;
using System.Reflection;

// Optional visual walkthrough; outputs and synthetic preview data stay under /tmp.
[InitializeOnLoad]
public static class CaptureTrialPreview
{
    private static int frames;
    static CaptureTrialPreview() { EditorApplication.update += Tick; }
    public static void Run()
    {
        EditorApplication.delayCall += () =>
        {
            EditorSceneManager.OpenScene("Assets/Scenes/Experiment.unity");
            SessionState.SetBool("Meowra.CapturePreview", true);
            var gameView = EditorWindow.GetWindow(System.Type.GetType("UnityEditor.GameView,UnityEditor"));
            gameView.Show(); gameView.Focus();
            EditorApplication.EnterPlaymode();
        };
    }
    private static void Tick()
    {
        if (!SessionState.GetBool("Meowra.CapturePreview", false) || !EditorApplication.isPlaying) return;
        frames++;
        var manager = Object.FindAnyObjectByType<ExperimentManager>();
        if (frames % 30 == 15)
        {
            string suffix = manager.Stage == ExperimentStage.Trial ? "-" + manager.CurrentTrial.Condition + "-" + manager.CurrentTrial.Scenario.scenarioId : "";
            ScreenCapture.CaptureScreenshot("/tmp/meowra-crossover-" + manager.Stage + suffix + ".png");
        }
        if (frames % 30 != 0) return;
        switch (manager.Stage)
        {
            case ExperimentStage.Setup:
                typeof(ExperimentManager).GetField("logger", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(manager,
                    new DataLogger(Path.Combine(Path.GetTempPath(), "MeowraVisualCheck-" + System.Guid.NewGuid().ToString("N"))));
                manager.PreviewLayout(); break;
            case ExperimentStage.Welcome: manager.ContinueWelcome(); break;
            case ExperimentStage.HostIntroduction:
            case ExperimentStage.Introduction:
            case ExperimentStage.Instructions:
            case ExperimentStage.Transition:
            case ExperimentStage.BlockCompletion: manager.ContinueIntroduction(); break;
            case ExperimentStage.Background: manager.ContinueBackground(); break;
            case ExperimentStage.Trial:
                var trials = Object.FindAnyObjectByType<TrialManager>();
                trials.SelectAnswer(AnswerChoice.A); trials.Submit(); break;
            case ExperimentStage.Evaluation:
                var evaluation = Object.FindAnyObjectByType<UeqsView>();
                foreach (var group in evaluation.GetComponentsInChildren<ToggleGroup>()) group.GetComponentsInChildren<Toggle>()[3].isOn = true;
                manager.ContinueEvaluation(); break;
            case ExperimentStage.Preference:
                Object.FindAnyObjectByType<FinalMeasuresView>().GetComponentsInChildren<Toggle>()[0].isOn = true;
                manager.ContinuePreference(); break;
            case ExperimentStage.OpenResponse: manager.ContinueOpenResponse(); break;
            case ExperimentStage.Complete:
                SessionState.SetBool("Meowra.CapturePreview", false);
                EditorApplication.Exit(0); break;
        }
    }
}
