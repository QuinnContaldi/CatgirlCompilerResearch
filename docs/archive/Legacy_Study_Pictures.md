> LEGACY: superseded experiment. See [the current protocol](../../Research_Design.md).
> Historical descriptions and paths below are preserved for provenance only.

# Original study pictures (archive)

**The participant display now uses highlighted text.** See [Study_Text.md](Study_Text.md)
for the current implementation and stimulus-review notes. The description below
records the earlier image implementation; pictures are retained for comparison.
The archive importer no longer modifies the trial prefab.

All 24 source PNGs in `Stimuli/ReferenceImages/` are copied unchanged into
`UnityProject/Assets/Images/Study/`. Each is imported as an uncompressed Sprite
(a Unity image asset usable by the UI), with mipmaps disabled for clear text.

| Scenario asset | Picture family |
| --- | --- |
| Scenario01 | statement_termination |
| Scenario02 | name_resolution |
| Scenario03 | delimiter_matching |
| Scenario04 | type_use |
| Scenario05 | function_call |
| Scenario06 | operator_use |

This follows the scenario-family order in Research_Design.md. Source filename aliases
`statment_termination`, `delimitor_matching`, and Neutral's `type_mismatch`
map to the corresponding correctly spelled family names in Unity.

Each ScenarioData asset holds one code Sprite and three feedback Sprites.
TrialView selects the feedback Sprite using the current FeedbackCondition.
The TrialPanel prefab contains the shared feedback Image area; it preserves
aspect ratio and hides the old text explanation when a picture is assigned.
Text remains available as a fallback for scenarios without feedback pictures.
All conditions use the same 250-unit feedback area, accommodating the taller
Raw pictures without cropping. The source images' own styling is preserved.

To inspect: open Assets/Data/Scenarios and select any scenario to see its four
assigned pictures in the Inspector. Open Experiment, enter Play mode, and use
Preview Layout to inspect the pictures while the question content is incomplete.
Scroll through the trial, and select other condition orders/assignment sets to
inspect each treatment. Tools > Experiment > Run Smoke Check verifies all
18 scenario/feedback combinations plus existing progression and data checks.

To refresh after changing source PNGs, exit Play mode and choose
Tools > Experiment > Import Study Pictures. This deliberately overwrites the
24 imported copies and their scenario image assignments; it leaves questions,
answer keys, feedback text, condition order and rotation rules untouched.

The six scenarios now include comprehension questions, four answer choices each,
and explicit correct answers: **B, D, A, C, A, B**, respectively. These prompts
and choices are shared across conditions. No questionnaire wording, condition
definitions or counterbalancing rules were changed.

Scenario01 asks specifically about the missing statement terminator: its code
picture also lacks a declaration for `cat`, while the feedback picture shows
`int cat = 5`. The pictures have not been edited; reconcile that existing mismatch
before freezing the study stimuli.

Commit the imported images and their .meta files, six scenario assets, TrialPanel
prefab, ScenarioData and TrialView changes, ImportStudyPictures editor script and
.meta, the image checks in ExperimentSmokeCheck, and this document. Preserve any
separate in-progress changes in the working tree; no commit is made automatically.
