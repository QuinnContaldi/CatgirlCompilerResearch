# Current programming tasks

Scientific authority: [Research_Design.md](../Research_Design.md), sections 12–20.
`SetA/` and `SetB/` each contain four editable JSON task definitions, with embedded
answer keys (`correctAnswer`: 0=A, 1=B, 2=C, 3=D) and matched-pair IDs.

Use **Tools → Experiment → Import Study Content** to update Unity's eight runtime
assets. Do not edit those generated copies independently. Each task is usable in
either condition; condition presentation lives in `study-content/`.

These are draft stimuli requiring pilot review before the protocol's content/build
freeze. The obsolete six-task/compiler-feedback materials have been removed;
earlier versions remain in Git history.

## Remaining pilot considerations

- A1/B1 assign a value in an if condition, which is valid C. The correct option
  says comparison "appears to be intended"; check whether participants interpret
  that intent consistently.
- Both sets currently use keys A, B, C, D in order and identical paired answer
  options. Check for answer-position learning and recall across blocks.
- Ask why participants rejected distractors; check plausibility and unintended
  alternative correct answers across all eight tasks.
- Compare matched A/B difficulty and reading burden, and inspect scrolling at the
  intended participant resolution. Matching structure and passing software checks
  do not establish task equivalence.

Task snippets are fragments rather than standalone programs. Compiler checks of
wrapped fragments can produce unrelated warnings; those are not new task targets.
Changes to tasks, options or keys require explicit review, import and validation
before freezing content. See [Research_Design.md](../Research_Design.md), sections
55–56, for the collection checklist and freeze requirements.
