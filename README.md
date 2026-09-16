# Can a Catgirl Make Compiler Errors Better?

<p align="center"> <img src="DrMeowra.jpg" alt="Dr. Meowra" width="500"> </p>

Overview

This repository contains the materials and implementation for a small HCI + AI study exploring how programmers experience different styles of compiler feedback.

The study compares three forms of compiler-error feedback:

Raw compiler diagnostics
Neutral human-centered explanations
Persona-based explanations delivered by Dr. Meowra

Dr. Meowra is a friendly anthropomorphic catgirl programming assistant designed to provide technically equivalent compiler explanations using a more social, supportive, and personable communication style.

The central question is not whether Dr. Meowra makes programmers faster or better at debugging. Instead, the study focuses on the user experience of receiving compiler feedback, including pragmatic and hedonic UX, perceived helpfulness, trust, frustration, and preference.

This project is intended as a Programming Languages + Human-Computer Interaction study examining whether the presentation of compiler feedback should be treated as a meaningful design dimension of programming-language tooling.

Study Documentation

Detailed information about the experimental design, motivation, measures, counterbalancing, participant flow, and planned analysis can be found in the included research design document:

Research_Design_Document.pdf

The research design document contains the full study protocol and should be treated as the primary reference for the experiment.

A separate statistics and measurement guide is also included for documentation and study preparation. It explains the statistical methods and measurement instruments used in the experiment, including UEQ-S scoring, paired Wilcoxon signed-rank tests, Holm correction, effect sizes, and the rationale for the planned within-subject analysis.

Experimental Design

The experiment uses a within-subject design in which each participant experiences all three feedback conditions.

At a high level:

Raw Compiler
     ↓
Neutral Explanation
     ↓
Dr. Meowra Persona

The key comparisons are:

Raw vs. Neutral — evaluates whether human-centered explanation improves pragmatic user experience.
Neutral vs. Dr. Meowra — evaluates whether adding a social persona improves hedonic user experience beyond the explanation itself.

Condition order and scenario assignment are counterbalanced across participants.

Application

The study application is being developed in Unity as a simple page-based interface.

The initial project is in `UnityProject/`. See [Unity setup and navigation](UnityProject/README.md)
for launch instructions, the scene structure, and the initial navigation checks.

Participants will:

Complete a short programming-background questionnaire.
Complete a neutral tutorial and practice scenario.
Work through six short compiler-error scenarios.
Evaluate each feedback condition using UX measures.
Complete selected Dr. Meowra persona measures.
Select their preferred feedback style.
Optionally explain their preference.

The application will automatically record responses, condition order, scenario assignment, comprehension responses, questionnaire data, and exploratory response-time measurements.

Measures

The primary UX instrument is the User Experience Questionnaire – Short Version (UEQ-S).

The study also includes:

targeted Likert items for helpfulness, trust, frustration, and willingness to use the feedback style;
selected Credible and Engaging subscales from the Agent Persona Instrument;
lightweight multiple-choice comprehension checks;
final feedback-style preference; and
an optional qualitative response.

For the complete measurement and statistical rationale, see the included study documentation rather than this README.

Repository Structure

Current project files include:

.
├── DrMeowra.jpg
├── README.md
├── Research_Design_Document.pdf
└── UnityProject/

Additional experiment materials, stimuli, analysis scripts, and documentation may be added as the study develops.

Research Scope

This is intentionally a small exploratory study.

The goal is not to establish that persona-based compiler feedback improves programming performance. Instead, the project asks whether a socially designed programming assistant can change the experience of interacting with compiler diagnostics while preserving the underlying technical information.

Future work could examine direct debugging performance, learning and retention, long-term use, professional programmers, adaptive personas, and the separate effects of visual appearance, tone, encouragement, and anthropomorphic cues.

Status

Current stage: Experimental design and Unity implementation.

The study protocol and planned analysis have been defined. Development of the participant-facing Unity application is ongoing.

# Statistics & Measurement Guide

## Dr. Meowra Persona-Based Compiler Feedback Study

