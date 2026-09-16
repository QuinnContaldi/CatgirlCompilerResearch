# Add your questions without changing C#

The project contains **six blank scenario assets**. No study questions,
explanations or answer keys have been authored for you.

A *ScriptableObject asset* is a saved form in Unity's Project window. It holds
content independently of the Canvas. A *prefab* is a reusable saved UI object.
The TrialPanel prefab reads whichever scenario the experiment manager assigns,
so you do not need six copies of the Canvas or separate pages for each condition.

## 1. Import your code images

1. Drag your screenshots into `Assets/Images/Code` in the Project window.
2. Select each image. In its Inspector, set **Texture Type → Sprite (2D and UI)**,
   **Sprite Mode → Single**, then click **Apply**.
3. Use readable crops and a resolution sufficient for the code. Consider disabling
   texture compression and increasing Max Size if Unity makes small text blurry.
4. Do not put the answer key or condition-specific explanation in the code image:
   the same code image is used for that scenario in every condition.

## 2. Fill the six scenario assets

Select `Assets/Data/Scenarios/Scenario01.asset` through `Scenario06.asset`.
Each Inspector has these fields:

| Field | What you provide |
| --- | --- |
| Scenario ID | A unique stable ID; defaults to `scenario-01` through `scenario-06`. |
| Code Image | Drag your imported Sprite here. |
| Question | Your comprehension prompt. |
| Answer A, B, C, D | The four answer texts in fixed order. |
| Correct Answer | Explicitly select A, B, C or D. It starts as **Unassigned**. |
| Raw Feedback | The raw compiler diagnostic for this scenario. |
| Neutral Feedback | Your neutral explanation. |
| Meowra Feedback | Your matched persona explanation. |

The Inspector reports the first missing field. Fill all three feedback texts for
**each** scenario because the stimulus sets rotate scenarios between conditions.
Keep Neutral and Meowra technically matched as required by the design document.
Text is displayed literally, so code-like `<...>` fragments are not treated as
rich-text formatting.

You can create replacement assets through **Assets → Create → Meowra → Scenario**.
Then assign them in the study definition. Merely creating a new asset does not
add it to the session automatically.

**Drag the image into the scenario asset's Code Image field**, rather than onto
the TrialPage image in the scene. TrialView fills the scene's Image and Text
components from the selected scenario at runtime; direct content edits to those
components are overwritten when a trial begins.

## 3. Configure the study asset

Open `Assets/Data/StudyDefinition.asset`:

- Keep exactly six distinct scenarios in the **Scenarios** list.
- Slots 1–2 form pair 1; slots 3–4 pair 2; slots 5–6 pair 3. The Inspector numbers
  array elements from 0, so Element 0 is human-readable slot 1.
- Fill **Meowra Introduction** with your reviewed text. The text is deliberately blank.
- **Meowra Portrait** already references the imported Dr. Meowra sprite and can be replaced.

The scene's root `ExperimentManager` references this study asset. You can drag a
replacement Study Definition onto that component if you create a separate version.

## 4. Pick the researcher-menu assignment

Press Play in `Assets/Scenes/Experiment.unity`.

| Order | Block sequence |
| --- | --- |
| 1 | Raw → Neutral → Meowra |
| 2 | Raw → Meowra → Neutral |
| 3 | Neutral → Raw → Meowra |
| 4 | Neutral → Meowra → Raw |
| 5 | Meowra → Raw → Neutral |
| 6 | Meowra → Neutral → Raw |

Choose the stimulus set independently:

| Set | Raw scenarios | Neutral scenarios | Meowra scenarios |
| --- | --- | --- | --- |
| 1 | Slots 1–2 | Slots 3–4 | Slots 5–6 |
| 2 | Slots 3–4 | Slots 5–6 | Slots 1–2 |
| 3 | Slots 5–6 | Slots 1–2 | Slots 3–4 |

Each session uses all six scenarios **once**, with two per condition. The order
controls when conditions appear; the set controls which scenarios receive each
condition. The mapping is deterministic and retained in Session. It does not
randomize or automatically assign participants. Review your chosen pair grouping
before data collection; the code implements the protocol's rotation requirement
without choosing your question content or difficulty matching for you.

**Preview layout** works with the blank templates and uses visible bracketed
placeholders. Its banner identifies the session as unscored. Select A–D and
Submit to walk through all six slots. **Start scored session** becomes available
when all six assets and the Meowra introduction/portrait validate. This checks
trial content, not completion of the full research protocol.

## 5. Selection and scoring

- No answer is preselected. Submit stays disabled until a choice is selected.
- Only one answer can be selected. Clicking it again clears the selection and
  disables Submit.
- Submit records the answer once, advances, and clears the next trial's selection.
- Submitted trials have no Back button, preventing accidental re-answering.
- Correct = 1, incorrect = 0. No correctness messages or scores appear in the
  participant-facing UI.
- Preview sessions never contribute scored responses, even if assets have keys.

While still in Play mode, select the root **ExperimentManager**, then expand
**Session** in its Inspector. You can inspect the order/set, planned scenario IDs
and conditions, responses, Correct Count and Scored Count. Returning to the menu
retains those results; starting the next session replaces them.

**There is no disk logging yet.** Stopping Play mode or closing the app loses
responses. This is an authoring/testing skeleton, not the final data-collection app.

## 6. Edit the UI template

Double-click `Assets/Prefabs/TrialPanel.prefab` to edit its layout in Prefab mode:

```text
TrialPanel
  Progress
  TrialScroll
    Viewport
      Content
        CodeHeading
        CodeArea
          CodeImage
          CodePlaceholder
        FeedbackHeader
          Heading
          MeowraPortrait
        Explanation
        QuestionPrompt
        Answers
          AnswerA / AnswerB / AnswerC / AnswerD
  SubmitButton
```

The Inspector's TrialView component has serialized references to these controls.
Keep those references connected if you rename or rearrange objects. The image
preserves its aspect ratio. The content scrolls vertically so longer explanations
and answers can expand without overlapping. Test your longest real content and
code-image readability at the actual study resolution.

All conditions share this prefab, typography, spacing and answer controls. The
Meowra portrait is only shown in her condition. Space for the feedback heading
is reserved consistently so the other content does not jump between conditions.

The surrounding pages live under `Canvas/Pages` in the scene. Welcome,
Instructions and Evaluation are explicitly marked placeholders for later work.
The Meowra introduction is inserted immediately before her block, regardless of
whether she is first, second or third. No questionnaire items, consent text,
practice scenarios, timers or statistical analysis have been added.

## Check your work

1. Preview orders 1 and 5 to see Meowra last and first.
2. Check that each image, explanation and all four answers are readable; scroll
   to the final answer and verify Submit stays available outside the scroll area.
3. After authoring all six assets, run a scored walkthrough with known correct
   and incorrect selections and inspect Session in Play mode.
4. Run **Tools → Experiment → Run Smoke Check** to exercise all 18 assignments
   automatically with temporary synthetic data. This does not assess the scientific
   quality of your authored content.

Next development step after reviewing this structure: incremental local response
saving. Questionnaires and the other protocol pages can then fill the reserved
flow positions without moving scoring or condition logic into the UI scripts.
