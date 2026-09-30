# AGENTS.md

## Purpose

Maintain the Unity implementation of the **two-condition participant-experience experiment**.

The old Raw/Neutral/Meowra compiler-feedback experiment is obsolete.

The canonical research protocol is:

- `Research_Design.md`

Read `Research_Design.md` before making any experimental-design decision.

This file contains the implementation constraints Codex must preserve while maintaining the Unity project. Its Meowra voice guide supports content authoring; it is not a second scientific protocol.

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

Preserve the unresolved assignment-timing discrepancy described in protocol section 64 as an issue to resolve before the build freeze. A personality edit must not silently move assignment selection or otherwise resolve it.

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

## Dr. Meowra: Personality and Voice

### Character brief

Dr. Meowra is a cheerful, slightly theatrical catgirl researcher who enjoys welcoming people into her study. She speaks in the first person, takes pride in being a good host, and makes short administrative moments feel personable.

Her voice should be **warm, recognizably feline, conversational, and lightly mischievous**. She can sound delighted to see the participant and amused by her own cat puns. Her enthusiasm concerns hosting and participation; it never depends on how well the participant answers.

Write her dialogue as dialogue spoken by this character. A generic administrative sentence with "nya" pasted onto the end is a weak default. Give the line a natural greeting, a conversational rhythm, or a small feline flourish while keeping the functional message easy to identify.

Useful traits:

- **Welcoming:** "Hi there, nya! I'm Dr. Meowra, your host for this section."
- **Playful:** "My paws are ready for the next page!"
- **Companionable:** "We'll move through this section together."
- **Appreciative:** "Thanks for joining me, meow!"
- **Clear:** "Select one answer, then press Submit."

She treats participants as adults. Keep normal grammar and an approachable research-host voice.

### Make the catgirl voice audible

Use a rotating vocabulary rather than repeatedly stripping out feline expressions:

| Expression | Good use |
| --- | --- |
| `nya!` | A greeting, brief upbeat interjection, or transition ending |
| `meow!` | A cheerful greeting or acknowledgement of a completed section |
| `purrfect` | A clearly identified workflow milestone, never an answer judgement |
| `paws` | A short joke about the host or the next page |
| `paw-some` | Occasional playful description of the host's welcome or gratitude |

For short greetings and transitions, one feline expression is a useful starting point. A slightly longer introduction can carry two if it still reads naturally. These are authoring heuristics, **not empirically validated frequencies or mandatory quotas**. Do not put a cue in every sentence or stack several puns into the same line.

Use "nya" and "meow" as small interjections. Keep essential controls and instructions literal: `Submit` stays `Submit`, and answer labels remain unchanged. Keep code, prompts, options, and questionnaire wording free of catgirl substitutions.

The protocol's caution against **excessive** "nya" and constant puns does not require eliminating them. Make her identity noticeable in her hosting moments while keeping the overall presentation mildly playful and readable.

### Encouragement concerns participation

Celebrate observable progress through the section. Thank participants for their time. Express readiness for the next page. These statements can be the same regardless of which answer was selected.

Allowed:

- "One of four questions submitted, nya! Three remain."
- "We're halfway through this section—two submitted, two remaining. Thanks for joining me!"
- "Purrfect—we've reached the halfway point! Two of four questions are submitted; two remain."
- "Three of four questions submitted! My paws are ready for the last page."
- "Four of four questions submitted, meow! Thanks for spending this section with me."

Use `purrfect` only when the next clause makes its referent unmistakably a workflow milestone. "Purrfect answer!", "Purrfect choice!", and an isolated "Purrfect!" immediately following submission can sound like correctness feedback and must not be used.

Avoid ungrounded ability or performance praise such as "You're a coding genius!", "You nailed it!", or "You're doing amazingly!". There is no need to suppress friendliness to avoid those claims: a cheerful "Thanks for joining me, nya!" is sufficient.

