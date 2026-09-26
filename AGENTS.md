# AGENTS.md

## Purpose

Rewrite the existing Unity project to implement the **new participant-experience experiment**.

The old Raw/Neutral/Meowra compiler-feedback experiment is obsolete.

The canonical research protocol is:

- `Research_Design.md`

Read `Research_Design.md` before making any experimental-design decision.

This file contains the implementation constraints Codex must preserve while rewriting the Unity project.

---

## Study Design

This is a **within-subject crossover study** with two conditions:

1. **Neutral**
2. **Dr. Meowra**

Every participant completes both conditions.

There are two matched programming task sets:

- Set A: 4 questions
- Set B: 4 questions

Use exactly these crossover assignments:

| Assignment | Block 1 | Block 2 |
| --- | --- | --- |
| 1 | Neutral + Set A | Meowra + Set B |
| 2 | Meowra + Set A | Neutral + Set B |
| 3 | Neutral + Set B | Meowra + Set A |
| 4 | Meowra + Set B | Neutral + Set A |

A participant must never receive the same task set twice.

---

## Required Participant Flow

```text
Neutral informed consent
-> background questions
-> crossover assignment
-> Block 1 introduction
-> Block 1 tutorial
-> Task 1
-> transition/progress
-> Task 2
-> transition/progress
-> Task 3
-> transition/progress
-> Task 4
-> block completion
-> neutral UEQ-S
-> Block 2 introduction
-> Block 2 tutorial
-> Task 1
-> transition/progress
-> Task 2
-> transition/progress
-> Task 3
-> transition/progress
-> Task 4
-> block completion
-> neutral UEQ-S
-> final preference
-> open-ended "why"
-> completion
```

---

## Frozen Condition Rules

| Component | Neutral | Dr. Meowra |
| --- | --- | --- |
| Actual informed consent | Neutral for everyone | Neutral for everyone |
| Condition introduction | Administrative | Character-mediated |
| Tutorial information | Same | Same |
| Tutorial presentation | Neutral | Meowra |
| Avatar | No | Yes |
| Tasks | Matched A/B | Matched A/B |
| Task structure | Identical | Identical |
| Hints | None | None |
| Correctness feedback | None | None |
| Progress | Neutral | Encouraging |
| Transitions | Administrative | Playful/supportive |
| UEQ-S screen | Neutral | Neutral |
| UEQ-S | All 8 items | All 8 items |
| Accuracy | Logged | Logged |
| Response time | Logged | Logged |

Dr. Meowra is the **study host**, not a programming tutor.

She may:
- greet the participant;
- explain the section;
- present the tutorial conversationally;
- acknowledge progress;
- encourage the participant to continue;
- appear visually throughout the Meowra block.

She must not:
- explain how to solve a programming task;
- provide hints;
- reveal correctness;
- praise a specific answer as correct;
- change the underlying task content.

---

## Measures

After **each condition**, collect all 8 UEQ-S items.

Primary outcome:
- UEQ-S Hedonic Quality

Secondary outcomes:
- UEQ-S Pragmatic Quality
- task accuracy
- task response time

After both conditions:

1. Ask:
   - `If you were invited to participate in another programming-language study of similar length and difficulty, which study format would you prefer?`
   - Options: `Neutral`, `Dr. Meowra`

2. Ask:
   - `Why did you prefer that study format? Please describe anything about the presentation or interaction that influenced your choice.`

The Agent Persona Instrument is no longer used.

---

## Timing

For each programming task:

1. fully display the task;
2. start the timer;
3. participant reads/selects an answer;
4. participant presses Submit;
5. stop the timer.

Do not include:
- Meowra transition text;
- progress messages;
- tutorial time;
- UEQ-S time

inside task response time.

---

## Architecture

Do **not** build separate duplicated Neutral and Meowra task scenes.

Separate these concerns:

```text
task content
assignment logic
condition presentation
survey logic
session state
timing
data logging
```

Preferred structure:

- `ExperimentManager`
  - controls study progression
- `AssignmentManager`
  - assigns one of the four crossover cells
- `ParticipantSession`
  - stores participant/session state
- `TaskManager`
  - loads Set A / Set B task data
- `ConditionView`
  - renders Neutral vs. Meowra framing
- `SurveyManager`
  - handles UEQ-S, preference, open response
- `TimerManager`
  - records task response time
- `DataLogger`
  - saves session data incrementally

The condition renderer should receive:

```text
condition
task_set
task_index
```

and render the same task data under the correct presentation layer.

---

## Task Data

Task content should be data-driven rather than hard-coded into scene logic.

Each task should contain fields such as:

```text
task_id
task_set
matched_pair_id
error_category
code
prompt
option_a
option_b
option_c
option_d
correct_option
```

The same task object must be usable under either Neutral or Meowra presentation.

---

## Data Logging

At minimum save:

```text
participant_id
assignment_cell
condition_order
task_set_by_condition

condition
task_id
selected_answer
correct_answer
is_correct
task_response_time_ms

neutral_ueqs_1 ... neutral_ueqs_8
meowra_ueqs_1 ... meowra_ueqs_8

final_preference
open_response

study_version
unity_version
session_start_utc
session_end_utc
```

Save incrementally after meaningful participant actions.

Do not wait until the end of the experiment to write the only copy of the session.

---

## Rewrite Strategy

Before changing code:

1. inspect the existing Unity project;
2. identify reusable systems;
3. identify obsolete systems from the old experiment;
4. produce a short rewrite plan;
5. then implement incrementally.

Reuse working infrastructure where sensible:
- navigation;
- session state;
- data logging;
- timers;
- reusable UI controls.

Remove/refactor obsolete:
- Raw compiler-feedback condition;
- Neutral AI explanation condition;
- old three-condition assignment logic;
- Agent Persona Instrument;
- old compiler-feedback generation/display logic.

Do not:
- create a nested Git repository;
- add live AI/LLM calls;
- add game mechanics;
- add points or correctness rewards;
- change validated UEQ-S wording;
- invent new experimental measures;
- silently alter the protocol.

If implementation requirements conflict with `Research_Design.md`, stop and flag the conflict rather than improvising a new study design.

---

## Immediate Goal

Rewrite the current Unity project so it implements the new two-condition crossover experiment faithfully.

First deliver:
1. an inventory of reusable vs obsolete code;
2. the proposed new scene/state flow;
3. the proposed data model;
4. the implementation order.

Then begin the rewrite.


## Content authoring and legacy archive

`Research_Design.md` is the sole scientific protocol. Do not create another protocol.
Edit condition text in `study-content/Neutral_Host.md` and `Meowra_Host.md`, shared
participant copy in `study-content/Study_Text.md`, consent in `Consent_Text.md`, and
tasks/keys in `Stimuli/SetA` and `Stimuli/SetB`. Use Unity's **Tools → Experiment →
Import Study Content** to regenerate runtime assets. Do not put dialogue defaults
in scripts or edit imported text independently. Preserve validated survey wording.

`docs/archive/` contains obsolete compiler-feedback and persona-instrument material;
its historical instructions do not apply to the current experiment. `docs/Content_Review.md`
is a dated review snapshot, not a protocol or content source. Preserve existing
Unity GUIDs and historical condition IDs when maintaining runtime compatibility.
