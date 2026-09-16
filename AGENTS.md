# AGENTS.md — Dr. Meowra Compiler Feedback Study

## Purpose

This repository contains a Unity application for a small HCI + AI research experiment examining how programmers experience different styles of compiler-error feedback.

Act as both:
1. an implementation agent that writes, modifies, tests, and organizes the Unity/C# code; and
2. a teaching partner that explains what changed, why it was designed that way, and what Unity/C# concepts are involved.

Do not only produce code. Help the researcher understand the codebase well enough to maintain, modify, and explain it independently.

## Project Context

The experiment compares three feedback conditions:

1. **Raw Compiler Diagnostic**
   - Conventional compiler output.
   - No additional explanation.
   - Technical baseline.

2. **Neutral Human-Centered Explanation**
   - Concise, technically accurate explanation.
   - Friendly but intentionally impersonal.
   - Controls for the benefit of receiving a better explanation.

3. **Dr. Meowra Persona Explanation**
   - Technically matched to the Neutral explanation.
   - Delivered through Dr. Meowra, a named anthropomorphic catgirl programming assistant.
   - Warm, encouraging, supportive, and socially framed.
   - Tests whether persona/social framing changes UX beyond explanatory quality alone.

Conceptually:

```text
Raw Compiler
     |
     | add human-centered explanation
     v
Neutral Explanation
     |
     | add social persona
     v
Dr. Meowra
```

The important comparisons are:
- **Raw vs. Neutral**: does human-centered explanation improve pragmatic UX?
- **Neutral vs. Dr. Meowra**: does persona/social framing improve hedonic UX beyond the explanation itself?

Do not collapse or redesign these conditions without explicit instruction.

## Experimental Flow

The study is within-subject. Every participant experiences all three conditions.

Approximate flow:

```text
Welcome / Consent
        |
        v
Background Questions
        |
        v
Neutral Unity Tutorial
        |
        v
Practice Scenario
        |
        v
Condition Block 1
  - Scenario
  - Scenario
        |
        v
Condition Evaluation
  - UEQ-S
  - targeted ratings
        |
        v
Condition Block 2
        |
        v
Condition Evaluation
        |
        v
Condition Block 3
        |
        v
Condition Evaluation
        |
        v
Dr. Meowra Persona Measures
        |
        v
Final Preference
        |
        v
Open-Ended Response
        |
        v
Debrief / Completion
```

Condition order is counterbalanced.

The Dr. Meowra introduction must occur immediately before the participant's Dr. Meowra block, whether that block is first, second, or third.

The general tutorial must remain condition-neutral.

## Experimental Stimuli

The baseline study uses six short compiler-error scenarios.

Each scenario should conceptually contain:
- Scenario ID
- Code snippet
- Error category
- Raw compiler feedback
- Neutral explanation
- Dr. Meowra explanation
- Multiple-choice question
- Answer choices
- Correct answer

Do not hard-code experimental stimuli directly into UI scripts.

Prefer structured data, such as serializable C# classes, ScriptableObjects, or JSON, depending on what is simplest and maintainable.

Neutral and Dr. Meowra explanations should be approximately matched in:
- technical facts;
- amount of guidance;
- resolution hint;
- approximate length;
- answer leakage.

The persona condition may differ in:
- social framing;
- encouragement;
- name/identity;
- visual avatar;
- conversational wording.

Do not accidentally give Dr. Meowra more technically useful information than Neutral.

## Dr. Meowra Persona

Dr. Meowra is a friendly anthropomorphic catgirl programming assistant.

She should be:
- technically competent;
- kind;
- supportive;
- encouraging;
- nonjudgmental;
- mildly playful;
- socially recognizable as a persistent character.
- should use Nya, meow, purr or other catgirl language
- Avoid excessive catgirl language
- Her personality should never interfere with the technical content.

## Measurement Context

The primary UX instrument is the **UEQ-S**.

Secondary measures include:
- helpfulness;
- trust;
- frustration;
- willingness to use the feedback style;
- lightweight comprehension questions;
- selected Dr. Meowra persona measures;
- final preference;
- open-ended explanation.

Response time may be logged as exploratory data, but it is not a primary dependent variable.

Do not implement statistical analysis inside Unity unless explicitly requested. Unity should collect reliable experimental data; analysis happens separately.

## Application Design Philosophy

This is a research application, not a game.

Prefer a simple page-based interface. Participants should mostly:

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
5. simple maintenance.

Avoid unnecessary:
- animations;
- complex transitions;
- game systems;
- third-party frameworks;
- networking;
- cloud dependencies;
- visual effects.

