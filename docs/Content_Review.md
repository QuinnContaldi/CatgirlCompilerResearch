# Content review — two-condition draft v2

Reviewed 2026-09-26 against [Research_Design.md](../Research_Design.md). This is a desk review, not a
pilot, content freeze, or institutional approval. Scored sessions were disabled at
the time of this review. They have since been enabled for complete scored runs;
see [current session instructions](Running_Sessions.md).

This is a dated review snapshot. Repeated task/host wording below is evidence of
that review, not an editable content source. Current sources are in `Stimuli/`
and `study-content/`; see [authoring](../UnityProject/AUTHORING.md).

## Changes applied

- Revised incorrect options in all eight tasks. A/B versions have identical option
  wording and keys, so revisions do not favor either task set or interface.
- Preserved all code snippets, the common prompt, correct options, keys, task order,
  four-cell assignment, UEQ-S anchors/scales, and final preference wording.
- Synchronized `Consent_Text.md` with the active asset. Removed obsolete task and
  questionnaire descriptions and unsupported duration/compensation assumptions.
- Advanced the draft version to `two-condition-draft-v2`.

## Task review

| Pair | Key | Intended concept | Review finding |
| --- | --- | --- | --- |
| A1 / B1 | A | Assignment where comparison appears intended | Assignment in an if condition is valid C. The correct option intentionally says “appears to be intended,” not “fails compilation.” Intent remains an inference; probe this in the pilot. |
| A2 / B2 | B | String assigned to integer | Correct option describes the mismatch, without promising identical compiler behavior across toolchains. |
| A3 / B3 | C | Missing closing parenthesis | Both snippets omit the same delimiter and retain their closing brace. |
| A4 / B4 | D | Commas instead of semicolons in for header | Both snippets have the same separator issue; additional compiler diagnostics can cascade from it. |

The snippets are fragments, not standalone programs. For local verification they
were wrapped in `void task(void) { ... }` with `#include <stdio.h>`. GCC 15.2.0,
`-std=c17 -Wall -Wextra -fsyntax-only`, warned about assignment used as a truth
value for A1/B1 and diagnosed the intended issues for the remaining pairs. No
participant-facing compiler or diagnostics were added. Unused-variable warnings
from this wrapper are not additional task targets.

Current keys are A, B, C, D in both sets. This creates a predictable positional
pattern and identical options may support recall across sets. Those are pilot
risks, not evidence that the sets are equivalent. No keys, order, or options were
randomized during this review; changing them would require a separate design
revision and corresponding logging/validation changes.

## Current answer options

### A1 / B1

- **A.** The code assigns a value where a comparison appears to be intended.
- **B.** The variable is used before it is declared.
- **C.** The if statement is missing its opening parenthesis.
- **D.** There is no issue with the code.

### A2 / B2

- **A.** The variable is used before it is declared.
- **B.** A string value is being assigned to an integer variable.
- **C.** The declaration is missing a statement-ending semicolon.
- **D.** There is no issue with the code.

### A3 / B3

- **A.** The condition is missing its comparison operator.
- **B.** The if block is missing its closing brace.
- **C.** The closing parenthesis of the if condition is missing.
- **D.** There is no issue with the code.

### A4 / B4

- **A.** The loop variable is used before it is declared.
- **B.** The loop header is missing its update expression.
- **C.** The loop body is missing its closing brace.
- **D.** The for header uses commas where semicolons are required.

## Treatment review

Both tutorials convey the same six facts: read code, select the best issue,
Submit to advance, selection can change before submission, submitted answers
cannot change, and no hints or correctness feedback. No technical advantage was
found in Meowra's wording. Encouragement depends on progress, not correctness.
No treatment text was changed in this pass.

| Screen | Neutral words | Meowra words | Functional information |
| --- | --- | --- | --- |
| Introduction | 10 | 21 | Four questions in this section. Meowra additionally identifies herself as host. |
| Tutorial | 39 | 45 | Same controls and restrictions. |
| Completion | 11 | 15 | Section finished; proceed to evaluation. |

The short introduction is longer in Meowra because of identity/social framing;
introduction and transition reading time is excluded from task timing. Header
framing remains constant within each condition. Both conditions use the same
code font, answer controls, task geometry, and progress counter.

### Exact treatment copy

**Introduction — Neutral:** You will complete four short programming questions in this section.

**Introduction — Meowra:** Hi, I'm Dr. Meowra. I'll be your host for four short programming questions. We'll take them one at a time, nya.

**Tutorial — Neutral:** For each question, read the code and select the answer that best identifies the issue. Press Submit to continue. You can change your selection before submitting. Submitted answers cannot be changed. No hints or correctness feedback will be shown.

**Tutorial — Meowra:** For each question, take a look at the code and choose the answer that best identifies the issue. Press Submit and we'll keep going together. You can change your choice before submitting; submitted answers cannot be changed. I won't provide hints or correctness feedback, nya.

**Completion — Neutral:** You have completed this section. Continue to rate the study interface.

**Completion — Meowra:** Thank you for completing this section with me, nya. Continue to rate the study interface.

## Consent and background review

The consent now describes the neutral overview, optional background, eight tasks,
two evaluations, future-study preference, optional prose, local logging and lack
of correctness feedback. Its document and asset text match exactly.

The researcher must supply:

- Institution, researcher and participant-rights contact details.
- Who accesses records, storage protections, retention, and sharing/quotation policy.
- Whether and how submitted records can be withdrawn, with any deadline.
- Compensation/course-credit arrangements or an explicit no-compensation decision.
- Expected duration established with the revised flow.

Background currently uses one optional free-text prompt for five suggested
variables. This is not a validated instrument and requires manual coding; a
structured replacement would be a separate questionnaire decision. Do not infer
that all five variables were answered when the field is nonempty.

## Pilot next

1. Check whether novice readers infer comparison intent in A1/B1.
2. Ask pilot participants why they rejected each distractor; look for obviously
   implausible choices and unintended alternative correct answers.
3. Compare A/B accuracy and reading burden; do not infer equivalence from matching
   word counts or successful software tests.
4. Watch for key-position learning, answer recall, and scrolling difficulties.
5. Measure full-session duration and finalize consent details, then review/freeze
   the version explicitly before formal participant collection.