Do not make encouragement contingent on accuracy, response time, the selected option, or the error category. Do not infer frustration or confidence from a participant's behaviour. Do not introduce "take your time", "hurry", "double-check", or other condition-specific advice that could change task strategy or pace.

### Humor and warmth

Prefer a tiny joke about Meowra herself or her hosting role:

- "My paws are ready for the next page!"
- "I'll handle the greetings; you handle the buttons, nya!"

Keep jokes short enough that participants can immediately see what happens next. Avoid long stories, technical jokes about the code, mockery, flirtation, baby talk, and obligatory catchphrases.

Use invitations to continue without social pressure. Never ask participants to keep going to make her happy, suggest that quitting would disappoint her, or ask for favourable survey ratings. Do not tell participants that this condition is supposed to be more fun, interesting, or enjoyable.

### Where her voice belongs

| Screen | Authoring guidance |
| --- | --- |
| Actual consent, overview, background | Preserve shared neutral copy; no Meowra persuasion |
| Meowra introduction | Strongest opportunity for greeting, identity, and a small feline flourish |
| Meowra tutorial | Same functional information as Neutral, in conversational language |
| Meowra task screens | Preserve approved task content and layout; maintain her approved visual presence |
| Meowra progress/transition | Short, encouraging, feline wording tied only to completed-task count |
| Meowra block completion | Friendly thanks and factual completion acknowledgement |
| UEQ-S, final preference, open response | Neutral copy and presentation; Meowra disappears |

Do not add new task-screen banter, popups, animation, audio, delays, or interaction steps merely to strengthen the persona. Use the existing approved content slots. During timed tasks, preserve task readability and avoid additional commentary that competes with reading the code.

### Candidate dialogue examples

These are **authoring examples**, not automatically approved replacements for runtime text. Before using them, compare each line with the current Neutral source and preserve all functional information, navigation instructions, and screen-specific facts. Do not assume these examples cover instructions that have not been inspected.

**Condition introduction**

> Hi there, nya! I'm Dr. Meowra, your host for this section. We'll go through four short programming questions together. My paws are ready—let's get started!

**Tutorial**

> Here's how this section works, nya! For each question, review the code and choose the answer that best identifies the primary issue. Select one answer, then press Submit. You'll complete four questions in this section.

**After question 1**

> One of four questions submitted, nya! Three remain. On to the next page!

**After question 2**

> Purrfect—we've reached the halfway point! Two of four questions are submitted; two remain.

**After question 3**

> Three of four questions submitted! One remains. My paws are ready for the last page!

**Block completion**

> Four of four questions submitted, meow! This section is complete. Thanks for joining me!

Keep any existing instruction about the next neutral questionnaire factually intact when editing completion copy. Do not add a request for positive ratings or a persona farewell to the questionnaire itself.

### Writing procedure

When asked to improve Meowra dialogue:

1. Read `Research_Design.md`, the current `study-content/Meowra_Host.md`, and the corresponding Neutral source.
2. Identify each screen's required facts and actions.
3. Rewrite the hosting voice with a natural conversational sentence and, where suitable, a brief feline cue.
4. Verify that warmth has not introduced a hint, performance judgement, pace instruction, or new functional information.
5. Ensure each displayed line is fixed by its existing screen/progress slot. Examples can vary while authoring; do not randomize or generate wording during participant sessions.
6. Edit the canonical content source, import it, and run the existing navigation smoke check when the Unity project is available.

Do not interpret "research integrity" as a reason to flatten all approved Meowra dialogue into administrative copy. Tone, social framing, encouragement, and playful language are part of the defined treatment. Preserve those differences deliberately.

---

## Research Rationale for the Voice Guide

This section informs researchers and content authors. It is not participant-facing dialogue and does not change the protocol, hypotheses, measures, or analysis plan.

**Character presence and affective experience.** Lester et al. [1] studied animated pedagogical agents with 100 middle-school students. Students rated the agents positively, including a muted agent that provided no advice. The authors also cautioned about distraction and generalization. This motivates investigating an appealing character separately from task assistance; it does not establish that adult PL-study participants will prefer Meowra.

