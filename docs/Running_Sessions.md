# Run a complete scored session

Scientific protocol: [Research_Design.md](../Research_Design.md).
Scored sessions are enabled as of 2026-10-01 at the researcher's request after
confirming the current content is finished. `Content Reviewed` is enabled on
`Assets/Data/StudyDefinition.asset`; Preview remains available.
The current version label remains `two-condition-draft-v2` so exported data accurately identifies the
content used. Consent now uses the researcher-provided UNR template and an approximately
15-minute estimate. Confirm the estimate in the pilot and finalize institutional
review, compensation arrangements, and data-handling procedures before collection;
no consent approval or content/build freeze has been inferred.

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

## Meowra dialogue revision — 2026-09-29

Meowra's existing introduction, tutorial, task banners, progress and completion
copy now follows the voice guide in [AGENTS.md](../AGENTS.md). Progress pages use
separate host banner and body text instead of displaying the same dialogue twice.
Task-pace advice was removed; encouragement remains independent of answers.

The content import passed source/runtime validation. The navigation smoke check
passed all four assignments in preview and scored modes using its synthetic
content-review override, including checks for distinct progress banner text.
This result does not re-enable scored sessions or establish a content freeze.
Review the presentation at the intended participant resolution before collection.

## Consent revision — 2026-10-01

[Consent_Text.md](../Consent_Text.md) now contains a shortened, neutral adaptation
of the researcher-provided UNR template, including the supplied 15-minute
estimate, study activities, minor discomforts, confidentiality, voluntary
participation with no grade consequences, and researcher/UNR rights contacts.
The estimate still needs pilot confirmation. Institutional review, compensation
arrangements, and data-handling procedures remain collection preparation items.

The content import and navigation smoke check passed all four assignments in
preview and scored modes using the existing synthetic content-review override.
Scored sessions remain disabled pending review; this validation does not establish
consent approval or a content/build freeze.

## Background question revision — 2026-10-01

The optional background question now asks participants to rate their C/C++
experience from 1 (no experience) to 10 (very extensive experience), replacing
the broad written description. Participants enter the rating in the existing
optional response field; it is saved as `programmingBackground`. Review the
wording in the pilot and include it in the content/build freeze.

Content import and the navigation smoke check passed all four assignments in
preview and scored modes with the existing synthetic review override. Scored
sessions remain disabled pending content review.