*A plain-language study guide for the planned within-subject HCI + AI experiment*

## What This Guide Is For

This document explains what the study measures, why each statistical method is being used, what the outputs mean, and how to interpret results with a small sample of approximately 15–20 participants. It is a learning guide, not a substitute for statistical consultation or a preregistration.

## 1. The Study in Statistical Terms

The experiment has one independent variable: how compiler feedback is presented. Every participant experiences all three feedback conditions, so the design is **within-subject (repeated measures)**.

| Condition | What changes | Scientific purpose |
| --- | --- | --- |
| Raw compiler diagnostic | Compiler output only | Baseline |
| Neutral explanation | Adds human-centered explanatory content | Tests the information-design effect |
| Dr. Meowra | Adds a persistent social persona to approximately matched explanatory content | Tests the persona/social-framing effect |

> **The core causal logic**
>
> Raw → Neutral changes the quality/presentation of the explanation. Neutral → Dr. Meowra changes the social/persona presentation while attempting to hold technical information approximately constant. This is why Neutral vs. Dr. Meowra is the cleanest test of the persona idea.

### Primary Hypotheses

| Hypothesis | Planned outcome | Planned comparison |
| --- | --- | --- |
| H1: Information-design effect | UEQ-S Pragmatic Quality | Raw vs. Neutral |
| H2: Persona effect | UEQ-S Hedonic Quality | Neutral vs. Dr. Meowra |

### What Is Confirmatory vs. Secondary?

A **confirmatory test** is one you decide on before seeing the data and use to evaluate a prespecified hypothesis. Secondary or exploratory measures help explain the result but are not treated as additional headline hypothesis tests.

| Role | Measures |
| --- | --- |
| Confirmatory | H1: Pragmatic UEQ-S, Raw vs. Neutral; H2: Hedonic UEQ-S, Neutral vs. Meowra |
| Secondary/descriptive | UEQ-S Overall, four targeted Likert items, comprehension accuracy, final preference |
| Persona characterization | Agent Persona Instrument: selected Credible and Engaging subscales |
| Qualitative | Open-ended reason for final preference |

## 2. UEQ-S: The Main User-Experience Measure

The **User Experience Questionnaire – Short (UEQ-S)** contains eight 7-position semantic-differential items. It gives separate Pragmatic and Hedonic Quality scores. In the study, the same UEQ-S is completed after Raw, Neutral, and Dr. Meowra.

| UEQ-S score | What it represents here | Why it matters |
| --- | --- | --- |
| Pragmatic Quality | Supportive, easy, efficient, clear | Best match for whether a human-centered explanation makes diagnostics easier to use |
| Hedonic Quality | Interesting, exciting, inventive, novel | Best match for whether persona/social framing changes the experiential quality |
| Overall score | Average across all eight items | Useful descriptively, but not a separate confirmatory hypothesis |

### How UEQ-S Scoring Works

Each item is answered on a seven-position scale between two opposing adjectives. Standard UEQ-S scoring converts responses to a scale from −3 to +3. The four pragmatic items are averaged to form Pragmatic Quality; the four hedonic items are averaged to form Hedonic Quality. An overall eight-item average can also be calculated, but in this study the two subscales are theoretically more informative.

> **Example**
>
> Suppose Participant 07 has Pragmatic scores of Raw = 0.50, Neutral = 1.50, and Meowra = 1.60. Their Raw → Neutral pragmatic difference is +1.00. For the persona hypothesis, suppose Hedonic scores are Neutral = 0.40 and Meowra = 1.70. Their Neutral → Meowra hedonic difference is +1.30.

### Why Not Use the Eight Individual UEQ-S Items as Eight Separate Tests?

Doing so would create many statistical tests and inflate the chance of false-positive findings. The validated subscale scores are the intended unit of analysis and better match the hypotheses.

## 3. Secondary Measures

### Four Targeted 7-Point Agreement Items

- I found this feedback helpful.
- I would trust this feedback while programming.
- This feedback would make compiler errors feel less frustrating.
- I would like my programming tools to communicate errors in this style.

