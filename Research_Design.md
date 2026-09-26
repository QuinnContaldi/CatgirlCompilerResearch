# Designing for the Participant

## Exploring Playful Interfaces in Programming Language User Studies

**Working research-design document — September 2026**

**Planned implementation:** Unity  
**Design:** Two-condition within-subject crossover study  
**Primary UX instrument:** UEQ-S  
**Primary comparison:** Dr. Meowra vs. Neutral on UEQ-S Hedonic Quality  
**Secondary UX outcome:** UEQ-S Pragmatic Quality  
**Secondary task outcomes:** Accuracy and response time  
**Preference outcome:** Preferred format for participating in a future similar study  
**Qualitative outcome:** Open-ended explanation of preference  
**Target completion date for data collection:** October 9, 2026

---

# 1. Study in One Page

Programming-language research increasingly recognizes the importance of empirical evidence about programmers. However, human-subject studies remain comparatively uncommon in programming-language research, and user evaluations are difficult and costly to conduct. Stefik et al. (2014) documented the limited empirical basis used for human-factors decisions in programming-language research; in the comparison that motivates this project, programming-language user studies were the least represented group among the research categories shown. Related work in software engineering has also documented practical barriers to user evaluation, including participant recruitment and researcher time.

This project asks a different methodological question from a conventional PL user study:

> **Can the participant-facing interface of a programming-language user study be intentionally designed to improve the participant experience without changing the underlying scientific task?**

The study compares two versions of the same short programming-language experiment:

1. **Neutral Study Interface**  
   A conventional, plain research interface. Instructions are factual and administrative. There is no character, social host, playful framing, or encouragement beyond what is functionally necessary.

2. **Dr. Meowra Study Interface**  
   A playful, encouraging, character-mediated research interface. Dr. Meowra is visibly present throughout the experimental block, greets the participant, presents the same functional instructions in a conversational style, acknowledges progress, and encourages the participant to continue. She does **not** provide task hints, correctness feedback, or additional technical information.

The scientific task itself is held as constant as practical. Participants answer short multiple-choice questions about simple C/C-like programming errors. Two matched four-question task sets are used so that participants do not answer the exact same questions twice.

The primary question is whether the Meowra interface produces a measurably better **hedonic user experience**. The study also examines whether the playful presentation introduces tradeoffs in **pragmatic user experience**, task accuracy, or task response time.

The final question asks which format participants would prefer if they were invited to participate in another programming-language study of similar length and difficulty, followed by an open-ended explanation of why.

---

# 2. Core Contribution

This study is not primarily about catgirls, anthropomorphism, compiler diagnostics, or AI-generated explanations.

The methodological contribution is:

> **The participant-facing interface of a user study is itself an interactive system and can therefore be treated as a design variable.**

Programming-language researchers routinely make careful decisions about:

- experimental tasks;
- treatment ordering;
- measurements;
- statistical analysis;
- sampling;
- validity threats; and
- implementation.

This study asks whether the **experience of participating in the experiment itself** should also receive deliberate design attention.

The project explores whether a playful, supportive participant-facing layer can improve the experience of participating in a PL study while preserving the same underlying scientific task.

---

# 3. Problem Statement

Programming-language design includes many decisions that directly affect human programmers, yet empirical evidence about those human effects has historically been limited.

Stefik et al. (2014) systematically examined empirical evidence used for human-factors decisions in programming-language research. Their analysis was motivated by the broader concern that human-facing language-design claims often lack the kind of empirical evidence expected in other human-centered disciplines.

The practical difficulty of running user evaluations is also well documented outside PL. Buse, Sadowski, and Weimer (2011) surveyed software-engineering researchers about barriers to user evaluation. Most respondents agreed that user evaluation was difficult; recruitment and time commitment were among the most commonly identified barriers.

These observations create a methodological tension:

```text
PL needs evidence about programmers
              ↓
evidence requires participant studies
              ↓
participant studies are expensive and difficult
              ↓
the participant experience itself may deserve design attention
```

Research methodology usually focuses on scientific validity from the researcher's perspective. This project adds a participant-centered question:

> **Can we design the research interface so that participation itself is a better user experience while still preserving the study task?**

The present experiment is intentionally narrow. It does not attempt to prove that playful interfaces solve recruitment, retention, fatigue, motivation, or any other broad problem. It first tests the more fundamental premise that the participant-facing study environment can be redesigned in a way that measurably changes UX without obviously compromising the underlying task.

---

# 4. Research Scope

## 4.1 What This Study Tests

The study tests whether a **playful, character-mediated participant-facing interface** changes:

- Hedonic Quality;
- Pragmatic Quality;
- stated preference for future participation;
- programming-task accuracy; and
- programming-task response time.

## 4.2 What This Study Does Not Test

The study does **not** claim to establish that:

- all user studies should contain characters;
- catgirl interfaces are universally better;
- playful interfaces increase actual recruitment rates;
- playful interfaces reduce attrition;
- playful interfaces improve truthfulness;
- playful interfaces improve learning;
- playful interfaces improve programming ability;
- playful interfaces eliminate period effects;
- playful interfaces are appropriate for every research population;
- any single Meowra design feature independently causes an effect; or
- a nonsignificant task-performance difference proves equivalence.

The treatment is intentionally a **package**. If the package appears promising, future studies can isolate individual design components.

---

# 5. Research Questions and Hypothesis

## RQ1 — Participant User Experience

> **How does a playful, character-mediated study interface affect the user experience of participating in a programming-language user study compared with a neutral study interface?**

### H1 — Hedonic Quality

