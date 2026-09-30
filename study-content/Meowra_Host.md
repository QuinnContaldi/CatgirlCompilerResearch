# Meowra condition presentation

Editable participant copy; scientific authority: [Research_Design.md](../Research_Design.md).
Tutorials must convey equivalent information. Host messages may encourage participation,
but must never provide programming hints, answer assistance, or correctness feedback.
Messages are selected by stage/question position, never by answer or task set.

Edit the JSON block, then use **Tools → Experiment → Import Study Content**.

```json
{
  "meowraBlockIntroduction": "Hi there, nya! I'm Dr. Meowra, your host for this section. We'll go through four short programming questions together. My paws are ready to welcome you!",
  "meowraTutorial": "Here's how this section works, nya! For each question, read the code and select the answer that best identifies the issue. Select one answer, then press Submit to continue. You can change your choice before submitting; submitted answers cannot be changed. I won't provide hints or correctness feedback.",
  "meowraCompletion": "Four of four questions submitted, meow! This section is complete. Thanks for joining me! Continue to rate the study interface.",
  "meowraTransitions": [
    "One of four questions submitted, nya! Three remain. Continue to question 2.",
    "We've reached the halfway point! Two of four questions submitted; two remain. Continue to question 3.",
    "Three of four questions submitted! One remains. Continue to question 4."
  ],
  "meowraIntroductionDialogue": "Welcome to my corner of the study, meow! I'm glad you're here.",
  "meowraTutorialDialogue": "I'll handle the greetings; you handle the buttons, nya! Here's how they work.",
  "meowraTaskDialogue": [
    "Question 1 of 4. I'm your host for this section, nya!",
    "Question 2 of 4. I'm here with you.",
    "Question 3 of 4. Your host is still here, meow!",
    "Question 4 of 4. The last question in our section."
  ],
  "meowraCompletionDialogue": "That's my hosting shift wrapped up, nya! Thank you for spending this section with me.",
  "meowraTransitionDialogue": [
    "Meow! Our little study has turned its first page.",
    "Purrfect—we've reached the halfway point! Thanks for joining me.",
    "My paws are ready to welcome you to the last page, nya!"
  ]
}
```