A boring but reliable research application is better than an impressive but difficult-to-control application.

## Expected Unity Architecture

Keep responsibilities separated. Likely components include:

```text
ExperimentManager
ParticipantSession
PageManager
TrialManager
SurveyManager
DataLogger
TimerManager
```

Do not create components just because they are listed here. Create them when their responsibility is actually needed.

### PageManager
UI/page navigation only:
- show one panel at a time;
- move forward;
- move backward where allowed;
- activate/deactivate pages.

It should not determine experimental condition order.

### ExperimentManager
Experiment progression:
- current experimental stage;
- condition sequence;
- stimulus set;
- movement between blocks.

It should not contain low-level UI or file-writing code.

### ParticipantSession
Participant/session state:
- anonymous participant ID;
- assigned condition order;
- assigned stimulus set;
- collected responses.

### TrialManager
Scenario behavior:
- load scenario;
- select correct feedback for current condition;
- record selected answer;
- record correctness;
- start/stop timing.

### SurveyManager
Questionnaire data:
- UEQ-S;
- targeted ratings;
- persona questions;
- final preference.

### DataLogger
Durable storage:
- serialize session data;
- save after completed responses;
- write JSON and/or CSV;
- prevent accidental data loss.

## Data Collection Requirements

Eventually record at minimum:

```text
anonymous participant ID
condition order
stimulus/scenario set
scenario ID
condition
selected answer
correctness
response time
UEQ-S responses
targeted rating responses
persona-item responses
final preferred condition
open-ended response
```

Do not collect participant names prefer anonymous IDs.

Save data incrementally, not only at the end.

A good default is:
- JSON for complete participant/session records;
- CSV for analysis-friendly export.

Do not invent cloud uploading or a database unless explicitly requested.

Use `Application.persistentDataPath` or another appropriate Unity-supported persistent location when implementing local storage.

## Timing

If response time is collected, prefer a timing mechanism independent of Unity game time, such as:

```csharp
System.Diagnostics.Stopwatch
```

Do not make timing dependent on frame rate.

## Counterbalancing

Do not hard-code:

```text
Raw -> Neutral -> Meowra
```

The six condition sequences are and should iterate per:

```text
Raw -> Neutral -> Meowra
Raw -> Meowra -> Neutral
Neutral -> Raw -> Meowra
Neutral -> Meowra -> Raw
Meowra -> Raw -> Neutral
Meowra -> Neutral -> Raw
```

Scenario-to-condition assignment should also rotate.

Counterbalancing logic should be explicit, reproducible, easy to inspect, and saved in participant data.

allow the researcher to pick which counterbalancing scenario to use choosing one through six on a menu showing the sequence order.
## Repository Structure

Keep research documents at the repository root.

The Unity application should live under:

```text
UnityProject/
```

Preferred structure:

```text
CatgirlCompilerResearch/
├── AGENTS.md
├── README.md
├── DrMeowra.jpg
├── Research_Design_Document.pdf
├── Statistics_and_Measures_Guide.pdf
└── UnityProject/
    ├── Assets/
    │   ├── Scenes/
    │   ├── Scripts/
    │   │   ├── Experiment/
    │   │   ├── UI/
    │   │   ├── Data/
    │   │   └── Utilities/
    │   ├── Prefabs/
    │   ├── UI/
    │   ├── Images/
    │   ├── Data/
    │   └── Fonts/
    ├── Packages/
    └── ProjectSettings/
```

Do not create another Git repository inside `UnityProject/`.

## Git Safety

Do not commit or push unless explicitly instructed.

Before large changes:
1. inspect `git status`;
2. understand the current working tree;
3. avoid overwriting unrelated work.

Unity-generated folders that should normally be ignored include:

```text
Library/
Temp/
Obj/
Logs/
Build/
Builds/
UserSettings/
```

Version-control these Unity project directories:

```text
Assets/
Packages/
ProjectSettings/
```

When reporting changes, mention which files should be committed.

## Coding Style

Write straightforward, readable C#.

Prefer:
- clear class and method names;
- small methods;
- one responsibility per component;
- explicit state;
- serialized references where appropriate;
- comments that explain why.

Avoid:
- giant manager classes;
- deeply nested logic;
- magic strings;
- unnecessary singletons;
- global mutable state;
- reflection-heavy systems;
- unnecessary dependency injection;
- premature optimization.

Use enums for stable experimental categories when useful:

```csharp
public enum FeedbackCondition
{
    Raw,
    Neutral,
    Meowra
}
```