> **Participants will report higher UEQ-S Hedonic Quality in the Dr. Meowra condition than in the Neutral condition.**

This is the primary confirmatory hypothesis.

The purpose of this outcome is not merely to ask whether a deliberately playful interface is "fun." It serves two functions:

1. it tests whether the actual implementation successfully produces the intended experiential change; and
2. it quantifies the magnitude of that UX change using a validated instrument.

A good idea can still be executed poorly. If Dr. Meowra is annoying, distracting, flat, confusing, or unappealing, the intervention may fail to produce a meaningful Hedonic Quality improvement. The UEQ-S therefore evaluates the **implemented treatment**, not the abstract concept.

---

## RQ2 — Future-Participation Preference

> **Which interface would participants prefer if they were invited to participate in another programming-language study of similar length and difficulty, and why?**

The forced-choice preference provides an intuitive participant-centered outcome.

The open-ended explanation provides qualitative evidence about the reasons behind that preference.

This question is deliberately broader than "Which interface was more fun?" A participant could prefer Neutral despite finding Meowra novel, or prefer Meowra because the study felt more welcoming, supportive, engaging, or pleasant.

---

## RQ3 — Tradeoffs

> **Does the playful Dr. Meowra condition show evidence of a tradeoff in pragmatic user experience, task accuracy, or task response time relative to the Neutral condition?**

This question addresses an important methodological concern:

> A more enjoyable experiment is not useful if the participant-facing treatment substantially interferes with the scientific task.

The study therefore records:

- UEQ-S Pragmatic Quality;
- task accuracy; and
- task response time.

These are primarily secondary or exploratory outcomes.

A failure to find a statistically significant performance difference will **not** be interpreted as proof that the two interfaces are equivalent. Formal equivalence or non-inferiority testing would require a prespecified acceptable margin and substantially stronger performance measurement.

---

# 6. Experimental Design

## 6.1 Design Type

The experiment uses a **within-subject crossover design**.

Every participant experiences both:

- the Neutral interface; and
- the Dr. Meowra interface.

This allows each participant to serve as their own comparison.

Within-subject comparison is useful for a small exploratory study because stable individual differences in rating style, programming experience, personality, and general interface preferences are reduced as sources of between-person noise.

---

## 6.2 Why There Are Two Task Sets

Participants cannot answer the exact same four programming questions twice because they may remember the answers during the second condition.

The study therefore uses:

```text
Task Set A: four questions
Task Set B: four matched questions
```

Both sets contain the same four error categories.

The individual code examples differ, but the underlying concepts and question structures are matched as closely as practical.

---

## 6.3 Four-Cell Crossover Assignment

Participants are distributed across four assignment cells:

| Assignment | Block 1 | Block 2 |
| --- | --- | --- |
| 1 | Neutral + Set A | Meowra + Set B |
| 2 | Meowra + Set A | Neutral + Set B |
| 3 | Neutral + Set B | Meowra + Set A |
| 4 | Meowra + Set B | Neutral + Set A |

This controls two potential confounds:

### Condition-order effect

Half the assignments begin with Neutral and half begin with Meowra.

### Task-set effect

Each task set appears under both Neutral and Meowra.

The intended result is that interface condition is not permanently tied to one set of programming questions.

---

# 7. The Experimental Manipulation

The experiment manipulates the **participant-facing research interface**.

The programming task remains functionally equivalent.

The treatment is not merely a few "nya" phrases. Dr. Meowra acts as a persistent study host throughout her assigned block.

---

# 8. Neutral Condition

The Neutral interface represents a conventional plain research application.

It should be:

- clear;
- professional;
- factual;
- minimal;
- non-social; and
- non-playful.

The Neutral condition contains the same functional information as the Meowra condition.

Examples:

### Introduction

> You will complete four short programming questions in this section.

### Tutorial

> Review each code example and select the answer that best identifies the primary issue. Select one response and press Submit.

### Progress

> Question 2 of 4 complete. Two questions remain.

### Completion

> This section is complete.

The Neutral condition should **not** be intentionally unpleasant, confusing, ugly, or hostile.

It is a competent conventional baseline.

The experiment is not:

```text
bad interface vs. good interface
```

It is:

```text
conventional neutral study presentation
                vs.
playful participant-centered study presentation
```

---

# 9. Dr. Meowra Condition

Dr. Meowra is a persistent character-mediated study host.

The treatment consists of multiple participant-facing elements that operate together as one composite intervention.

## 9.1 Visual Identity

During the Meowra experimental block:

- Dr. Meowra's avatar is visible on study-interface screens;
- her name is displayed;
- her visual identity remains consistent;
- the interface may use presentation elements that support her presence while preserving task readability.

Her presence should continue across:

- condition introduction;
- tutorial;
- task screens;
- progress/transition screens; and
- completion.

The UEQ-S and final evaluation screens are **not** hosted by Meowra and should remain neutral.

---

## 9.2 Social Framing

Meowra communicates in the first person as a study host.

Example:

> Hi! I'm Dr. Meowra, and I'll be guiding you through this section.

The purpose is to make the study feel socially hosted rather than administratively presented.

---

## 9.3 Tutorial Presentation

The Meowra tutorial communicates the **same functional information** as Neutral.

Example:

> I'll show you four short programming questions. For each one, take a look at the code and choose the answer that best identifies the issue. Once you've made your choice, press Submit and we'll keep going together.

The Meowra tutorial may be warmer and more conversational, but it must not teach participants how to solve the programming questions.

---

## 9.4 Encouragement

Meowra may provide encouragement related to **participation**, not correctness.

Allowed examples:

> Nice work — you're halfway there!

