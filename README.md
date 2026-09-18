# Can a Catgirl Make Compiler Errors Better?

<p align="center">
  <img src="DrMeowra.jpg" alt="Dr. Meowra" width="340">
</p>

<p align="center">
  <strong>Dr. Meowra Persona-Based Compiler Feedback Study</strong><br>
  Programming Languages + Human-Computer Interaction + AI
</p>

---

## Current Protocol Status

**Current protocol date:** September 18, 2026  
**Study type:** Small exploratory within-subject HCI study  
**Implementation:** Unity  
**Primary measurement:** UEQ-S  
**Persona characterization:** Agent Persona Instrument (Credible + Engaging subscales)  
**Planned sample:** approximately 15–20 participants, with 18 as the practical target

This README is intended to be the **canonical high-level description of the current experiment** and the best starting point for another researcher who wants to understand or reproduce the study.


# 1. Study Overview

Compiler diagnostics are an interface between a programming language implementation and a programmer. They are technically generated messages, but they are consumed by humans.

A conventional compiler error may be accurate while still being terse, impersonal, difficult to interpret, or unpleasant to work with. Modern large language models make it practical to transform raw diagnostics into contextual natural-language explanations. That creates a new HCI question:

> **If the technical explanation is already good, does giving that explanation a recognizable social persona improve the user experience of compiler feedback?**

This study explores that question using **Dr. Meowra**, a friendly anthropomorphic catgirl programming assistant.

The experiment compares three feedback conditions:

| Condition | Participant sees | Scientific purpose |
| --- | --- | --- |
| **Raw** | Conventional compiler diagnostic | Technical baseline |
| **Neutral** | Human-centered natural-language explanation | Measures the benefit of explanatory rewriting |
| **Dr. Meowra** | Approximately matched technical explanation delivered through a persistent social persona | Measures the added effect of persona/social framing |

The conceptual progression is:

```text
Raw Compiler
     |
     | add human-centered explanation
     v
Neutral Explanation
     |
     | add social/persona framing
     v
Dr. Meowra
```

This structure allows the study to ask two distinct questions rather than comparing only a bad baseline with a much richer treatment.

---

# 2. Research Framing

The project is primarily an **HCI + AI study in a programming-languages context**.

It is not intended to prove that:

- Dr. Meowra makes programmers faster;
- persona-based feedback improves debugging performance;
- cat ears themselves improve user experience;
- a single wording choice causes an effect;
- an LLM is intrinsically better than a compiler diagnostic; or
- results from a small student sample generalize to all programmers.

Instead, the study asks whether **persona-based compiler feedback is a promising UX design direction worth deeper investigation**.

The broader PL+HCI motivation is that programming tools are often evaluated primarily in terms of correctness, performance, language features, and technical capability. Those tools are also interactive systems. Their users experience trust, frustration, clarity, engagement, cognitive effort, and preference.

Compiler feedback is therefore not only technical output. It is part of the programming-language user experience.

---

# 3. Research Question and Hypotheses

## Primary Research Question

> **RQ1. How does compiler-feedback presentation style affect programmers’ subjective user experience when the amount of technical guidance and explanatory content are controlled as closely as practical across conditions?**

## H1 — Information-Design Effect

> **Neutral human-centered explanations will receive higher UEQ-S Pragmatic Quality scores than Raw compiler diagnostics.**

This comparison asks whether explanatory rewriting improves the practical experience of compiler feedback.

```text
Raw Pragmatic Quality
        vs.
Neutral Pragmatic Quality
```

## H2 — Persona Effect

> **Dr. Meowra will receive higher UEQ-S Hedonic Quality scores than Neutral explanations.**

This comparison asks whether adding a recognizable social persona changes the experiential quality of the interaction after the technical explanation has already been improved.

```text
Neutral Hedonic Quality
        vs.
Dr. Meowra Hedonic Quality
```

The **Neutral vs. Dr. Meowra comparison** is the most important comparison for the persona argument because the technical guidance is intended to remain approximately matched.

A direct Raw vs. Meowra comparison can still be shown descriptively, but it is not the cleanest causal test of persona because those two conditions differ in both explanation quality and persona framing.

---

# 4. Why Dr. Meowra Is a “Persona Package”

Dr. Meowra is intentionally not reduced to one isolated design element.

The treatment includes:

- a persistent character identity;
- the name **Dr. Meowra**;
- a static visual avatar;
- feline `kemonomimi`/catgirl visual cues;
- warm and encouraging language;
- a competent and nonjudgmental communication style;
- a brief introduction before the Meowra condition; and
- continuity across the two Meowra tasks.

The current experiment therefore evaluates **persona-based presentation as a composite treatment**.

The study does **not** claim that any one component caused an observed effect.

For example, if Meowra receives higher Hedonic Quality ratings, the current experiment cannot determine whether that increase came primarily from:

- the avatar;
- the name;
- the catgirl visual design;
- supportive wording;
- social framing;
- encouragement;
- character continuity; or
- the combination of these features.

That limitation is intentional and acceptable for an exploratory first study.

The first question is:

> **Does a strong persona treatment appear promising at all?**

If the answer is encouraging, later studies can decompose the package using more targeted factorial or component-level designs.

Possible follow-up studies could independently manipulate avatar/no avatar, warm/neutral wording, named/anonymous assistant, encouragement/no encouragement, or different visual personas.

---

# 5. Dr. Meowra Design Principles

Dr. Meowra should be recognizable and personable without becoming a parody that overwhelms the technical content.

Her intended personality is:

- competent;
- knowledgeable;
- kind;
- encouraging;
- supportive;
- nonjudgmental;
- mildly playful; and
- technically accurate.

Avoid excessive:

- “nya” language;
- meme language;
- baby talk;
- flirtation;
- jokes that distract from the task;
- emotional overreaction; or
- unnecessary verbosity.

