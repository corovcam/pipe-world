---
layout: default
---

### [LevelHandler.cs](https://github.com/corovcam/pipe-world/blob/main/Assets/Scripts/LevelHandler.cs)

The `LevelHandler` script is responsible for managing various aspects of the level in the game. It handles level building, tile and pipe management, restart functionality, tile shuffling, rotation, and audio.

The script contains several public variables for storing prefabs, sprites, and game objects related to pipes and tiles. These variables are set in the Unity Inspector and provide flexibility for customization.

The `Start` method is called at the beginning of the game. It sets default values for the level if they weren't set in the main menu, initializes the tile grid, generates a new grid and level, sets the active tile, stores game pieces, sets start and end pipe sprites, and shuffles the pipes.

The `GenerateNewGrid` method creates a new grid of back tiles with a chosen sprite and sets their position and parent.

The `GenerateLevel` method generates the level based on random or input data provided. It creates pipes based on the level data, instantiates pipe prefabs, and positions them on top of the corresponding tiles.

The `SetActiveTile` method configures the current active tile and updates the tile pointer accordingly.

The `StoreGamePieces` method stores a 2D array of `PipeHandler` script instances after the grid and pipes are generated. It allows easy access to pipe scripts for manipulation.

The `Shuffle` coroutine is used to shuffle the game pieces after the grid is generated. It rotates each game piece randomly to simulate a rotating effect.

The `SetStartEndPipeSprites` method changes the start and end pipe sprites to their corresponding variants based on the liquid type and whether they are filled or not.

The `ResetLevel` method resets all pipe sprites to their default variants, sets the active tile to the start pipe, and shuffles the game pieces.

The `RotateActiveTile` method rotates the current active tile.

The `MoveActiveTileUp`, `MoveActiveTileDown`, `MoveActiveTileRight`, and `MoveActiveTileLeft` methods move the active tile to different positions on the game board within the boundaries.

Overall, this script provides the functionality to build levels, manage tiles and pipes, handle game logic such as rotation and movement, and control audio cues in the game.


1. **What is the purpose of the `LevelData` class and how is it used in this script?**

   The `LevelData` class is used to store and manage data related to the game level, such as board size, level number, pipe types, and their positions. It is accessed to set and retrieve level data throughout the script.

2. **How does the `GenerateNewGrid` function position the back tiles and what is the purpose of the `temp` variable?**

   The `GenerateNewGrid` function generates a grid of back tiles by instantiating the `backTilePrefab` and configuring each tile's sprite, position, name, and parent. The `temp` variable is used to store the instantiated back tile game object before assigning it to the corresponding position in the `tileObjects` 2D array.

3. **What is the purpose of the `SetActiveTile` function and how is it used in the script?**

   The `SetActiveTile` function sets the current active tile by assigning the provided tile game object to the `activePipe` variable. It also positions and activates the `tilePointer` object above the active tile. This function is called to update the active tile during gameplay and allows the player to interact with the currently selected tile.