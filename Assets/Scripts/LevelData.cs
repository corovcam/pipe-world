using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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

public static class LevelData
{
    public static bool IsArcadeMode { get; set; } = false;
	public static int LevelNumber { get; set; }
	public static int BoardSize { get; set; }
	public static Position StartPipe { get; set; }
	public static Position EndPipe { get; set; }
    public static PipeHandler[,] GamePieces { get; set; }

	public static Pipe[,] GetRandomPuzzle(int width, int height)
    {
        Dictionary<WallState, List<Pipe>> wallsToPipesMap 
            = new Dictionary<WallState, List<Pipe>>();
        FillWallsToPipesMap(ref wallsToPipesMap);

        WallState[,] puzzleWalls = PuzzleGenerator.Generate(width, height);
		Pipe[,] pipes = new Pipe[width, height];
		for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
				var wallStateWOVisited = puzzleWalls[i, j] & ~WallState.VISITED;
				var possibleWalls = wallsToPipesMap[wallStateWOVisited];
				Pipe chosenPipe = possibleWalls[Random.Range(0, possibleWalls.Count)];
				pipes[i, j] = chosenPipe;
            }
        }
		StartPipe = new Position { X = 0, Y = BoardSize - 1 };
		EndPipe = new Position { X = BoardSize - 1, Y = 0 };
		return pipes;
	}

    public static Pipe[,] ReadInputLevelData()
    {
        string levelData = Resources.Load<TextAsset>("Level" + LevelNumber.ToString()).text;
        bool nextIsValue = false;
        int xCoord = 0;
        int yCoord = 0;
        int value = 0;
        Pipe[,] pipes = new Pipe[BoardSize, BoardSize];
        int offset = BoardSize - 1;
        foreach (char currentChar in levelData)
        {
            if (currentChar == ',')
            {
                if (value == 0)
                    pipes[xCoord, yCoord] = Pipe.EMPTY;
                else
                    pipes[xCoord, yCoord] = (Pipe)(value - 1);
                xCoord++;
                nextIsValue = false;
            }
            if (currentChar == 'S')
            {
                StartPipe = new Position { X = xCoord, Y = yCoord + offset };
            }
            if (currentChar == 'E')
            {
                EndPipe = new Position { X = xCoord, Y = yCoord + offset };
            }
            if (nextIsValue == true)
            {
                value = currentChar - '0';
            }
            if (currentChar == ':')
            {
                nextIsValue = true;
            }
            if (currentChar == '\n')
            {
                yCoord++;
                xCoord = 0;
                offset -= 2;
            }
        }
        return pipes;
    }

    private static void FillWallsToPipesMap(ref Dictionary<WallState, List<Pipe>> map)
    {
        // 1 Wall
        map.Add(WallState.UP, new List<Pipe> { Pipe.ThreeWay, Pipe.Cross });
        map.Add(WallState.DOWN, new List<Pipe> { Pipe.ThreeWay, Pipe.Cross });
        map.Add(WallState.LEFT, new List<Pipe> { Pipe.ThreeWay, Pipe.Cross });
        map.Add(WallState.RIGHT, new List<Pipe> { Pipe.ThreeWay, Pipe.Cross });

        // 2 Walls
        map.Add(WallState.UP | WallState.DOWN,
            new List<Pipe> { Pipe.Straight, Pipe.ThreeWay, Pipe.Cross });
        map.Add(WallState.RIGHT | WallState.LEFT,
            new List<Pipe> { Pipe.Straight, Pipe.ThreeWay, Pipe.Cross });

        map.Add(WallState.UP | WallState.RIGHT,
            new List<Pipe> { Pipe.Round, Pipe.ThreeWay, Pipe.Cross });
        map.Add(WallState.RIGHT | WallState.DOWN,
            new List<Pipe> { Pipe.Round, Pipe.ThreeWay, Pipe.Cross });
        map.Add(WallState.DOWN | WallState.LEFT,
            new List<Pipe> { Pipe.Round, Pipe.ThreeWay, Pipe.Cross });
        map.Add(WallState.LEFT | WallState.UP,
            new List<Pipe> { Pipe.Round, Pipe.ThreeWay, Pipe.Cross });

        // 3 Walls
        map.Add(WallState.UP | WallState.RIGHT | WallState.DOWN,
            new List<Pipe> { Pipe.Straight, Pipe.Round, Pipe.ThreeWay, Pipe.Cross });
        map.Add(WallState.RIGHT | WallState.DOWN | WallState.LEFT,
            new List<Pipe> { Pipe.Straight, Pipe.Round, Pipe.ThreeWay, Pipe.Cross });
        map.Add(WallState.DOWN | WallState.LEFT | WallState.UP,
            new List<Pipe> { Pipe.Straight, Pipe.Round, Pipe.ThreeWay, Pipe.Cross });
        map.Add(WallState.LEFT | WallState.UP | WallState.RIGHT,
            new List<Pipe> { Pipe.Straight, Pipe.Round, Pipe.ThreeWay, Pipe.Cross });
    }
}