Dr. Meowra is not being designed around a specific anime personality archetype such as *moe*. The catgirl visual identity is used as a distinctive anthropomorphic character design, while the behavioral persona emphasizes competence, warmth, and support.

The personality should be strong enough that participants clearly experience a character, but the **technical explanation must remain the primary content**.

---

# 6. Experimental Design

## Design Type

The study uses a **within-subject / repeated-measures design**.

Every participant experiences:

- Raw;
- Neutral; and
- Dr. Meowra.

Each participant therefore serves as their own comparison.

This is particularly useful for a small study because people differ substantially in:

- programming experience;
- confidence;
- rating behavior;
- tolerance for compiler diagnostics;
- familiarity with C;
- familiarity with AI assistants; and
- general preference for playful interfaces.

A within-subject design reduces some of that between-person variability because the same participant evaluates every condition.

## Number of Tasks

The baseline design contains **six short compiler-error tasks**:

- 2 Raw tasks;
- 2 Neutral tasks;
- 2 Dr. Meowra tasks.

Participants do not type or repair code directly.

Instead, they read a small code example, view the assigned feedback condition, and answer a lightweight multiple-choice interpretation/comprehension question.

The objective of the task is to expose the participant to the feedback style in a controlled way so they can meaningfully evaluate the experience.

This is not intended to be a full debugging-performance experiment.

---

# 7. Why Participants Do Not Directly Edit Code

Direct code editing introduces additional constructs that are not central to the current study, including:

- typing speed;
- syntax recall;
- IDE familiarity;
- code-navigation skill;
- debugging strategy;
- trial-and-error behavior;
- familiarity with build systems;
- willingness to experiment;
- task-specific programming expertise.

The present study is focused on **reading, interpreting, and experiencing compiler feedback**.

A controlled multiple-choice task allows the study to isolate that interaction more cleanly.

This design comes with an ecological-validity limitation: answering a controlled question in Unity is not the same as debugging a real program in an IDE.

That limitation should be reported directly. Real debugging performance is a natural follow-up study.

---

# 8. Scenario Design

The planned six scenario families are:

| Scenario family | Example |
| --- | --- |
| Statement termination | Missing semicolon |
| Name resolution | Undeclared identifier |
| Delimiter matching | Missing parenthesis or brace |
| Type use | Simple incompatible type |
| Function call | Wrong number/type of arguments |
| Operator use | Invalid operand/operator pairing |

Each scenario should:

- contain one primary error;
- be short enough to understand quickly;
- avoid cascading diagnostics;
- avoid obscure language behavior;
- avoid relying heavily on programmer intent;
- have a clearly defensible correct interpretation; and
- use a compiler message that can be frozen exactly.

Each scenario record should eventually contain at least:

```text
scenario_id
error_family
code_snippet
raw_diagnostic
neutral_explanation
meowra_explanation
question
answer_options
correct_answer
```

The six final stimuli should be stored in a version-controlled structured format rather than embedded directly into UI code.

---

# 9. Illustrative Scenario

Example code:

```c
int main() {
    int total = 10
    printf("%d\n", total);
}
```

Example feedback treatments:

### Raw

```text
error: expected ',' or ';' before 'printf'
```

### Neutral

```text
The compiler reached `printf` while it was still expecting the previous
statement to end. Inspect the statement immediately above `printf` for a
missing terminator.
```

### Dr. Meowra

```text
Dr. Meowra: The compiler reached `printf` while it was still waiting for
the previous statement to finish. Take a look at the line just above
`printf` for a missing terminator. You're close—you've got this!
```

The Meowra version should **not add extra technical information** that is absent from Neutral.

For example, saying “this is only a small syntax error” adds a technical classification and should be avoided unless the Neutral condition communicates the same information.

---

# 10. AI Generation and Stimulus Freezing

The Neutral and Dr. Meowra explanations should be produced through the **same underlying generation pipeline**.

The model should receive the same:

- source code;
- compiler output;
- code context;
- technical constraints; and
- target level of guidance.

The persona treatment should change the **social presentation**, not the underlying technical assistance.

Before data collection:

1. generate all candidate outputs;
2. manually review them for accuracy;
3. check Neutral/Meowra pairs for technical equivalence;
4. remove accidental answer leakage;
5. approximately match message length and amount of guidance;
6. freeze the final text;
7. record the model/version/date;
8. save the prompts/persona instructions; and
9. version-control the final stimuli.

The participant-facing application should **not call a live LLM**.

No live AI calls are needed during the experiment.

Freezing outputs avoids:

- model drift;
- stochastic differences;
- latency differences;
- API failures;
- accidental differences in technical quality;
- changing outputs during data collection; and
- reproducibility problems.

A replication should be able to use the exact frozen messages that participants originally saw.

---

# 11. Condition-Control Requirements

Neutral and Meowra should be matched as closely as practical on:

| Feature | Neutral | Dr. Meowra |
| --- | --- | --- |
| Technical facts | Same | Same |
| Amount of guidance | Same | Same |
| Resolution hint | Same level | Same level |
| Approximate length | Similar | Similar |
| Complete answer | Prohibited | Prohibited |
| Tone | Professional / impersonal | Warm / supportive |
| Named identity | None | Dr. Meowra |
| Avatar | None | Dr. Meowra image |
| Persistent social role | None | Present |

The rest of the interface should remain visually consistent.

Do not make the Meowra condition easier to read simply because it is visually prettier.

Where possible, preserve the same:

- code font;
- code size;
- question layout;
- button positions;
- panel sizes;
- background;
- spacing;
- response controls; and
- navigation behavior.

---

# 12. Persona Introduction

The general study tutorial must remain **condition-neutral**.

Participants should learn how to:

- read a scenario;
- select an answer;
- submit;
- use the UEQ-S screen; and
- move forward.

Dr. Meowra should not appear in that neutral tutorial.

Instead, a short Dr. Meowra introduction is inserted **immediately before the participant begins the Meowra condition**, regardless of whether Meowra is first, second, or third.

The introduction should establish:

- her name;
- her role as a programming assistant;
- her visual identity;
- her supportive personality; and
- continuity across the next two tasks.

Example concept:

```text
Hi! I'm Dr. Meowra.

I'm a programming assistant, and my job is to help explain what the compiler
is trying to tell you. I'll be helping with the next examples.

Read each explanation normally and choose the response that makes the most
sense to you.
```

The introduction is part of the **persona treatment**.

---

# 13. Counterbalancing

The condition order must not always be:

```text
Raw -> Neutral -> Meowra
```

All six possible condition orders should be used:

| Order | Sequence |
| ---: | --- |
| 1 | Raw → Neutral → Meowra |
| 2 | Raw → Meowra → Neutral |
| 3 | Neutral → Raw → Meowra |
| 4 | Neutral → Meowra → Raw |
| 5 | Meowra → Raw → Neutral |
| 6 | Meowra → Neutral → Raw |

This reduces systematic order effects caused by:

- practice;
- fatigue;
- novelty;
- contrast; and
- learning how the experiment works.

## Scenario Rotation

Condition order alone is not enough.

If one condition always receives easier compiler errors, the task difficulty becomes confounded with the condition.

Use three scenario-assignment sets.

For six scenarios `S1`–`S6`, a simple rotation is:

| Stimulus set | Raw | Neutral | Meowra |
| --- | --- | --- | --- |
| **A** | S1, S2 | S3, S4 | S5, S6 |
| **B** | S3, S4 | S5, S6 | S1, S2 |
| **C** | S5, S6 | S1, S2 | S3, S4 |

Combining:

```text
6 condition orders × 3 stimulus sets = 18 assignment combinations
```

This makes **18 participants** particularly convenient: each participant can be assigned one unique order × stimulus-set combination.

If fewer or more than 18 participants are recruited, cycle through the assignment table as evenly as practical and save each participant's exact assignment.

Prefer deterministic assignment over opaque randomization.

For example:

```text
Participant index 001 -> Order 1 / Set A
Participant index 002 -> Order 1 / Set B
Participant index 003 -> Order 1 / Set C
Participant index 004 -> Order 2 / Set A
...
Participant index 018 -> Order 6 / Set C
```

A replication should preserve the assignment schedule used in the original study.

---

# 14. Participant Population

Participants should have enough programming background to read small C/C-like programs.

A reasonable population for the current exploratory study is:

- undergraduate CS/CSE students;
- novice-to-intermediate programmers; or
- comparable programmers with introductory C/C++ familiarity.

Suggested background variables:

| Variable | Example representation |
| --- | --- |
| Programming experience | categorical bands |
| C/C++ familiarity | self-rated |
| Programming frequency | categorical |
| Prior AI coding-assistant use | yes/no or frequency |
| Student level/coursework | categorical |

These variables are mainly used to describe the sample rather than create a large set of subgroup analyses.

With a sample of approximately 15–20, avoid fragmenting the data into many small experience subgroups.

---

# 15. Sample Size and Power

The practical recruitment target is approximately **18 completed participants**, with **15–20** considered feasible.

This is a small exploratory study.

The study should therefore be described as being designed to detect **large, consistent within-person UX effects**, not subtle effects.

## Power Planning

The planned inferential method is the **paired-samples t-test**.

For paired designs, the planning effect size is commonly expressed as **Cohen's \(d_z\)**:

```text
mean paired difference
----------------------
SD of paired differences
```

Approximate two-sided paired-t sample sizes for 80% power at `alpha = .05` are:

| Assumed paired effect | Approximate completed N |
| ---: | ---: |
| dz = 0.50 | 34 |
| dz = 0.60 | 24 |
| dz = 0.70 | 19 |
| dz = 0.80 | 15 |

Therefore, **N ≈ 18 is not sufficient for reliable detection of a modest dz = 0.60 effect at ordinary alpha = .05**. It is closer to a study powered for an effect around `dz ≈ 0.70`.

Because this study has two planned confirmatory comparisons, Holm correction is planned. Conservatively treating the smallest Holm threshold as approximately `alpha = .025` makes the power requirement stricter. Under that conservative planning assumption, approximately 18 participants corresponds more closely to a **large effect around dz ≈ 0.80**.

The sample-size argument should therefore be:

> This rapid exploratory study is designed to detect large within-participant UX effects. Smaller effects may appear as descriptive trends with wide uncertainty and should motivate a larger follow-up rather than strong causal claims.

Do not choose or exaggerate the persona treatment solely to force statistical significance. The goal is to make the treatment **conceptually strong and clearly recognizable**, then measure the resulting effect honestly.

---

# 16. Participant Flow

The intended participant flow is:

```text
Consent / Study Instructions
        ↓
Programming Background
        ↓
Condition-Neutral Unity Tutorial
        ↓
Practice Scenario
        ↓
Condition Block 1
    ├── Task 1
    ├── Task 2
    └── UEQ-S (8 items)
        ↓
Condition Block 2
    ├── Task 1
    ├── Task 2
    └── UEQ-S (8 items)
        ↓
Condition Block 3
    ├── Task 1
    ├── Task 2
    └── UEQ-S (8 items)
        ↓
Agent Persona Instrument
    ├── Credible (5 items)
    └── Engaging (5 items)
        ↓
Final Preferred Condition
        ↓
Open-Ended “Why?”
        ↓
Debrief / Finish
```

The **Dr. Meowra introduction is dynamically inserted immediately before whichever condition block is assigned to Meowra.**

