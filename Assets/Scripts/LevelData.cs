using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Liquid
{
    Water, Lava
}

/// <summary>
/// Public enum of Pipe types (last is always EMPTY)
/// </summary>
public enum PipeType
{
    Straight, Round, ThreeWay, Cross, EMPTY
}

public struct Pipe
{
    static Pipe() => new Pipe(Liquid.Water, PipeType.EMPTY);

    public Pipe(Liquid liquid, PipeType type)
    {
        Liquid = liquid;
        Type = type;
    }

    public Liquid Liquid { get; }
    public PipeType Type { get; }
}

/// <summary>
/// Struct used to store X and Y coordinates of Pipes on the Board. 
/// Overrides basic equality operators.
/// </summary>
public struct Position
{
    public int X;
    public int Y;

    public override bool Equals(object obj)
    {
        return base.Equals(obj);
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public override string ToString()
    {
        return $"({X}, {Y})";
    }

    public static bool operator ==(Position first, Position second)
    {
        return first.Equals(second);
    }

    public static bool operator !=(Position first, Position second)
    {
        return !first.Equals(second);
    }
}

/// <summary>
/// Static class used to store all information about the current Game and Level
/// </summary>
public static class LevelData
{
    public static bool IsArcadeMode { get; set; } = false;
    public static bool IsFreeWorldMode { get; set; } = false;
	public static int LevelNumber { get; set; }
	public static int BoardSize { get; set; }
    public static int TimeLimit { get; set; }
	public static Position StartPipe { get; set; }
	public static Position EndPipe { get; set; }
    public static PipeHandler[,] GamePieces { get; set; }

    // Temporary data structure for level loading
    public static string[] lvlData;