> Two more to go. You've got this!

> Thanks for sticking with me!

> One more question!

Not allowed:

> Great answer!

> You got that one right!

> Remember that `==` checks equality!

> Look closely at the parentheses.

The first category supports the participant.

The second category changes the scientific task.

---

## 9.5 Playfulness

Meowra may use light playful language, but playfulness must not overwhelm instructions.

The character should be:

- warm;
- friendly;
- enthusiastic;
- encouraging;
- supportive;
- recognizable; and
- mildly playful.

Avoid:

- excessive "nya";
- constant cat puns;
- baby talk;
- flirtation;
- answer hints;
- distracting jokes;
- task-specific technical commentary;
- correctness feedback.

The treatment should feel intentionally different from the Neutral interface without turning the programming task itself into a joke.

---

# 10. What Must Remain Controlled

The following properties should remain identical or closely matched between conditions:

| Feature | Neutral | Meowra |
| --- | --- | --- |
| Number of programming tasks | 4 | 4 |
| Task categories | Matched | Matched |
| Question format | Same | Same |
| Number of answer options | Same | Same |
| Functional tutorial content | Same | Same |
| Technical hints | None | None |
| Correctness feedback | None | None |
| Button behavior | Same | Same |
| Code font | Same | Same |
| Code size | Same | Same |
| Task layout | Same | Same |
| Response controls | Same | Same |
| UEQ-S screen | Neutral | Neutral |
| Task-response timing rule | Same | Same |

The manipulated properties include:

| Feature | Neutral | Meowra |
| --- | --- | --- |
| Character | None | Dr. Meowra |
| Avatar | None | Present |
| Named host | None | Present |
| Social framing | Minimal | Present |
| Encouragement | Administrative only | Supportive |
| Progress language | Neutral | Encouraging |
| Tone | Plain | Playful/warm |

---

# 11. Consent and Ethics

The **actual informed-consent process is neutral and identical for every participant**.

Dr. Meowra does not persuade participants to consent and does not act as the mechanism through which voluntary consent is solicited.

The flow begins:

```text
neutral consent
      ↓
consent accepted
      ↓
experimental application
```

After consent, the participant may encounter either Neutral or Meowra first depending on the crossover assignment.

This distinction is important because informed consent is an ethical requirement, not an experimental treatment.

Before collecting publishable data:

- obtain the required institutional IRB/ethics determination;
- use approved consent language;
- avoid unnecessary identifying information;
- follow approved data-storage and retention procedures; and
- obtain permission for the use of participant quotations if required.

---

# 12. Programming Task Design

The programming questions exist to provide a real PL-style study task while the participant-facing interface is manipulated.

They are **not** the central research contribution.

The task should therefore be:

- short;
- standardized;
- easy to understand;
- easy to reproduce;
- low in irrelevant complexity; and
- similar in difficulty across matched pairs.

Denny, Prather, and Becker (2020) provide useful methodological precedent for using predetermined debugging material so that participants encounter the same errors rather than generating different errors through open-ended code writing.

Hristova et al. (2003) provide a published list of common novice programming errors. The present task categories draw on that style of common-error taxonomy while adapting examples to short C/C-like snippets.

---

# 13. Why Multiple Choice Instead of Coding

Participants are not asked to repair or rewrite code.

Direct coding would introduce additional constructs:

- typing speed;
- syntax recall;
- IDE familiarity;
- navigation skill;
- debugging strategy;
- trial-and-error behavior;
- tool familiarity; and
- individual solution style.

Those constructs are unnecessary for the present research question.

The experimental payload only needs to be cognitively meaningful enough that participants are genuinely completing a small PL task.

Multiple-choice questions provide:

- standardized exposure;
- objective accuracy;
- easy response-time measurement; and
- reduced variability in task execution.

This improves control for a study whose main manipulated variable is the participant-facing environment.

---

# 14. Final Task Categories

Both Set A and Set B contain one example from each category:

1. assignment versus comparison (`=` vs. `==`);
2. incompatible value/type use;
3. delimiter mismatch;
4. incorrect separators in a `for` loop.

The categories are intentionally simple.

The study is not attempting to distinguish advanced programming expertise.

---

# 15. Standard Task Prompt

Every question should use the same basic prompt:

> **What is the primary issue in this code?**

Each question contains four response options.

The number of options, interaction style, and submission process remain the same across all eight tasks.

---

# 16. Task Set A

## A1 — Assignment vs. Comparison

```c
int score = 4;

if (score = 5) {
    printf("Match");
}
```

**Question**

> What is the primary issue in this code?

**Correct concept**

The assignment operator `=` is being used where a comparison with `==` appears to be intended.

---

## A2 — Type/Value Mismatch

```c
int count = "five";
```

**Question**

> What is the primary issue in this code?

**Correct concept**

A string value is being assigned to an integer variable.

**Implementation note**

In strict C, the exact compiler behavior depends on compiler and warning settings. The study therefore asks participants to identify the programming issue rather than asking whether the line must fail compilation.

---

## A3 — Delimiter Mismatch

```c
int total = 3;

if (total > 2 {
    printf("Large");
}
```

**Question**

> What is the primary issue in this code?

**Correct concept**

The closing parenthesis for the `if` condition is missing.

---

## A4 — `for`-Loop Separators

```c
for (int i = 0, i < 5, i++) {
    printf("%d\n", i);
}
```

**Question**

> What is the primary issue in this code?

**Correct concept**

The expressions in the `for` header should be separated by semicolons rather than commas.

---

# 17. Task Set B

## B1 — Assignment vs. Comparison

