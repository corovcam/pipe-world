---
layout: default
---

## [Player.cs](https://github.com/corovcam/pipe-world/blob/main/Assets/Scripts/Player.cs)

MonoBehaviour script, `Player`, is responsible for handling player input and interacting with the `GameManager` and `LevelHandler` classes in the game. The purpose of this script is to enable player controls for moving and rotating pipe tiles, as well as starting the flow within the pipe system.

The following controls are supported:

- **Pointer movement**: The player can move the active pipe tile using the arrow keys or the WASD keys.
- **Rotate Active Pipe**: The player can rotate the active pipe tile by pressing the 'R' key.
- **Start Flow**: The player can start the flow in the pipe system by pressing the 'F' key.

The `Player` class has two private fields:

- `gm` (GameManager): A reference to the GameManager component attached to the same GameObject as the Player script.
- `levelHandler` (LevelHandler): A reference to the LevelHandler instance in the scene.

In the `Start()` method, the references to the `GameManager` and `LevelHandler` are initialized.

The `Update()` method checks for player input and calls the corresponding methods of the `LevelHandler` and `GameManager` classes:

```csharp
levelHandler.MoveActiveTileUp();
levelHandler.MoveActiveTileDown();
levelHandler.MoveActiveTileRight();
levelHandler.MoveActiveTileLeft();
levelHandler.RotateActiveTile();
gm.StartFlow();
```

These method calls are wrapped in a conditional statement, ensuring that player input is only processed if the game is not paused and has not ended (checked using `PauseControl.GameIsPaused` and `GUIHandler.IsEndGame`).
