using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GridPipe : MonoBehaviour
{
    private bool draggingItem = false;
    private Vector2 touchOffset;

    public Vector2 startingPosition;
    public Transform myParent;

    [SerializeField] private List<Transform> touchingTiles = new();
    //private AudioSource audSource;

    void Start()
    {
        startingPosition = transform.position;
        myParent = transform.parent;
        //audSource = gameObject.GetComponent<AudioSource>();
    }

    void Update()
    {
        if (!draggingItem) return;

        transform.position = GetMousePos() + touchOffset;
    }

    void OnMouseDown()
    {
        if (!PauseControl.GameIsPaused && !GUIHandler.IsEndGame)
        {
            draggingItem = true;
            touchOffset = (Vector2)transform.position - GetMousePos();
            PickUp();
        }
    }

    void OnMouseUp()
    {
        if (!PauseControl.GameIsPaused && !GUIHandler.IsEndGame)
        {
            draggingItem = false;
            Drop();
        }
    }

    Vector2 GetMousePos() => Camera.main.ScreenToWorldPoint(Input.mousePosition);

    public void PickUp()
    {
        transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
        gameObject.GetComponent<SpriteRenderer>().sortingOrder = 1;
    }

    public void Drop()
    {
        transform.localScale = new Vector3(1, 1, 1);
        gameObject.GetComponent<SpriteRenderer>().sortingOrder = 0;
        Vector2 newPosition;
        if (touchingTiles.Count == 0)
        {
            transform.position = startingPosition;
            transform.SetParent(myParent);
            return;
        }
        var currentCell = touchingTiles[0];
        if (touchingTiles.Count == 1)
        {
            newPosition = currentCell.position;
        }
        else
        {
            var distance = Vector2.Distance(transform.position, touchingTiles[0].position);
            foreach (Transform cell in touchingTiles)
            {
                if (Vector2.Distance(transform.position, cell.position) < distance)
                {
                    currentCell = cell;
                    distance = Vector2.Distance(transform.position, cell.position);
                }
            }
            newPosition = currentCell.position;
        }
        Transform otherPipeTransform = currentCell.GetChild(0);
        if (otherPipeTransform.GetComponent<PipeHandler>().pipeType.Type != PipeType.EMPTY)
        {
            transform.position = startingPosition;
            transform.SetParent(myParent);
            return;
        }
        else
        {
            UpdatePipeTransformsAndHandlers(otherPipeTransform, newPosition);
        }
    }

    private void UpdatePipeTransformsAndHandlers(Transform otherPipeTransform, Vector2 newPosition)
    {
        var thisPipeHandler = gameObject.GetComponent<PipeHandler>();
        var otherPipeHandler = otherPipeTransform.GetComponent<PipeHandler>();

        transform.SetParent(otherPipeTransform.parent);
        StartCoroutine(SlotIntoPlace(transform.position, newPosition, thisPipeHandler));
        LevelData.GamePieces[thisPipeHandler.location.X, thisPipeHandler.location.Y] = otherPipeHandler;

        Transform otherPipeParent = otherPipeTransform.parent;
        otherPipeTransform.SetParent(myParent);
        otherPipeTransform.position = startingPosition;
        LevelData.GamePieces[otherPipeHandler.location.X, otherPipeHandler.location.Y] = thisPipeHandler;
        //var otherGridPipeComp = otherPipeTransform.GetComponent<GridPipe>();
        //otherGridPipeComp.startingPosition = startingPosition;
        //otherGridPipeComp.myParent = myParent;
        
        startingPosition = newPosition;
        myParent = otherPipeParent;

        thisPipeHandler.ProcessNearbyPipeChanges(setNearbyPipeHandlers: true);
        otherPipeHandler.ProcessNearbyPipeChanges(setNearbyPipeHandlers: true);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag != "Tile") return;
        if (!touchingTiles.Contains(other.transform))
        {
            touchingTiles.Add(other.transform);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag != "Tile") return;
        if (touchingTiles.Contains(other.transform))
        {
            touchingTiles.Remove(other.transform);
        }
    }

    IEnumerator SlotIntoPlace(Vector2 startingPos, Vector2 endingPos, PipeHandler pipeHandler)
    {
        float duration = 0.1f;
        float elapsedTime = 0;
        //audSource.Play();
        while (elapsedTime < duration)
        {
            transform.position = Vector2.Lerp(startingPos, endingPos, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        transform.position = endingPos;
        pipeHandler.SetActiveTileToThisGO();
    }
}