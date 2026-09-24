# AGENTS.md — Dr. Meowra Compiler Feedback Study

## Purpose

This repository contains a Unity HCI research experiment comparing three compiler-feedback conditions:

1. **Raw** — conventional compiler output.
2. **Neutral** — concise, technically accurate, human-centered explanation.
3. **Meowra** — technically matched to Neutral, but delivered through the Dr. Meowra persona.

Act as:

* an implementation agent that writes, modifies, tests, and organizes Unity/C# code;
* a teaching partner that briefly explains important design and Unity/C# concepts.

Prefer simple, maintainable solutions.

---

## Research Design

The study is within-subject: every participant experiences all three conditions.

Important comparisons:

* Raw vs. Neutral → effect of improved explanation.
* Neutral vs. Meowra → effect of persona/social framing.

Do not alter the experimental design unless explicitly instructed.

### Feedback Matching

Neutral and Meowra must remain approximately matched in:

* technical facts;
* guidance;
* resolution hints;
* length;
* answer leakage.

Meowra may differ only through:

* social framing;
* encouragement;
* character identity;
* avatar;
* conversational wording.

Do not give Meowra more technically useful information than Neutral.

---

## Dr. Meowra

Dr. Meowra is a friendly anthropomorphic catgirl programming assistant.

She should be:

* technically competent;
* kind;
* supportive;
* encouraging;
* nonjudgmental;
* mildly playful;
* recognizable as a persistent character.

She may occasionally use words such as `nya`, `meow`, or `purr`, but avoid excessive catgirl language.

Personality must never interfere with technical accuracy.

---

## Experiment Flow

Approximate order:

```text
Welcome / Consent
Background Questions
Neutral Tutorial
Practice
Condition Block
Condition Evaluation
Condition Block
Condition Evaluation
Condition Block
Condition Evaluation
Meowra Persona Measures
Final Preference
Open Response
Debrief
```

Condition order must be counterbalanced.

Valid sequences:

```text
Raw -> Neutral -> Meowra
Raw -> Meowra -> Neutral
Neutral -> Raw -> Meowra
Neutral -> Meowra -> Raw
Meowra -> Raw -> Neutral
Meowra -> Neutral -> Raw
```

Allow the researcher to select counterbalancing sequence 1–6.

The Dr. Meowra introduction must appear immediately before the participant's Meowra block.

The general tutorial must remain condition-neutral.

---

## Experimental Stimuli

The baseline study contains six compiler-error scenarios.

Each scenario should contain:

```text
scenario ID
code snippet
error category
raw feedback
neutral feedback
Meowra feedback
question
answer choices
correct answer
```

Do not hard-code stimuli into UI scripts.

Prefer structured data such as:

* serializable C# classes;
* ScriptableObjects;
* JSON.

---

## Architecture

Keep responsibilities separated.

Suggested components:

```text
ExperimentManager
ParticipantSession
PageManager
TrialManager
SurveyManager
DataLogger
TimerManager
```

### Responsibilities

**ExperimentManager**

* experiment progression;
* condition sequence;
* stimulus assignment.

**ParticipantSession**

* anonymous participant ID;
* assigned sequence;
* responses.

**PageManager**

* UI/page visibility and navigation only.

**TrialManager**

* load scenarios;
* select feedback condition;
* record answers and correctness;
* control response timing.

**SurveyManager**

* UEQ-S;
* targeted ratings;
* persona measures;
* final preference.

**DataLogger**

* save session data;
* write JSON/CSV;
* save incrementally.

Avoid giant manager classes and unnecessary architecture.

---

## Data Collection

Record at minimum:

```text
participant ID
condition order
stimulus assignment
scenario ID
condition
selected answer
correctness
response time
UEQ-S responses
targeted ratings
persona responses
final preference
open-ended response
```

Do not collect participant names.

Save incrementally.

Prefer:

* JSON for full session records;
* CSV for analysis.

Use Unity-supported persistent storage such as:

```csharp
Application.persistentDataPath
```

For timing, prefer:

```csharp
System.Diagnostics.Stopwatch
```

Timing must not depend on frame rate.

---

## UI

This is a research application, not a game.

Participants should primarily:

```text
read
select
click
advance
```

Prioritize:

1. experimental consistency;
2. clarity;
3. reproducibility;
4. reliable data collection;
5. maintainability.

Avoid unnecessary:

* animations;
* transitions;
* game systems;
* networking;
* cloud dependencies;
* visual effects.

Keep typography, spacing, controls, and code presentation comparable across conditions.

Meowra-specific differences may include:

* avatar;
* name;
* dialogue framing;
* persona wording.

Do not make Meowra easier to read or visually superior to Neutral.

---

## No Live AI During Participant Sessions

Do not call a live language model during the experiment.

Neutral and Meowra responses must be generated, reviewed, and frozen before data collection.

This avoids:

* stochastic outputs;
* model drift;
* latency;
* service failure;
* uncontrolled technical differences.

---

## Coding Style

Write straightforward C#.

Prefer:

* clear names;
* small methods;
* explicit state;
* serialized references;
* enums for experimental categories;
* comments explaining why.

Example:

```csharp
public enum FeedbackCondition
{
    Raw,
    Neutral,
    Meowra
}
```

Avoid:

* giant classes;
* magic strings;
* unnecessary singletons;
* global mutable state;
* reflection-heavy systems;
* premature optimization.

---

## Repository Structure

Research documents remain at the repository root.

Unity project:

```text
UnityProject/
```

Typical structure:

```text
Assets/
├── Scenes/
├── Scripts/
│   ├── Experiment/
│   ├── UI/
│   ├── Data/
│   └── Utilities/
├── Prefabs/
├── UI/
├── Images/
└── Data/
```

Do not create another Git repository inside `UnityProject/`.

Ignore normal Unity-generated directories such as:

```text
Library/
Temp/
Obj/
Logs/
Build/
Builds/
UserSettings/
```

Version control:

```text
Assets/
Packages/
ProjectSettings/
```

Do not commit or push unless explicitly instructed.

---

## Development Strategy

Implement incrementally:

```text
1. Unity setup
2. Page navigation
3. Participant/session state
4. Experiment progression
5. Scenario data model
6. Scenario presentation
7. Counterbalancing
8. Data logging
9. Questionnaire UI
10. UEQ-S
11. Meowra introduction
12. Final preference/open response
13. Validation
14. Build/export
```

Complete and test one layer before adding major new behavior.

---

## Testing

When practical, verify:

* Unity opens;
* scripts compile;
* navigation works;
* only intended pages are visible;
* condition assignments are valid;
* stimuli map to the correct conditions;
* Meowra introduction appears at the correct point;
* responses save correctly;
* generated JSON/CSV contains expected data.

Do not claim something works without testing it when testing is possible.

---

## Research Integrity

Do not silently modify:

* conditions;
* stimuli;
* questionnaires;
* validated wording;
* rating scales;
* counterbalancing;
* condition assignment;
* participant flow;
* primary outcomes;
* exclusion rules.

Do not asymmetrically change Neutral and Meowra technical content.

If an implementation decision could affect experimental validity, flag it before changing the design.

---

## When Implementing

Before coding:

1. inspect relevant files;
2. identify the smallest reasonable change;
3. preserve working behavior.

After meaningful work, briefly report:

### Changed

Files added or modified.

### What it does

Plain-language behavior.

### How it works

Important Unity/C# concepts.

### How to test

Exact testing steps.

### Research-design impact

State whether the experimental design changed.

### Next step

Recommend one small logical next task.

Do not automatically implement unrelated future work.
