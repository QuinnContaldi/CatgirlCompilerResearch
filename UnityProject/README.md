# Initial Unity application

Created with Unity **6000.6.1f1** (`7efac9f6c10e`) using the locally installed
**Universal 2D 7.0.0** template. Unused template tutorials, sample scenes,
animation/tilemap tools, visual scripting, collaboration and IDE integrations
were removed. URP, uGUI, the Input System, Sprite support and Unity's built-in
modules remain. Unity automatically adds its Linux SDK/toolchain packages.
There are no third-party Unity packages.

## Open and inspect

On the machine used for setup, run this from the repository root:

```bash
./Tools/open-unity.sh
```

The launcher supplies the local Linux compatibility libraries described below.
On another machine where the Editor starts normally, add this `UnityProject`
folder through Unity Hub. Use 6000.6.1f1 to avoid an unintended upgrade.
You can override the launcher's Editor path with the `UNITY_EDITOR` environment
variable.

1. Open `Assets/Scenes/Experiment.unity`.
2. Press Play and open the Game tab.
3. Confirm Welcome appears first and Back is disabled.
4. Click Next twice: Instructions, then Experiment placeholder. Next is now disabled.
5. Click Back twice to return to Welcome.
6. Stop and restart Play mode: Welcome should appear again.
7. Check the Game view at 1280×720 and a smaller window for readable text and buttons.

There is no Inspector wiring left to do. The scene is already the only enabled
entry in the build scene list. All visible text is prototype copy, not approved
consent, study instructions or experimental stimuli.

## How navigation works

The scene stores the UI layout and references:

```text
Main Camera
Canvas
  Background
  Header
  Pages
    WelcomePage
    InstructionsPage
    PlaceholderExperimentPage
  Navigation
    PreviousButton
    NextButton
PageManager
EventSystem
```

A **GameObject** is an object in the scene; components give it behavior.
The Canvas displays UI, while the EventSystem and Input System UI module deliver
input to buttons. The Canvas scales its 1280×720 reference layout to the window.

`Assets/Scripts/UI/PageManager.cs` is the only application runtime script:

- Its serialized `pages` array stores references to the three panels in order.
  `[SerializeField]` lets Unity save these private fields and expose them in the Inspector.
- `Awake()` runs when the component initializes in Play mode and calls `ShowPage(0)`.
- `ShowPage(int)` activates the chosen panel and deactivates the others. Invalid
  indices do nothing. It also updates Back/Next availability.
- The buttons' saved **On Click()** events call `PreviousPage()` and `NextPage()`.
- There is no wraparound at either end. `CurrentPageIndex` exposes the current
  zero-based index without allowing another component to change it directly.

Select the root **PageManager** object in the Hierarchy to inspect its references.
Select each button to see its On Click event. To edit a hidden page, temporarily
activate its panel while outside Play mode; `Awake()` restores one-page visibility
when Play begins. Keep page panels as distinct siblings and keep PageManager
outside them, so hiding a page does not disable navigation.

A later ExperimentManager can decide the study stage and call `ShowPage(int)`.
PageManager only handles visibility; participant state, trial content, surveys,
conditions and logging will be separate responsibilities. No such systems are
implemented yet, and no experimental-design changes have been made.

## Assets and Git

The requested directories exist under Assets: Scenes; Scripts/Experiment, UI,
Data and Utilities; Prefabs; UI; Images; Data; and Fonts. Empty folders are
reserved for later work. `.gitkeep` files retain empty folders in Git, and
their `.meta` files preserve Unity's folder identities.
`Assets/Settings` contains the template's render settings.

`Assets/Images/DrMeowra.jpg` is a byte-for-byte copy of the root image, imported
as a sprite. It is not displayed in the initial scene. The root image and
research documents remain untouched.

Commit the root `.gitignore`, launcher and documentation, plus this project's
`Assets/` (including **all `.meta` files**), `Packages/` (including the lockfile),
and `ProjectSettings/`. Include the repository's `AGENTS.md` if desired.
Do not commit Library, Temp, Obj, Logs, Build, Builds, UserSettings, IDE output
or `.unity-compat`. No nested Git repository was created.

## Repeatable navigation check

`Assets/Editor/NavigationSmokeCheck.cs` is an Editor-only integration check.
It opens the saved scene, enters Play mode, uses the actual button event handlers,
and checks startup, exclusive visibility, forward/back navigation, disabled
boundary buttons and direct page selection. It does not ship in a player build.

Close the Editor before running:

```bash
./Tools/open-unity.sh -batchmode -nographics \
  -executeMethod NavigationSmokeCheck.Run \
  -logFile /tmp/meowra-navigation-check.log
```

Do not add `-quit`: the check exits the Editor after Play mode completes. A
successful check exits with code 0 and logs `NAVIGATION_CHECK_OK`. In the Editor,
use **Tools → Navigation → Run Smoke Check** while outside Play mode. This check
does not replace visually inspecting the Game view or clicking through it yourself.

Initial validation passed in Unity 6000.6.1f1: scripts compiled and the Play mode
check completed with exit code 0. The check waits for Editor startup callbacks
before entering Play mode, allowing Unity to initialize its search index on a
fresh project. Visual layout and physical mouse/keyboard input still need the
manual check above.

## Local Linux compatibility workaround

This workstation runs Ubuntu 26.04.1 and has `libxml2.so.16`, but this Editor
requires `libxml2.so.2`. Official Ubuntu packages were extracted into the ignored
repository-local `.unity-compat/` directory, without installing or replacing
system libraries. The launcher adds that directory to `LD_LIBRARY_PATH` only
for Unity and its child processes. Opening this project directly through Hub
on this workstation will still need the dependency issue resolved separately.

Packages used, downloaded over HTTPS from Ubuntu's official archive:

- [libxml2 2.9.14+dfsg-1.3ubuntu3.8 (amd64)](https://archive.ubuntu.com/ubuntu/pool/main/libx/libxml2/libxml2_2.9.14+dfsg-1.3ubuntu3.8_amd64.deb)
- [libicu74 74.2-1ubuntu3.1 (amd64)](https://archive.ubuntu.com/ubuntu/pool/main/i/icu/libicu74_74.2-1ubuntu3.1_amd64.deb)

SHA-256 values recorded for the downloaded archives, in that order:

```text
bfd07c01d6e5ab3e327f3ca5819409b1914bbfb3f1a016d53e4dabd5f96143bb
c9a70989678660eed9a1e904c74fa043da8bec8e2036856fc16e31ced79b04f8
```

If this local folder is removed, download those packages, verify their hashes,
then extract each with `dpkg-deb -x <downloaded-package.deb> .unity-compat` from
the repository root. This is a workstation workaround, not a Unity asset or a
dependency to distribute with the study application.