    /// <summary>
    /// Gets random puzzle using Random Maze generator and a Dictionary map to 
    /// convert it to the Pipes Puzzle
    /// </summary>
    /// <param name="width">Width of the Puzzle</param>
    /// <param name="height">Height of the Puzzle</param>
    /// <returns>2D Array of Pipes = the Pipe Puzzle</returns>
	public static Pipe[,] GetRandomPuzzle(int width, int height)
    {
        // Used to translate CellWalls in a maze to potential List of Pipes that can
        // occupy the given cell
        Dictionary<CellWalls, List<PipeType>> wallsToPipesMap 
            = new Dictionary<CellWalls, List<PipeType>>();
        FillWallsToPipesMap(ref wallsToPipesMap);

        CellWalls[,] puzzleWalls = PuzzleGenerator.GenerateMaze(width, height);
		Pipe[,] pipes = new Pipe[width, height];
		for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                // Removes the VISITED Flag from the CellWall
				var wallStateWOVisited = puzzleWalls[i, j] & ~CellWalls.VISITED;
				var possibleWalls = wallsToPipesMap[wallStateWOVisited];
                // Chooses a random Pipe from the Dictionary to generate random (but correct) Pipe Puzzle
				PipeType chosenPipeType = possibleWalls[Random.Range(0, possibleWalls.Count)];
				pipes[i, j] = new Pipe(Liquid.Water, chosenPipeType);
            }
        }
		StartPipe = GetRandomStartPos();
		EndPipe = GetRandomEndPos();
		return pipes;
	}

    /// <summary>
    /// Chooses a random StartPipe Position in the upper-left half of the Board
    /// </summary>
    private static Position GetRandomStartPos()
    {
        int x = Random.Range(0, (int)(BoardSize / 2.0) + 1);
        int y;
        if (x != 0)
            y = BoardSize - 1;
        else
            y = Random.Range((int)(BoardSize / 2.0), BoardSize - 1);
        
        return new Position { X = x, Y = y };
    }

    /// <summary>
    /// Chooses a random EndPipe Position in the lower-right half of the Board
    /// </summary>
    private static Position GetRandomEndPos()
    {
        int x = Random.Range((int)(BoardSize / 2.0), BoardSize);
        int y;
        if (x != BoardSize - 1)
            y = 0;
        else
            y = Random.Range(0, (int)(BoardSize / 2.0) + 1);

        return new Position { X = x, Y = y };
    }

    /// <summary>
    /// Read Input from a text file in a given format
    /// </summary>
    /// <returns>2D list of Pipes for the puzzle</returns>
    public static Pipe[,] ReadInputLevelData()
    {
        bool isPipeInfo = false;
        int xCoord = 0;
        int yCoord = 0;
        int value = 0;
        Liquid pipeLiquid = Liquid.Water;
        Pipe[,] pipes = new Pipe[BoardSize, BoardSize];
        int offset = BoardSize - 1;
        foreach (string line in lvlData[2..])
        {
            foreach (char current in line)
            {
                if (current == ';')
                {
                    if (value == 0)
                        pipes[xCoord, yCoord] = new Pipe(pipeLiquid, PipeType.EMPTY);
                    else
                        pipes[xCoord, yCoord] = new Pipe(pipeLiquid, (PipeType)(value - 1));
                    xCoord++;
                    isPipeInfo = false;
                }
                if (current == 'S')
                {
                    StartPipe = new Position { X = xCoord, Y = yCoord + offset };
                }
                if (current == 'E')
                {
                    EndPipe = new Position { X = xCoord, Y = yCoord + offset };
                }
                if (isPipeInfo == true)
                {
                    int testValue = current - '0';
                    if (testValue < 0 || 9 < testValue)
                        pipeLiquid = current == 'W' ? Liquid.Water : Liquid.Lava;
                    value = testValue;
                }
                if (current == ':')
                {
                    isPipeInfo = true;
                }
            }
            yCoord++;
            xCoord = 0;
            offset -= 2;
        }

        lvlData = null;
        return pipes;
    }

    /// <summary>
    /// Auxiliary function to fill a translation map between CellWalls enum to List of potential Pipes
    /// </summary>
    private static void FillWallsToPipesMap(ref Dictionary<CellWalls, List<PipeType>> map)
    {
        // 1 Wall
        map.Add(CellWalls.UP, new List<PipeType> { PipeType.ThreeWay, PipeType.Cross });
        map.Add(CellWalls.DOWN, new List<PipeType> { PipeType.ThreeWay, PipeType.Cross });
        map.Add(CellWalls.LEFT, new List<PipeType> { PipeType.ThreeWay, PipeType.Cross });
        map.Add(CellWalls.RIGHT, new List<PipeType> { PipeType.ThreeWay, PipeType.Cross });

        // 2 Walls
        map.Add(CellWalls.UP | CellWalls.DOWN,
            new List<PipeType> { PipeType.Straight });
        map.Add(CellWalls.RIGHT | CellWalls.LEFT,
            new List<PipeType> { PipeType.Straight });

        map.Add(CellWalls.UP | CellWalls.RIGHT,
            new List<PipeType> { PipeType.Round });
        map.Add(CellWalls.RIGHT | CellWalls.DOWN,
            new List<PipeType> { PipeType.Round });
        map.Add(CellWalls.DOWN | CellWalls.LEFT,
            new List<PipeType> { PipeType.Round });
        map.Add(CellWalls.LEFT | CellWalls.UP,
            new List<PipeType> { PipeType.Round });

        // 3 Walls
        map.Add(CellWalls.UP | CellWalls.RIGHT | CellWalls.DOWN,
            new List<PipeType> { PipeType.Straight, PipeType.Round });
        map.Add(CellWalls.RIGHT | CellWalls.DOWN | CellWalls.LEFT,
            new List<PipeType> { PipeType.Straight, PipeType.Round });
        map.Add(CellWalls.DOWN | CellWalls.LEFT | CellWalls.UP,
            new List<PipeType> { PipeType.Straight, PipeType.Round });
        map.Add(CellWalls.LEFT | CellWalls.UP | CellWalls.RIGHT,
            new List<PipeType> { PipeType.Straight, PipeType.Round });
    }
}