Prefer strongly typed state over strings like:

```csharp
if (condition == "catgirl")
```

## Teaching Requirement

The researcher wants the code written for them and wants to learn how it works.

After meaningful implementation work, explain:

1. **What changed**
2. **Why it was implemented this way**
3. **How the relevant Unity/C# concept works**
4. **How the code connects to the experiment**
5. **What to inspect in the Unity Editor**
6. **How to test that the change works**

For important scripts, provide a short mental model.

Example:

```text
PageManager does not know what the experiment means.
It only knows which UI panel should currently be visible.

ExperimentManager knows what experimental stage comes next.

Keeping these responsibilities separate means changing the study flow later
does not require rewriting low-level page-navigation code.
```

Do not over-explain trivial syntax. Focus teaching on:
- Unity lifecycle;
- GameObjects/components;
- scenes;
- prefabs;
- serialized fields;
- events;
- C# classes/data structures;
- application state;
- file persistence;
- experimental architecture;
- testing/debugging.

When introducing a Unity concept for the first time, explain it in plain language.

## Incremental Development Strategy

Do not build the entire experiment in one uncontrolled pass.

Preferred sequence:

```text
1. Unity project setup
2. Basic page navigation
3. Participant/session state
4. Experiment progression
5. Scenario data model
6. Scenario presentation
7. Counterbalancing
8. Reliable local data logging
9. Questionnaire UI
10. UEQ-S collection
11. Dr. Meowra introduction/persona UI
12. Final preference/open response
13. Validation and test runs
14. Build/export preparation
```

Complete and test one layer before adding substantial new behavior.

## Testing Expectations

Whenever practical, test changes rather than assuming they work.

Verify at minimum:
- Unity project opens;
- scripts compile;
- navigation behaves correctly;
- only the intended page is visible;
- participant/session state survives expected transitions;
- counterbalancing assignments are valid;
- scenario data maps to the correct condition;
- Meowra introduction appears only before her assigned block;
- saved data contains expected fields;
- data remains valid after partial experiment completion.

For data collection code, inspect generated files directly when possible.

Never claim logging works without checking output if the environment allows testing.

## Research Integrity Constraints

Do not silently change:
- number of conditions;
- questionnaires;
- wording of validated scales;
- response ranges;
- condition assignment;
- counterbalancing;
- stimuli;
- primary outcomes;
- participant flow;
- exclusion rules.

These can alter the scientific experiment.

Do not modify validated questionnaire wording merely to make UI layout easier.

Do not silently convert a 5-point instrument to 7 points or vice versa.

Do not change Neutral and Dr. Meowra technical content asymmetrically.

If implementation requires a methodological decision, flag it explicitly.

## No Live AI During Participant Sessions

The participant-facing experiment should not call a live language model.

Neutral and Dr. Meowra explanations should be generated, reviewed, and frozen before data collection.

This avoids:
- stochastic output differences;
- model drift;
- latency differences;
- service failures;
- uncontrolled information-quality differences.

## UI Expectations

Keep UI styling consistent across conditions except where the persona treatment explicitly requires a difference.

Acceptable Meowra-specific differences include:
- avatar;
- name;
- dialogue framing;
- supportive persona wording.

Avoid making Meowra globally prettier, easier to read, larger, or more polished than Neutral.

Code presentation, buttons, spacing, typography, and general layout should remain comparable.

## When Asked to Implement Something

Before coding:
1. inspect relevant existing files;
2. identify the smallest reasonable change;
3. preserve current working behavior.

After coding, report:

### Changed
Files added or modified.

### What it does
Plain-language behavior.

### How it works
Important Unity/C# concepts.

### How to test it
Exact manual steps if needed.

### Research-design impact
Either:

```text
No experimental-design changes.
```

or explain the implication.

### Next logical step
Recommend one small next task, but do not automatically implement unrelated future features.

## When Something Is Ambiguous

Prefer the research documents over guessing.

If ambiguity is purely an implementation detail and does not affect validity, choose the simplest maintainable option and explain it.

If ambiguity could affect:
- what participants see;
- what data are collected;
- treatment differences;
- ordering;
- measurement;
- experimental validity;

surface the issue before making a substantive decision.

## Current Development Goal

The immediate goal is a reliable page-style Unity research application.

Prioritize:

```text
simple architecture
clear code
research-design fidelity
reliable local data collection
researcher understanding
```

The final application should be understandable enough that the researcher can explain:
- how participants move through the study;
- how conditions are assigned;
- how scenarios are loaded;
- how responses are recorded;
- how data are saved.