These items are study-specific rather than a new validated scale. Their main purpose is interpretation: for example, if Meowra improves Hedonic Quality, these ratings can help show whether the change seems related to frustration, trust, helpfulness, or willingness to use the style. They should primarily be summarized descriptively; any inferential tests should be labeled exploratory.

### Agent Persona Instrument (API)

The study plans to use selected Credible and Engaging subscales once after participants experience Dr. Meowra. These do not compare all three conditions. Their role is to characterize whether the persona treatment actually came across as credible and engaging.

> **Important interpretation**
>
> A high API score would not prove that Meowra caused better UX. It would show that participants perceived the intended persona characteristics. Think of it as evidence that the persona manipulation was successfully experienced.

### Comprehension Accuracy

Each scenario has one multiple-choice comprehension question. With only two scenarios per condition, this measure is too sparse to support strong claims about learning or debugging performance. It is best used as a sanity check: did a more enjoyable presentation obviously harm understanding?

### Final Preference and Open Response

The forced-choice preference gives an intuitive descriptive result: how many participants chose Raw, Neutral, or Meowra. The open-ended response provides reasons for those choices and can be coded into a small number of recurring themes such as clarity, friendliness, professionalism, distraction, trust, or novelty.

## 4. Why This Is a Paired/Within-Subject Study

Every participant experiences every condition. That means the observations are **paired**. The study is not comparing one group of Raw users against a different group of Meowra users.

| Participant | Raw Pragmatic | Neutral Pragmatic | Difference |
| --- | ---: | ---: | ---: |
| P01 | 0.4 | 1.3 | +0.9 |
| P02 | 1.1 | 1.7 | +0.6 |
| P03 | −0.2 | 1.0 | +1.2 |
| P04 | 0.8 | 0.7 | −0.1 |

The important quantity is the **within-person difference**. A participant who rates everything harshly is compared with themselves, which reduces noise due to stable individual differences in rating style, experience, confidence, or personality.

> **Why this helps with N = 15–20**
>
> A within-subject design is statistically efficient because each participant serves as their own control. It does not make a small sample magically powerful, but it usually gives more sensitivity than splitting 20 people into separate groups.

## 5. Wilcoxon Signed-Rank Test

The planned confirmatory test is the **paired Wilcoxon signed-rank test**. It asks whether paired differences are systematically centered above or below zero. It is appropriate for a small paired study when you do not want to rely heavily on the normality assumption of a paired *t*-test.

> **Plain-English question**
>
> For the same participants, do the scores consistently move in one direction from Condition A to Condition B, and are the larger differences mostly in that direction?

### Worked Example

| Participant | Neutral Hedonic | Meowra Hedonic | Difference (M − N) | Absolute difference | Rank |
| --- | ---: | ---: | ---: | ---: | ---: |
| P1 | 0.5 | 1.0 | +0.5 | 0.5 | 2 |
| P2 | 0.8 | 1.8 | +1.0 | 1.0 | 4 |
| P3 | 0.2 | 1.5 | +1.3 | 1.3 | 5 |
| P4 | 1.3 | 1.1 | −0.2 | 0.2 | 1 |
| P5 | 0.4 | 1.3 | +0.9 | 0.9 | 3 |

The positive ranks sum to 2 + 4 + 5 + 3 = 14. The negative ranks sum to 1. The data strongly lean toward Meowra. The exact Wilcoxon test converts the observed signed-rank imbalance into a test statistic and *p*-value.

### What the *p*-Value Means

The *p*-value is not the probability that the hypothesis is true. It asks: if there were no systematic condition difference, how surprising would a signed-rank pattern this extreme be? A small *p*-value is evidence against the no-difference null model, but it does not tell you how large or practically important the effect is.

> **Do not write**
>
> “*p* < .05 means Meowra works.” Instead: “The paired ratings provided evidence of a condition difference; the direction, effect size, and uncertainty determine what that difference means.”

### Assumptions and Small-Sample Details

