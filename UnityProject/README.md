# Experiment authoring skeleton

Unity **6000.6.1f1** (`7efac9f6c10e`), based on **Universal 2D 7.0.0**.
The project uses Unity's uGUI, Input System and URP; no third-party Unity
packages or runtime frameworks were added.

**Start with [the authoring guide](AUTHORING.md)** for dragging in code images,
entering your own questions and explanations, assigning A–D answer keys, and
choosing counterbalancing orders.

## Open and preview

From the repository root on this workstation:

```bash
./Tools/open-unity.sh
```

Open `Assets/Scenes/Experiment.unity`, press Play, choose order **1–6** and
stimulus set **1–3**, then click **Preview layout**. All six question assets
are intentionally blank. A scored session is enabled only when the question
assets and Meowra introduction are complete.

On a machine where Unity starts normally, add `UnityProject` through Unity Hub.
Use the same Editor version to avoid an unintended upgrade. Override the
launcher's Editor path with `UNITY_EDITOR` if necessary.

## Responsibility boundaries

| Component / asset | Responsibility |
| --- | --- |
| `ScenarioData` | Your image, prompt, four answers, key and three feedback texts. |
| `StudyDefinition` | Six scenario slots and your Meowra introduction/portrait. |
| `Counterbalancing` | Builds a deterministic schedule from order and stimulus set. |
| `ExperimentManager` | Runs the page/block sequence and owns the current session. |
| `ParticipantSession` / `TrialResponse` | Keeps the assignment, responses and scores in memory. |
| `TrialManager` | Accepts one explicit answer and scores a single submission. |
| `TrialView` | Binds question data to the reusable trial panel. |
| `StudyShellView` | Researcher menu and surrounding page presentation. |
| `PageManager` | Shows one panel at a time; knows nothing about study conditions. |

`Awake()` connects trial UI events; `ExperimentManager.Start()` initializes the
menu after all active objects have initialized. The manager selects a panel
through `PageManager.ShowPage(GameObject)`. The reusable panel is the connected
`Assets/Prefabs/TrialPanel.prefab` instance under `Canvas/Pages/TrialPage`.

The flow is researcher setup → welcome placeholder → instructions placeholder →
three blocks of two trials, each followed by an evaluation placeholder → trial
section completion. The Meowra introduction appears immediately before her block
in every order. Background questions, practice/tutorial content, questionnaires,
final measures, timers and disk logging remain future work. This is a template
for authoring, not a completed participant protocol.

Scoring is researcher-only: one point for a correct answer, zero for an incorrect
answer. Preview responses are explicitly unscored. Inspect `ExperimentManager`
in Play mode to see Session, Correct Count, Scored Count and individual Responses.
Results survive page transitions and return to the researcher menu. They are
replaced when another session starts and are lost when Play mode/app exits.

## Verify

Close the Editor before running this from the repository root:

```bash
./Tools/open-unity.sh -batchmode -nographics \
  -executeMethod NavigationSmokeCheck.Run \
  -logFile /tmp/meowra-experiment-check.log
```

Do not add `-quit`; the check exits after Play mode finishes. Success is exit
code 0 with `NAVIGATION_CHECK_OK`. In the Editor, use **Tools → Experiment →
Run Smoke Check** outside Play mode.

The checks exercise the saved UI, blank-content preview, authoring validation,
all 18 order/set combinations, one-page visibility, exactly one Meowra
introduction, two trials per evaluation, feedback/image binding, explicit single
selection, no answer carryover, duplicate-submit protection and scoring.
Synthetic test content is created only in memory and never saved into your assets.
The checks live in `Assets/Editor` and do not ship in a player.

## Version control

Commit `Assets/` including all `.meta` files, `Packages/` including the lockfile,
`ProjectSettings/`, and this documentation. The root `.gitignore` excludes Unity
caches, builds, IDE output and the local compatibility libraries. Keep the root
research documents and image; no nested repository is needed.

## Local Linux compatibility workaround

This workstation runs Ubuntu 26.04.1 and has `libxml2.so.16`, but this Editor
requires `libxml2.so.2`. Official Ubuntu packages were extracted into the ignored
repository-local `.unity-compat/` directory, without installing or replacing
system libraries. The launcher adds that directory to `LD_LIBRARY_PATH` only
for Unity and its child processes. Opening this project directly through Hub
on this workstation will still need the dependency issue resolved separately.

Packages used, downloaded over HTTPS from Ubuntu's official archive:

- [libxml2 2.9.14+dfsg-1.3ubuntu3.8 (amd64)](https://archive.ubuntu.com/ubuntu/pool/main/libx/libxml2/libxml2_2.9.14+dfsg-1.3ubuntu3.8_amd64.deb)
- [libicu74 74.2-1ubuntu3.1 (amd64)](https://archive.ubuntu.com/ubuntu/pool/main/i/icu/libicu74_74.2-1ubuntu3.1_amd64.deb)

SHA-256 values recorded for the downloaded archives, in that order:

```text
bfd07c01d6e5ab3e327f3ca5819409b1914bbfb3f1a016d53e4dabd5f96143bb
c9a70989678660eed9a1e904c74fa043da8bec8e2036856fc16e31ced79b04f8
```

If this local folder is removed, download those packages, verify their hashes,
then extract each with `dpkg-deb -x <downloaded-package.deb> .unity-compat` from
the repository root. This is a workstation workaround, not a Unity asset or a
dependency to distribute with the study application.