```c
int attempts = 2;

if (attempts = 3) {
    printf("Done");
}
```

**Question**

> What is the primary issue in this code?

**Correct concept**

The assignment operator `=` is being used where a comparison with `==` appears to be intended.

---

## B2 — Type/Value Mismatch

```c
int level = "high";
```

**Question**

> What is the primary issue in this code?

**Correct concept**

A string value is being assigned to an integer variable.

---

## B3 — Delimiter Mismatch

```c
int value = 7;

if (value < 10 {
    printf("Small");
}
```

**Question**

> What is the primary issue in this code?

**Correct concept**

The closing parenthesis for the `if` condition is missing.

---

## B4 — `for`-Loop Separators

```c
for (int j = 0, j < 8, j++) {
    printf("%d\n", j);
}
```

**Question**

> What is the primary issue in this code?

**Correct concept**

The expressions in the `for` header should be separated by semicolons rather than commas.

---

# 18. Task-Matching Requirements

The A/B pair for each category should be reviewed using the following rubric:

| Property | Requirement |
| --- | --- |
| Error concept | Same |
| Number of intentional issues | One |
| Approximate code length | Similar |
| Lines of code | Similar |
| Required programming knowledge | Same |
| Prompt | Same |
| Number of options | Same |
| Distractor structure | Similar |
| Expected difficulty | Similar |

The task content should remain semantically neutral.

Avoid making one task set unusually playful or cat-themed because that would create an additional treatment cue.

Dr. Meowra should be what makes the **study interface** playful.

---

# 19. Answer Options

The final answer options should be frozen before data collection.

The A/B versions of a matched category should use distractors of similar plausibility.

For example, an assignment-versus-comparison item might use:

```text
A. The code assigns a value where a comparison appears to be intended.
B. The integer variable has the wrong type.
C. The condition requires a semicolon before the opening brace.
D. There is no issue with the code.
```

The exact wording should be pilot-tested.

Do not allow the distractors in one task set to be obviously weaker than the distractors in the other.

---

# 20. Pilot Testing the Task Sets

The literature can justify the general use of common programming-error categories.

It cannot prove that the exact Set A and Set B stimuli are equally difficult.

Before formal data collection, pilot the study with several people.

For task matching, examine:

- accuracy on each A/B pair;
- obvious confusion;
- ambiguous answer choices;
- unusually slow items;
- comments indicating multiple plausible answers.

The goal is not to run statistical significance tests on the pilot.

The goal is to catch obvious mismatches.

Example warning sign:

```text
A3 accuracy: 100%
B3 accuracy: 40%
```

If one member of a pair is clearly harder, revise it before freezing the experiment.

Pilot participants should not be included in the final analysis unless the final protocol explicitly permits this and they experienced an identical frozen study.

---

# 21. Participant Flow

A complete session should follow this structure:

```text
Neutral informed consent
        ↓
Brief neutral study overview
        ↓
Programming-background questions
        ↓
Assignment selection
        ↓
Condition Block 1
    ├── condition-specific introduction
    ├── condition-specific tutorial
    ├── Task 1
    ├── transition/progress
    ├── Task 2
    ├── transition/progress
    ├── Task 3
    ├── transition/progress
    ├── Task 4
    └── condition completion
        ↓
Neutral UEQ-S screen
        ↓
Condition Block 2
    ├── condition-specific introduction
    ├── condition-specific tutorial
    ├── Task 1
    ├── transition/progress
    ├── Task 2
    ├── transition/progress
    ├── Task 3
    ├── transition/progress
    ├── Task 4
    └── condition completion
        ↓
Neutral UEQ-S screen
        ↓
Neutral final preference question
        ↓
Neutral open-ended explanation
        ↓
Debrief / completion
```

---

# 22. Why the Evaluation Screens Are Neutral

Dr. Meowra should disappear before participants evaluate the condition.

The UEQ-S screen should be identical after both conditions.

The final preference and open-ended-response screens should also be neutral.

This reduces the possibility that the character's continued social presence pressures the participant while they are evaluating her condition.

The logic is:

```text
experience treatment
        ↓
treatment ends
        ↓
neutral measurement screen
```

---

# 23. UEQ-S

The **User Experience Questionnaire — Short Version (UEQ-S)** is the central quantitative UX measure.

Schrepp, Hinderks, and Thomaschewski (2017) developed the eight-item UEQ-S for situations where a shorter UX questionnaire is useful.

The instrument contains:

- four Pragmatic Quality items; and
- four Hedonic Quality items.

All eight items are administered after **both** Neutral and Meowra.

Each participant therefore provides:

```text
Neutral Pragmatic
Neutral Hedonic
Meowra Pragmatic
Meowra Hedonic
```

---

# 24. UEQ-S Pragmatic Quality

The four Pragmatic items are:

```text
obstructive      ○ ○ ○ ○ ○ ○ ○      supportive
complicated      ○ ○ ○ ○ ○ ○ ○      easy
inefficient      ○ ○ ○ ○ ○ ○ ○      efficient
confusing        ○ ○ ○ ○ ○ ○ ○      clear
```

In this experiment, Pragmatic Quality is important because it addresses whether the playful interface creates a UX cost.

The ideal pattern is not simply:

```text
Meowra is more exciting
```

The stronger methodological pattern is:

```text
Meowra Hedonic ↑
Meowra Pragmatic maintained or improved
```

A substantial pragmatic decrease would indicate that the playful treatment made participation more interesting at the expense of clarity, efficiency, or ease.

Pragmatic Quality is therefore a **tradeoff measure**.

---

# 25. UEQ-S Hedonic Quality