**Social hosting and desire for future interaction.** Bickmore and Picard [2] compared a relational agent with a task-oriented agent in a month-long exercise-adoption study with 101 users. The relational agent was liked and trusted more, and users expressed greater desire to continue interacting with it. This provides a rationale for warmth and social framing, while the different domain and duration limit transfer to this short experiment.

**Consistent linguistic personality.** Walker, Cahn, and Whittaker [3] treat linguistic style as an important component of agent character and propose mechanisms for producing socially expressive utterances. This supports designing a coherent voice rather than relying only on an avatar. It is a linguistic-style proposal, not evidence that cat puns improve UX.

**Design inference for this implementation:** use a stable, welcoming character voice, brief feline expressions, and acknowledgement of participation. The specific vocabulary, cue frequency, and dialogue examples above are design choices to review in the pilot. None of these papers establishes that "nya", "meow", or "purrfect" improves Hedonic Quality, recruitment, retention, accuracy, or response time. The experiment evaluates the complete Meowra presentation package; it cannot isolate the effect of a particular word.

### References

[1] Lester, J. C., Converse, S. A., Kahler, S. E., Barlow, S. T., Stone, B. A., & Bhogal, R. S. (1997). The persona effect: Affective impact of animated pedagogical agents. In *Proceedings of the ACM SIGCHI Conference on Human Factors in Computing Systems* (pp. 359–366). ACM. [Official conference paper](https://chi1997.acm.org/proceedings/paper/jl.html).

[2] Bickmore, T. W., & Picard, R. W. (2005). Establishing and maintaining long-term human-computer relationships. *ACM Transactions on Computer-Human Interaction, 12*(2), 293–327. [Author manuscript](https://www2.ccs.neu.edu/research/rag/publications/05_CHI_BTPR.pdf). [MIT publication page](https://www.media.mit.edu/publications/establishing-and-maintaining-long-term-human-computer-relationships/) (lists the earlier manuscript date).

[3] Walker, M. A., Cahn, J. E., & Whittaker, S. J. (1997). *Improvising linguistic style: Social and affective bases for agent personality* [Preprint]. arXiv. https://arxiv.org/abs/cmp-lg/9702015

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

## Implementation Maintenance

The two-condition rewrite is implemented. For any further substantial rewrite:

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

## Current Goal

Keep the implemented two-condition crossover flow faithful to `Research_Design.md`.
Use the existing navigation smoke check to validate changes. Remaining collection
preparation includes consent completion, pilot review, and an explicit content/build
freeze; see `docs/Running_Sessions.md` and protocol sections 55–56.

## Content authoring and historical material

`Research_Design.md` is the sole scientific protocol. Do not create another protocol.
Edit condition text in `study-content/Neutral_Host.md` and `Meowra_Host.md`, shared
participant copy in `study-content/Study_Text.md`, consent in `Consent_Text.md`, and
tasks/keys in `Stimuli/SetA` and `Stimuli/SetB`. Use Unity's **Tools → Experiment →
Import Study Content** to regenerate runtime assets. Do not put dialogue defaults
in scripts or edit imported text independently. Preserve validated survey wording.

This file guides future authors; changing it alone does not change the dialogue displayed in Unity. Apply reviewed wording in `study-content/Meowra_Host.md`, then import it. Keep literature references in researcher-facing documentation, not in Meowra's participant dialogue.

Obsolete compiler-feedback and persona-instrument material has been removed from
the working tree. Historical versions in Git are not current instructions. `docs/Content_Review.md`
is a dated review snapshot, not a protocol or content source. Preserve existing
Unity GUIDs and historical condition IDs when maintaining runtime compatibility.

Do not silently change experimental content after formal collection begins. Before collection, review the revised wording in the pilot and include the exact final text in the content/build freeze.
