# Meowra condition presentation

Editable participant copy; scientific authority: [Research_Design.md](../Research_Design.md).
Tutorials must convey equivalent information. Host messages may encourage participation,
but must never provide programming hints, answer assistance, or correctness feedback.
Messages are selected by stage/question position, never by answer or task set.

Edit the JSON block, then use **Tools → Experiment → Import Study Content**.

```json
{
  "meowraBlockIntroduction": "Hi, I'm Dr. Meowra. I'll be your host for four short programming questions. We'll take them one at a time, nya.",
  "meowraTutorial": "For each question, take a look at the code and choose the answer that best identifies the issue. Press Submit and we'll keep going together. You can change your choice before submitting; submitted answers cannot be changed. I won't provide hints or correctness feedback, nya.",
  "meowraCompletion": "Thank you for completing this section with me, nya. Continue to rate the study interface.",
  "meowraTransitions": [
    "One question finished, nya. Let's continue to question 2 of 4.",
    "Two questions finished. Keep going at your own pace with question 3 of 4!",
    "Three questions finished, nya. Let's continue to question 4 of 4."
  ],
  "meowraIntroductionDialogue": "Welcome, nya! I'm happy to keep you company for this section.",
  "meowraTutorialDialogue": "Let's get settled in. Take a moment to read how this section works.",
  "meowraTaskDialogue": [
    "Let's begin with our first question, nya. Take your time.",
    "Here we are at question two. Keep going at your own pace!",
    "Welcome to question three, nya. I'm here to keep you company.",
    "One last question in this section. Thank you for sticking with me!"
  ],
  "meowraCompletionDialogue": "That's our section wrapped up, nya. Thank you for your time and effort!"
}
```
