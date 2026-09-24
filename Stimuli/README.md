# Original stimulus materials

- `SourceCode/`: six C examples and `run_feedback.sh`, the original diagnostic
  authoring helper. It requires Bash and GCC and can be called from any directory:

  ```bash
  ./Stimuli/SourceCode/run_feedback.sh all raw
  ```

- `ReferenceImages/`: original screenshots grouped into Coding, Raw, Neutral,
  and Meowra. Original spellings are preserved so source material is unchanged.

These are reference/authoring materials, not participant-session inputs.
The current code and feedback text live in Unity's six Scenario assets under
`UnityProject/Assets/Data/Scenarios/`. See [Study_Text.md](../Study_Text.md) for
transcription notes and unresolved content differences. Editing a C source or
reference image does not update those text assets automatically.

**Tools → Experiment → Archive → Import Original Study Pictures** copies these
images into the existing Unity archive and updates only the hidden image
references. It does not replace the participant-facing text.

Former locations: `coding/` is now `Stimuli/SourceCode/`; `Pictures/` is now
`Stimuli/ReferenceImages/`. The original files were moved without changing the
C snippets or screenshots.
