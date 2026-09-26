> LEGACY: superseded experiment. See [the current protocol](../../Research_Design.md).
> Historical descriptions and paths below are preserved for provenance only.

# Persistent Dr. Meowra host — September 2026

Requested protocol revision: Dr. Meowra is now present across the whole study.
Her portrait and short dialogue appear in a shared banner on every normal page.
Consent has a risks-reading prompt; the original consent text remains separate.
Accept leads directly to her personal introduction, then instructions. Immediately
before her assigned condition block she says it is her turn to explain errors.

Each coding question shows its encouragement in the shared blue dialogue bar
before the participant answers. Submit saves the response and advances directly
to the next question or evaluation; there are no separate compliment pages.
Six frozen messages rotate by question position, regardless of condition or
selected option. Survey pages continue to ask for honest impressions.
Timing includes reading the on-question encouragement and ends at Submit.

Question pages display a head crop created in the UI from the original sprite.
Other forms, including consent and both introductions, display the large original
portrait beside the form. The source image file is unchanged.

## Research implications

Raw and Neutral now include Meowra's portrait, social presence, and encouragement.
Raw retains raw compiler *error content*, but is no longer a persona-free experience.
Neutral versus Meowra now compares error-explanation framing within a shared
persona-hosted experience; it does not isolate persona presence versus absence.
Persona ratings also reflect exposure throughout the study. The tutorial's
technical content remains unchanged but its presentation now includes the host.
The six sequences, scenario rotation, answer keys and questionnaires are unchanged.
Update the protocol and hypotheses to reflect this revision before data collection.
This note supersedes the earlier persona-only-in-her-block presentation rule for
this requested implementation, without rewriting the original research documents.

## Editing and understanding

Select `UnityProject/Assets/Data/StudyDefinition.asset` in the Unity Inspector.
A ScriptableObject is an editable data asset: its host quotes, introduction,
block introduction, portrait and encouragement list supply the UI's wording.
`StudyShellView` builds the shared banner and displays messages;
`ExperimentManager` decides when to introduce her or select question encouragement;
`TrialManager` still handles selection and stops the response timer.
`ParticipantSession` saves protocol ID `persistent-host-v2` (schema 5), and each
trial's exact encouragement in session.json. Existing CSV columns are unchanged.
The banner reserves space above pages; other forms also reserve a left portrait column.

## Manual check

1. Open `Assets/Scenes/Experiment.unity` and enter Play mode.
2. Choose Preview Layout (real sessions still require authored consent).
3. Inspect the consent quote; click Accept and check her personal introduction.
4. Continue to instructions and trials. Check the head crop and encouragement
   before answering. Submit each choice: the next question or evaluation should
   appear immediately, without an extra Continue. Try correct and wrong choices.
5. Try orders 1 and 5 to check her block transition when she is last and first.
6. Check portrait visibility and readable scrolling on surveys and completion.
7. Run Tools > Experiment > Run Smoke Check for all 18 assignments and save retries.

Commit the changed StudyDefinition asset; StudyDefinition, ParticipantSession,
ExperimentManager, StudyShellView and TrialView scripts; ExperimentSmokeCheck and
CaptureTrialPreview editor scripts; and this document. Existing uncommitted work
was already present; review it separately. No commit or push was performed.

Next small task: review the host wording and layout in a participant walkthrough.

Validation: Unity integration smoke checks passed all 18 assignments and one preview run, including save retries and direct JSON/CSV checks. Saved JSON records were also inspected for the v2 protocol and on-question encouragement. Consent, introduction and question layouts were visually checked using Unity captures.

Every stage now has its own frozen banner wording, including three distinct block-evaluation prompts. Evaluation wording follows block position, not condition. StudyDefinition rejects empty or duplicate banner/encouragement messages. Questionnaire items and response scales are unchanged.
