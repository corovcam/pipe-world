---
layout: default
---

## [SceneHandler.cs](https://github.com/corovcam/pipe-world/blob/main/Assets/Scripts/StaticClasses/SceneHandler.cs)

The `SceneHandler` class is a static class responsible for handling the loading of different scenes in the game and configuring default values for different game modes. It provides four public methods: `LoadMainMenuScene()`, `LoadLevelSelectScene(bool isFreeWorldMode)`, `LoadArcadeGameScene()`, and `LoadLevel(int levelNumber)`.

The `LoadMainMenuScene()` method loads the main menu scene in the game, while `LoadLevelSelectScene(bool isFreeWorldMode)` loads the level select scene and sets the `IsFreeWorldMode` flag in `LevelData`, which determines whether a user is in free world mode or not. 

The `LoadArcadeGameScene()` method loads the game scene for arcade mode and sets various default values in `LevelData` such as `BoardSize` and `TimeLimit`. It also sets the `IsArcadeMode` flag to true and initializes `Starts`, `Ends`, `defaultStart`, `defaultEnd`, and `GamePieces` to null. 

The `LoadLevel(int levelNumber)` method loads the game scene and sets various default values in `LevelData` for a specific level number. It also reads level data from a text file based on whether the game is in free world mode or not. It sets `IsArcadeMode` to false, initializes `Starts`, `Ends`, `defaultStart`, `defaultEnd`, and `GamePieces` to null.

The `SceneHandler` class can be used in the larger project to handle scene transitions and initialize default values for different game modes and levels. For example, when a user selects a level from the level select menu, the `LoadLevel(int levelNumber)` method could be called with the corresponding level number to load the game scene and initialize default values for that level. Alternatively, when a user selects the arcade mode, the `LoadArcadeGameScene()` method could be called to initialize default values for arcade mode and load the game scene.

Here is an example of how `LoadMainMenuScene()` method could be called in a Unity button's `OnClick()` event:

```csharp
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Button startButton;

    void Start()
    {
        startButton.onClick.AddListener(LoadMainMenu);
    }

    void LoadMainMenu()
    {
        SceneHandler.LoadMainMenuScene();
    }
}
```

In this example, when the user clicks the start button in the main menu, the `LoadMainMenu()` method is called, which in turn calls the `LoadMainMenuScene()` method in the `SceneHandler` class to load the main menu scene.

### Questions & Answers

1. What is `LevelData` and how is it used in this code?

   `LevelData` is likely a class or struct used to store data related to the game's levels, such as the current level number, whether the game is in free world mode or arcade mode, and various level-specific data such as starting positions and game pieces. It is used in this code to set default values for different game modes and levels.

2. What happens if `LoadLevel(int levelNumber)` is called when `IsFreeWorldMode` is true?

   If `LoadLevel(int levelNumber)` is called when `IsFreeWorldMode` is true, the code loads level data from a text file in the `FreeWorldLevels` folder using the `Resources.Load()` method and sets `LevelData.lvlData` to an array of strings containing the level data. If `IsFreeWorldMode` is false, the code loads level data from a text file in the `LevelSelectLevels` folder. The rest of the method initializes various default values for the level based on the loaded level data.

3. What is the purpose of `SceneManager.LoadScene()` in this code?

   `SceneManager.LoadScene()` is used to load different scenes in the game, such as the main menu, level select, and game scenes. It takes a string argument representing the name of the scene to load and a `LoadSceneMode` argument specifying whether to load the scene additively or as the onlyscene in the game. In this code, `SceneManager.LoadScene()` is used to load the main menu, level select, and game scenes depending on the method called.
