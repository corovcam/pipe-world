---
layout: default
---

## [LevelSelectHandler.cs](https://github.com/corovcam/pipe-world/blob/main/Assets/Scripts/LevelSelectHandler.cs)

The `LevelSelectHandler` class is responsible for handling the level selection menu in the Unity project. It creates and manages the level selection UI, including pagination and level buttons.

The class contains public properties to set the number of levels for both the level select and free world modes. It has references to button prefabs, sprites for button backgrounds, and level numbering.

The `Start()` method initializes the level selection menu. It generates the title text, configures level buttons, and sets up the main menu, previous, and next page buttons.

```csharp
void Start()
{
    // ...
    GenerateTitleText();
    // ...
    GenerateLevelsPage(pageID);
    // ...
    GenerateLevelButton(levelID, pageID);
    // ...
    ConfigureMainMenuButton();
    ConfigurePrevNextButtons();
    // ...
}
```

`GenerateTitleText()` creates the title text object for the level select menu. The method `GenerateLevelsPage(int pageID)` generates a new page for levels, while `GenerateLevelButton(int levelNumber, int pageID)` creates a level button with the specified level number and page ID.

The `ConfigurePrevNextButtons()` method sets up the previous and next page buttons for pagination, with click listeners for navigating between pages.

```csharp
previousBtn.onClick.AddListener(() => { 
    PreviousPage();
    HandleFirstLastPage();
});
nextBtn.onClick.AddListener(() => { 
    NextPage();
    HandleFirstLastPage();
});
```

The `PreviousPage()` and `NextPage()` methods handle navigation between pages, while `HandleFirstLastPage()` updates the interactability of the previous and next buttons based on whether the user is on the first or last page.

Finally, `ConfigureMainMenuButton()` creates and configures the main menu button, with a click listener to load the main menu scene.


### Questions & Answers

1. **How does the script handle the addition or removal of levels to the `Resources/LevelSelectLevels` and `Resources/FreeWorldLevels` folders?**

    The script requires manual updating of the `levelSelectlvlCount` and `freeWorldLvlCount` variables in the Unity Inspector when levels are added or removed from the respective folders.

2. **How is the pagination of level buttons implemented in this script?**

    Level buttons are organized into pages containing a maximum of 8 levels. The `PreviousPage` and `NextPage` methods handle pagination along with the `HandleFirstLastPage` method, which manages the interactability of the Previous and Next buttons.

3. **How are level numbers displayed on the level buttons?**

    Level numbers are displayed using a combination of `Sprite` objects for each digit in the level number. The `GenerateLevelButton` method iterates through the digits and assigns the appropriate sprites to create the multi-digit level number display.