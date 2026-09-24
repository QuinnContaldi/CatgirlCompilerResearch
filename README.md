# Dr. Meowra Compiler Feedback Study

A Unity research application comparing three compiler-error feedback styles:
**Raw**, **Neutral**, and **Dr. Meowra**. Each participant completes six scenarios,
experiences all three conditions, and answers questionnaires. Feedback is authored
in advance; no live AI or compiler runs during participant sessions.

## Start here

1. Open `UnityProject/` in **Unity 6000.6.1f1** through Unity Hub. On this Linux
   workstation, run `./Tools/open-unity.sh` from the repository root instead.
2. Open `Assets/Scenes/Experiment.unity` and press **Play**.
3. Choose counterbalance order **1–6** and task rotation (**stimulus set**) **1–3**.
4. Choose **Preview layout** for an unscored walkthrough or **Start session** for
   a scored session. The setup screen explains any missing required content.
5. Participants accept consent, read the introduction/instructions, select answers,
   and click **Continue**. Responses save before the next page appears.

**Before participant collection:** the consent text is a draft with contact and
policy placeholders. Finalize it through your institution's process. Review the
known stimulus inconsistencies in [Study_Text.md](Study_Text.md). Background
questions, a separate practice task, and additional targeted ratings described
in the research plan are not currently implemented.

Dr. Meowra currently hosts the study throughout, including the instructions.
This differs from the original neutral-tutorial plan. Her feedback-block
introduction still appears immediately before her assigned block.

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
| `assignment.csv` | All six planned tasks, counterbalance, rotation and submission status. |
| `api.csv` | Ten Dr. Meowra persona ratings on their 1–5 scale. |
| `preference.csv` | Preferred feedback style and written explanation. |
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
├── README.md                     # Setup, results access and repository guide
├── AGENTS.md                     # Implementation and research constraints
├── Research_Design.md            # Preserved detailed research plan
├── Research_Design_Document.pdf   # Research reference
├── Consent_Text.md               # Consent draft and remaining institutional details
├── Meowra_Host.md                 # Persona/host protocol notes
├── Study_Text.md                  # Current text stimuli and review notes
├── Study_Pictures.md              # Earlier screenshot implementation archive
├── DrMeowra.jpg                   # Original character artwork
├── Stimuli/
│   ├── README.md
│   ├── SourceCode/                # Six C examples and diagnostic helper
│   └── ReferenceImages/           # Original code/feedback screenshots
├── Tools/open-unity.sh            # Local Unity launcher
└── UnityProject/
    ├── README.md                 # App architecture and validation
    ├── AUTHORING.md              # Editing scenarios and questionnaires
    ├── Assets/                   # Scenes, scripts, assets, images and fonts
    ├── Packages/                 # Dependencies and lockfile
    └── ProjectSettings/          # Shared Unity settings
```

Original material in `Stimuli/` is retained for reference. Participant-facing
content lives in `UnityProject/Assets/Data/`. Unity assets stay in their existing
locations with their `.meta` files, preserving scene and prefab references.

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
