---
layout: default
---

## [GridPipe.cs](https://github.com/corovcam/pipe-world/blob/main/Assets/Scripts/DragAndDrop/GridPipe.cs)

The `GridPipe` script handles drag and drop events for a pipe object in a Unity game project. It is designed to work in conjunction with a game board and a set of pipes that can be moved and rotated to create a complete pipeline.

The script utilizes several variables to keep track of the state of the pipe during drag and drop events. The `draggingItem` boolean flag is used to indicate whether the pipe is currently being dragged, and the `touchOffset` vector stores the initial offset of the mouse (or touch) position from the center of the pipe when the drag starts. The `startingPosition` vector and `myParent` transform store the starting position and parent transform of the current pipe, respectively. The `touchingTiles` list stores the tiles that the pipe is currently touching.

The `Start()` method initializes the starting position, parent transform, and main camera of the game. The `Update()` method updates the position of the pipe as the mouse or touch moves, but only if the pipe is currently being dragged. The `OnMouseDown()` method sets the `draggingItem` flag to true and obtains the touch offset when the pipe is clicked. The `PickUp()` method is called to enlarge the sprite and change the sorting order of the pipe when it is dragged.

The `OnMouseUp()` method ends the dragging of the pipe and calls the `Drop()` method to reposition the pipe on the game board. If the pipe is nottouching any tile, the pipe is returned to its starting position and parent GameObject. If the pipe is touching a tile, the script finds the tile closest to the pipe and checks if it is already filled. If the tile is empty, the script swaps the positions and parent GameObjects of the pipes on the current and target tiles, updates the game board data with the new location of the moved pipe, and updates the nearby pipes of the current and target pipes that were affected by the swap.

The `GetMousePos()` method is a helper function that converts the mouse or touch position to world position. The `PickUp()` method enlarges the sprite and changes the sorting order of the pipe when dragged. The `Drop()` method drops the pipe and repositions it on the game board. The `UpdatePipeTransformsAndHandlers()` method updates the positions and parent GameObjects of the pipes on the current and target tiles and updates game board data. The `OnTriggerEnter2D()` and `OnTriggerExit2D()` methods are called when the Collider2D component on the GameObject first makes contact with or no longer makes contact with another Collider2D.

The `SlotIntoPlace()` method is a coroutine used to move the pipe to a new position over a short time interval. It lerps the pipe position from starting position to ending position over the duration of the coroutine.

Overall, the `GridPipe` script is an essential component of the pipe-world project, which allows players todrag and drop pipes to create a complete pipeline. It handles the logic for moving and swapping pipes on the game board and ensures that the pipeline is complete and functional. The script is used in conjunction with other scripts and game objects to create a comprehensive pipeline game.

### Questions & Answers

1. **How is drag and drop implemented for pipes?**

    The OnMouseDown, OnMouseUp, PickUp, and Drop methods handle detecting mouse clicks, moving the pipe, dropping the pipe, and handling placement logic.

2. **What happens if a pipe is dropped on an occupied tile?**

    The pipe is returned to its starting position since only one pipe is allowed per tile. 

3. **What other scripts does this GridPipe script interact with?**

    It interacts with PipeHandler and PauseControl.