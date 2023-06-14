---
layout: default
---

## [LevelData](https://github.com/corovcam/pipe-world/blob/main/Assets/Scripts/StaticClasses/LevelData.cs)

This file contains a static class called `LevelData` which is used to store information about the current game and level in the `pipe-world` project. It also contains several structs and enums which are used to represent the various objects in the game.

### Enums

The first enum defined is `Liquid`, which is used to represent the different types of liquids that can flow through the pipes in the game. Currently, only water and lava are defined as possible liquids.

The second enum defined is `PipeType`, which is used to represent the different types of pipes in the game. The possible types are straight, round, three-way, cross, and empty. The last type, `EMPTY`, is always the last type in the enum.

### Structs

The first struct defined is `Pipe`, which represents a pipe in the game. It has two properties: `Liquid`, which is the type of liquid that can flow through the pipe, and `Type`, which is the type of pipe.

The second struct defined is `Position`, which represents the x and y coordinates of a pipe on the game board. It has two integer properties: `X` and `Y`. This struct also overrides the basic equality operators to allow for comparison of positions.

### Static Class

The `LevelData` class itself is a static class used to store all information about the current game and level. It has severalproperties and methods that are used throughout the project.

The first set of properties are booleans used to represent the current mode of the game. `IsArcadeMode` and `IsFreeWorldMode` are both set to `false` by default. 

The `LevelNumber` property represents the level number of the current game, while `BoardSize` represents the size of the puzzle game board. `TimeLimit` is the time limit for the current level.

`Starts` and `Ends` are both dictionaries that store the starting and ending positions for a specific liquid type. `GamePieces` is a two-dimensional array of `PipeHandlers` representing the game pieces. 

The `lvlData` property is a temporary data structure used for level loading.

`defaultStart` and `defaultEnd` are the default starting and ending positions for the liquid type. 

The `GetRandomPuzzle` method generates a random puzzle using a random maze generator and a dictionary map to convert it to the pipes puzzle. It takes in the width and height of the puzzle and returns a 2D array of pipes representing the pipe puzzle.

The `GetRandomStartPos` and `GetRandomEndPos` methods choose a random starting and ending position for the liquid type, respectively.

The `ReadInputLevelData` method reads level data from a text file in a specific format and returns a 2D list of pipes for the puzzle.

The `FillWallsToPipesMap`FillWallsToPipesMap` is a private method defined in the `PipeGameManager` class in the `pipe-world/Assets/Scripts/Managers` directory of the `pipe-world` project. This method is used to create a dictionary that maps the x and y coordinates of the game board to the corresponding `PipeHandler` object.

The method takes in a 2D array of `PipeHandler` objects representing the game board. It then loops through each `PipeHandler` object and checks if it is null or not. If it is not null, the method adds the object's position as a key to the dictionary with the `PipeHandler` object as the corresponding value.

The resulting dictionary can be used to quickly look up the `PipeHandler` object corresponding to a given position on the game board. This can be useful for various game mechanics such as checking if a pipe can be rotated or if a liquid can flow through a certain path.

### Questions & Answers

1. **What is the purpose of the `Position` struct?**

    The `Position` struct is used to store X and Y coordinates of pipes on the board and overrides basic equality operators.

1. **What is the purpose of the `LevelData` static class?**

    The `LevelData` static class is used to store all information about the current game and level, including the mode of the game, level number, board size, time limit, starting and ending positions, and the game pieces.

1. **What is the purpose of the `GetRandomPuzzle` method in the `LevelData` static class?**

    The `GetRandomPuzzle` method generates a random pipe puzzle using a random maze generator and a dictionary map to convert it to the pipes puzzle, and returns a 2D array of `Pipe`s. It also sets the default starting and ending positions for the liquid type and adds them to the corresponding dictionaries.