Estimated session duration should be established by pilot testing rather than assumed. Earlier planning estimates were approximately 12–18 minutes.

---

# 17. Measurement Plan

The current questionnaire battery is intentionally compact.

Each participant completes:

| Measurement | Count |
| --- | ---: |
| UEQ-S after Raw | 8 responses |
| UEQ-S after Neutral | 8 responses |
| UEQ-S after Meowra | 8 responses |
| API Credible subscale | 5 responses |
| API Engaging subscale | 5 responses |
| Final preferred condition | 1 response |
| Open-ended explanation | 1 response |

Total questionnaire-style scaled responses:

```text
24 UEQ-S + 10 API = 34 scaled responses
```

plus:

```text
1 forced preference + 1 open-ended explanation
```

There is **no additional custom Likert battery** in the current protocol.

The six task-answer selections are logged separately as task responses.

---

# 18. UEQ-S

The **User Experience Questionnaire — Short Version (UEQ-S)** is the main UX instrument.

The same complete eight-item UEQ-S is administered after **each condition**.

The English UEQ-S is:

## Pragmatic Quality

```text
obstructive      ○ ○ ○ ○ ○ ○ ○      supportive
complicated      ○ ○ ○ ○ ○ ○ ○      easy
inefficient      ○ ○ ○ ○ ○ ○ ○      efficient
confusing        ○ ○ ○ ○ ○ ○ ○      clear
```

## Hedonic Quality

```text
boring           ○ ○ ○ ○ ○ ○ ○      exciting
not interesting  ○ ○ ○ ○ ○ ○ ○      interesting
conventional     ○ ○ ○ ○ ○ ○ ○      inventive
usual            ○ ○ ○ ○ ○ ○ ○      leading edge
```

The UEQ-S uses seven positions.

The standard scoring maps the response positions to:

```text
-3  -2  -1   0   +1   +2   +3
```

Because the UEQ-S short form is already arranged with the negative pole on the left and positive pole on the right, implementation should preserve that orientation exactly.

## UEQ-S Scale Scores

For each participant and each condition:

```text
Pragmatic Quality = mean(items 1–4)
Hedonic Quality   = mean(items 5–8)
Overall UEQ-S     = mean(items 1–8)
```

The Overall score may be reported descriptively, but it is not a primary hypothesis outcome.

## Primary UEQ-S Use

H1 uses:

```text
Raw Pragmatic vs Neutral Pragmatic
```

H2 uses:

```text
Neutral Hedonic vs Meowra Hedonic
```

The full eight items are still collected in every condition.

That means researchers can also inspect:

- Raw Hedonic;
- Neutral Hedonic;
- Meowra Pragmatic; and
- overall scores

without turning each one into a new confirmatory hypothesis.

## Why UEQ-S Fits This Experiment

UEQ-S was designed for situations where a short UX measurement is needed, including experimental settings where participants evaluate multiple product variants in one session.

That maps well to this study because one participant evaluates three forms of the same underlying compiler-feedback interaction.

---

# 19. Using the UEQ Analysis Tools

The official UEQ resources provide spreadsheets/tools for:

- scoring;
- means;
- standard deviations;
- confidence intervals;
- data-quality checks; and
- benchmark-oriented interpretation.

Use those tools for the UEQ-S scoring and descriptive summaries when helpful.

However, the study's main inferential comparisons are **paired** because the same participant evaluates all three conditions.

Do **not** treat Raw, Neutral, and Meowra as three independent participant samples.

If an official comparison spreadsheet uses an independent/two-sample test, that is not a replacement for the planned paired-samples analysis.

The official UEQ tools and this study's paired t-tests serve different purposes:

```text
UEQ tool:
score and summarize the instrument correctly

paired t-test:
test the planned within-person condition difference
```

---

# 20. Agent Persona Instrument

After all three conditions, participants evaluate **Dr. Meowra specifically** using selected complete subscales from the **Agent Persona Instrument (API)**.

The study uses:

- **Credible** — 5 items
- **Engaging** — 5 items

Total:

```text
10 API responses
```

The API is a **persona-characterization measure**, not the primary treatment-effect measure.

Conceptually:

```text
UEQ-S:
Did the experience differ across conditions?

API:
Did participants actually perceive Dr. Meowra as
a credible and engaging persona?
```

The API provides context for interpreting the persona treatment.

For example:

### Possible pattern A

```text
Meowra Hedonic > Neutral Hedonic
API Engaging = high
API Credible = high
```

Interpretation: the persona condition was experienced positively and the participant also perceived the intended persona qualities.

### Possible pattern B

```text
Meowra Hedonic ≈ Neutral Hedonic
API Engaging = high
API Credible = high
```

Interpretation: participants recognized the persona, but the manipulation did not produce a clear UX improvement in this small sample.

### Possible pattern C

```text
Meowra Hedonic ≈ Neutral Hedonic
API Engaging = low
```

Interpretation: the persona manipulation itself may have been too weak or poorly received.

## API Administration

Use the **published wording and response anchors** from the original instrument.

Do not rewrite the items merely to make them sound more natural in the app.

Preserve the published **5-point Likert response format** unless a later protocol explicitly documents a modification.

The exact API item wording should be sourced from the linked API paper and frozen in the study materials before data collection.

The two selected subscales were chosen because they align well with the current static assistant:

- Credible helps characterize competence/usefulness;
- Engaging helps characterize the affective/social persona.

The Human-like subscale is not planned because some items were designed for richer animated agents and are a poorer fit for a static text-and-image character.

Facilitating Learning is also not part of the current battery because this experiment is not primarily testing learning outcomes.

---

# 21. Final Preference and Open-Ended Response

After the API, participants answer:

> **If you could choose one of these feedback styles for your programming environment, which would you choose?**

