using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Handles Level building, Tile/Pipe management, Restart, Tiles shuffle, rotation and audio
/// </summary>
public class LevelHandler : MonoBehaviour
{
    // Holds the list of water pipe prefabs
    public GameObject[] waterPipePrefabs;

    // Holds the list of lava pipe prefabs
    public GameObject[] lavaPipePrefabs;

    // Holds the list of basic water green pipe sprites and filled green pipe sprites
    public Sprite[] greenPipeSprites;
    public Sprite[] filledGreenPipeSprites;

    // Holds the list of lava grey pipe sprites and filled grey pipe sprites
    public Sprite[] greyPipeSprites;
    public Sprite[] filledGreyPipeSprites;

    // Holds the list of start/end water blue-green pipe sprites and filled blue-green pipe sprites
    public Sprite[] blueGreenPipeSprites;
    public Sprite[] filledBlueGreenPipeSprites;

    // Holds the list of start/end lava red-grey pipe sprites and filled red-grey pipe sprites
    public Sprite[] redGreyPipeSprites;
    public Sprite[] filledRedGreyPipeSprites;

    // Game object that hovers and flickers above a tile/pipe to mark the current active tile
    public GameObject tilePointer;

    // Holds the back tile prefab and its sprites
    public GameObject backTilePrefab;
    public List<Sprite> backTileSprites;
    public Sprite backTileEnd;

    // Audio sources for playing pipe rotation and winning sounds
    public AudioSource pipeRotationAudio;
    public AudioSource winningAudio;

    // Serialized fields to be seen in the Inspector, activePipe and activePipeHandler
    [SerializeField] GameObject activePipe;
    [SerializeField] PipeHandler activePipeHandler;

    // Board size and level number, and a flag for arcade mode
    [SerializeField][Range(5, 10)] int boardSize = 10;
    [SerializeField][Range(1, 15)] int levelNum = 1;
    [SerializeField] bool arcadeMode = false;

    // Stores the 2D array of tile objects
    public GameObject[,] tileObjects;

    // Reference to PauseControl and GameManager scripts
    public PauseControl pauseControl;
    public GameManager gameManager;

    /// <summary>
    /// Method that sets default values at the start of the game if they weren't set 
    /// in Main Menu (for debugging purposes).
    /// It initializes default square board and generates new grid & level. 
    /// </summary>
    void Start()
    {
        //Set default static values at the start of the game if they weren't set in Main Menu(for debugging purposes)
        if (LevelData.LevelNumber == 0 && LevelData.BoardSize == 0)
        {
            LevelData.IsArcadeMode = arcadeMode;
            LevelData.LevelNumber = levelNum;
            LevelData.BoardSize = boardSize;
        }
        boardSize = LevelData.BoardSize;

        //Initialize default square board
        tileObjects = new GameObject[boardSize, boardSize];

        //Generate new grid & level
        GenerateNewGrid();
        GenerateLevel();

        // The StartPipe is the first Active Tile
        SetActiveTile(tileObjects[LevelData.defaultStart.Value.X, LevelData.defaultStart.Value.Y]);

        //Store the GamePieces
        StoreGamePieces();

        //Set start & end pipe sprites after iterating through start pipes and end pipes
        SetStartEndPipeSprites(iterateStarts: true);
        SetStartEndPipeSprites(iterateStarts: false);

        //Shuffle the Pipes 
        StartCoroutine(Shuffle());
    }


    /// <summary>
    /// This function generates a new grid of back tiles with a chosen sprite and sets their position
    /// and parent.
    /// </summary>
    void GenerateNewGrid()
    {
        // Choose the BackTile Sprite at random
        Sprite chosenBackTileSprite = backTileSprites[Random.Range(0, backTileSprites.Count)];
        // For every BackTile position create a new GO, configure it, set its Transform and store it in tileObjects
        for (int y = boardSize - 1; y >= 0; y--)
        {
            for (int x = 0; x < boardSize; x++)
            {
                GameObject temp = Instantiate(backTilePrefab);
                temp.GetComponent<SpriteRenderer>().sprite = chosenBackTileSprite;
                Vector2 tileTransform = new Vector2(x, y);
                temp.transform.position = tileTransform;
                temp.name = string.Format("({0}, {1})", x, y);
                temp.transform.parent = gameObject.transform;

                /* Sets the layer of tiles to "Ignore Raycast" to Raycast Pipes only.
                   Prevents various unexpected errors when using built-in Physics2D engine. */
                if (LevelData.IsFreeWorldMode)
                {
                    temp.layer = LayerMask.NameToLayer("Ignore Raycast");
                    var collider = temp.AddComponent<BoxCollider2D>();
                    collider.isTrigger = true;
                    collider.size = new Vector2(1, 1);
                }

                tileObjects[x, y] = temp;
            }
        }
        // Position the entire grid in the middle of the Camera ViewPort
        gameObject.transform.position = new Vector2(-(float)(boardSize / 2.0 - 0.5), -(float)(boardSize / 2.0 - 0.5));
    }