The four Hedonic items are:

```text
boring           ○ ○ ○ ○ ○ ○ ○      exciting
not interesting  ○ ○ ○ ○ ○ ○ ○      interesting
conventional     ○ ○ ○ ○ ○ ○ ○      inventive
usual            ○ ○ ○ ○ ○ ○ ○      leading edge
```

Hedonic Quality is the primary outcome.

It quantifies whether the actual Meowra implementation creates a more stimulating and interesting experience than the Neutral interface.

This matters because the intervention could fail even if the concept sounds appealing.

A poor implementation might be:

- distracting;
- irritating;
- visually cluttered;
- childish;
- repetitive; or
- simply uninteresting.

Therefore, Hedonic Quality serves as the primary quantitative test that the participant-centered treatment actually changed UX in the intended direction.

---

# 26. UEQ-S Scoring

Responses are scored on the standard UEQ-S scale from:

```text
-3  -2  -1   0   +1   +2   +3
```

For each participant and condition:

```text
Pragmatic Quality = mean(items 1–4)
Hedonic Quality   = mean(items 5–8)
```

The overall eight-item average may be shown descriptively if useful, but it is not required for the primary analysis.

Use the official UEQ-S scoring guidance rather than inventing a new scoring method.

---

# 27. Final Preference Question

After both conditions:

> **If you were invited to participate in another programming-language study of similar length and difficulty, which study format would you prefer?**

Options:

```text
Neutral
Dr. Meowra
```

This question connects UX to a concrete participant-centered choice.

It does **not** establish actual recruitment behavior.

It measures stated preference for future participation.

---

# 28. Open-Ended Question

Immediately after the preference choice:

> **Why did you prefer that study format? Please describe anything about the presentation or interaction that influenced your choice.**

This question is intentionally broad.

Do not tell participants that the researchers expect Meowra to be more enjoyable.

Potential themes may include:

- engagement;
- encouragement;
- professionalism;
- clarity;
- distraction;
- warmth;
- novelty;
- efficiency;
- visual preference;
- social presence;
- annoyance; or
- preference for minimalism.

These themes should not be treated as predetermined findings.

They are examples of concepts that may emerge from participant responses.

---

# 29. Qualitative Analysis

The open-ended responses will receive a lightweight qualitative analysis appropriate to the small exploratory study.

Suggested procedure:

1. read all responses without assigning formal themes;
2. identify recurring reasons participants provide;
3. create a small coding scheme;
4. code each response using one or more applicable themes;
5. report theme frequencies descriptively;
6. include brief representative quotations where permitted.

The qualitative data should help explain the quantitative results.

Example:

```text
UEQ-S says:
Meowra was more hedonic.

Preference says:
most participants chose Meowra.

Open responses explain:
participants valued encouragement and character presence.
```

Or, alternatively:

```text
UEQ-S says:
Meowra was more hedonic.

Preference is mixed.

Open responses explain:
some participants enjoyed the character,
while others considered it distracting or unprofessional.
```

Both outcomes are scientifically useful.

---

# 30. Accuracy

Every task has one objectively correct answer.

For each participant and condition:

```text
accuracy = number correct / 4
```

Because only four questions are presented per condition, accuracy is a coarse measure.

It should therefore be treated as secondary context rather than as a high-precision performance outcome.

The study can report:

- mean number correct;
- median number correct;
- percentage correct; and
- participant-level paired differences.

Do not claim performance equivalence merely because a significance test fails to reject a difference.

---

# 31. Response Time

Unity should automatically log response time for every programming question.

## Timing rule

Start timing only after the complete task screen is visible.

Stop timing when the participant submits an answer.

```text
task fully displayed
        ↓
timer starts
        ↓
participant reads/selects
        ↓
Submit
        ↓
timer stops
```

Do **not** include Meowra's transition text or progress messages in task-response time.

Otherwise the treatment would mechanically increase measured time simply because additional interface content exists.

Optionally record total block duration separately.

Primary task-response time should mean the same thing under both conditions.

---

# 32. Participant Background

Keep participant-background questions short.

Potential variables:

- years of programming experience;
- C/C++ familiarity;
- programming frequency;
- student level; and
- relevant coursework.

These variables describe the sample.

With a small sample, they should not automatically become additional moderator analyses.

---

# 33. Participant Population

Participants should have enough programming background to understand very small C/C-like examples.

A reasonable target population is:

- undergraduate CS/CSE students;
- students who have completed introductory programming; or
- programmers with comparable experience.

Claims in the final paper must be scoped to the population actually recruited.

---

# 34. Sample Size

The study is exploratory and constrained by the October 9 data-collection deadline.

A practical target remains approximately **18–20 completed participants**.

For the four crossover cells:

- 20 participants allows 5 participants per assignment cell;
- 18 participants can be distributed approximately 5/5/4/4.

Do not stop recruitment early because the desired p-value is achieved.

Recruit according to the planned practical target and deadline.

---

# 35. Power and Sensitivity

The primary hypothesis uses a paired-samples t-test.

For a two-sided paired t-test at:

```text
alpha = .05
power = .80
```

approximate detectable standardized paired effects are:

| Completed participants | Approximate detectable dz |
| ---: | ---: |
| 15 | 0.78 |
| 18 | 0.70 |
| 20 | 0.66 |
| 24 | 0.60 |

The redesigned treatment is intentionally stronger and more comprehensive than the earlier idea of modifying a few lines of compiler feedback. That may increase the observed effect, but the true effect is unknown.

The correct planning claim is:

> The study is designed primarily to detect large within-participant UX effects and should be interpreted as exploratory if effects are smaller or estimates are imprecise.

