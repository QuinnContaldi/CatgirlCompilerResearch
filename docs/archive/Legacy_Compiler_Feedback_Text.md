> LEGACY: superseded experiment. See [the current protocol](../../Research_Design.md).
> Historical descriptions and paths below are preserved for provenance only.

# Code and feedback as text

The six scenario assets now store `codeText`, `rawFeedback`, `neutralFeedback`
and `meowraFeedback`. Edit them in **Assets/Data/Scenarios** in the Unity Inspector.
These are plain strings; do not insert rich-text tags. The shared TrialPanel
prefab renders them with TextMeshPro and the bundled Liberation Mono font.

## How it works

- `ScenarioData` is a ScriptableObject: a saved content form independent of the UI.
  Validation requires code text and all three feedback texts, even if old image
  references exist.
- `StudyTextFormatter` colors C keywords, directives, strings, numbers and comments.
  Feedback colors filenames/locations, severity labels, quoted references and
  numbered source excerpts. It does not detect errors or add corrective hints.
  Literal angle brackets are protected from TextMeshPro's rich-text parser.
- `TrialView` selects the assigned condition's text and supplies formatted strings
  to the two serialized TMP references. Nothing compiles or calls AI at runtime.
- `TrialPanel.prefab` uses matching dark panels and 24-unit monospace text for
  every condition. Unity's layout groups measure preferred text height, expanding
  panels within the existing vertical scroll view. Long lines wrap; text does not
  shrink to fit. Inspect the longest diagnostic at the actual study resolution.
- `ConfigureStudyText` is an Editor-only setup tool. Its output is saved in the
  prefab and font assets; setup does not run during participant sessions.

## Transcription and research review

Code was copied from the six `Stimuli/SourceCode/*.c` files, checked against `Stimuli/ReferenceImages/Coding`,
with leading blank lines retained and unused trailing blank lines removed.
Feedback was transcribed from the original 18 feedback pictures. Terminal-width
line breaks inside sentences were removed so Unity can wrap naturally. Source
excerpts, existing carets, notes, punctuation and wording were retained. Editor
breadcrumbs, terminal prompts/commands, cursor decorations, inlay parameter hints,
editor squiggles and active-line backgrounds are not part of the new text display.
The original pictures and hidden scenario image references remain as an archive.
**Tools > Experiment > Archive > Import Original Study Pictures** only refreshes
that archive; it does not change the text display or its content.

This is the requested presentation change. Condition count, order, rotation,
answer keys, questionnaires and logging behavior are unchanged. Styling is now
shared across conditions instead of inheriting each screenshot's styling.

Existing stimulus inconsistencies were deliberately not corrected:

- Scenario01 source says `cat = 5` without a declaration or semicolon. Raw reports
  an undeclared `cat` and shows `cat = 5;`. Neutral/Meowra discuss a missing
  semicolon and show `int cat = 5`. The question asks about the semicolon.
- Name-resolution feedback refers to line 3 while the source's leading blank
  line places the assignment on line 4.
- Type-use Raw points to column 7; Neutral/Meowra point to column 9.

Review and resolve these deliberately before freezing the study. Text conversion
is not validation of the scientific equivalence of the three feedback versions.

## Inspect and test

1. Open `Assets/Scenes/Experiment.unity` and enter Play mode.
2. Choose **Preview Layout**, accept consent and continue the instructions.
3. Inspect the code and feedback, scroll to the answers and submit.
4. Use orders 1 and 5 to inspect Meowra last/first; change stimulus sets to inspect
   each scenario under every condition. Font, panel color and size should match.
5. Run **Tools > Experiment > Run Smoke Check**. It checks all 18 authored
   scenario/condition text bindings, literal punctuation rendering and font settings,
   then runs existing synthetic navigation, scoring, counterbalancing and durable
   JSON/CSV checks. Synthetic data go to a temporary directory, not participant data.

When committing, include the scenario assets, ScenarioData, TrialView,
StudyTextFormatter and its .meta, TrialPanel prefab, updated Editor scripts and
new .meta files, Assets/Fonts assets/licenses, the bundled Assets/TextMesh Pro
resources and .meta files, and the updated authoring/readme documentation.
Do not commit Library, Temp, crash dumps or generated participant/test data.