- The observations must be paired correctly: each participant contributes both scores being compared.
- Differences should be meaningfully rankable and approximately symmetric for the standard signed-rank interpretation.
- Zero differences and tied absolute differences need appropriate handling by the software.
- With a small sample, use an exact or small-sample-appropriate Wilcoxon implementation when available.

## 6. Holm Correction: Why We Adjust the Two Confirmatory Tests

The study has two confirmatory tests: H1 (Raw vs. Neutral Pragmatic Quality) and H2 (Neutral vs. Meowra Hedonic Quality). Testing multiple hypotheses increases the chance of at least one false positive. **Holm correction** controls the family-wise Type I error rate while being less wasteful than a simple Bonferroni correction.

### Step-by-Step Example

| Raw *p*-value | Order | Holm threshold | Decision |
| ---: | --- | ---: | --- |
| 0.018 | Smallest | 0.05 / 2 = 0.025 | Passes because 0.018 < 0.025 |
| 0.031 | Second | 0.05 / 1 = 0.05 | Passes because 0.031 < 0.05 |

Software can also return Holm-adjusted *p*-values directly. The practical rule is simple: report the adjusted *p*-values for the two confirmatory hypotheses, not the uncorrected values as if each were the only test.

> **Why only these two?**
>
> The custom Likert items, Overall UEQ-S, comprehension, and preference are secondary. Treating every possible measure as a new confirmatory test would create a “garden of *p*-values” and make a small exploratory study much harder to interpret.

## 7. Effect Size: How Strong Is the Treatment?

Statistical significance answers whether the data are inconsistent with a no-difference model at a chosen threshold. **Effect size** answers a different question: how strong and consistent is the observed difference? For a Wilcoxon signed-rank comparison, a useful paired effect size is the **rank-biserial correlation**.

One intuitive form is:

$$
r_{rb} = \frac{\text{positive rank sum} - \text{negative rank sum}}{\text{total rank sum}}
$$

The value ranges from −1 to +1. The sign indicates direction; the magnitude indicates how strongly the paired ranks favor one condition. For the worked example above:

$$
r_{rb} = \frac{14 - 1}{15} = 0.87
$$

This represents a very strong tendency favoring Meowra in that toy dataset.

| Value | Interpretation in this study |
| --- | --- |
| Near 0 | No consistent paired tendency |
| Positive | The second condition tends to score higher |
| Negative | The first condition tends to score higher |
| Closer to ±1 | More consistently one-sided paired differences |

> **For N = 15–20, effect size matters a lot**
>
> A nonsignificant result can still contain useful information if the estimated effect is substantial but uncertain. Report the effect estimate and the score distributions rather than collapsing the conclusion to “significant” versus “not significant.”

## 8. Power and Sensitivity With 15–20 Participants

**Power** is the probability that a study detects an effect of a specified size when that effect truly exists. You do not know the true Dr. Meowra effect in advance, so a **sensitivity analysis** is more honest than pretending to know the effect size.

> **Sensitivity-analysis question**
>
> Given the sample I can realistically recruit, what size within-participant effect would this study have a good chance of detecting?

Using the familiar paired-*t* framework only as a planning approximation, 80% power at alpha = .05 corresponds roughly to the following standardized paired effects. Exact Wilcoxon power depends on the paired-difference distribution, ties, and analysis details, so these are orientation numbers rather than guarantees.

| Completed participants | Approximate standardized paired effect for ~80% power |
| ---: | ---: |
| 15 | $d_z$ ≈ 0.78 |
| 18 | $d_z$ ≈ 0.70 |
| 20 | $d_z$ ≈ 0.66 |

Interpretation: the planned study is much better suited to detecting large, consistent UX shifts than subtle ones. That is acceptable for an exploratory conference paper as long as the claim is proportional to the sample.

### What If *p* > .05?

Do not conclude that the two interfaces are equivalent. With a small sample, failure to reach *p* < .05 can mean no effect, a small effect, or simply insufficient precision. Report the medians, paired effect size, participant-level pattern, and uncertainty.

## 9. The Exact Analysis Workflow

