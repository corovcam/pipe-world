using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Handles drag and drop events for the Pipe game object.
/// </summary>
public class GridPipe : MonoBehaviour
{
    private bool draggingItem = false; // Flag to indicate if the pipe is currently being dragged
    private Vector2 touchOffset; // Offset of the mouse/touch position from center of the pipe when drag starts
    private Camera mainCamera; // Main camera of the game

    public Vector2 startingPosition; // Starting position of the current pipe
    public Transform myParent; // Parent transform of the current pipe

    [SerializeField] private List<Transform> touchingTiles = new(); // List of tiles currently touched by the pipe

    void Start()
    {
        // Set initial values for starting position, parent Transform and main camera
        startingPosition = transform.position;
        myParent = transform.parent;
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (!draggingItem) return; // Exit function if pipe is not being dragged

        // Update the position of the pipe as the mouse/touch moves
        transform.position = GetMousePos() + touchOffset;
    }

    void OnMouseDown()
    {
        if (!PauseControl.GameIsPaused && !GUIHandler.IsEndGame)
        {
            // Set draggingItem flag to true and obtain touch offset
            draggingItem = true;
            touchOffset = (Vector2)transform.position - GetMousePos();
            PickUp();
        }
    }

    void OnMouseUp()
    {
        // End dragging of pipe
        draggingItem = false;
        Drop();
    }

    /// <summary>
    /// Helper function to convert mouse/touch position to world position.
    /// </summary>
    Vector2 GetMousePos() => mainCamera.ScreenToWorldPoint(Input.mousePosition);

    /// <summary>
    /// Helper function to enlarge the sprite and change the sorting order of the pipe when dragged.
    /// </summary>
    public void PickUp()
    {
        transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
        gameObject.GetComponent<SpriteRenderer>().sortingOrder = 1;
    }

    /// <summary>
    /// Helper function to drop the pipe and reposition it on the game board.
    /// </summary>
    public void Drop()
    {
        transform.localScale = new Vector3(1, 1, 1);
        gameObject.GetComponent<SpriteRenderer>().sortingOrder = 0;
        Vector2 newPosition;
        if (touchingTiles.Count == 0)
        {
            // If not touching any tile, return the pipe to its starting position and parent GameObject
            transform.position = startingPosition;
            transform.SetParent(myParent);
            return;
        }

        // Find the tile closest to the pipe
        var currentCell = touchingTiles[0];
        if (touchingTiles.Count == 1)
        {
            newPosition = currentCell.position;
        }
        else
        {
            // Loop through list of touching tiles and find the one closest to the pipe
            var distance = Vector2.Distance(transform.position, touchingTiles[0].position);
            foreach (Transform cell in touchingTiles)
            {
                if (Vector2.Distance(transform.position, cell.position) < distance)
                {
                    currentCell = cell;
                    distance = Vector2.Distance(transform.position, cell.position);
                }
            }
            newPosition = currentCell.position; // Set the new position to closest tile position
        }
        // Obtain the PipeHandler component of the pipe on the tile and check if it's not already filled
        Transform otherPipeTransform = currentCell.GetChild(0);
        if (otherPipeTransform.GetComponent<PipeHandler>().pipeType.Type != PipeType.EMPTY)
        {
            // If the pipe is already filled, return the pipe to its starting position and parent GameObject
            transform.position = startingPosition;
            transform.SetParent(myParent);
            return;
        }
        else
        {
            // Swap the positions and parent GameObjects of the pipes on the current and target tiles
            UpdatePipeTransformsAndHandlers(otherPipeTransform, newPosition);
        }
    }

    /// <summary>
    /// Updates the positions and parent GameObjects of the pipes on the current and target tiles and updates game board data.
    /// </summary>
    /// <param name="otherPipeTransform">The transform component reference of the pipe being swapped with the current pipe.</param>
    /// <param name="newPosition">The new position vector of the current pipe.</param>
    private void UpdatePipeTransformsAndHandlers(Transform otherPipeTransform, Vector2 newPosition)
    {
        // Get PipeHandler components of current and target pipes
        var thisPipeHandler = gameObject.GetComponent<PipeHandler>();
        var otherPipeHandler = otherPipeTransform.GetComponent<PipeHandler>();

        // Change parent GameObject of current pipe and start coroutine to move it to the new position
        transform.SetParent(otherPipeTransform.parent);
        StartCoroutine(SlotIntoPlace(transform.position, newPosition, thisPipeHandler));

        // Update game board data with new location of moved pipe
        LevelData.GamePieces[thisPipeHandler.location.X, thisPipeHandler.location.Y] = otherPipeHandler;

        // Swap parent GameObjects of the pipes on the current and target tiles
        Transform otherPipeParent = otherPipeTransform.parent;
        otherPipeTransform.SetParent(myParent);
        otherPipeTransform.position = startingPosition;
        LevelData.GamePieces[otherPipeHandler.location.X, otherPipeHandler.location.Y] = thisPipeHandler;

        // Update starting position and parent GameObject of the current pipe
        startingPosition = newPosition;
        myParent = otherPipeParent;

        // Update nearby pipes of the current and target pipes that were affected by the swap
        thisPipeHandler.ProcessNearbyPipeChanges(setNearbyPipeHandlers: true);
        otherPipeHandler.ProcessNearbyPipeChanges(setNearbyPipeHandlers: true);
    }

    // OnTriggerEnter2D is called when the Collider2D component on this GameObject first makes contact with another Collider2D
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag != "Tile") return; // Exit function if trigger isn't a tile
        if (!touchingTiles.Contains(other.transform))
        {
            touchingTiles.Add(other.transform); // Add tile to list of touching tiles
        }
    }

    // OnTriggerExit2D is called when the Collider2D component on this GameObject no longer makes contact with another Collider2D
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag != "Tile") return; // Exit function if trigger isn't a tile
        if (touchingTiles.Contains(other.transform))
        {
            touchingTiles.Remove(other.transform); // Remove tile from list of touching tiles
        }
    }

    /// <summary>
    /// Coroutine to move the pipe to a new position over a short time interval.
    /// </summary>
    /// <param name="startingPos">Starting position of the pipe.</param>
    /// <param name="endingPos">Ending position of the pipe.</param>
    /// <param name="pipeHandler">The pipe handler whose tile will be activated after the coroutine completes.</param>
    /// <returns></returns>
    IEnumerator SlotIntoPlace(Vector2 startingPos, Vector2 endingPos, PipeHandler pipeHandler)
    {
        float duration = 0.1f;
        float elapsedTime = 0;
        while (elapsedTime < duration)
        {
            // Lerp the pipe position from starting position to ending position over the duration of the coroutine
            transform.position = Vector2.Lerp(startingPos, endingPos, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        transform.position = endingPos; // Set the pipe position to the new position after coroutine completes
        pipeHandler.SetActiveTileToThisGO(); // Activate the tile for the current pipe
    }
}