---

# 36. Primary Statistical Analysis

## H1

For each participant:

```text
D_i = Meowra Hedonic_i - Neutral Hedonic_i
```

The primary analysis is a **paired-samples t-test** of whether the mean paired difference is different from zero.

Although H1 is directional, use a two-sided alpha of `.05` unless the analysis plan is explicitly changed before data collection.

Report:

- Neutral Hedonic mean and SD;
- Meowra Hedonic mean and SD;
- mean paired difference;
- 95% confidence interval for the difference;
- `t`;
- degrees of freedom;
- `p`;
- paired effect size `Cohen's dz`.

Because there is one primary confirmatory test, no multiple-comparison correction is required for the primary hypothesis.

---

# 37. Paired t-Test Assumption

The paired t-test concerns the distribution of the **within-participant difference scores**.

It does not require the Neutral and Meowra scores individually to be perfectly normally distributed.

Before interpreting the test:

- inspect the paired-difference distribution;
- inspect a Q-Q plot;
- look for severe skew;
- identify extreme influential outliers.

Do not switch statistical tests merely because another method produces a more favorable p-value.

Any sensitivity analysis should be identified as such.

---

# 38. Secondary UX Analysis

Pragmatic Quality is a secondary tradeoff outcome.

For each participant:

```text
Pragmatic difference =
Meowra Pragmatic - Neutral Pragmatic
```

Report:

- condition means;
- standard deviations;
- paired mean difference;
- confidence interval; and
- effect size.

An exploratory paired t-test may be reported if desired, but it should remain clearly secondary to H1.

The interpretation should focus on whether the playful treatment appears to preserve or alter the practical usability of the study interface.

---

# 39. Preference Analysis

Report:

```text
number preferring Neutral
number preferring Meowra
percentage preferring each
```

An exact binomial test against a 50/50 null may be reported as a secondary exploratory analysis.

The preference result should not replace the UEQ-S.

The two outcomes answer different questions:

```text
UEQ-S:
How did the experience change?

Preference:
Which format would the participant choose again?
```

---

# 40. Performance Analysis

Accuracy and response time are secondary indicators of possible task-performance cost.

Report them descriptively.

For accuracy:

- mean correct out of 4;
- percentage correct;
- paired condition difference.

For response time:

- participant-level median or mean task response time per condition;
- distribution;
- paired condition difference.

If exploratory significance tests are used, label them exploratory.

Do not claim:

> no statistically significant difference = identical performance

Instead write something like:

> No obvious task-performance cost was observed in this small exploratory sample.

or:

> Accuracy was descriptively similar across conditions, although the study was not designed as a formal equivalence test.

---

# 41. Exclusion Rules

Exclusion rules should be frozen before formal analysis.

Possible exclusions include:

- participant withdraws consent;
- incomplete study;
- corrupted session file;
- participant does not meet the minimum programming-background requirement;
- technical failure prevents exposure to both conditions.

Do not exclude participants because:

- their preference is unexpected;
- their UEQ-S ratings oppose the hypothesis;
- their performance is low but valid;
- removing them improves significance.

Document all exclusions.

---

# 42. Data Logging

At minimum, save:

```text
participant_id
assignment_cell
block_order
task_set_by_condition

programming_background

condition
task_id
error_category
selected_answer
correct_answer
is_correct
task_response_time_ms

neutral_ueqs_1 ... neutral_ueqs_8
meowra_ueqs_1 ... meowra_ueqs_8

neutral_pragmatic_score
neutral_hedonic_score
meowra_pragmatic_score
meowra_hedonic_score

final_preference
open_response

study_version
unity_version
session_start_utc
session_end_utc
```

Calculated scores may be regenerated during analysis, so saving the **raw questionnaire responses** is essential.

---

# 43. Incremental Saving

Save data after meaningful participant actions.

Recommended:

```text
participant submits response
        ↓
session state updated
        ↓
data written to disk
        ↓
next screen displayed
```

Do not wait until the end of the entire experiment to save the only copy of the participant session.

---

# 44. Unity Implementation Principles

The Unity application is a research instrument.

Prioritize:

- reliability;
- reproducibility;
- readable layouts;
- fixed study logic;
- transparent assignment;
- durable data logging; and
- minimal technical dependencies.

Avoid unnecessary systems such as:

- network services;
- live LLM calls;
- complex animation;
- game mechanics;
- scoring;
- points;
- rewards tied to correctness;
- random content generation.

The treatment is playful, but the software does not need to become a game.

---

# 45. Recommended Unity Architecture

```text
ExperimentManager
AssignmentManager
ParticipantSession
TaskManager
ConditionView
SurveyManager
TimerManager
DataLogger
```

## ExperimentManager

Controls overall participant progression.

## AssignmentManager

Assigns one of the four crossover cells.

## ParticipantSession

Stores all participant data and current state.

## TaskManager

Loads Set A/Set B programming questions.

## ConditionView

Controls Neutral vs. Meowra presentation without changing the underlying task object.

## SurveyManager

Presents UEQ-S, final preference, and open response on neutral measurement screens.

## TimerManager

Measures only task interaction time.

## DataLogger

Writes durable JSON/CSV records.

---

# 46. Recommended Task Data Model

Each task should be data rather than hard-coded into a scene.

Example conceptual structure:

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

The **same task object** should be displayable inside either condition presentation.

Do not create a "Meowra version" of the programming answer content.

That would reintroduce a confound.

---

# 47. Recommended Treatment Data Model

Condition presentation can contain:

