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

The consent still has institutional/contact/privacy placeholders. `Content Reviewed`
is currently enabled at the researcher's request to allow complete scored runs.
This software setting does not complete the consent placeholders or establish a
content freeze. Set the study version when finalizing the collection content. See the dated [content review](../docs/Content_Review.md).

## Verify the flow

Use each of assignment cells 1–4. Check neutral consent, overview and background;
both introduction/tutorial/four-task blocks with separate progress pages; neutral
UEQ-S after each block; preference, optional reason and completion. Meowra appears
only within her condition. Confirm no hints or correctness feedback, no repeated
task set, and no transition time inside task response time.

Historical compiler-feedback assets and Agent Persona Instrument code are stored
outside Unity under `../docs/archive/`. They do not compile or load in this study.