    /// <summary>
    /// Generate Level based on random or input data provided
    /// </summary>
    void GenerateLevel()
    {
        Pipe[,] pipes;
        if (LevelData.IsArcadeMode)
            pipes = LevelData.GetRandomPuzzle(boardSize, boardSize);
        else
            pipes = LevelData.ReadInputLevelData();

        // Offset is used to transform the Unity's default coord system from the lower-left corner
        // to a more practical upper-left corner start position (0,0)
        int offset = boardSize - 1;
        List<Pipe> pipesList = new List<Pipe>();
        // If the level is in free world mode, add all the pipes to a list. This is used to randomly select pipes without replacements.
        if (LevelData.IsFreeWorldMode)
        {
            foreach (var pipe in pipes)
                pipesList.Add(pipe);
        }

        // For each row
        for (int y = 0; y < boardSize; y++)
        {
            // For each column
            for (int x = 0; x < boardSize; x++)
            {
                Pipe pipe;
                if (LevelData.IsFreeWorldMode)
                {
                    // If the level is in free world mode, then we want to randomly select a pipe without replacement, unless it's a start or end pipe.
                    if (LevelData.Starts.Select(kv => kv.Value).Any(v => v.Contains(new Position(x, y + offset))) ||
                        LevelData.Ends.Select(kv => kv.Value).Any(v => v.Contains(new Position(x, y + offset))))
                    {
                        pipe = pipes[x, y];
                        pipesList.Remove(pipe);
                    }
                    else
                    {
                        int randIndex = Random.Range(0, pipesList.Count);
                        pipe = pipesList[randIndex];
                        pipesList.RemoveAt(randIndex);
                    }
                }
                else
                {
                    pipe = pipes[x, y];
                }

                // Instantiate the pipe prefab.
                GameObject pipePrefab = pipe.Liquid == Liquid.Water ? waterPipePrefabs[(int)pipe.Type] : lavaPipePrefabs[(int)pipe.Type];
                GameObject pipeGO = Instantiate(pipePrefab);

                // Add the grid pipe component if the level is in free world mode.
                if (LevelData.IsFreeWorldMode)
                {
                    pipeGO.AddComponent<GridPipe>();
                    var rb = pipeGO.AddComponent<Rigidbody2D>();
                    rb.bodyType = RigidbodyType2D.Kinematic;
                }

                // Set the pipe type on the pipe handler.
                pipeGO.GetComponent<PipeHandler>().pipeType = pipe;
                PositionPipeGO(tileObjects[x, y + offset], pipeGO);
            }
            offset -= 2;
        }
        // Set the End pipe backTile to a Finish Flag
        LevelData.Ends.Select(kv => kv.Value).ToList()
            .ForEach(endsList =>
                endsList.ForEach(end => tileObjects[end.X, end.Y].GetComponent<SpriteRenderer>().sprite = backTileEnd)
                );
    }

    /// <summary>
    /// Configure and place Pipe prefab on top of Tile GO.
    /// </summary>
    /// <param name="tile">The Tile GO where the Pipe should be placed on.</param>
    /// <param name="pipeGO">The Pipe game object to be placed on the tile.</param>
    private void PositionPipeGO(GameObject tile, GameObject pipeGO)
    {
        pipeGO.transform.position = new Vector2(tile.transform.position.x, tile.transform.position.y);
        pipeGO.transform.parent = tile.transform;
    }

    /// <summary>
    /// Configure current Active Tile to the <paramref name="tile"/>
    /// </summary>
    /// <param name="tile">Grid Tile GO to set as Active Tile</param>
    public void SetActiveTile(GameObject tile)
    {
        activePipe = tile;
        if (!tilePointer.activeSelf)
            tilePointer.SetActive(true);
        tilePointer.transform.position = activePipe.transform.position;
        // Active PipeHandler script instance for Pipe manipulation
        activePipeHandler = activePipe.GetComponentInChildren<PipeHandler>();
    }

    /// <summary>
    /// Auxiliary function to store 2D array of PipeHandler script instances after 
    /// the grid and pipes were generated
    /// </summary>
    public void StoreGamePieces()
    {
        LevelData.GamePieces = new PipeHandler[boardSize, boardSize];
        foreach (var piece in GameObject.FindGameObjectsWithTag("Pipe"))
        {
            int xCoord = (int)piece.transform.parent.localPosition.x;
            int yCoord = (int)piece.transform.parent.localPosition.y;
            LevelData.GamePieces[xCoord, yCoord] = piece.GetComponent<PipeHandler>();
        }
    }

    /// <summary>
    /// IEnumerator to be used in Shuffle Coroutine function. Only runs once.
    /// </summary>
    IEnumerator Shuffle()
    {
        // Wait one frame after the grid was generated and shuffle the game pieces afterwards
        // to simulate rotating effect
        yield return new WaitForEndOfFrame();
        for (int x = 0; x < boardSize; x++)
        {
            for (int y = 0; y < boardSize; y++)
            {
                int k = Random.Range(0, 4);

                for (int i = 0; i < k; i++)
                {
                    LevelData.GamePieces[x, y]?.RotatePiece();
                }
            }
        }
    }

