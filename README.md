# Would You Rather Participate in a User Study With a Catgirl? Exploring Participant Preference and Experience in Programming Language User Studies

A Unity research application comparing **Neutral** and **Dr. Meowra** interfaces
for the same programming-language task. Every participant completes two matched
sets of four questions, rates each interface with UEQ-S, and states a preference
for a future similar study. No live AI or compiler runs during participation.

## Start here

1. Open `UnityProject/` in Unity **6000.6.1f1**, or use `./Tools/open-unity.sh` here.
2. Open `Assets/Scenes/Experiment.unity` and press Play.
3. Choose assignment cell **1–4** and select **Preview layout**.
4. Follow neutral consent, overview and background, then both condition blocks.

This implements the revised [research design](Research_Design.md). Meowra appears
only within her block; evaluation and consent screens are neutral. There is no
compiler-feedback manipulation or persona questionnaire in the active protocol.

**Scored sessions are enabled:** select assignment cell **1–4**, then **Start scored
session** to run the complete study and save scored results. Use **Latest live CSVs**
after completion to open the exports. See [running sessions](docs/Running_Sessions.md).
Consent still contains researcher placeholders; enabling the software does not
resolve them or establish a scientific content freeze.

## Open your CSV results

On the researcher setup screen, click:

- **Open saved data** for all saved sessions, including previews.
- **Latest live CSVs** for the most recently saved live participant, including a
  partial session.

These shortcuts also appear under **Tools → Experiment** outside Play mode.
Open a CSV in LibreOffice Calc or Excel; use UTF-8, comma-separated fields, and
`"` as the text delimiter if asked. Reload the file to see later submissions.

| File | Contents |
| --- | --- |
| `trials.csv` | Selected answer, correctness, time in seconds, condition and task order. |
| `ueqs.csv` | Eight responses per condition block, with original 1–7 positions. |
| `assignment.csv` | All eight planned tasks, assignment cell, set and submission status. |
| `preference.csv` | Preferred study format and written explanation. |
| `session.json` | Complete session record, including consent wording and timestamp. |

Each participant receives a unique folder. Submitted responses survive stopping
Play mode, closing Unity, and starting another session. Previews are stored
separately. Unsubmitted answers are not saved, and the app cannot yet resume an
interrupted session. A save error blocks progression and offers Retry.

Storage uses `Application.persistentDataPath/StudySessions/`. On this Linux setup:

```text
~/.config/unity3d/CatgirlCompilerResearch/Compiler Feedback Study/StudySessions/
├── Participants/<anonymous-ID>/
└── Previews/<anonymous-ID>/
```

Keep participant data outside Git. Back up the folder to your approved storage;
local `.bak` snapshots are not an off-device backup. Keep Unity's Company Name
and Product Name unchanged to retain the same storage location.

## Repository map

```text
CatgirlCompilerResearch/
├── AGENTS.md                 # Implementation constraints
├── README.md                 # Repository and running guide
├── Research_Design.md        # Sole scientific protocol
├── Consent_Text.md           # Editable neutral consent source
├── docs/
│   ├── Content_Review.md     # Dated review; not a protocol or content source
│   └── archive/              # Obsolete documents, stimuli and Unity assets
├── Stimuli/
│   ├── SetA/                 # Four task JSON definitions, including keys
│   └── SetB/                 # Four matched task JSON definitions, including keys
├── Pictures/                 # Original Dr. Meowra artwork
├── study-content/
│   ├── Neutral_Host.md       # Neutral condition copy
│   ├── Meowra_Host.md        # Host-only Meowra copy
│   └── Study_Text.md         # Shared participant copy
├── Tools/
└── UnityProject/
```

[Research_Design.md](Research_Design.md) is the only scientific protocol.
Archived material is historical and must not guide the current experiment.
Edit the content sources above, then run **Tools → Experiment → Import Study
Content** in Unity. Runtime assets are imported copies, not independent authoring
sources. Play mode and builds check synchronization. Importing changed content
resets Content Reviewed. See [authoring](UnityProject/AUTHORING.md).

Task Data + Condition Presentation + Crossover Assignment = Rendered Study Block.
Both conditions use the same task objects and shared scene; task content is never
copied into condition-specific files.

## Editing and testing

See [UnityProject/AUTHORING.md](UnityProject/AUTHORING.md) for content editing and
[UnityProject/README.md](UnityProject/README.md) for architecture, flow and checks.
In Unity, exit Play mode and use **Tools → Experiment → Run Smoke Check**.
Alternatively, close the Editor and run:

```bash
./Tools/open-unity.sh -batchmode -nographics \
  -executeMethod NavigationSmokeCheck.Run \
  -logFile /tmp/meowra-experiment-check.log
```

Success is exit code 0 with `NAVIGATION_CHECK_OK`. Do not add `-quit`; the check
exits when finished. Test data go into temporary folders, not participant storage.

## What belongs in Git

Commit source code, these documents, original stimuli/artwork, Unity `Assets/`
**including `.meta` files**, `Packages/` including `packages-lock.json`, and
`ProjectSettings/`. There is one repository; do not initialize Git inside UnityProject.

The root `.gitignore` excludes Unity caches, logs, crash dumps, builds, IDE output,
local compatibility libraries, and named participant exports. Store any renamed
or combined exports in an ignored `Results/`, `DataExports/`, or `StudySessions/`
folder. Ignore rules prevent future additions; they do not remove files from Git
history or protect files added with `git add -f`.
