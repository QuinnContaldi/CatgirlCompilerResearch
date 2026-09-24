# Experiment authoring skeleton

Unity **6000.6.1f1** (`7efac9f6c10e`), based on **Universal 2D 7.0.0**.
The project uses Unity's uGUI, Input System and URP; no third-party Unity
packages or runtime frameworks were added.

**Start with [the authoring guide](AUTHORING.md)** for entering code text,
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
| `ScenarioData` | Your code text, prompt, four answers, key and three feedback texts. |
| `StudyDefinition` | Six scenario slots and your Meowra introduction/portrait. |
| `Counterbalancing` | Builds a deterministic schedule from order and stimulus set. |
| `ExperimentManager` | Runs the page/block sequence and owns the current session. |
| `ParticipantSession` / `TrialResponse` | Keeps the assignment, responses and scores in memory. |
| `TrialManager` | Times a scenario and accepts one explicit submission. |
| `DataLogger` | Writes complete JSON and trial CSV snapshots after each response. |
| `TrialView` | Binds question data to the reusable trial panel. |
| `StudyShellView` | Researcher menu and surrounding page presentation. |
| `PageManager` | Shows one panel at a time; knows nothing about study conditions. |

`Awake()` connects trial UI events; `ExperimentManager.Start()` initializes the
menu after all active objects have initialized. The manager selects a panel
through `PageManager.ShowPage(GameObject)`. The reusable panel is the connected
`Assets/Prefabs/TrialPanel.prefab` instance under `Canvas/Pages/TrialPage`.

The flow is researcher setup → welcome placeholder → instructions placeholder →
three blocks of two trials, each followed by all eight UEQ-S items → ten API items → final preference →
open-ended explanation → completion. The Meowra introduction appears immediately before her block
in every order. Background questions, practice/tutorial content, other questionnaires remain future work. Scenario timing and incremental disk logging are implemented. This is a template
for authoring, not a completed participant protocol.

Scoring is researcher-only: one point for a correct answer, zero for an incorrect
answer. Preview responses are explicitly unscored. Inspect `ExperimentManager`
in Play mode to see Session, Correct Count, Scored Count and individual Responses.
Results survive page transitions and return to the researcher menu. The in-memory session is
replaced when another session starts; saved files remain after Play mode/app exits.
See the authoring guide for storage paths and timing semantics.

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
introduction, two trials per evaluation, feedback/text binding, explicit single
selection, no answer carryover, duplicate-submit protection, scoring, real elapsed
time with game time stopped, partial JSON/CSV snapshots, and save-failure retry.
Synthetic session files go into a unique `MeowraLoggingCheck-*` temporary folder
reported in the Console, never into participant storage.
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

## UEQ-S after each block

After each pair of tasks, the same questionnaire asks all eight supplied items,
with seven selectable positions and no default answers. All eight are required.
`UeqsView` builds the rows when Play mode starts; inspect them under
`Canvas/Pages/EvaluationPlaceholderPage` during Play mode. The existing panel name
is retained so scene references remain intact. `ToggleGroup` makes choices within
one row mutually exclusive. `ExperimentManager` supplies the completed block's
condition; the view does not decide condition order.

`UeqsResponse` stores block number, condition, UTC submission time, and eight
positions in item order. Positions are **1 = left anchor, 7 = right anchor**;
no scale means or statistical analyses are calculated in Unity. Session schema 3
includes `ueqsResponses`. Each submitted questionnaire is saved in `session.json`
and `ueqs.csv` under the session directory before proceeding. CSV has eight rows
per evaluation, including item number, dimension, both anchors and position.
Existing trial saving and preview/participant separation remain in place.
Selections on an unsubmitted questionnaire are not yet saved. A failed submission
save retains the response in memory and shows the existing retry screen.

To inspect manually, enter Preview layout, answer two tasks, and check that Submit
stays disabled until all eight rows have a selection. Change a selection, submit,
and repeat for the other two blocks. Each new questionnaire must start blank.
Inspect `ExperimentManager.Session.UeqsResponses` and the files at Session Directory:
a complete run has three evaluations and 24 data rows in `ueqs.csv`.
The smoke check verifies this across all 18 order/set assignments and reads back
partial and completed files. Item wording and seven-position ranges are exactly
as requested; counterbalancing, stimuli and Meowra introduction placement are unchanged.

## Final measures

