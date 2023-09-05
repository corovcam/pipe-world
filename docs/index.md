---
layout: home
---

# Documentation

## Table of Contents

- [Documentation](#documentation)
  - [Table of Contents](#table-of-contents)
  - [Overview](#overview)
    - [Game Mechanics](#game-mechanics)
    - [Board Setup](#board-setup)
    - [Pipes](#pipes)
    - [Flowing Mechanic](#flowing-mechanic)
      - [Fluid Mechanics](#fluid-mechanics)
    - [Score Calculation](#score-calculation)
    - [Level Progression](#level-progression)
    - [Game Flow](#game-flow)
  - [Game Modes](#game-modes)
    - [Level Select Mode](#level-select-mode)
    - [Arcade Mode](#arcade-mode)
    - [Free World Mode](#free-world-mode)
  - [Unity Project Structure](#unity-project-structure)
    - [Folders](#folders)
    - [Assets](#assets)
  - [Technical details](#technical-details)
    - [Script (Class) Diagram](#script-class-diagram)
    - [Score Calculation Formula](#score-calculation-formula)
    - [Random Level Generation (Arcade Mode)](#random-level-generation-arcade-mode)
  - [Scenes](#scenes)
    - [MainMenu](#scene-mainmenu)
    - [LevelSelect](#scene-levelselect)
    - [Tutorial](#scene-tutorial)
    - [Game](#scene-game)
  - [Build, install and run](#build-install-and-run)
  - [Credits and 3rd Party Assets](#credits-and-3rd-party-assets)


## Overview
Pipe World is a puzzle game developed in Unity 2021.3.5f1 using C# scripting language. The game is designed for the WebGl platform and is inspired by popular puzzle games such as Pipe Mania and Water Pipes. The player's goal is to transport liquid from one end of the game board to the other using interconnected pipes.

### Game Mechanics
The game consists of a static matrix-like game board that contains randomly rotated pipes of different types, including straight, right-bent/left-bent, three-way, and cross. The player can rotate/move a pipe by clicking/dragging it. The player must create a connection with nearby pipes to transport the liquid (water, lava) from one edge of the board (typically located at the top left corner of the grid) to the other edge (typically at the bottom right corner). The player has a set time limit during which they can rotate/drag pipes. The earlier the player finishes, the more total score they receive. After every other level, the game board gets bigger, or the time limit is changed to reflect the increasing difficulty of the level.

### Board Setup
Upon starting the game, the player is presented with a game board that consists of a matrix-like grid with certain pipes rotated randomly. The player must connect the pipes in order to create a continuous path from the start pipe to the end pipe.

### Pipes
There are four types of pipes: straight, right-bent/left-bent, three-way, and cross pipes. These pipes are randomly placed on the game board and the player must rotate (or drag and rotate) them by clicking on (or dragging) them until they are connected to the adjacent pipes.

### Flowing Mechanic
The objective of the game is to create a continuous path from the start pipe to the end pipe, allowing water/lava to flow through the connected pipes. The liquid flows through the pipes automatically once the Flow button is pressed. The player has a limited amount of time to connect the pipes before the water/lava begins to flow. If the player fails to connect the pipes before the time limit expires, the game ends.

#### Fluid Mechanics
Water and lava are the two types of liquids that can flow through the pipes. Water flows through the pipes at a constant rate (1 second), while lava flows through the pipes at a slower rate (2 * speed of water). 
Assuming there's an **infinite pressure** source, each liquid is pushed trough their respective pipes in any direction horizontally, vertically or both (this also assumes **infinite amount** of liquid coming from the *Starts*). When any *liquid* tries to flow/exit out of a pipe in a direction not connected to another pipe (which include *board* boundaries), the pipe is sealed off (in that direction) and *liquid* doesn't get out of it (this is not *visually* represented in the game). 
The game also assumes that *Water* and *Lava* liquids can't mix, so if the liquids meet in the same pipe during the flow, the game ends.

### Score Calculation
The game score is calculated based on the time it takes the player to connect the pipes and complete the level. The faster the player completes (builds) the level, the higher the score. Check out the [Technical Details](#score-calculation-formula) section for more details.

### Level Progression
After completing a level, the player is presented with the option to go back and choose the next levels. As the player progresses through the levels, the game board becomes larger and more complex, and the time limit becomes shorter, making it more challenging to complete the levels.

### Game Flow

1. **Main Menu**

    When the game is launched, the player is presented with the Main Menu. The Main Menu contains two buttons: *Level Select*, *Arcade* and *Free World*. Clicking on the respective buttons takes the player to the *Level Select Menu* or directly to the *Game Scene* in *Arcade Mode*.

2. **Level Select Menu**

    The Level Select Menu contains a paginated list of available levels that the player can choose to play. Each level displays the level number. Clicking on a level takes the player to the Game Scene with the selected level loaded.

3. **Tutorial Scene**

    The Tutorial Scene is a simple scene that contains a Video Player and a Skip Button.

4. **Game Scene**

    The Game Scene is where the player can play the game. The game board is displayed in the center of the screen, and the GUI components are displayed on the sides of the screen. The player can rotate the pipes by clicking on them, and start the liquid *Flow* by clicking the *Flow button* or pressing *F* key. The player can pause the game by clicking on the *Pause button* and after *Flow* press *Skip button* to accelerate flow.

5. **Pause Menu**

    When the game is paused, the Pause Menu is displayed. The Pause Menu contains buttons: *Help*, *Restart*, *Quit* and *Resume*. Clicking on *Resume* returns the player to the Game Scene. Clicking on *Quit* takes the player back to the Main Menu. *Restart* restarts the current level. *Help* displays the Help Menu (Keyboard Shortcuts).

6. **End Game Menu**

    When the player completes a level or runs out of time, the End Game Menu is displayed. The End Game Menu displays the player's final score. The End Game Menu contains two buttons: *Restart* and *Quit*. Clicking on *Restart* restarts the level. Clicking on *Quit* takes the player back to the *Main Menu*.


## Game Modes
There are 3 game modes in Pipe World: **Level Select Mode**, **Arcade Mode**, and **Free World Mode**.

### Level Select Mode

[![Level Select Mode Flow](images/level-select-flow.png)](images/level-select-flow.png)

The Level Select Mode is the main game mode of Pipe World. The player can select a level from the Level Select Menu and play it. The player can also skip a level if they are unable to complete it or replay a level to improve their score. There is only one simple mechanic in this mode: the player can rotate the pipes *clock-wise* by clicking on them.

### Arcade Mode

[![Arcade Mode Flow](images/arcade-mode-flow.png)](images/arcade-mode-flow.png)

In the ***Arcade*** game mode, the player, like in the *Level Select* mode, has a generated static game grid. The difference is the random generation of pipe types (variants) and the random placement of the *Start* and *End*. The time limit is fixed at 30 seconds (modifiable in the code).

The level generation algorithm uses simple backtracking to generate a solvable maze. The algorithm is described in detail in the [Technical Details](#random-level-generation-arcade-mode) section.


### Free World Mode

[![Free World Recording](images/free-world-recording.gif)](images/free-world-recording.gif)

The ***Free World*** mode is a variant of the ***Level Select*** mode, where the player is given several pipe options (forming the correct path from source to destination) + their green and gray variants. The player has to move and rotate these pipes (using drag-and-drop) on the game grid to create a connection with some adjacent pipes. The player can move already placed pipes to other empty locations. If the player drops the Pipe onto an occupied location (or outside Grid), the Pipe will return to its original position.

There are 2 types of liquids (with their given source and destination pipes) on the game grid: water and lava. Green pipes can only transport water, while gray pipes can only transport lava. The pipe sources are marked with graphically visible filled liquids of respective types. The goal is to transport the given types of fluids to the correct destination pipes within the time limit. In the first few levels, there is only water on the game grid, and afterwards, the lava is added. 

By clicking on *Flow*, or when the time limit expires, the flow starts from all *sources* and the countdown pauses (as in previous variants). Lava flow animation updates at half the speed of water (water passes from one pipe to another in 1 second). The 2 liquid types can't mix, so if the liquids meet during the flow, the game ends. The game is won when all the liquids reach their respective *destinations*.

## Unity Project Structure

### Folders
- **Assets** - scripts, pictures, sprites, audio, animations and user-made files
- **Packages** - contains information about required packages to install and build the Unity project
- **ProjectSettings** - contains Project settings and custom Unity Editor preferences

### Assets
- **Animations** (Controllers have just a default implementation)
    - PointerAnimation - red-invisible Pointer animation that points at the current active tile
    - SkipAnimation - Skip button animation to make it flicker
    - Start Flow Animation - Flow button animation to make it flicker
- **Prefabs** (Instantiated and configured in scripts)
    - BackTile Prefab - Background tile used in Grid for the individual Pipes
    - Pointer Prefab - Instantiated on the grid as pointing/marking tool
    - **AudioSources** - Audio Source Prefabs with Audio Clips used for button clicking, hovering, pipe rotation and End Game win sound respectively
    - **Buttons** - Level Select button Prefabs used in pagination for Next Page and Previous Page buttons
    - **Pipes** - Default Pipe Prefabs with default green/red sprites with pre-set *tileType* and *IODirs* for respective Pipe types (except for EMPTY Pipe)
- **Puzzle stage & settings GUI Pack**[^1] - A Unity Asset Store GUI Pack used for labels, buttons, Pause and End Game menus
- **Resources** (Uses custom Unity API for access)
    - *Level Select Levels* - Custom level data accessible from Level Select menu
        - *LevelX* - new level can be added easily following this pattern:
            - First row: Grid row/column length
            - Second row: *TimeLimit* for each Difficulty
                - 3 integers separated by semicolon ';'
                - First integer: Easy Difficulty Time Limit
                - Second integer: Medium Difficulty Time Limit
                - Third integer: Hard Difficulty Time Limit
            - GameBoard:
                - Each cell in the matrix is divided by semicolon ';'
                - The first symbol can be either: S = StartPipe, E = EndPipe, 0 = Any other Pipe
                - Symbols are separated by ':'
                - The second symbol/number can be either: 0 = No pipe, 1 = Straight pipe, 2 = Round, 3 = ThreeWay, 4 = Cross
                - Each row has to be finished with another semicolon ';'
    - *Free World Levels* - Custom level data accessible from Free World (Level Select) menu
        - *LevelX* - new level can be added easily following this pattern:
            - First row: Grid row/column length
            - Second row: *TimeLimit* for each Difficulty
                - 3 integers separated by semicolon ';'
                - First integer: Easy Difficulty Time Limit
                - Second integer: Medium Difficulty Time Limit
                - Third integer: Hard Difficulty Time Limit
            - GameBoard:
                - Each cell in the matrix is divided by semicolon ';'
                - The first symbol can be either: S = StartPipe, E = EndPipe, 0 = No Pipe
                - Symbols are separated by ':'
                - The second symbol can be either: W = Water, L = Lava, 0 = No Pipe
                - The third symbol/number can be either: 1 = Straight pipe, 2 = Round, 3 = ThreeWay, 4 = Cross, 0 = No pipe (Empty)
                - Each row has to be finished with another semicolon ';'
    - **Sounds** - Audio Clips[^4] used in *AudioSources* Prefabs
    - **UI** - Additional Sprite and Font data used in-game
- **Scenes**
    - *Game* - Main Game scene where all the mechanics and physics occur, either chosen *LevelX* from Level Select menu or Arcade mode
    - *LevelSelect* - All levels included in Resources are automatically added to the Level Select menu in a paginated view
    - *MainMenu* - First scene in Build with 3 buttons
- **Scripts** (Only MonoBehaviour scripts attached to GameObjects) - more documentation can be found in the code *comments* and [*Scripts* page](scripts)
    - *Drag And Drop*
        - *GridPipe.cs* - Attached to Pipe Prefab and used to drag and drop Pipes in Free World mode
    - *Static Classes*
        - *Extensions.cs* - Used only to extend RectTransform methods by adding an amount of screen units to left/right/top/bottom offset
        - *LevelData.cs* - Static class used to store all information about the current Game and Level
        - *PuzzleGenerator.cs* - Static class used to generate a maze puzzle consisting of cells that are surrounded by walls in a 2D Array made by CellWalls struct
        - *SceneHandler.cs* - Used to handle the load of new Scenes in the game and to configure the static default values
    - *GameManager.cs* - Manages core liquid flowing mechanic after Flow start is triggered
    - *GUIHandler.cs* - Handles *Game* scene GUI Components, Pause Menu, End Game Menu, Total Score calculaction and Timer and its mechanism
    - *LevelHandler.cs* - Handles Level building, Tile/Pipe management, Restart, Tiles shuffle, rotation and audio
    - *LevelSelectHandler.cs* - Handles Level Select menu GUI components, their construction and management including pagination buttons and page counting
    - *TutorialPlayer.cs* - Handles Tutorial video player and skip button
    - *MenuHandler.cs* - Handles Main Menu GUI components construction and management
    - *PauseControl.cs* - Handles Pause mechanic in the game as well as Pause menu and buttons
    - *PipeHandler.cs* - Handles individual information about a Pipe (location, rotation) and its surrounding Pipes
    - *Player.cs* - Player Input interaction with the game. Game Controls.
- **Settings** - Universal Render Pipeline default settings (not used extensively in the project)
- **Sprites**
    - Square.png - Used for Marker animation
    - **BackTiles** - BackTiles[^2] used in Grid, that are chosen at random when the game starts
    - **PipeTiles**
        - Green Pipes and Grey Pipes, that are chosen at random when the game starts
        - Blue-Green Pipes and Red-Grey Pipes that represent the *StartPipes* and *EndPipes*
        - Their filled -lava and -water versions
        - **BlenderFiles** - Blender files used to create the PipeTiles
        - Red Pipes[^3] are the original pipe sprites
- **StreamingAssets** - Video files used in Tutorial scene
- **Objects** - Contains VideoTexture object used in Tutorial scene to set up the video player (resolution)
- **TextMesh Pro** - A Unity plugin used in all TextMeshPro components
- **UI Toolkit** - Not used in the project, just the default UI settings


## Technical details
All scripts attached to GameObjects inherit from the base MonoBehaviour class in Unity. MonoBehaviour is a class that allows the user to attach scripts to GameObjects in the Unity Editor. The MonoBehaviour class contains several methods that are called at specific moments in the game loop. These methods are called in the following order:
1. Awake
2. Start
3. Update

### Script (Class) Diagram

[![Class Diagram](images/class-diagram.png)](images/class-diagram.png)

Check out individual **Script Docs** for more information:

1. [GameManager.cs](scripts-docs/GameManager)
2. [GUIHandler.cs](scripts-docs/GUIHandler)
3. [LevelHandler.cs](scripts-docs/LevelHandler)
4. [PauseControl.cs](scripts-docs/PauseControl)
5. [PipeHandler.cs](scripts-docs/PipeHandler)
6. [Player.cs](scripts-docs/Player)
7. [PauseControl.cs](scripts-docs/PauseControl)
8. [MenuHandler.cs](scripts-docs/MenuHandler)
9. [LevelSelectHandler.cs](scripts-docs/LevelSelectHandler)
10. [TutorialPlayer.cs](scripts-docs/TutorialPlayer)
11. [DragAndDrop/GridPipe.cs](scripts-docs/DragAndDrop/GridPipe)
12. [StaticClasses/Extensions.cs](scripts-docs/StaticClasses/Extensions)
13. [StaticClasses/LevelData.cs](scripts-docs/StaticClasses/LevelData)
14. [StaticClasses/PuzzleGenerator.cs](scripts-docs/StaticClasses/PuzzleGenerator)
15. [StaticClasses/SceneHandler.cs](scripts-docs/StaticClasses/SceneHandler)


### Score Calculation Formula

The score is calculated using the following code snippet:

```csharp
// Minimum Score: 0
// Maximum Score (Arcade): 10000
double weight = LevelData.IsArcadeMode ? 1.0 : LevelSelectHandler.MaxTimeLimit / (double)LevelData.TimeLimit;
double notNormalizedScore = currentTime / (double)LevelData.TimeLimit / weight;
int score = Mathf.RoundToInt((float)(notNormalizedScore * MAXIMUM_SCORE));
return score.ToString();
```

The **MAXIMUM_SCORE** is a constant value of 10000 (can be modified in *GUIHandler.cs*). The **weight** is a value that is used to normalize the score based on the time limit. The **notNormalizedScore** is the score that is not normalized. The **score** is the final score. The final *score* can actually be higher than *MAXIMUM_SCORE* in non-Arcade modes, but it is normalized to *MAXIMUM_SCORE*, so the score is always relative to the given *Game Mode* levels.


### Random Level Generation (Arcade Mode)

1. Initialize the maze
2. Start at the first cell (0, 0)
3. Visit cells using depth-first search (DFS) and backtracking

#### Pseudocode

```python
def GenerateMaze(width, height):
    maze = InitializeMaze(width, height)
    maze = Backtrack(maze, width, height)
    return maze

def InitializeMaze(width, height):
    Create a 2D array 'maze' with dimensions (width, height)
    Set all cells in 'maze' to have all walls (RIGHT, LEFT, UP, DOWN)
    return maze

def Backtrack(maze, width, height):
    position = (0, 0)
    Mark 'position' as visited in 'maze'
    While there are unvisited cells:
        adjacentCells = GetUnvisitedAdjacentCells(maze, position, width, height)
        if adjacentCells is not empty:
            Randomly choose a cell from 'adjacentCells' = chosen cell
            Remove common wall between 'position' and chosen cell in 'maze'
            Remove opposite wall to the common wall in the chosen cell in 'maze'
            Mark the chosen cell as visited in 'maze'
    return maze
```

The `GenerateMaze` function initializes the maze, sets the starting cells, and then calls the `Backtrack` function to visit cells using DFS and backtracking. The maze is updated in-place, and the final generated maze is returned.

The time complexity of this maze generation algorithm is O(width * height), because it visits each cell exactly once during the depth-first search (DFS) and backtracking process. In the worst-case scenario, every cell in the maze needs to be visited to create the maze (which is also the only case).


## Scenes

### **Scene: MainMenu**

[![Scene MainMenu](images/scene-mainmenu.png)](images/scene-mainmenu.png)

#### Summary
Scene contains a Title, 3 Buttons representing *Level Select*, *Arcade* and *Free World* modes and a *Difficulty* Dropdown input. Structurally, the Scene is very simple and similar to [LevelSelect](#scene-levelselect) Scene. The *MenuHandler* script attached to the Main Camera is used to generate and handle the whole Scene (check out the [Script Docs](scripts-docs/MenuHandler) for more information).

### **Scene: LevelSelect**

[![Scene LevelSelect](images/scene-levelselect.png)](images/scene-levelselect.png)

#### Summary
Scene contains a Title and a Grid of Levels in a paginated view. Each Level is a button that can be clicked to start the game. The Grid is automatically generated from the *Resources* folder. The Grid is paginated and can be navigated using the *Previous Page* and *Next Page* buttons. The number of Levels (for respective Game Modes) in the Grid is manually configured in the *LevelSelectHandler* script. The number of Levels in the Grid is also used to calculate the number of pages in the Grid. The Grid is generated using the *LevelSelectHandler.cs* script attached to the Main Camera. The *LevelSelectHandler.cs* script is also used to manage the pagination buttons and the page counter.
Similarly to the [MainMenu](#scene-mainmenu) Scene, the *LevelSelectHandler* script attached to the Main Camera is used to generate and handle the whole Scene (check out the [Script Docs](scripts-docs/LevelSelectHandler) for more information).

#### Scene Objects & Details
- **Main Camera** - A *LevelSelectHandler.cs* is attached to the Camera.
    - Inspector view and *LevelSelectHandler.cs* settings:
        - *Previous Page Btn*: Reference to the *Previous Page Btn* Prefab in *</Assets/Prefabs/Buttons>*
        - *Next Page Btn*: Reference to the *Next Page Btn* Prefab in *</Assets/Prefabs/Buttons>*
        - *Level Btn Background* - Reference to the *candy bar* Sprite in *</Assets/Puzzle stage & settings GUI Pack/Image_green>*
        - *Numbers* - Reference to the respective number Sprite in *</Assets/Puzzle stage & settings GUI Pack/Image_green/text>*
        - *Levels Count*: When the number of Levels in Resources changes, this number should be changed to reflect the current number of Levels.
- **Audio Click Source** - Audio Source used in scripts when a button is clicked
- **Audio Enter Source** - Audio Source used in scripts when a mouse hovers over a button
- **Level Select Canvas** - The Canvas is rendered upon the Main Camera (referenced in Inspector)
    - *Canvas Scaler* - Used for Responsive design that scales the width and height (***Screen Match Mode***) equally (***Match*** = 0.5) using the 1920x1080 resolution as a reference point
    - **Page #** - Represents a collection of at most 8 *Level Numbers* all organized using the *Grid Layout Group*
        - *Grid Layout Group* - Childs are aligned along the middle center line using fixed *Cell Size* and *Spacing* values
        - **# (Buttons)** - Are all Interactable button parents
            - **Number#** - Contains *Horizontal Layout Group* component to organize multi-digit Numbers one by one; Also has custom padding
    - **Previous Page Btn(Clone)**, **Next Page Btn(Clone)** - An instantiated prefab that represents a cloned button that changes ***Interactable*** property when the *currentPage* is either the *Last Page* or *First Page*

### **Scene: Tutorial**

[![Scene Game](images/scene-tutorial.png)](images/scene-tutorial.png)

#### Summary
The Tutorial Scene is a simple scene that contains a Video Player and a Skip Button. The Video Player is used to play the tutorial video. The Skip Button is used to skip the tutorial video and go to the respective Game. The *TutorialPlayer.cs* script attached to the VidPlayer GameObject is used to play the tutorial video and handle the skip button click event.

### **Scene: Game**

[![Scene Game](images/scene-game.png)](images/scene-game.png)

#### Summary
Game scene is the main scene in the build. It contains the Grid, the Pointer, the Pause Button and the Pause Menu. The Grid is generated using the *LevelHandler.cs* script attached to the Grid GameObject. The Pointer is a GameObject that is used to point to the currently selected Pipe. The Pointer is moved using the *Player.cs* script attached to the Main Camera. The Pause Button is a GameObject that is used to pause the game. The Pause Button is managed using the *PauseControl.cs* script attached to the Grid GameObject. The Pause Menu is a GameObject that is used to show the Pause Menu. The Pause Menu is managed using the *PauseControl.cs* script attached to the Grid GameObject.

#### Scene Objects & Details
- **Grid** - A *LevelHandler.cs* script is attached to Grid GameObject.
    - Inspector view and *LevelHandler.cs* settings:
        - *Pipe Prefabs* - Referenced from the *</Assets/Prefabs/Pipes>* directory; The last one is always EMPTY
        - *Pipe Sprites*, *Filled Pipe Sprites*, *Red Pipe Sprites*, *Filled Red Pipe Sprites* - all respectively referenced using the spritesheets in *<Assets/Sprites/PipeTiles>*
        - *Tile Pointer* - Referenced ***Pointer*** Scene GameObject
        - *Back Tile Prefab* - Referenced from the *</Assets/Prefabs>* directory
        - *Back Tile Sprites* - Chosen at random at the beginning of the game
        - *Active Pipe*, *Active Pipe Handler* (private fields) - reference to the currently selected Active Pipe and its PipeHandler script instance
        - *Board Size*, *Level Num*, *Arcade Mode* (private fields) - for Debug purposes shown in the Inspector
        - *Pause Control* - Referenced from the ***Manager*** scene GameObject
    - **Pointer** - GameObject
    - **(X, Y)** - Tile GameObjects rendered as *BackTile* Sprites at [X, Y] coordinates in the Grid
        - **Pipe(Clone)** - Instantiated *Pipe Prefab* (with default values mentioned in the ***Prefabs/Pipes*** section) with a *PipeHandler.cs* script instance attached to it (and *GridPipe.cs* in *Free World Mode*)
- **GUI** - Scales with resolution, same as ***Level Select Canvas***
    - Contains ***Pause Button***, ***Skip Button***, ***Timer Panel*** and ***Start Flow Button*** GameObjects all anchored to their respective corners
- **Pause Menu** - Scales with resolution, same as ***Level Select Canvas***; Can be enabled in-game using *ESC* key or the ***Pause Button***
    - **Menu Background** - The white-green *box* panel Sprite
        - **Panel** - Used for *Vertical Layout Group* child layout positioning
            - **Sound Control** - *Horizontal Layout Group* for ***Sound*** Image and ***Toggle*** Interactable
                - **Toggle** - Default property value ***IsOn*** is true
                    - Toggle mechanism is handled inside *PauseControl.cs* script instance of the ***Manager*** GameObject
            - **Help Button** - Displays the ***Help Dialog*** (enables the GameObject), rendered over the ***Pause Menu***
            - **Restart Button**, **Quit Button**, **Resume Button** - all referenced in ***Manager*** GameObject and managed using *PauseControl.cs* and *GUIHandler.cs* scripts
    - **Help Dialog** - Contains ***Cancel Help Dialog Button*** to set the ***Help Dialog*** as Not Enabled
- **End Game Menu** - Scales with resolution, same as ***Level Select Canvas*** and ***Pause Menu***; Enabled when the ***Countdown*** runs to 0, or when the ***Flow Button*** is pressed and the flow runs to the *End Pipe* (Won game) or the Flow is stuck and can't reach the *End Pipe* (Lost game)
    - Similar hierarchy as ***Pause Menu***
    - **Menu Background** - Anchored to the center to scale with bigger/smaller screens in the middle
        - **Panel** - *Vertical Layout Group*
            - **Score Number** - Calculated and handled using the *GUIHandler.cs* script in the ***Manager** GameObject
- **Manager** - An auxiliary (invisible) GameObject holding and managing Script instances
    - **Game Manager** - *GameManager.cs*
    - **Player** - *Player.cs*
    - **Pause Control** - *PauseControl.cs*
    - **GUI Handler** - *GUIHandler.cs*
        - *Is Debug* (private field) - For debugging purposes, look inside code
        - *Default Time Limit* (private field) - For debugging purposes, mainly to increse/decrease Arcade Mode Time Limit
- **Audio Click Source**, **Audio Enter Source**, **Audio Rotate Source**, **Audio Winning Source** - Same as in ***LevelSelect*** scene, referenced with their respective *Audio Clips*
    - ***Audio Winning Source*** plays only when the player *Won* game, not when the player *Lost* game


## Build, install and run

Game is built using the Unity Editor, version 2021.3.5f1, so the assemblies and scripts are mostly compatible with the 2021.3 major versions. If run built using a version lower than this one, compatibility issues may arise considering that the game uses newer Universal Render Pipeline, Input system and Coroutines.  

When cloned, start the Editor using the Unity Hub (or manually) and wait till the Editor finishes downloading and importing packages.

The game is mainly configured for the WebGl (browser) platform (though it also works on Android and Windows). The intended resolution aspect ratio is 16:9 (developed using the 1920x1080 resolution as a reference endpoint) and played in the landscape mode. Though the game can be run on mobile device browsers, the settings need to be configured properly for the game to run smoothly.

The WebGl build can be found here: <https://corovcam.itch.io/pipe-world> 


## Credits and 3rd Party Assets

[^1]: <https://assetstore.unity.com/packages/2d/gui/puzzle-stage-settings-gui-pack-147389> "Puzzle stage & settings GUI Pack"
[^2]: <https://opengameart.org/content/puzzle-pack-2-795-assets> "Puzzle Pack 2, made by Kenney.nl"
[^3]: <https://opengameart.org/content/2d-pipe-parts> "2D Pipe parts, made by TwistedDonkey in Blender, further costumized by me"
[^4]: <https://opengameart.org/content/51-ui-sound-effects-buttons-switches-and-clicks> "51 UI sound effects (buttons, switches and clicks), made by Kenney.nl"