    /// <summary>
    /// Change StartPipe and EndPipe sprites to their corresponding red variants
    /// </summary>
    void SetStartEndPipeSprites(bool iterateStarts)
    {
        // Iterate through either the starting or ending positions of liquid pipes in a game level 
        // and update their sprites based on their liquid type and whether they are filled or not. 
        // If the game is in free world mode, the GridPipe component of the start/end pipes is also destroyed 
        // (to prevent dragging events). 
        var iterable = iterateStarts ? LevelData.Starts : LevelData.Ends;

        foreach (Liquid liq in iterable.Keys)
        {
            foreach (Position pos in iterable[liq])
            {
                var pipe = LevelData.GamePieces[pos.X, pos.Y];
                Pipe pipeType = pipe.pipeType;
                Sprite chosenSprite;

                if (iterateStarts)
                {
                    chosenSprite = pipeType.Liquid == Liquid.Water ?
                            filledBlueGreenPipeSprites[(int)pipeType.Type] : filledRedGreyPipeSprites[(int)pipeType.Type];
                }
                else
                {
                    chosenSprite = pipeType.Liquid == Liquid.Water ?
                            blueGreenPipeSprites[(int)pipeType.Type] : redGreyPipeSprites[(int)pipeType.Type];
                }

                pipe.GetComponent<SpriteRenderer>().sprite = chosenSprite;

                if (LevelData.IsFreeWorldMode)
                {
                    Destroy(pipe.GetComponent<GridPipe>());
                }
            }
        }
    }

    /// <summary>
    /// Resets all pipe sprites to their default (without liquid) variants, sets the Active Tile to the StartPipe
    /// and shuffles the GamePieces.
    /// </summary>
    public void ResetLevel()
    {
        // If Free World mode is active, do nothing and return
        if (LevelData.IsFreeWorldMode)
            return;

        // Reset each pipe to its default sprite variant (with or without a liquid)
        foreach (var pipe in LevelData.GamePieces)
        {
            Pipe pipeType = pipe.pipeType;
            if (pipeType.Type != PipeType.EMPTY)
            {
                var chosenSprite = pipeType.Liquid == Liquid.Water ?
                    greenPipeSprites[(int)pipeType.Type] : greyPipeSprites[(int)pipeType.Type];
                pipe.GetComponent<SpriteRenderer>().sprite = chosenSprite;
            }
        }

        // Set the sprites for the StartPipe and EndPipe
        SetStartEndPipeSprites(iterateStarts: true);
        SetStartEndPipeSprites(iterateStarts: false);

        // Set the Active Tile to the StartPipe
        SetActiveTile(LevelData.GamePieces[LevelData.defaultStart.Value.X, LevelData.defaultStart.Value.Y].gameObject);

        // Shuffle the GamePieces
        StartCoroutine(Shuffle());
    }


    /// <summary>
    /// Rotate the current ActiveTile
    /// </summary>
    public void RotateActiveTile()
    {
        activePipeHandler.RotatePiece();
    }

    // Functions used to modify the Pointer and ActiveTile position

    /// <summary>
    /// This function moves the active tile up by one position on the game board.
    /// </summary>
    public void MoveActiveTileUp()
    {
        int x = activePipeHandler.location.X;
        int y = activePipeHandler.location.Y + 1;
        if (y >= 0 && y < LevelData.BoardSize)
        {
            SetActiveTile(LevelData.GamePieces[x, y].gameObject);
        }
    }

    /// <summary>
    /// This function moves the active tile down by one unit on a game board.
    /// </summary>
    public void MoveActiveTileDown()
    {
        int x = activePipeHandler.location.X;
        int y = activePipeHandler.location.Y - 1;
        if (y >= 0 && y < LevelData.BoardSize)
        {
            SetActiveTile(LevelData.GamePieces[x, y].gameObject);
        }
    }

    /// <summary>
    /// This function moves the active tile to the right if it is within the game board's boundaries.
    /// </summary>
    public void MoveActiveTileRight()
    {
        int x = activePipeHandler.location.X + 1;
        int y = activePipeHandler.location.Y;
        if (x >= 0 && x < LevelData.BoardSize)
        {
            SetActiveTile(LevelData.GamePieces[x, y].gameObject);
        }
    }

    /// <summary>
    /// This function moves the active tile to the left if it is within the bounds of the game board.
    /// </summary>
    public void MoveActiveTileLeft()
    {
        int x = activePipeHandler.location.X - 1;
        int y = activePipeHandler.location.Y;
        if (x >= 0 && x < LevelData.BoardSize)
        {
            SetActiveTile(LevelData.GamePieces[x, y].gameObject);
        }
    }

    /// <summary>
    /// This function plays an audio clip for pipe rotation.
    /// </summary>
    public void PlayPipeRotationAudio()
    {
        pipeRotationAudio.Play();
    }

    /// <summary>
    /// The function "PlayWinningAudio" plays a winning audio file.
    /// </summary>
    public void PlayWinningAudio()
    {
        winningAudio.Play();
    }
}
