using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Flags]
public enum CellWalls
{
    // 0000 -> NO WALLS
    // 1111 -> LEFT,RIGHT,UP,DOWN
    NO_WALLS = 0, // 0000

    LEFT = 1, // 0001
    RIGHT = 2, // 0010
    UP = 4, // 0100
    DOWN = 8, // 1000

    VISITED = 128, // 1000 0000
}

/// <summary>
/// A struct used to identify a Adjacent/Neighbouring Cell that shares a CommonWall with
/// a given Cell
/// </summary>
public struct AdjacentCell
{
    public Position Location;
    public CellWalls CommonWall;
}

public static class PuzzleGenerator
{
    private static CellWalls GetOppositeWall(CellWalls wall)
    {
        switch (wall)
        {
            case CellWalls.RIGHT: 
                return CellWalls.LEFT;
            case CellWalls.LEFT: 
                return CellWalls.RIGHT;
            case CellWalls.UP: 
                return CellWalls.DOWN;
            case CellWalls.DOWN: 
                return CellWalls.UP;
            default: 
                return CellWalls.LEFT;
        }
    }

    /// <summary>
    /// A backtracking algorithm to visit all cells in a graph using DFS
    /// </summary>
    /// <param name="cells">2D List of Cells in a grid maze</param>
    /// <param name="width">Width of the maze</param>
    /// <param name="height">Height of the maze</param>
    private static CellWalls[,] Backtrack(CellWalls[,] cells, int width, int height)
    {
        var posStack = new Stack<Position>();
        var position = new Position { X = 0, Y = 0 };

        // Mark first position as visited
        cells[position.X, position.Y] |= CellWalls.VISITED;
        posStack.Push(position);

        // When there are unvisited Cells, continue
        while (posStack.Count > 0)
        {
            Position current = posStack.Pop();
            // Look for nearby unvisited Cells to continue
            var adjacents = GetUnvisitedAdjacentCells(current, cells, width, height);

            if (adjacents.Count > 0)
            {
                posStack.Push(current);

                // Choose a random adjacent Cell to continue (to simulate randomicity of a maze)
                int randIndex = UnityEngine.Random.Range(0, adjacents.Count);
                AdjacentCell randomAdjacent = adjacents[randIndex];

                Position randAdjPos = randomAdjacent.Location;
                // Remove the CommonWall (of the chosen random Adjacent) from the current cell
                cells[current.X, current.Y] &= ~randomAdjacent.CommonWall;
                // Remove the opposite wall from the random chosen = the wall removed from the current cell
                cells[randAdjPos.X, randAdjPos.Y] &= ~GetOppositeWall(randomAdjacent.CommonWall);
                // Mark the new cell as visited
                cells[randAdjPos.X, randAdjPos.Y] |= CellWalls.VISITED;

                posStack.Push(randAdjPos);
            }
        }

        return cells;
    }

    /// <summary>
    /// Get a list of Adjacent cells from the current position <paramref name="p"/>
    /// </summary>
    /// <returns>A list of AdjacentCell structs</returns>
    private static List<AdjacentCell> GetUnvisitedAdjacentCells
        (Position p, CellWalls[,] cells, int width, int height)
    {
        var adjacents = new List<AdjacentCell>();

        if (p.X > 0) // Look for Left adjacent
        {
            if (!cells[p.X - 1, p.Y].HasFlag(CellWalls.VISITED)) // If not visited
            {
                adjacents.Add(new AdjacentCell // Add to the list of Adjacents
                {
                    Location = new Position
                    {
                        X = p.X - 1,
                        Y = p.Y
                    },
                    CommonWall = CellWalls.LEFT
                });
            }
        }

        if (p.Y > 0) // Look for Lower adjacent
        {
            if (!cells[p.X, p.Y - 1].HasFlag(CellWalls.VISITED))
            {
                adjacents.Add(new AdjacentCell
                {
                    Location = new Position
                    {
                        X = p.X,
                        Y = p.Y - 1
                    },
                    CommonWall = CellWalls.DOWN
                });
            }
        }

        if (p.Y < height - 1) // Look for Upper adjacent
        {
            if (!cells[p.X, p.Y + 1].HasFlag(CellWalls.VISITED))
            {
                adjacents.Add(new AdjacentCell
                {
                    Location = new Position
                    {
                        X = p.X,
                        Y = p.Y + 1
                    },
                    CommonWall = CellWalls.UP
                });
            }
        }

        if (p.X < width - 1) // Look for Right adjacent
        {
            if (!cells[p.X + 1, p.Y].HasFlag(CellWalls.VISITED))
            {
                adjacents.Add(new AdjacentCell
                {
                    Location = new Position
                    {
                        X = p.X + 1,
                        Y = p.Y
                    },
                    CommonWall = CellWalls.RIGHT
                });
            }
        }

        return adjacents;
    }

    /// <summary>
    /// Public method used for Maze generation using <paramref name="width"/> and <paramref name="height"/>
    /// of the requested maze as input parameters
    /// </summary>
    public static CellWalls[,] GenerateMaze(int width, int height)
    {
        CellWalls[,] maze = new CellWalls[width, height];

        // All cells in the maze are initialized with all walls present
        CellWalls initialCell = CellWalls.RIGHT | CellWalls.LEFT | CellWalls.UP | CellWalls.DOWN;
        for (int i = 0; i < width; ++i)
        {
            for (int j = 0; j < height; ++j)
            {
                maze[i, j] = initialCell;  // 1111
            }
        }

        // The algorithm will remove some walls to create a maze
        return Backtrack(maze, width, height);
    }
}