```text
condition_id
display_name
avatar
intro_text
tutorial_text
transition_after_q1
transition_after_q2
transition_after_q3
completion_text
```

Neutral and Meowra can therefore use different participant-facing framing while loading the same underlying task structure.

---

# 48. Demand Characteristics

Participants may infer that Meowra is the experimental treatment.

Mitigation:

- recruitment materials should describe the experiment neutrally;
- do not say the study is testing whether Meowra is "more fun";
- do not tell participants which result is expected;
- use neutral measurement screens;
- counterbalance condition order.

A reasonable study description is:

> This study compares alternative interfaces for completing short programming-language research tasks.

---

# 49. Novelty

Dr. Meowra is deliberately unusual.

Novelty may contribute to Hedonic Quality, especially because the UEQ-S Hedonic scale includes concepts such as inventive and leading edge.

This is not hidden.

The study evaluates the **short-term experience of the complete intervention**.

It cannot establish whether an advantage would persist after repeated or long-term exposure.

That is a future-work question.

---

# 50. Composite-Treatment Limitation

The Meowra condition includes multiple simultaneous differences:

- avatar;
- character identity;
- social framing;
- warmth;
- encouragement;
- playful language;
- progress acknowledgement.

Therefore:

> **The causal unit is the participant-centered Meowra interface package.**

A positive result does not establish which component caused the effect.

That is appropriate for this first exploratory study.

A future factorial experiment could isolate:

- avatar vs. no avatar;
- encouragement vs. no encouragement;
- named host vs. anonymous host;
- warm vs. neutral wording;
- visual playfulness vs. social language.

---

# 51. Task-Set Equivalence Limitation

Set A and Set B are designed to be comparable but cannot be assumed to be perfectly equivalent.

Mitigation:

- matched error categories;
- similar code length;
- identical question format;
- similar distractor structure;
- crossover assignment;
- pilot testing.

Because each task set appears under both conditions across participants, systematic task-set difficulty should not remain permanently attached to one treatment.

---

# 52. Learning and Carryover

Participants complete one block before another.

They may become more familiar with the question format during the second block.

Mitigation:

- half of assignment cells begin with Neutral;
- half begin with Meowra;
- both task sets appear first and second;
- exact code questions are not repeated.

Condition order should be retained in the dataset for descriptive inspection.

---

# 53. Population Validity

If participants are primarily university CS students, the study should not claim that the results automatically generalize to:

- professional developers;
- expert PL researchers;
- non-programmers;
- older populations;
- long-term workplace studies.

State the sampled population clearly.

---

# 54. Ecological Validity

Answering four multiple-choice programming questions in Unity is not equivalent to participating in every possible programming-language user study.

The experiment deliberately prioritizes internal control.

The appropriate claim is:

> The study provides evidence that participant-facing presentation can influence the UX of this controlled PL study format.

It does not prove that the same effect applies to every experiment type.

---

# 55. Pilot Checklist

Before formal data collection, verify:

- consent works;
- assignment is balanced;
- A/B task loading is correct;
- Neutral and Meowra both contain four tasks;
- Meowra appears only during the Meowra treatment block;
- Meowra provides no technical hints;
- Neutral contains equivalent functional information;
- UEQ-S screens are neutral;
- UEQ-S item orientation is correct;
- all raw responses save correctly;
- timers start when task content appears;
- timers stop on Submit;
- transition time is excluded from task time;
- final preference works;
- open response saves;
- task distractors are unambiguous;
- A/B paired tasks appear reasonably similar in difficulty;
- no task contains multiple unintended programming issues.

---

# 56. Pre-Data-Collection Freeze

Before the first formal participant:

1. freeze the four Set A tasks;
2. freeze the four Set B tasks;
3. freeze all answer options;
4. freeze the correct-answer key;
5. freeze Neutral introduction/tutorial/transitions;
6. freeze Meowra introduction/tutorial/transitions;
7. freeze Dr. Meowra image assets;
8. freeze UEQ-S wording and orientation;
9. freeze final preference wording;
10. freeze open-ended question wording;
11. freeze assignment logic;
12. freeze exclusion criteria;
13. freeze analysis plan;
14. verify data schema;
15. pilot the build;
16. fix technical problems;
17. create a Git tag/release for the study version.

Example:

```text
study-v2.0-participant-ux
```

Do not silently change experimental content after formal collection begins.

---

# 57. Planned Interpretation Patterns

## Pattern A — Desired Methodological Result

```text
Meowra Hedonic > Neutral Hedonic
Pragmatic similar or better
accuracy similar
response time similar
participants prefer Meowra
```

Interpretation:

> The participant-centered interface improved subjective UX and was preferred without an obvious descriptive task-performance cost.

---

## Pattern B — Fun but Costly

```text
Meowra Hedonic > Neutral Hedonic
Pragmatic lower
accuracy lower and/or time substantially higher
```

Interpretation:

> The playful treatment improved experiential quality but introduced a usability or task-performance tradeoff.

This would be important negative design evidence.

---

## Pattern C — Treatment Failure

```text
Meowra Hedonic ≈ Neutral Hedonic
preference mixed
```

Interpretation:

> The particular Meowra implementation did not produce a meaningful UX improvement.

This does not prove that participant-centered design is impossible.

It means this implementation did not clearly succeed.

---

## Pattern D — Positive UX but Mixed Preference

```text
Meowra Hedonic > Neutral Hedonic
preference approximately split
```

Interpretation:

> Participants recognized the playful interface as more stimulating, but that experiential advantage did not universally translate into a preference for future participation.

The open-ended responses become especially important here.

---

# 58. Candidate Paper-Level Claim