Options:

```text
Raw
Neutral
Dr. Meowra
```

Then:

> **Why did you prefer that style?**

The preference result is descriptive.

The open-ended response provides qualitative context that may explain why participants chose a condition.

Possible themes may include:

- clarity;
- professionalism;
- warmth;
- friendliness;
- annoyance;
- distraction;
- trust;
- novelty;
- encouragement;
- efficiency;
- preference for concise output; or
- preference for personality.

Do not invent a fixed codebook before seeing the responses unless preregistration requires one.

A lightweight approach is to code recurring themes transparently after data collection and report representative quotations where permitted by consent/IRB.

---

# 22. Task Responses and Response Time

Each compiler scenario includes a lightweight multiple-choice response.

This serves two purposes:

1. it makes the participant actively interpret the diagnostic rather than passively look at it;
2. it provides a basic sanity check that the feedback is not catastrophically harming comprehension.

With only two tasks per condition, task accuracy is **not** intended to support a strong inferential claim about debugging performance or learning.

Task selections can be reported descriptively if useful.

Unity may also log response time automatically.

Response time is exploratory telemetry, not a primary dependent variable.

Do not add a formal time-based hypothesis unless the protocol is explicitly changed.

---

# 23. Primary Statistical Analysis

The inferential plan is intentionally small.

There are two planned paired comparisons.

## Test 1 — H1

```text
Raw Pragmatic Quality
        vs.
Neutral Pragmatic Quality
```

Use a **paired-samples t-test**.

## Test 2 — H2

```text
Neutral Hedonic Quality
        vs.
Dr. Meowra Hedonic Quality
```

Use a **paired-samples t-test**.

These tests are paired because every participant contributes both scores in each comparison.

---

# 24. How the Paired t-Test Works in This Study

For H1, calculate for every participant:

```text
Neutral Pragmatic - Raw Pragmatic
```

For H2:

```text
Meowra Hedonic - Neutral Hedonic
```

The paired t-test tests whether the **mean within-person difference** is distinguishable from zero.

The key distributional assumption concerns the **paired difference scores**, not whether each condition's raw scores individually form perfect normal distributions.

Before interpreting the t-tests:

- inspect the paired difference distributions;
- look for severe skew;
- inspect for extreme influential outliers;
- use a histogram and/or Q-Q plot; and
- document any exclusions using rules established before looking for significance.

Do not switch between t-tests and Wilcoxon after seeing which produces the smaller p-value.

If the paired-difference assumptions are severely violated, document that issue and consult the statistical plan/advisor before using an alternative sensitivity analysis.

---

# 25. Multiple-Testing Control

There are two planned confirmatory comparisons.

The current plan is to use **Holm correction** across those two p-values.

With two tests, Holm operates approximately as:

1. sort the two p-values;
2. compare the smaller to `0.05 / 2 = 0.025`;
3. if it passes, compare the larger to `0.05`.

This controls the family-wise Type I error rate while preserving more power than simply treating many exploratory outcomes as independent confirmatory tests.

The API, final preference, task accuracy, and open-ended themes are not additional confirmatory hypothesis tests.

---

# 26. Effect Sizes and Confidence Intervals

Do not report only p-values.

For each paired t-test, report:

- Raw/Neutral/Meowra scale means as appropriate;
- standard deviations;
- mean paired difference;
- 95% confidence interval for the paired difference;
- t statistic;
- degrees of freedom;
- Holm-adjusted p-value; and
- a paired standardized effect size such as **Cohen's dz**.

The effect size is particularly important because the sample is small.

A non-significant p-value does not prove that the conditions are equivalent.

A small study may observe an effect estimate in the expected direction with wide uncertainty.

That should be reported as preliminary/exploratory evidence rather than proof.

---

# 27. Interpretation Philosophy

This study should not be reduced to:

```text
p < .05 = success
p >= .05 = failure
```

Interpret the full pattern.

For example:

### Strong positive pattern

```text
Neutral Pragmatic > Raw Pragmatic
Meowra Hedonic > Neutral Hedonic
large paired effects
high API Credible/Engaging
most participants prefer Meowra
open-ended responses describe warmth/engagement
```

This would provide converging exploratory evidence that the persona direction deserves deeper study.

### Mixed pattern

```text
Neutral Pragmatic > Raw
Meowra Hedonic ≈ Neutral
API Engaging high
```

This would suggest that better explanations matter, while the persona may not add a detectable hedonic benefit.

### Suggestive but underpowered pattern

```text
Meowra Hedonic > Neutral descriptively
paired effect is meaningful
confidence interval is wide
Holm-adjusted p > .05
API and preference favor Meowra
```

The correct conclusion would be:

> the small exploratory sample produced a promising directional pattern, but the study is insufficient to make a definitive population-level claim.

That is still useful evidence for designing a larger follow-up.

---

# 28. Unity Application Design

The study application is intentionally a simple page-style Unity interface.

Participants should mostly:

```text
read
select
click
advance
```

This is a research instrument, not a game.

Prioritize:

- reliability;
- identical presentation across conditions;
- simple navigation;
- reproducibility;
- readable code;
- incremental data saving; and
- minimal experimental confounds.

Avoid unnecessary:

- animations;
- complex transitions;
- physics;
- game systems;
- external network dependencies;
- live AI calls; or
- decorative UI differences between conditions.

---

# 29. Suggested Unity Architecture

A reasonable architecture is:

```text
PageManager
ExperimentManager
ParticipantSession
TrialManager
SurveyManager
DataLogger
TimerManager
```

Suggested responsibilities:

## PageManager

Handles only page/panel visibility and navigation.

It should not know why a page exists scientifically.

## ExperimentManager

Controls experimental progression:

