using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles Level building, Tile/Pipe management, Restart, Tiles shuffle, rotation and audio
/// </summary>
public class LevelHandler : MonoBehaviour
{
    public GameObject[] waterPipePrefabs;
    public GameObject[] lavaPipePrefabs;

    // basic water green Pipe sprites
    public Sprite[] greenPipeSprites;
    public Sprite[] filledGreenPipeSprites;

    // lava grey Pipe sprites
    public Sprite[] greyPipeSprites;
    public Sprite[] filledGreyPipeSprites;

    // Start/End water blue-green Pipe sprites
    public Sprite[] blueGreenPipeSprites;
    public Sprite[] filledBlueGreenPipeSprites;

    // Start/End lava red-grey Pipe sprites
    public Sprite[] redGreyPipeSprites;
    public Sprite[] filledRedGreyPipeSprites;

    // Hovers and flickers above a tile/pipe to mark the current active tile
    public GameObject tilePointer;

    // Back tiles that create the grid
    public GameObject backTilePrefab;
    public List<Sprite> backTileSprites;

    public AudioSource pipeRotationAudio;
    public AudioSource winningAudio;

    [SerializeField] // to be seen in the Inspector
    GameObject activePipe;
    [SerializeField]
    PipeHandler activePipeHandler;

    [SerializeField]
    [Range(5, 10)]
    int boardSize = 10;

    [SerializeField]
    [Range(1, 15)]
    int levelNum = 1;

    [SerializeField]
    bool arcadeMode = false;

    // Initial storage of grid backTile 2D array
    public GameObject[,] tileObjects;

    public PauseControl pauseControl;

    void Start()
    {
        // Set default static values at the start of the game if they weren't set
        // in Main Menu (for debugging purposes)
        if (LevelData.LevelNumber == 0 && LevelData.BoardSize == 0)
        {
            LevelData.IsArcadeMode = arcadeMode;
            LevelData.LevelNumber = levelNum;
            LevelData.BoardSize = boardSize;
        }
        boardSize = LevelData.BoardSize;
        tileObjects = new GameObject[boardSize, boardSize]; // Initialize default square board
        GenerateNewGrid();
        GenerateLevel();
        // The StartPipe is the first Active Tile
        SetActiveTile(tileObjects[LevelData.StartPipe.X, LevelData.StartPipe.Y]);
        StoreGamePieces();
        SetStartEndPipeSprites();
        StartCoroutine(Shuffle()); // Shuffle the Pipes
    }

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
        for (int y = 0; y < boardSize; y++)
        {
            for (int x = 0; x < boardSize; x++)
            {
                ConfigurePipePrefab(tileObjects[x, y + offset], pipes[x, y]);
            }
            offset -= 2;
        }
    }

    /// <summary>
    /// Configure and place Pipe prefab on top of Tile GO
    /// </summary>
    /// <param name="tile">The Tile GO where the Pipe should be placed on</param>
    /// <param name="pipeID">The corresponding Pipe enum index</param>
    private void ConfigurePipePrefab(GameObject tile, Pipe pipe)
    {
        GameObject pipePrefab = pipe.Liquid == Liquid.Water ? waterPipePrefabs[(int)pipe.Type] : lavaPipePrefabs[(int)pipe.Type];
        GameObject pipeGO = Instantiate(pipePrefab);
        pipeGO.GetComponent<PipeHandler>().pipeType = pipe;

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
    private void StoreGamePieces()
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
    void SetStartEndPipeSprites()
    {
        var startPipe = LevelData.GamePieces[LevelData.StartPipe.X, LevelData.StartPipe.Y];
        Pipe pipeType = startPipe.pipeType;
        var chosenSprite = pipeType.Liquid == Liquid.Water ?
                    blueGreenPipeSprites[(int)pipeType.Type] : redGreyPipeSprites[(int)pipeType.Type];

        startPipe.GetComponent<SpriteRenderer>().sprite = chosenSprite;
        
        var endPipe = LevelData.GamePieces[LevelData.EndPipe.X, LevelData.EndPipe.Y];
        pipeType = startPipe.pipeType;
        chosenSprite = pipeType.Liquid == Liquid.Water ? 
            blueGreenPipeSprites[(int)pipeType.Type] : redGreyPipeSprites[(int)pipeType.Type];

        endPipe.GetComponent<SpriteRenderer>().sprite = chosenSprite;
    }

    /// <summary>
    /// Reset all pipe sprites to their default (without liquid) variants, Set Active Tile to the StartPipe
    /// and Shuffle the GamePieces
    /// </summary>
    public void ResetLevel()
    {
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
        SetStartEndPipeSprites();
        SetActiveTile(LevelData.GamePieces[LevelData.StartPipe.X, LevelData.StartPipe.Y].gameObject);
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

    public void MoveActiveTileUp()
    {
        int x = activePipeHandler.location.X;
        int y = activePipeHandler.location.Y + 1;
        if (y >= 0 && y < LevelData.BoardSize)
        {
            SetActiveTile(LevelData.GamePieces[x, y].gameObject);
        }
    }

    public void MoveActiveTileDown()
    {
        int x = activePipeHandler.location.X;
        int y = activePipeHandler.location.Y - 1;
        if (y >= 0 && y < LevelData.BoardSize)
        { 
            SetActiveTile(LevelData.GamePieces[x, y].gameObject);
        }
    }

    public void MoveActiveTileRight()
    {
        int x = activePipeHandler.location.X + 1;
        int y = activePipeHandler.location.Y;
        if (x >= 0 && x < LevelData.BoardSize)
        {
            SetActiveTile(LevelData.GamePieces[x, y].gameObject);
        }
    }

    public void MoveActiveTileLeft()
    {
        int x = activePipeHandler.location.X - 1;
        int y = activePipeHandler.location.Y;
        if (x >= 0 && x < LevelData.BoardSize)
        {
            SetActiveTile(LevelData.GamePieces[x, y].gameObject);
        }
    }

    public void PlayPipeRotationAudio()
    {
        pipeRotationAudio.Play();
    }

    public void PlayWinningAudio()
    {
        winningAudio.Play();
    }
}
