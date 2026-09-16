using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using Meowra.Experiment;
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
        if (frames == 20) ScreenCapture.CaptureScreenshot("/tmp/meowra-preview-setup.png");
        if (frames == 35) GameObject.Find("Canvas/Pages/ResearcherSetupPage/OrderDropdown").GetComponent<Dropdown>().Show();
        if (frames == 50) ScreenCapture.CaptureScreenshot("/tmp/meowra-preview-orders.png");
        if (frames == 70)
        {
            GameObject.Find("Canvas/Pages/ResearcherSetupPage/OrderDropdown").GetComponent<Dropdown>().Hide();
            var manager = Object.FindAnyObjectByType<ExperimentManager>();
            manager.PreviewLayout(); manager.ContinueWelcome(); manager.ContinueInstructions();
        }
        if (frames == 100) ScreenCapture.CaptureScreenshot("/tmp/meowra-preview-trial-top.png");
        if (frames == 120) GameObject.Find("Canvas/Pages/TrialPage/TrialScroll").GetComponent<ScrollRect>().verticalNormalizedPosition = 0;
        if (frames == 140) ScreenCapture.CaptureScreenshot("/tmp/meowra-preview-trial-bottom.png");
        if (frames == 160)
        {
            SessionState.SetBool("Meowra.CapturePreview", false);
            EditorApplication.Exit(0);
        }
    }
}
