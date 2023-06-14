---
layout: default
---

## [MenuHandler.cs](https://github.com/corovcam/pipe-world/blob/main/Assets/Scripts/MenuHandler.cs)

The `MenuHandler` class is responsible for constructing and managing the main menu GUI components in the Unity project. This script is attached to a GameObject in the main menu scene.

When the game starts, the `Start()` method initializes the menu by creating necessary UI elements:

1. Generates two `AudioSource` objects for click and rollover sounds.
2. Creates a canvas GameObject using `GenerateCanvasGO()` to hold the menu components.
3. Constructs three menu buttons with `GenerateMenuBtn()`, each with a corresponding action:
   - "Level Select": Loads level select scene for regular mode.
   - "Arcade": Loads arcade game scene.
   - "Free World": Loads level select scene for free world mode.
4. Generates the title text using `GenerateTitleText()`.

The `GenerateCanvasGO()` method creates a canvas GameObject with proper scaling and rendering options.

The `GenerateMenuBtn()` method is responsible for creating a menu button with a specified text, position, and callback function. It sets up the button's appearance and sound effects using `ConfigureButtonSounds()`.

The `GenerateTitleText()` method creates the title text GameObject with specific font, size, and position.

`ConfigureButtonSounds()` sets up the sound effects for a button. It adds listeners for the onClick and onMouseEnter events, which trigger the specified callback functions.

Finally, `GenerateAudioSource()` creates an AudioSource GameObject with an AudioClip loaded from a given path and a custom name.

### Questions & Answers

1. **What does the `GenerateMenuBtn` method do?**

   The `GenerateMenuBtn` method creates a menu button with a specified label and position, and assigns a delegate function to be called when the button is clicked.

2. **How are the audio sources for button click and mouse enter events handled?**

   The audio sources are generated and stored in the `audioSources` list. The `ConfigureButtonSounds` method sets up the listeners for button click and mouse enter events, playing the corresponding audio sources when the events occur.

3. **How does the `GenerateCanvasGO` method set up the canvas for the main menu?**

   The `GenerateCanvasGO` method creates a new canvas game object, sets its properties like render mode and layer, and adds necessary components such as `Canvas`, `CanvasScaler`, and `GraphicRaycaster` to properly display and scale the UI elements on the screen.