- current block;
- assigned condition order;
- current scenario;
- transitions between blocks;
- Dr. Meowra introduction placement.

## ParticipantSession

Stores:

- anonymous participant ID;
- assignment;
- responses;
- scores/raw answers;
- timestamps;
- final preference;
- open-ended response.

## TrialManager

Loads and presents scenarios.

## SurveyManager

Collects:

- UEQ-S responses;
- API responses;
- preference;
- open-ended response.

## DataLogger

Writes durable local data.

## TimerManager

Records exploratory response time without tying timing to Unity frame rate.

---

# 30. Data Logging

The application should use an anonymous participant identifier such as:

```text
P001
P002
P003
...
```

or a randomized non-identifying code if required by the approved research protocol.

At minimum, log:

```text
participant_id
assignment_id
condition_order
stimulus_set
scenario_id
feedback_condition
task_response
task_correct
task_response_time

raw_ueqs_1 ... raw_ueqs_8
neutral_ueqs_1 ... neutral_ueqs_8
meowra_ueqs_1 ... meowra_ueqs_8

api_credible_1 ... api_credible_5
api_engaging_1 ... api_engaging_5

final_preference
open_response
```

It is also useful to save:

```text
app_version
study_protocol_version
unity_version
study_start_timestamp
study_completion_timestamp
```

## Incremental Saving

Do not keep the entire experiment only in memory until the final screen.

Save after meaningful participant actions.

For example:

```text
answer submitted
    ↓
update session
    ↓
write durable file
    ↓
advance page
```

If the application crashes halfway through a participant session, already-completed data should remain recoverable.

## File Format

A practical approach is:

- **JSON** for complete per-participant session records;
- **CSV** for analysis-ready tables.

Store files using `Application.persistentDataPath` or another approved local location.

The final storage and retention process must follow the approved institutional research protocol.

---

# 31. Suggested Repository Structure

```text
CatgirlCompilerResearch/
├── AGENTS.md
├── README.md
├── DrMeowra.jpg
├── Research_Design_Document.pdf
├── Dr_Meowra_Statistics_and_Measures_Guide.pdf
│
├── study-materials/
│   ├── stimuli/
│   │   ├── scenarios.json
│   │   └── stimulus_manifest.md
│   ├── prompts/
│   │   ├── neutral_prompt.md
│   │   └── meowra_persona.md
│   ├── measures/
│   │   ├── UEQS_notes.md
│   │   └── API_notes.md
│   └── assignments/
│       └── counterbalancing.csv
│
├── analysis/
│   ├── README.md
│   └── scripts/
│
└── UnityProject/
    ├── Assets/
    │   ├── Data/
    │   ├── Fonts/
    │   ├── Images/
    │   ├── Prefabs/
    │   ├── Scenes/
    │   ├── Scripts/
    │   │   ├── Data/
    │   │   ├── Experiment/
    │   │   ├── UI/
    │   │   └── Utilities/
    │   └── UI/
    ├── Packages/
    └── ProjectSettings/
```

Not all directories need to exist immediately.

The purpose of this structure is to separate:

```text
research documentation
study stimuli
analysis
participant-facing Unity implementation
```

---

# 32. Reproducing the Unity Environment

The exact Unity Editor version used for data collection should be preserved in:

```text
UnityProject/ProjectSettings/ProjectVersion.txt
```

A reproducing researcher should:

1. clone the repository;
2. inspect the tagged study version;
3. install the Unity version listed in `ProjectVersion.txt`;
4. open `UnityProject/`;
5. verify all packages from `Packages/manifest.json`;
6. open the experiment scene;
7. run a test participant;
8. confirm the assigned condition order;
9. confirm the correct stimuli are shown;
10. complete all surveys;
11. inspect the resulting output file; and
12. compare the output schema with the documented data dictionary.

---

# 33. Pre-Data-Collection Freeze

Before collecting publishable participant data, freeze the study.

Recommended freeze procedure:

```text
1. Finalize six scenarios.
2. Finalize Raw diagnostics.
3. Finalize Neutral explanations.
4. Finalize Meowra explanations.
5. Finalize Dr. Meowra introduction.
6. Finalize UEQ-S implementation.
7. Finalize exact API subscale wording.
8. Finalize counterbalancing schedule.
9. Finalize participant background items.
10. Finalize data schema.
11. Verify local saving.
12. Run pilot sessions.
13. Fix implementation bugs.
14. Freeze the protocol.
15. Create a Git tag / release for the exact study version.
```

Example tag:

```text
study-v1.0
```

Once real data collection begins, do not silently modify stimuli, questionnaire wording, condition behavior, or assignment logic.

If a necessary change occurs, document it and version it.

---

# 34. Pilot Testing

Pilot with several people before formal data collection.

The pilot should verify:

- instructions are understandable;
- task text fits on screen;
- code formatting is readable;
- participant cannot advance without answering required items;
- UEQ-S anchors are oriented correctly;
- API uses the intended 5-point response format;
- Meowra introduction appears only at the correct time;
- counterbalancing works;
- correct stimuli appear under each condition;
- no condition leaks into another;
- data is written after every important step;
- final output contains every expected field;
- response timing does not break when navigating;
- session length is reasonable.

Do not include pilot participants in the final analysis unless the protocol explicitly allows it and the finalized study they experienced is identical to the frozen experiment.

---

# 35. Threats to Validity

## Persona Package Confound

The study does not isolate avatar, tone, name, encouragement, or catgirl aesthetics individually.

**Interpretation:** the causal unit is the persona package.

## Novelty

A catgirl programming assistant is unusual.

The UEQ-S Hedonic scale includes dimensions such as inventive and leading-edge, so novelty may contribute directly to the measured effect.

**Interpretation:** this is part of the short-term UX response being explored, but long-term habituation remains unknown.

## Demand Characteristics

