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

public enum Difficulty
{
    Easy, Normal, Hard
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

    public Position(int x, int y)
    {
        X = x;
        Y = y;
    }

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
    /// <summary>
    /// The current mode of the game: "Arcade".
    /// </summary>
    public static bool IsArcadeMode { get; set; } = false;

    /// <summary>
    /// The current mode of the game: "Free World".
    /// </summary>
    public static bool IsFreeWorldMode { get; set; } = false;

    /// <summary>
    /// The level number of the current game.
    /// </summary>
    public static int LevelNumber { get; set; }

    /// <summary>
    /// The size of the puzzle game board.
    /// </summary>
    public static int BoardSize { get; set; }

    /// <summary>
    /// The time limit for the current level.
    /// </summary>
    public static int TimeLimit { get; set; }

    public static Difficulty Difficulty { get; set; }

    /// <summary>
    /// A dictionary containing the starting positions for a specific liquid type.
    /// </summary>
    public static Dictionary<Liquid, List<Position>> Starts = new();

    /// <summary>
    /// A dictionary containing the ending positions for a specific liquid type.
    /// </summary>
    public static Dictionary<Liquid, List<Position>> Ends = new();

    /// <summary>
    /// A two-dimensional array of PipeHandlers representing the game pieces.
    /// </summary>
    public static PipeHandler[,] GamePieces { get; set; }

    /// <summary>
    /// Temporary data structure for level loading
    /// </summary>
    public static string[] lvlData;

    /// /// <summary>
    /// The default starting position for the liquid type.
    /// </summary>
    public static Position? defaultStart;

    /// <summary>
    /// The default ending position for the liquid type.
    /// </summary>
    public static Position? defaultEnd;

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
        Dictionary<CellWalls, List<PipeType>> wallsToPipesMap = new Dictionary<CellWalls, List<PipeType>>();
        // Fills Dictionary of CellWall pipe possibilities.
        FillWallsToPipesMap(ref wallsToPipesMap);

        // Generate maze and store the maze Cell states for each cell.
        CellWalls[,] puzzleWalls = PuzzleGenerator.GenerateMaze(width, height);
        // Pipes Array of dimensions Width * Height.
        Pipe[,] pipes = new Pipe[width, height];

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                // Removes the VISITED Flag from the CellWall
                var wallStateWOVisited = puzzleWalls[i, j] & ~CellWalls.VISITED;
                var possiblePipes = wallsToPipesMap[wallStateWOVisited];
                // Selects a random Pipe Type to generate a random Pipe Puzzle for each cell.
                PipeType chosenPipeType = possiblePipes[Random.Range(0, possiblePipes.Count)];
                pipes[i, j] = new Pipe(Liquid.Water, chosenPipeType);
            }
        }
        defaultStart = GetRandomStartPos();
        defaultEnd = GetRandomEndPos();
        // Dictionary entry to store Liquid and corresponding list of starting cell positions.
        Starts.Add(Liquid.Water, new List<Position> { defaultStart.Value });
        // Dictionary entry to store Liquid and corresponding list of ending cell positions.
        Ends.Add(Liquid.Water, new List<Position> { defaultEnd.Value });
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
    /// Read level data from text file in given format.
    /// </summary>
    /// <returns>2D list of Pipes for the puzzle.</returns>
    public static Pipe[,] ReadInputLevelData()
    {
        bool isPipeInfo = false;
        bool isStart = false;
        bool isEnd = false;
        int xCoord = 0;
        int yCoord = 0;
        int value = 0;
        Liquid pipeLiquid = Liquid.Water;
        Position savedPos = new Position();
        Pipe[,] pipes = new Pipe[BoardSize, BoardSize];
        int offset = BoardSize - 1;

        foreach (string line in lvlData[2..])
        {
            int i = 0;
            while (i < line.Length)
            {
                char current = line[i];

                // If current character is semicolon, create new Pipe object with correct liquid and pipe type.
                if (current == ';')
                {
                    if (value == 0)
                    {
                        pipes[xCoord, yCoord] = new Pipe(pipeLiquid, PipeType.EMPTY);
                    }
                    else
                    {
                        pipes[xCoord, yCoord] = new Pipe(pipeLiquid, (PipeType)(value - 1));
                    }

                    // Check if Pipe is Start, End, or neither and add to corresponding dictionary.
                    if (isStart)
                    {
                        if (Starts.ContainsKey(pipeLiquid))
                        {
                            Starts[pipeLiquid].Add(savedPos);
                        }
                        else
                        {
                            Starts.Add(pipeLiquid, new List<Position> { savedPos });
                        }
                    }
                    else if (isEnd)
                    {
                        if (Ends.ContainsKey(pipeLiquid))
                        {
                            Ends[pipeLiquid].Add(savedPos);
                        }
                        else
                        {
                            Ends.Add(pipeLiquid, new List<Position> { savedPos });
                        }
                    }

                    // Update xCoord and reset flags.
                    xCoord++;
                    isPipeInfo = false;
                    isStart = false;
                    isEnd = false;
                }
                else if (current == 'S') // If current character is S, it is start position.
                {
                    // Save starting position and set default start position if necessary.
                    savedPos = new Position { X = xCoord, Y = yCoord + offset };
                    defaultStart = defaultStart == null ? savedPos : defaultStart;
                    isStart = true;
                }
                else if (current == 'E') // If current character is E, it is end position.
                {
                    // Save ending position and set default end position if necessary.
                    savedPos = new Position { X = xCoord, Y = yCoord + offset };
                    defaultEnd = defaultEnd == null ? savedPos : defaultEnd;
                    isEnd = true;
                }

                // If isPipeInfo flag is true, update pipeLiquid and pipeType values.
                if (isPipeInfo == true)
                {
                    int testValue = current - '0';
                    if (testValue < 0 || 9 < testValue)
                    {
                        pipeLiquid = current == 'W' ? Liquid.Water : Liquid.Lava;
                    }
                    value = testValue;
                    isPipeInfo = false;
                }
                // If current character is colon, set isPipeInfo flag to true.
                if (current == ':')
                {
                    isPipeInfo = true;
                }
                // Update i.
                i++;
            }
            // Update yCoord, xCoord, and offset.
            yCoord++;
            xCoord = 0;
            offset -= 2;
        }

        lvlData = null; // Set lvlData to null to free up memory.
        return pipes;
    }

    /// <summary>
    /// Auxiliary function to fill a translation map between CellWalls enum to List of potential Pipes
    /// </summary>
    private static void FillWallsToPipesMap(ref Dictionary<CellWalls, List<PipeType>> map)
    {
        // 0 Walls
        map.Add(CellWalls.NO_WALLS, new List<PipeType> { PipeType.Cross });

        // 1 Wall
        map.Add(CellWalls.UP, new List<PipeType> { PipeType.ThreeWay });
        map.Add(CellWalls.DOWN, new List<PipeType> { PipeType.ThreeWay });
        map.Add(CellWalls.LEFT, new List<PipeType> { PipeType.ThreeWay });
        map.Add(CellWalls.RIGHT, new List<PipeType> { PipeType.ThreeWay });

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