1. Score the UEQ-S for each participant separately for Raw, Neutral, and Dr. Meowra.
2. Calculate each participant’s Pragmatic and Hedonic Quality scores.
3. For H1, compare Raw vs. Neutral Pragmatic Quality using a paired Wilcoxon signed-rank test.
4. For H2, compare Neutral vs. Dr. Meowra Hedonic Quality using a paired Wilcoxon signed-rank test.
5. Apply Holm correction across the two confirmatory *p*-values.
6. Report condition medians and interquartile ranges plus the Wilcoxon statistic, Holm-adjusted *p*-value, and rank-biserial effect size.
7. Summarize Overall UEQ-S and the four targeted Likert items as secondary/descriptive evidence.
8. Report comprehension accuracy descriptively; do not make strong learning or debugging claims from two items per condition.
9. Report final preference as counts and percentages.
10. Code the open-ended responses into a small, transparent set of recurring themes.

## 10. How Different Result Patterns Would Be Interpreted

| Pattern | Interpretation |
| --- | --- |
| Raw < Neutral on Pragmatic; Neutral < Meowra on Hedonic | Evidence that explanation improves practical UX and persona adds an experiential benefit. |
| Raw < Neutral; Neutral ≈ Meowra | Evidence that better explanation matters, but persona adds little detectable UX benefit. |
| Raw ≈ Neutral; Neutral < Meowra | Suggests social/persona framing may matter more than neutral explanatory rewriting for the measured UX dimension. |
| No significant tests | Do not claim equivalence. Emphasize effect estimates, uncertainty, preference, and the exploratory nature of the sample. |

## 11. Reporting Templates

### H1 Example Sentence

> “Pragmatic Quality was higher in the Neutral condition than in the Raw condition. A paired Wilcoxon signed-rank test yielded *W* = [value], Holm-adjusted *p* = [value], with rank-biserial correlation $r_{rb}$ = [value].”

### H2 Example Sentence

> “Hedonic Quality was higher in the Dr. Meowra condition than in the Neutral condition. A paired Wilcoxon signed-rank test yielded *W* = [value], Holm-adjusted *p* = [value], with rank-biserial correlation $r_{rb}$ = [value].”

### Nonsignificant Example

> “Ratings favored Dr. Meowra descriptively, but the paired comparison did not reach the prespecified significance threshold after Holm correction. The estimated paired effect was [value], with substantial uncertainty due to the small exploratory sample.”

## 12. What to Learn Before You Analyze the Real Data

- How UEQ-S items are coded and averaged into Pragmatic and Hedonic Quality.
- What a paired difference is and why within-subject data must stay paired.
- How the Wilcoxon signed-rank test converts paired differences into signed ranks.
- The difference between a *p*-value and an effect size.
- Why Holm correction is applied to the two confirmatory hypothesis tests.
- How rank-biserial correlation communicates the strength and direction of a paired effect.
- Why a sensitivity analysis is more appropriate than pretending to know the true effect size in advance.
- Why secondary measures should be used to interpret the main result rather than creating many extra hypothesis tests.

> **One-sentence mental model**
>
> UEQ-S tells you what participants experienced; Wilcoxon asks whether the paired change is systematic; Holm protects the two planned tests; rank-biserial tells you how strongly the paired data favor one condition; sensitivity analysis tells you what size of effect your small sample is capable of detecting.

## 13. Measurement References Used in the Study Design

Schrepp, M., Hinderks, A., & Thomaschewski, J. (2017). Design and evaluation of a short version of the User Experience Questionnaire (UEQ-S). *International Journal of Interactive Multimedia and Artificial Intelligence, 4*(6), 103–108. <https://doi.org/10.9781/ijimai.2017.09.001>

Ryu, J., & Baylor, A. L. (2005). The psychometric structure of pedagogical agent persona. *Technology, Instruction, Cognition & Learning, 2*(4), 291–315.

Wilcoxon, F. (1945). Individual comparisons by ranking methods. *Biometrics Bulletin, 1*(6), 80–83.

Holm, S. (1979). A simple sequentially rejective multiple test procedure. *Scandinavian Journal of Statistics, 6*(2), 65–70.
