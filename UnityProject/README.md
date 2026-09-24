# Unity application guide

Start with the [repository README](../README.md) for opening the app and accessing
CSV results. Use [AUTHORING.md](AUTHORING.md) to edit study content without C#.

## Project and current flow

Editor: **Unity 6000.6.1f1** (`7efac9f6c10e`). The app uses Unity uGUI, Input
System, TextMeshPro and URP. Open `Assets/Scenes/Experiment.unity`.

```text
Researcher setup → Consent → Meowra host introduction → Instructions
  → 3 condition blocks (2 scenarios, then 8 UEQ-S items per block)
  → 10 persona items → Preference → Written explanation → Completion
```

The Meowra feedback introduction appears immediately before her block in every
order. The researcher selects one of six condition sequences and one of three
independent task rotations. All six authored scenarios appear once per session.
The persistent Meowra host and persona instructions are intentional changes from
the original neutral-tutorial plan. Background questions, a separate practice
scenario, and other targeted ratings in the research plan remain unimplemented.

**Preview layout** records unscored responses in a separate folder. **Start
session** validates required content and scores answers against the scenario key.
A nonempty consent draft passes technical validation; it does not establish
institutional approval. Complete the placeholders in [Consent_Text.md](../Consent_Text.md).

## Where to edit

| Content | Location in Unity |
| --- | --- |
| Six scenarios, feedback and answer keys | `Assets/Data/Scenarios/Scenario01.asset` through `Scenario06.asset` |
| Consent, portrait and host messages | `Assets/Data/StudyDefinition.asset` |
| Instructions | Scene: `Canvas/Pages/InstructionsPage/Body`, Text component |
| Question layout and Continue button | `Assets/Prefabs/TrialPanel.prefab` |
| Validated UEQ-S item definitions | `Assets/Scripts/Data/UeqsResponse.cs` |
| Persona and final-measure definitions | `Assets/Scripts/Data/FinalResponses.cs` |

Edit outside Play mode and save. Keep questionnaire wording, response ranges,
answer keys and treatment information aligned with the reviewed study protocol.
See [Study_Text.md](../Study_Text.md) for known source/feedback inconsistencies.
Original C examples and screenshots are grouped under [Stimuli/](../Stimuli/).

## How the code fits together

| Component | Responsibility |
| --- | --- |
| `StudyDefinition`, `ScenarioData` | Reusable study content stored as ScriptableObject assets. |
| `Counterbalancing` | Constructs the selected condition/task schedule. |
| `ExperimentManager` | Controls study stages and creates each participant session. |
| `ParticipantSession` | Holds the frozen assignment, consent, responses and scores. |
| `PageManager` | Displays one page at a time. |
| `TrialManager` | Measures question time and accepts one submission. |
| `TrialView`, `StudyShellView`, `ConsentView` | Present the trial, host/setup and consent UI. |
| `UeqsView`, `FinalMeasuresView` | Present questionnaires and final responses. |
| `DataLogger` | Saves JSON and CSV snapshots after submissions. |
| `StudyResults` | Finds saved session folders and opens them. |
| `StudyResultsMenu` | Provides Editor menu shortcuts, even outside Play mode. |

A ScriptableObject is a saved content asset; a prefab is a reusable UI object.
The scene's TrialPage uses the TrialPanel prefab. `Awake()` connects trial UI
events, and `ExperimentManager.Start()` initializes the menu and other views.
The manager decides what comes next; PageManager only changes page visibility.

## Data persistence

Use the researcher menu's **Open saved data** / **Latest live CSVs**, or the same
commands under **Tools → Experiment**. `ExperimentManager → Session Directory`
and the Console expose the path if needed. Results are stored under
`Application.persistentDataPath/StudySessions/Participants/<anonymous-ID>/`;
previews use `Previews/` instead.

Each submission updates `session.json`, `assignment.csv`, `trials.csv`,
`ueqs.csv`, `api.csv`, and `preference.csv`. The logger flushes a temporary file,
then replaces the old snapshot, retaining a `.bak`. JSON is authoritative if
an export write fails; the app waits for Retry before advancing.

- Question time uses `Stopwatch`, from display until submission, in seconds.
  It includes time away from the app; saving/questionnaire time is excluded.
- Trial rows contain the selected A–D answer, scoring flag and correctness.
- UEQ-S rows retain original positions 1–7: eight per block, 24 per completed study.
- Assignment rows record all six planned tasks, including unanswered ones.
- These three CSVs include counterbalance number, readable condition sequence,
  rotation number, full scenario order, participant ID, and preview flag.
- `session_completed` becomes true after the entire study is submitted. It
  replaces the older misleading `trial_section_completed` column name.

Earlier participants' files are preserved between sessions and across app exits.
Unsubmitted responses are not saved; interrupted sessions cannot yet resume.
Copy results to approved backup storage; do not commit participant data to Git.

## Verify changes

Exit Play mode and choose **Tools → Experiment → Run Smoke Check**. To run from
the repository root, close the Editor first:

```bash
./Tools/open-unity.sh -batchmode -nographics \
  -executeMethod NavigationSmokeCheck.Run \
  -logFile /tmp/meowra-experiment-check.log
```

Do not add `-quit`. Success is exit code 0 and `NAVIGATION_CHECK_OK` in the log.
Checks cover the saved scene, preview/live button pointer hits, validation, all
18 assignments, scoring, questionnaire resets, timing independent of game time,
partial JSON/CSV snapshots, save retries, and preservation of previous sessions.
The checks also run the authored scenarios with temporary synthetic consent.
Test data are isolated in `MeowraLoggingCheck-*` / `MeowraPersistenceCheck-*`
temporary folders. Test code lives in `Assets/Editor` and does not ship in a player.

For a manual check, run a preview, inspect the instructions and scrolling, submit
responses, finish a session, return to setup and open the results. Start another
session and confirm the earlier folder still exists. Desktop file-manager
launching itself is a manual check; the automated run does not open GUI windows.

## Version control

Keep `Assets/` with `.meta` files, `Packages/` with its lockfile, and
`ProjectSettings/`. `.meta` files store the GUIDs that keep scene/prefab references
connected. Never discard them when moving assets. Caches, builds, crash reports,
local settings and participant exports are ignored by the root `.gitignore`.

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
