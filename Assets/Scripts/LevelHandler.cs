using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Pipe
{
    Straight, Round, ThreeWay, Cross, EMPTY
}

public class LevelHandler : MonoBehaviour
{
    public List<GameObject> pipePrefabs;
    public List<Sprite> pipeSprites;
    public List<Sprite> filledPipeSprites;

    public GameObject tilePointer;

    public GameObject backTilePrefab;
    public List<Sprite> backTileSprites;

    [SerializeField]
    GameObject activePipe;
    [SerializeField]
    PipeHandler activePipeHandler;

    [SerializeField]
    [Range(5, 10)]
    int boardSize = 10;

    [SerializeField]
    [Range(1, 5)]
    int levelNum = 1;

    public bool arcadeMode = false;

    //Quick and dirty solution. Will come back to
    public GameObject[,] gridObjects;

    // Start is called before the first frame update
    void Start()
    {
        LevelData.IsArcadeMode = arcadeMode;
        LevelData.LevelNumber = levelNum;
        LevelData.BoardSize = boardSize;
        gridObjects = new GameObject[boardSize, boardSize];
        // TODO: Scale the board: with bigger board, tiles get smaller to accomodate dimensions
        // Camera stays the same
        GenerateNewGrid();
        GenerateLevel(isRandom: arcadeMode);
        SetActiveTile(gridObjects[LevelData.StartPipe.X, LevelData.StartPipe.Y]);
        StoreGamePieces();
        StartCoroutine(Shuffle());
    }

    void GenerateNewGrid()
    {
        Sprite chosenBackTileSprite = backTileSprites[Random.Range(0, backTileSprites.Count)];
        for (int y = boardSize - 1; y >= 0; y--)
        {
            for (int x = 0; x < boardSize; x++)
            {
                GameObject temp = Instantiate(backTilePrefab);
                temp.GetComponent<SpriteRenderer>().sprite = chosenBackTileSprite;
                Vector2 tileTransform = new Vector2(x, y);
                temp.transform.position = tileTransform;
                temp.name = string.Format("x:{0} y:{1}", x, y);
                temp.transform.parent = gameObject.transform;
                gridObjects[x, y] = temp;
            }
        }
        gameObject.transform.position = new Vector2(-(float)(boardSize / 2.0 - 0.5), -(float)(boardSize / 2.0 - 0.5));
    }

    void GenerateLevel(bool isRandom)
    {
        Pipe[,] pipes;
        if (isRandom)
            pipes = LevelData.GetRandomPuzzle(boardSize, boardSize);
        else
            pipes = LevelData.ReadInputLevelData();
        int offset = boardSize - 1;
        for (int y = 0; y < boardSize; y++)
        {
            for (int x = 0; x < boardSize; x++)
            {
                int pipeID = (int)pipes[x, y];
                ConfigurePipePrefab(gridObjects[x, y + offset], pipeID);
            }
            offset -= 2;
        }
    }

    private void ConfigurePipePrefab(GameObject tile, int pipeID)
    {
        GameObject pipe = Instantiate(pipePrefabs[pipeID]);
        pipe.transform.position = new Vector2(tile.transform.position.x, tile.transform.position.y);
        pipe.transform.parent = tile.transform;
    }

    public void SetActiveTile(GameObject tile)
    {
        activePipe = tile;
        if (!tilePointer.activeSelf)
            tilePointer.SetActive(true);
        tilePointer.transform.position = activePipe.transform.position;
        activePipeHandler = activePipe.GetComponentInChildren<PipeHandler>();
    }

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

    IEnumerator Shuffle()
    {
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


    public void ResetLevel()
    {
        foreach (var pipe in LevelData.GamePieces)
        {
            if (pipe != null)
            {
                pipe.GetComponent<SpriteRenderer>().sprite = pipeSprites[pipe.tileType];
            }
        }
        SetActiveTile(LevelData.GamePieces[LevelData.StartPipe.X, LevelData.StartPipe.Y].gameObject);
        StartCoroutine(Shuffle());
    }

    public void RotateActiveTile()
    {
        activePipeHandler.RotatePiece();
    }

    public void MoveActiveTileUp()
    {
        int x = activePipeHandler.location.X;
        int y = activePipeHandler.location.Y + 1;
        if (x < LevelData.BoardSize && y < LevelData.BoardSize)
        {
            SetActiveTile(LevelData.GamePieces[x, y].gameObject);
        }
    }

    public void MoveActiveTileDown()
    {
        int x = activePipeHandler.location.X;
        int y = activePipeHandler.location.Y - 1;
        if (x < LevelData.BoardSize && y < LevelData.BoardSize)
        { 
            SetActiveTile(LevelData.GamePieces[x, y].gameObject);
        }
    }

    public void MoveActiveTileRight()
    {
        int x = activePipeHandler.location.X + 1;
        int y = activePipeHandler.location.Y;
        if (x < LevelData.BoardSize && y < LevelData.BoardSize)
        {
            SetActiveTile(LevelData.GamePieces[x, y].gameObject);
        }
    }

    public void MoveActiveTileLeft()
    {
        int x = activePipeHandler.location.X - 1;
        int y = activePipeHandler.location.Y;
        if (x < LevelData.BoardSize && y < LevelData.BoardSize)
        {
            SetActiveTile(LevelData.GamePieces[x, y].gameObject);
        }
    }
}