After all three blocks and their UEQ-S evaluations, the study shows three separate
pages: **API**, **Final preferred condition**, and **Why did you prefer that style?**
This follows the research document's end-of-study placement even when Meowra is
first or second. The ten supplied statements retain their exact wording: five
Engaging items followed by five Credible items. All ten require an answer, with
no preselection. API uses 1 = Strongly disagree, 2 = Disagree, 3 = Neutral,
4 = Agree, 5 = Strongly agree. These are selected API subscales, not the full API.
Scale reference: [Ryu and Baylor's API paper](https://www.researchgate.net/publication/237627605_The_API_Agent_Persona_Instrument_for_Assessing_Pedagogical_Agent_Persona).

Preference requires exactly one explicit choice: Raw, Neutral, or Dr. Meowra.
The subsequent multiline explanation may be submitted blank; its submission is
still recorded. No names are requested. Nothing is preselected or carried into
the next participant's session.

`FinalResponses.cs` holds the fixed item wording and validated API response data.
`FinalMeasuresView` creates three panels during Play mode using the existing
questionnaire's font, colors and button. A `ToggleGroup` is a Unity component
that permits only one selected toggle in its group: each API row has its own
group, and the preference page has one group for its three choices. An
`InputField` captures the written explanation. `ExperimentManager` chooses the
next stage; `PageManager` only controls which panel is visible.

Inspect `Canvas/Pages/ApiPage`, `PreferencePage`, and `OpenResponsePage` in Play
mode. They are generated at startup and do not require scene reference changes.
The session's `apiSubmitted`, `preferenceSubmitted`, and `reasonSubmitted` flags
distinguish missing responses from an enum's default value or blank prose.
Schema 3 saves each submitted page before continuing. `session.json` retains
API ratings and submission timestamps, preference and explanation; `api.csv`
has ten rows after API submission and `preference.csv` has one row after the
preference, updated after the explanation. CSV quotes embedded commas, quotes
and newlines. `completed` becomes true only at the final submission. Unsubmitted
page edits are not saved, and this does not add session resumption after restart.

Manual check: preview order 5 (Meowra first), finish all three blocks and UEQ-S
pages, then verify API appears once at the end. Fill nine API rows: Submit must
remain disabled. Fill the tenth, change an answer, and submit. Choose one
preference, change it, submit, and enter multiline text on the next page. Inspect
the session directory for ten API rows and the exact preference/reason. Repeat
with a fresh preview to confirm all inputs reset. The smoke check automates all
18 order/set assignments, partial snapshots, blank prose, CSV escaping, and a
failed final save/retry.

Research impact: adds the requested secondary persona and final-preference
measures; existing UEQ-S wording, stimuli, condition order and introduction
placement are unchanged. Next step: visually pilot the three pages at the
participant workstation's intended display resolution.

Files for this change: add `Assets/Scripts/Data/FinalResponses.cs` and
`Assets/Scripts/UI/FinalMeasuresView.cs` with their `.meta` files; include updates
to `ParticipantSession.cs`, `DataLogger.cs`, `ExperimentManager.cs`,
`PageManager.cs`, `StudyShellView.cs`, `Assets/Editor/ExperimentSmokeCheck.cs`,
this README and `AUTHORING.md`. `DataLogger.cs` and the earlier UEQ-S work were
already uncommitted when this change began; include those dependencies when
committing the combined working implementation. No commit was made automatically.

See [Highlighted study text](../Study_Text.md) for code/diagnostic formatting, transcription provenance, and existing stimulus mismatches requiring review.

### Starting previews and live surveys

Open `Assets/Scenes/Experiment.unity` and enter Play mode. Choose condition order
1–6 and stimulus set 1–3, then select **Preview layout** or **Start session**.
The setup status now explains separately why either mode is unavailable.
If live mode requests consent, stop Play mode, select
`Assets/Data/StudyDefinition.asset`, enter the researcher-approved **Consent Text**,
save, and restart Play mode. Preview can run without this text.

The Console prints the session folder after its first successful save; it is also
shown in **ExperimentManager → Session Directory** during Play mode. On Linux,
live files are under
`~/.config/unity3d/CatgirlCompilerResearch/Compiler Feedback Study/StudySessions/Participants/<anonymous-ID>/`.
Preview files go under `StudySessions/Previews/` instead.
Each submission updates `session.json` and the relevant CSV snapshots:
`trials.csv`, `ueqs.csv`, `api.csv`, and `preference.csv`. These are separate files
per anonymous session, not a shared CSV that mixes participants and previews.

The smoke check now clicks the setup buttons through Unity's UI raycaster and
checks the saved study asset before using synthetic fixtures. It also completes
a live-mode run with the authored scenarios and temporary test consent, with all
test output isolated under `/tmp/MeowraLoggingCheck-*`. It never supplies consent
to the saved study asset. Edit study content through the Inspector: malformed
hand-edited Unity YAML can disable both modes even when the text looks complete.

### Scored-session CSV fields

Scored sessions save after every submitted response. Question timing uses
`Stopwatch`: elapsed seconds from question display until Submit, including time
away from the app. Selecting another answer does not restart the timer. Saving
and questionnaire time are excluded from the question's response time.

- `trials.csv`: one row per submitted question, with selected answer (A–D),
  `scored`, `correct`, `response_time_seconds`, scenario ID, condition, trial
  number, block number, and position within the block.
- `ueqs.csv`: eight rows per submitted block evaluation; keeps the original
  selected positions 1–7, item anchors, dimension, block, and condition.
- `assignment.csv`: all six planned tasks, saved before any answers; includes
  each task's condition, block, position, and whether it has been submitted.

All three files include anonymous participant ID, counterbalance `order_number`
(1–6), readable `condition_order`, rotation `stimulus_set` (1–3), full `task_order`,
and `preview`. The order comes from the schedule frozen when the session starts.
`session_completed` in trials and assignment becomes true only after the entire
study is submitted. It replaces the misleading old `trial_section_completed`
column name; analysis scripts should use column names rather than positions.
Preview sessions remain unscored and in a separate folder. A completed scored
session has six trial rows, 24 UEQ-S rows, and six assignment rows (plus headers).

### Instructions and viewing CSV files

The instructions page now speaks in Dr. Meowra's voice, as requested. This is a
change from the original condition-neutral tutorial: participants receive persona
framing before their assigned blocks. Question feedback, questionnaires, and
counterbalancing are unchanged. The trial submission button is labeled **Continue**
in all conditions; it still saves the selected answer before advancing.
Edit the instructions in `Experiment.unity` under
**Canvas → Pages → InstructionsPage → Body → Text** (outside Play mode).
The body rectangle leaves space for the Continue button below it.

To inspect a session's CSVs:
1. Start a live session or preview and submit some responses.
2. Select **ExperimentManager** in the Hierarchy. Copy **Session Directory** from
   the Inspector (the Console also prints this path when the session starts).
3. Paste the path into your file manager's location bar, usually Ctrl+L.
4. Open `trials.csv`, `ueqs.csv`, or `assignment.csv` in LibreOffice Calc or Excel.
   If an import dialog appears, choose UTF-8, comma separator, and double-quote
   text delimiter. A text editor can also open these files.

`trials.csv` contains question times in seconds and correctness; `ueqs.csv`
contains questionnaire responses; `assignment.csv` contains the counterbalance
and task rotation. Files update after submissions; reopen or reload them to see
new responses. Before responses are submitted, response CSVs contain headers only.
Preview files are under `Previews`; live files are under `Participants`.
On Linux the parent folder is
`~/.config/unity3d/CatgirlCompilerResearch/Compiler Feedback Study/StudySessions/`.

### One-click access to saved data

On the researcher setup screen:
- **Open saved data** opens the permanent `StudySessions` folder, containing
  `Participants` and `Previews`.
- **Latest live CSVs** opens the most recently saved live session's folder,
  including partial sessions. It ignores previews and works after app restarts.

The same shortcuts are available under **Tools → Experiment → Open saved data**
and **Open latest live CSVs**, including outside Play mode. Open the desired CSV
in your spreadsheet app. The setup status/Editor Console also shows the path.
Desktop folder opening uses Unity's local-folder support:
[Application.OpenURL](https://docs.unity3d.com/6000.0/Documentation/ScriptReference/Application.OpenURL.html).

Every new session gets a unique anonymous ID and a separate folder. Submitted
responses are flushed to disk before the app advances; CSV and JSON files persist
when Play mode stops, Unity closes, or another participant starts. Save failures
block progression and show Retry. Previous snapshots are retained as `.bak` files.
The smoke check verifies that later sessions do not modify previous sessions and
that partial results can be read by a fresh storage service.

An unsubmitted answer remains only on screen and is not saved. A partially
completed session's submitted data remain available, but the app does not yet
resume that participant's place after a restart. Storage is local to this computer
and user account; it is not an off-device backup. Copy the `StudySessions` folder
to your approved backup location after collection. Keep the Unity Company Name
and Product Name unchanged to keep using the same persistent storage location.
