# Run a complete scored session

Scientific protocol: [Research_Design.md](../Research_Design.md).
Scored sessions have been enabled at the researcher's request. The current version
label remains `two-condition-draft-v2` so exported data accurately identifies the
content used. Consent placeholders are still present; no institutional details or
scientific freeze have been inferred from the request to enable the software.

## Start

1. Open `UnityProject/Assets/Scenes/Experiment.unity` and press Play.
2. Select assignment cell 1–4 according to your recruitment allocation.
3. Click **Start scored session**, not Preview layout.
4. Complete consent, overview, background, both four-question blocks and their
   UEQ-S ratings, preference, and optional written explanation.
5. At completion, return to the researcher menu. **Latest live CSVs** opens the
   latest scored session; **Open saved data** opens all saved sessions.

Each run gets a fresh participant ID and its own directory. Assignment is manual;
the app does not automatically balance recruitment across the four cells.

## Check saved results

`Participants/<ID>/` contains:

- `session.json`: authoritative record, consent, background, version, completion
  flag and start/end times.
- `assignment.csv`: eight assigned tasks.
- `trials.csv`: eight submitted answers with keys, accuracy and task response times.
- `ueqs.csv`: sixteen item responses, eight per condition.
- `preference.csv`: preferred format and optional explanation.

On this workstation the root is
`~/.config/unity3d/CatgirlCompilerResearch/Compiler Feedback Study/StudySessions/`.
The historical product-directory name is retained to preserve storage continuity.

Responses save after submission. If saving fails, restore storage access and use
Retry saving before continuing. Closing the app preserves submitted responses but
cannot resume the interrupted run. Back up the session folders after collection.

A personal end-to-end test using Start scored session also creates a scored record
under Participants. Record its participant ID and exclude it from research analysis;
completing the app does not make a synthetic test a research participant.

## Content changes

Importing changed task or study text disables scored sessions again. After reviewing
the changes, enable `Content Reviewed` on `Assets/Data/StudyDefinition.asset` and
set the appropriate study version. Importing unchanged content preserves the setting.