Participants may infer that Dr. Meowra is the “interesting” condition.

**Mitigation:** neutral study instructions, counterbalancing, consistent UI, and avoiding language that tells participants which condition the researchers prefer.

## Message Equivalence

If Meowra provides better technical guidance than Neutral, the persona effect is confounded with information quality.

**Mitigation:** same source information, same generation pipeline, expert review, approximate length matching, frozen outputs.

## Order Effects

Repeated exposure may create practice, fatigue, and contrast.

**Mitigation:** six condition orders.

## Scenario Difficulty

Some errors are easier than others.

**Mitigation:** three rotating stimulus sets.

## Small Sample

15–20 participants gives limited power for moderate/small effects.

**Interpretation:** emphasize effect estimates, confidence intervals, descriptive patterns, and exploratory conclusions.

## Ecological Validity

A Unity multiple-choice task is not normal IDE debugging.

**Interpretation:** this study isolates UX; real-world debugging is future work.

## Population

Student programmers may differ from experienced professionals.

**Interpretation:** claims must match the sampled population.

---

# 36. Ethics and Human-Subjects Research

This repository is not an IRB approval document.

Before collecting data intended for publication:

- obtain the appropriate institutional IRB/ethics determination;
- use the approved consent language;
- collect only approved participant information;
- follow approved data-retention procedures;
- avoid collecting unnecessary identifying information; and
- ensure open-ended quotations are used only in ways permitted by the approved protocol.

The Unity application should not collect names unless required by the approved protocol.

---

# 37. Current Analysis Workflow

After data collection:

```text
1. Validate participant/session files.
2. Exclude only according to predetermined criteria.
3. Score each UEQ-S administration.
4. Compute Raw/Neutral/Meowra Pragmatic scores.
5. Compute Raw/Neutral/Meowra Hedonic scores.
6. Produce descriptive means, SDs, and 95% CIs.
7. Create H1 paired differences:
      Neutral Pragmatic - Raw Pragmatic.
8. Create H2 paired differences:
      Meowra Hedonic - Neutral Hedonic.
9. Inspect paired-difference distributions/outliers.
10. Run two paired-samples t-tests.
11. Apply Holm correction across the two confirmatory p-values.
12. Compute paired effect sizes (Cohen's dz).
13. Summarize API Credible and Engaging descriptively.
14. Report final preference counts/percentages.
15. Code the open-ended “why” responses.
16. Interpret the full pattern as exploratory evidence.
```

Do not run a large collection of unplanned significance tests simply because the data is available.

---

# 38. Reporting Example

A final paper could report H2 approximately as:

> Participants rated the Dr. Meowra condition more highly on UEQ-S Hedonic Quality than the Neutral condition. The paired mean difference was [value], 95% CI [lower, upper], `t(df) = [value]`, Holm-adjusted `p = [value]`, with paired effect size `dz = [value]`.

If the result is not significant:

> Hedonic ratings descriptively favored Dr. Meowra, but the paired comparison did not reach the prespecified significance threshold after correction. The estimated effect was [value] with a wide confidence interval, consistent with substantial uncertainty in this small exploratory sample.

Do not write that a non-significant result proves equivalence.

---

# 39. Interpreting a Positive Persona Result

If the persona package receives higher Hedonic Quality ratings, that result should be described narrowly.

Appropriate:

> Persona-based presentation produced a more positive hedonic UX in this exploratory sample.

Too strong:

> Catgirls make compiler errors better.

Also too strong:

> Friendly wording caused the effect.

The experiment evaluates the **package**.

The scientific value of a positive result is that it justifies more targeted follow-up studies capable of isolating individual social/visual features.

---

# 40. Development Instructions for Coding Agents

See [`AGENTS.md`](AGENTS.md).

Coding agents should:

- preserve experimental design;
- avoid changing validated scale wording;
- keep Neutral and Meowra technical content matched;
- explain Unity/C# changes;
- test data logging;
- work incrementally;
- avoid live AI integration;
- avoid committing generated Unity folders; and
- flag methodological decisions rather than silently making them.

---

# 41. Measurement and Instrument Resources

## UEQ / UEQ-S

Practical overview:

