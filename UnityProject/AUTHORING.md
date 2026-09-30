# Author the two-condition study

The sole scientific protocol is [Research_Design.md](../Research_Design.md).

## Edit sources

| Content | Authoritative editable source |
| --- | --- |
| Neutral introduction, tutorial, progress and completion | [Neutral_Host.md](../study-content/Neutral_Host.md) |
| Meowra introduction, tutorial, progress, task banners and completion | [Meowra_Host.md](../study-content/Meowra_Host.md) |
| Shared instructions, prompts and completion | [Study_Text.md](../study-content/Study_Text.md) |
| Neutral consent | [Consent_Text.md](../Consent_Text.md), under `Screen text` |
| Four Set A questions | `../Stimuli/SetA/A1.json` through `A4.json` |
| Four Set B questions | `../Stimuli/SetB/B1.json` through `B4.json` |
| Original portrait | `../Pictures/DrMeowra.jpg` |

Condition Markdown files contain one editable JSON block. Preserve field names,
JSON quoting and array order. Progress arrays have three entries; Meowra task
banners have four entries and follow question position under either task set.
`meowraTransitions` supplies the progress body; `meowraTransitionDialogue` supplies
three distinct host banner lines for those same pages. Do not repeat the body in the banner.
Tutorials convey the same functional information. Meowra may encourage participation,
but cannot give programming hints, answer assistance or correctness feedback.

Task JSON uses `scenarioId` (task ID), `taskSet`, `matchedPairId`, `errorCategory`,
`codeText`, `question`, `answerA`–`answerD`, and `correctAnswer` (0=A through 3=D).
Keys are researcher data and never displayed to participants. Do not duplicate
questions for each condition. Current content remains a draft requiring pilot review.

## Import and inspect

1. Exit Play mode and edit the source files.
2. Choose **Tools → Experiment → Import Study Content**.
3. Inspect the imported `Assets/Data/StudyDefinition.asset` and eight task assets.
4. Use Preview layout in `Assets/Scenes/Experiment.unity` to review presentation.
5. Run **Tools → Experiment → Run Smoke Check** after changes.

Import validates all content before modifying assets, preserves existing GUIDs,
and resets `Content Reviewed` when content changes. **Check Study Content** checks
source/runtime equality without writing. Play mode and build checks reject stale
copies. Do not edit imported text directly in the Inspector; edit its source and
import again. Portrait assignment, study version and the content-review flag remain
researcher-managed fields in StudyDefinition. The Unity portrait is an imported
copy of the original artwork; image changes require replacing/reimporting that copy.

For command-line import, with Unity closed:

```bash
./Tools/open-unity.sh -batchmode -nographics \
  -executeMethod ImportStudyContent.RunBatch -logFile /tmp/meowra-content-import.log
```

The consent still has institutional/contact/privacy placeholders. The 2026-09-29
Meowra dialogue import reset `Content Reviewed`; scored sessions are currently
disabled pending review of the revised copy.
This software setting does not complete the consent placeholders or establish a
content freeze. Set the study version when finalizing the collection content. See [running sessions](../docs/Running_Sessions.md) and the
[pilot considerations](../Stimuli/README.md).

## Verify the flow

Use each of assignment cells 1–4. Check neutral consent, overview and background;
both introduction/tutorial/four-task blocks with separate progress pages; neutral
UEQ-S after each block; preference, optional reason and completion. Meowra appears
only within her condition. Confirm no hints or correctness feedback, no repeated
task set, and no transition time inside task response time.

Historical compiler-feedback assets and Agent Persona Instrument code have been
removed. Earlier versions remain in Git history; they are not current content sources.

## Meowra dialogue maintenance

Follow the personality and voice guide in [AGENTS.md](../AGENTS.md), within the
constraints of [Research_Design.md](../Research_Design.md). Use fixed, distinct
lines for the existing content slots. Keep all tutorial controls and restrictions
aligned with Neutral. Encouragement concerns participation only; omit hints,
answer judgements and condition-specific advice about task pace.

The 2026-09-29 revision refreshed the existing Meowra copy and separated progress
banner dialogue from body text. Source/runtime import validation and the navigation
smoke check passed all four assignments in preview and scored modes. The smoke
check verifies the dedicated progress banner is displayed without repeating the
body. Review the revised text and layout before re-enabling `Content Reviewed`.
