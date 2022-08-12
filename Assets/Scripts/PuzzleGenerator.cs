using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Flags]
public enum CellWalls
{
    // 0000 -> NO WALLS
    // 1111 -> LEFT,RIGHT,UP,DOWN
    LEFT = 1, // 0001
    RIGHT = 2, // 0010
    UP = 4, // 0100
    DOWN = 8, // 1000

    VISITED = 128, // 1000 0000
}

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
            case CellWalls.RIGHT: return CellWalls.LEFT;
            case CellWalls.LEFT: return CellWalls.RIGHT;
            case CellWalls.UP: return CellWalls.DOWN;
            case CellWalls.DOWN: return CellWalls.UP;
            default: return CellWalls.LEFT;
        }
    }

    private static CellWalls[,] Backtrack(CellWalls[,] cells, int width, int height)
    {
        var rng = new System.Random();
        var positionStack = new Stack<Position>();
        var position = new Position { X = 0, Y = 0 };

        cells[position.X, position.Y] |= CellWalls.VISITED;
        positionStack.Push(position);

        while (positionStack.Count > 0)
        {
            var current = positionStack.Pop();
            var neighbours = GetUnvisitedAdjacentCells(current, cells, width, height);

            if (neighbours.Count > 0)
            {
                positionStack.Push(current);

                var randIndex = rng.Next(0, neighbours.Count);
                var randomNeighbour = neighbours[randIndex];

                var nPosition = randomNeighbour.Location;
                cells[current.X, current.Y] &= ~randomNeighbour.CommonWall;
                cells[nPosition.X, nPosition.Y] &= ~GetOppositeWall(randomNeighbour.CommonWall);
                cells[nPosition.X, nPosition.Y] |= CellWalls.VISITED;

                positionStack.Push(nPosition);
            }
        }

        return cells;
    }

    private static List<AdjacentCell> GetUnvisitedAdjacentCells
        (Position p, CellWalls[,] cells, int width, int height)
    {
        var list = new List<AdjacentCell>();

        if (p.X > 0) // left
        {
            if (!cells[p.X - 1, p.Y].HasFlag(CellWalls.VISITED))
            {
                list.Add(new AdjacentCell
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

        if (p.Y > 0) // DOWN
        {
            if (!cells[p.X, p.Y - 1].HasFlag(CellWalls.VISITED))
            {
                list.Add(new AdjacentCell
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

        if (p.Y < height - 1) // UP
        {
            if (!cells[p.X, p.Y + 1].HasFlag(CellWalls.VISITED))
            {
                list.Add(new AdjacentCell
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

        if (p.X < width - 1) // RIGHT
        {
            if (!cells[p.X + 1, p.Y].HasFlag(CellWalls.VISITED))
            {
                list.Add(new AdjacentCell
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

        return list;
    }

    public static CellWalls[,] GenerateMaze(int width, int height)
    {
        CellWalls[,] maze = new CellWalls[width, height];
        CellWalls initial = CellWalls.RIGHT | CellWalls.LEFT | CellWalls.UP | CellWalls.DOWN;
        for (int i = 0; i < width; ++i)
        {
            for (int j = 0; j < height; ++j)
            {
                maze[i, j] = initial;  // 1111
            }
        }

        return Backtrack(maze, width, height);
    }
}