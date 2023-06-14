---
layout: default
---

## [PuzzleGenerator.cs](https://github.com/corovcam/pipe-world/blob/main/Assets/Scripts/StaticClasses/PuzzleGenerator.cs)

The `PuzzleGenerator` static class in this code is responsible for generating a maze-like structure using a backtracking algorithm with depth-first search (DFS). The maze is represented as a 2D grid of `CellWalls` elements, where each cell has walls on the left, right, up, and down. The main method for generating the maze is `GenerateMaze(int width, int height)`.

The `CellWalls` enum is used to represent the presence of walls using bitwise flags, while the `AdjacentCell` struct is used to store information about neighboring cells with a common wall.

The `Backtrack` method is a private helper method that implements the backtracking algorithm. It starts from the top-left corner of the grid and marks the initial cell as visited. It then uses a stack to store the current cell and moves to an adjacent unvisited cell by removing the common wall, marking the new cell as visited, and pushing it onto the stack. This process continues until all cells have been visited.

In order to find unvisited adjacent cells, the `GetUnvisitedAdjacentCells` method checks the neighboring cells in each direction (left, right, up, down) and adds them to a list if they haven't been visited yet.

An example of how to generate a maze with this class is:

```csharp
int width = 10;
int height = 10;
CellWalls[,] maze = PuzzleGenerator.GenerateMaze(width, height);
```

This code will generate a maze of size 10x10, where each cell's walls are represented by bitwise flags of the `CellWalls` enum.

### Questions & Answers

1. **What is the purpose of the `CellWalls` enum and why is the `[Flags]` attribute used?**

   The `CellWalls` enum is used to represent the possible wall configurations for each cell in the maze. The `[Flags]` attribute is used to allow bitwise operations on the enum values, facilitating the combination and manipulation of wall configurations.

2. **How does the `Backtrack` method work to generate the maze?**

   The `Backtrack` method implements a depth-first search (DFS) algorithm to visit all cells in a grid maze, removing walls between adjacent cells in the process. It uses a stack to maintain the path, ensuring that every cell is visited and the maze is generated with a valid solution.

3. **How do the `GetUnvisitedAdjacentCells` and `AdjacentCell` struct work together in the maze generation process?**

   The `GetUnvisitedAdjacentCells` method returns a list of unvisited adjacent cells from the current position. The `AdjacentCell` struct stores information about each adjacent cell, such as its location and the common wall between the two cells. This information is used in the maze generation process to break down walls and mark cells as visited.