A proportionate final claim would be:

> **We present an exploratory within-subject study treating the participant-facing interface of a programming-language experiment as a methodological design variable. By comparing a conventional neutral study interface with a playful, character-mediated interface while holding the underlying programming task constant, we examine whether participant UX can be improved without an obvious cost to pragmatic usability or task performance.**

---

# 59. Candidate Contribution Statement

Potential contributions:

1. **Participant experience as methodology**  
   We frame the participant-facing interface of a PL user study as an explicit methodological design variable.

2. **Controlled prototype comparison**  
   We implement Neutral and playful character-mediated versions of the same short PL study task.

3. **Mixed evidence about participant experience**  
   We combine validated UX measurement, future-participation preference, open-ended explanation, and task-performance context.

4. **Reproducible study infrastructure**  
   The Unity implementation, matched task sets, crossover logic, and analysis plan can be preserved for replication and extension.

---

# 60. Candidate Titles

Working title:

> **Designing for the Participant: Exploring Playful Interfaces in Programming Language User Studies**

Other possibilities:

> **Can User Studies Be More Fun? Exploring Participant-Centered Interfaces for Programming Language Research**

> **The User Study Is an Interface Too: Designing Participant Experience in Programming Language Research**

> **Dr. Meowra Runs a User Study: Exploring Playful Participant-Facing Design in Programming Language Research**

The first is the safest academic title.

The last is the strongest poster/conference attention-grabber.

---

# 61. Reproducibility Checklist

A future researcher should be able to recover:

- exact Unity version;
- exact Git tag;
- exact Dr. Meowra image;
- exact Neutral wording;
- exact Meowra wording;
- exact Set A questions;
- exact Set B questions;
- exact answer options;
- exact correct-answer key;
- exact crossover assignment procedure;
- exact participant-background questions;
- exact UEQ-S items;
- exact UEQ-S scoring;
- exact preference question;
- exact open-ended question;
- exact timing definition;
- exact data schema;
- exact exclusion criteria;
- exact analysis script;
- exact study sample and recruitment description.

---

# 62. References

## Human-Factors Evidence and User-Study Methodology

Stefik, A., Hanenberg, S., McKenney, M., Andrews, A. A., Yellanki, S. K., & Siebert, S. (2014).  
**What is the foundation of evidence of human factors decisions in language design? An empirical study on programming language workshops.**  
*Proceedings of the 22nd International Conference on Program Comprehension (ICPC)*, 223–231.  
https://doi.org/10.1145/2597008.2597154

Buse, R. P. L., Sadowski, C., & Weimer, W. (2011).  
**Benefits and barriers of user evaluation in software engineering research.**  
*Proceedings of the 26th Annual ACM SIGPLAN Conference on Object-Oriented Programming, Systems, Languages, and Applications (OOPSLA)*, 643–656.  
https://doi.org/10.1145/2048066.2048117

## Controlled Programming-Error Tasks

Hristova, M., Misra, A., Rutter, M., & Mercuri, R. (2003).  
**Identifying and correcting Java programming errors for introductory computer science students.**  
*Proceedings of the 34th SIGCSE Technical Symposium on Computer Science Education*, 153–156.  
https://doi.org/10.1145/611892.611956

Denny, P., Prather, J., & Becker, B. A. (2020).  
**Error message readability and novice debugging performance.**  
*Proceedings of the 2020 ACM Conference on Innovation and Technology in Computer Science Education (ITiCSE)*, 480–486.  
https://doi.org/10.1145/3341525.3387384

## UEQ-S

Schrepp, M., Hinderks, A., & Thomaschewski, J. (2017).  
**Design and evaluation of a short version of the User Experience Questionnaire (UEQ-S).**  
*International Journal of Interactive Multimedia and Artificial Intelligence, 4*(6), 103–108.  
https://doi.org/10.9781/ijimai.2017.09.001

Practical UEQ overview:

https://www.surveylab.com/blog/user-experience-questionnaire-ueq/

Official UEQ resources:

https://www.ueq-online.org/

---

# 63. Current Frozen Decisions

As of this research-design version, the following decisions are considered settled unless explicitly revised before formal data collection:

- two study conditions: Neutral and Dr. Meowra;
- within-subject crossover design;
- two matched four-question task sets;
- four crossover assignment cells;
- four task categories;
- multiple-choice rather than code editing;
- Neutral informed consent for everyone;
- Meowra acts as study host, not programming tutor;
- avatar visible throughout Meowra treatment screens;
- no hints;
- no correctness feedback;
- all eight UEQ-S items after each condition;
- Hedonic Quality is the primary UX outcome;
- Pragmatic Quality is a secondary tradeoff outcome;
- accuracy and response time are secondary;
- task timing excludes transition/encouragement screens;
- final preference asks which format participants would prefer for another similar study;
- one open-ended explanation follows preference;
- no Agent Persona Instrument;
- primary inferential test is a paired-samples t-test;
- the main claim concerns participant UX, not compiler diagnostics or AI personification.

---

# 64. Immediate Next Steps

The design is sufficiently specified to begin implementation.

Before formal data collection:

```text
1. Finalize all eight multiple-choice distractor sets.
2. Finalize Neutral study-host text.
3. Finalize Dr. Meowra study-host text.
4. Implement the two conditions in Unity.
5. Implement four-cell crossover assignment.
6. Implement UEQ-S.
7. Implement timing and accuracy logging.
8. Implement preference + open response.
9. Pilot the complete study.
10. Correct task mismatches or UI problems.
11. Freeze and tag the study build.
12. Begin formal data collection.
```
