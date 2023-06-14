---
layout: default
---

## [PipeHandler.cs](https://github.com/corovcam/pipe-world/blob/main/Assets/Scripts/PipeHandler.cs)

This code defines a `PipeHandler` class that manages the behavior and properties of individual pipe pieces in the Unity game. The class is responsible for rotating pipe pieces, updating their connections with neighboring pipes, and handling user interaction.

The `PipeHandler` class utilizes a `Dir` enum to represent the four possible directions (UP, RIGHT, DOWN, LEFT) and a `bool[]` named `IODirs` to store the availability of input/output directions for a pipe. For example, if `IODirs` is `[true, false, true, false]`, the pipe has available connections for UP and DOWN.

In the `Start()` method, the script initializes `levelHandler` and updates the neighboring `PipeHandler` references. The `Update()` method animates the rotation of the pipe if it's rotated by the player.

The `UpdateNeighbouringPipeHandlers()` method updates the references to neighboring pipes based on the `Position` of the current pipe. The `OnMouseDown()` method is called when the pipe is clicked, rotating the pipe and setting it as the active tile. The `RotatePiece()` method rotates the pipe 90 degrees clockwise and updates its input/output directions and neighboring pipe connections.

The `RotateIODirections()` method updates the `IODirs` array by rotating the direction flags one step clockwise. The `ProcessNearbyPipeChanges()` method processes changes in the neighboring pipes after a rotation, updating the available sides for the current pipe and its neighbors. Lastly, the `UpdateAvailableSides()` method updates the `Free` flags (upFree, downFree, rightFree, leftFree) for each direction, indicating if the water can flow through that direction based on the connections with neighboring pipes.

1. **What does the `Pipe` class represent and how is the `pipeType` used?**

   It's a class that represents different types of pipe tiles in the game. The `pipeType` variable in `PipeHandler` is used to store an instance of the `Pipe` class and is used to determine the appearance and behavior of the pipe object.

2. **How does the `speed` variable affect the rotation animation of the pipe?**

   The `speed` variable determines how quickly the pipe rotates during the rotation animation. A higher value for `speed` will result in a faster rotation, while a lower value will result in a slower rotation.

3. **How are the `upFree`, `downFree`, `rightFree`, and `leftFree` variables used in the game?**

   These boolean variables indicate whether the corresponding neighboring tiles (up, down, right, and left) are free for the flow to continue. They are updated in the `UpdateAvailableSides` method by checking the availability of the neighboring pipes in the specified direction, and are used to determine the flow of water/lava through the pipes.