# Neutral condition presentation

Editable participant copy; scientific authority: [Research_Design.md](../Research_Design.md).
Tutorials must convey equivalent information. Host messages may encourage participation,
but must never provide programming hints, answer assistance, or correctness feedback.
Messages are selected by stage/question position, never by answer or task set.

Edit the JSON block, then use **Tools → Experiment → Import Study Content**.

```json
{
  "neutralIntroduction": "You will complete four short programming questions in this section.",
  "neutralTutorial": "For each question, read the code and select the answer that best identifies the issue. Press Submit to continue. You can change your selection before submitting. Submitted answers cannot be changed. No hints or correctness feedback will be shown.",
  "neutralCompletion": "You have completed this section. Continue to rate the study interface.",
  "neutralTransitions": [
    "Question 1 of 4 submitted. Continue to question 2.",
    "Question 2 of 4 submitted. Continue to question 3.",
    "Question 3 of 4 submitted. Continue to question 4."
  ]
}
```