- [SurveyLab — User Experience Questionnaire (UEQ)](https://www.surveylab.com/blog/user-experience-questionnaire-ueq/)

Official resources, questionnaires, handbooks, benchmarks, and analysis tools:

- [UEQ Online](https://www.ueq-online.org/)

Primary UEQ-S publication:

> Schrepp, M., Hinderks, A., & Thomaschewski, J. (2017).  
> *Design and Evaluation of a Short Version of the User Experience Questionnaire (UEQ-S).*  
> International Journal of Interactive Multimedia and Artificial Intelligence, 4(6), 103–108.  
> https://doi.org/10.9781/ijimai.2017.09.001

## Agent Persona Instrument

Instrument/paper resource:

- [Ryu & Baylor — Agent Persona Instrument](https://www.researchgate.net/publication/237627605_The_API_Agent_Persona_Instrument_for_Assessing_Pedagogical_Agent_Persona)

Related psychometric publication:

> Ryu, J., & Baylor, A. L. (2005).  
> *The Psychometric Structure of Pedagogical Agent Persona.*  
> Technology, Instruction, Cognition & Learning, 2(4), 291–315.

Use the published API wording and anchors when implementing the Credible and Engaging subscales.

---

# 42. Background Literature

The following literature motivates the study and/or informs its design.

### Compiler Diagnostics and Human Factors

Barik, T., Ford, D., Murphy-Hill, E., & Parnin, C. (2018).  
*How Should Compilers Explain Problems to Developers?*  
Proceedings of ESEC/FSE.  
https://doi.org/10.1145/3236024.3236040

Barik, T., Smith, J., Lubick, K., Holmes, E., Feng, J., Murphy-Hill, E., & Parnin, C. (2017).  
*Do Developers Read Compiler Error Messages?*  
ICSE.  
https://doi.org/10.1109/ICSE.2017.59

Becker, B. A. (2016).  
*An Effective Approach to Enhancing Compiler Error Messages.*  
SIGCSE.  
https://doi.org/10.1145/2839509.2844584

Becker, B. A., et al. (2019).  
*Compiler Error Messages Considered Unhelpful: The Landscape of Text-Based Programming Error Message Research.*  
ITiCSE Working Group Reports.  
https://doi.org/10.1145/3344429.3372508

Denny, P., Luxton-Reilly, A., & Carpenter, D. (2014).  
*Enhancing Syntax Error Messages Appears Ineffectual.*  
ITiCSE.  
https://doi.org/10.1145/2591708.2591748

Denny, P., Prather, J., & Becker, B. A. (2020).  
*Error Message Readability and Novice Debugging Performance.*  
ITiCSE.  
https://doi.org/10.1145/3341525.3387384

Dong, T., & Khandwala, K. (2019).  
*The Impact of “Cosmetic” Changes on the Usability of Error Messages.*  
CHI Extended Abstracts.  
https://doi.org/10.1145/3290607.3312978

Traver, V. J. (2010).  
*On Compiler Error Messages: What They Say and What They Mean.*  
Advances in Human-Computer Interaction.  
https://doi.org/10.1155/2010/602570

### LLM and Conversational Compiler Feedback

Taylor, A., Vassar, A., Renzella, J., & Pearce, H. (2024).  
*DCC --help: Transforming the Role of the Compiler by Generating Context-Aware Error Explanations with Large Language Models.*  
SIGCSE.  
https://doi.org/10.1145/3626252.3630822

Santos, E. A., & Becker, B. A. (2024).  
*Not the Silver Bullet: LLM-Enhanced Programming Error Messages Are Ineffective in Practice.*  
UKICER.  
https://doi.org/10.1145/3689535.3689554

Renzella, J., Vassar, A., Lee Solano, L., & Taylor, A. (2025).  
*Compiler-Integrated, Conversational AI for Debugging CS1 Programs.*  
SIGCSE.  
https://doi.org/10.1145/3641554.3701827

### Social Agents, Persona, and Anthropomorphism

Nass, C., Steuer, J., & Tauber, E. R. (1994).  
*Computers Are Social Actors.*  
CHI.  
https://doi.org/10.1145/191666.191703

Lester, J. C., Converse, S. A., Kahler, S. E., Barlow, S. T., Stone, B. A., & Bhogal, R. S. (1997).  
*The Persona Effect: Affective Impact of Animated Pedagogical Agents.*  
CHI.  
https://doi.org/10.1145/258549.258797

Moreno, R., Mayer, R. E., Spires, H. A., & Lester, J. C. (2001).  
*The Case for Social Agency in Computer-Based Teaching: Do Students Learn More Deeply When They Interact with Animated Pedagogical Agents?*  
Cognition and Instruction, 19(2), 177–213.  
https://doi.org/10.1207/S1532690XCI1902_02

Moundridou, M., & Virvou, M. (2002).  
*Evaluating the Persona Effect of an Interface Agent in a Tutoring System.*  
Journal of Computer Assisted Learning, 18(3), 253–261.  
https://doi.org/10.1046/j.0266-4909.2001.00237.x

Wang, N., Johnson, W. L., Mayer, R. E., Rizzo, P., Shaw, E., & Collins, H. (2008).  
*The Politeness Effect: Pedagogical Agents and Learning Outcomes.*  
International Journal of Human-Computer Studies, 66(2), 98–112.  
https://doi.org/10.1016/j.ijhcs.2007.09.003

Cohn, M., et al. (2024).  
*Believing Anthropomorphism: Examining the Role of Anthropomorphic Cues on Trust in Large Language Models.*  
CHI Extended Abstracts.  
https://doi.org/10.1145/3613905.3650818

### Catgirl / Kemonomimi Visual Context

Shijo, R., Sakurai, S., Hirota, K., & Nojima, T. (2021).  
*Consideration of Emotional Expression Interface Based on Emotional Expression Ability of Animal.*  
The Transactions of Human Interface Society, 23(4), 419–430.  
https://doi.org/10.11184/his.23.4_419

---

# 43. Reproducibility Checklist

A researcher attempting to reproduce the experiment should be able to identify:

- the exact Unity version;
- the exact study Git tag;
- the six exact source-code scenarios;
- the compiler and compiler version used for Raw diagnostics;
- the exact Raw diagnostics;
- the exact frozen Neutral explanations;
- the exact frozen Meowra explanations;
- the prompts/model/version used to generate them;
- the Dr. Meowra avatar;
- the persona introduction;
- the condition-order assignment schedule;
- the stimulus-set assignment schedule;
- all participant background questions;
- all eight UEQ-S items;
- the exact selected API subscales;
- the final preference question;
- the open-ended question;
- the data schema;
- the exclusion rules;
- the power-analysis assumptions;
- the analysis script; and
- the final statistical-reporting procedure.

If any of those items are unavailable, reproducibility is incomplete.

The repository should therefore preserve them before publication.

---

# 44. Project Goal

The project is not trying to establish a universal conclusion from a small one-month study.

Its contribution is narrower:

> **Explore whether social/persona-based presentation is a meaningful UX dimension for AI-assisted compiler feedback, using a controlled PL+HCI experiment that separates improved explanatory content from the additional persona treatment.**

A strong result would justify larger and more targeted research.

A weak or null result would still provide useful evidence about whether persona-based compiler feedback deserves further investment.

Either outcome is informative if the study is run transparently and the claims remain proportional to the evidence.
