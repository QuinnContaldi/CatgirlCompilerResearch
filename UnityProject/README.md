# Unity application guide

Open `Assets/Scenes/Experiment.unity` in Unity **6000.6.1f1**. On this workstation,
use `./Tools/open-unity.sh`. See [AUTHORING.md](AUTHORING.md) for editable sources and their Unity import.

## Current flow

Setup (four-cell assignment) → neutral consent → overview → optional background
→ introduction/tutorial → four tasks with separate progress screens → completion
→ neutral UEQ-S → second block → neutral UEQ-S → preference → reason → completion.

Meowra is present only in her treatment block. Raw feedback, compiler explanations,
and the persona questionnaire are no longer part of the active study. Eight task assets are imported from `Stimuli/SetA` and `Stimuli/SetB`. Legacy
scenarios and persona-instrument definitions have been removed from the repository. Scored sessions require `StudyDefinition.contentReviewed`. This flag was enabled
on 2026-10-01 at the researcher's request after confirming the current content is
finished; see [running sessions](../docs/Running_Sessions.md).

The researcher chooses the cell before consent; assignment is applied to treatment
presentation after the neutral opening pages. Background currently uses one optional
free-text field for the five suggested variables, pending wording review.

## Architecture

- `Counterbalancing`: four fixed crossover cells, two blocks, four tasks per block.
- `StudyDefinition` / `ScenarioData`: imported treatment text and task assets.
- `ImportStudyContent`: validates/imports Markdown and JSON sources; checks synchronization before Play/build.
- `ExperimentManager`: state progression and save-before-advance behavior.
- `PageManager`: one visible page at a time.
- `StudyShellView`: fixed shared page geometry and condition-specific host visibility.
- `TrialManager` / `TrialView`: answer selection, scoring, task display and Stopwatch.
- `UeqsView` / `FinalMeasuresView`: neutral questionnaires and background/prose input.
- `ParticipantSession` / `DataLogger`: session records and incremental JSON/CSV.

The timer starts after the task view populates and layout updates, and stops at
Submit. Transition, evaluation and save times are excluded. Time away from the app
is included; onset still has frame-level uncertainty. No correctness is displayed.

Meowra progress pages use `meowraTransitions` for body text and
`meowraTransitionDialogue` for three separate host banner messages. Both arrays
follow completed-question position under either task set. Validation requires
three nonempty banner messages, unique host dialogue across slides, and distinct
banner/body text at each progress position. All dialogue is authored in
[Meowra_Host.md](../study-content/Meowra_Host.md) and imported; no runtime wording
is randomized or generated.

## Data

Schema 6 identifies the two-condition protocol. JSON includes assignment cell,
study/Unity versions, consent text/time, optional background, planned task sets,
responses, final measures and session start/end times. Numeric condition IDs remain
Neutral = 1, Meowra = 2; Raw = 0 remains reserved for historical records only.

`assignment.csv` has eight planned rows; `trials.csv` has up to eight responses
including category, set, key, scoring flag and seconds/milliseconds. `ueqs.csv` has
16 raw response rows at completion; scores can be regenerated as positions minus
4 and averaged within each group of four. `preference.csv` records the preferred
study format and optional prose. No new `api.csv` is produced.

Storage remains `Application.persistentDataPath/StudySessions/Participants/<ID>/`
and `Previews/<ID>/`. Existing records and Company/Product names are preserved.
Do not combine historical three-condition records with the new protocol blindly.
JSON is authoritative; each file uses a flushed temporary replacement and `.bak`.
Save failures block advancement and offer Retry. Interrupted sessions cannot resume.

## Validation

Close Unity, then run from the repository root:

```bash
./Tools/open-unity.sh -batchmode -nographics \
  -executeMethod NavigationSmokeCheck.Run \
  -logFile /tmp/meowra-crossover-check.log
```

Success is exit code 0 and `NAVIGATION_CHECK_OK`; do not add `-quit`.
The scene check runs all four cells in preview and scored modes, with synthetic
consent and a temporary content-review override. It checks host visibility, task
assignment, answer resets/scoring, distinct Meowra progress banners, untimed
transitions, both evaluations, preference,
incremental exports, save failure/retry, and preservation of earlier records.
Tests write only to isolated temporary session folders.

For manual layout review, walk through cells 1 and 2 in Preview and inspect the
longest questions and text at the intended study resolution. Test success does not
establish content validity or readiness for data collection.

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
