using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Manages core water flowing mechanic after Flow start is triggered
/// </summary>
public class GameManager : MonoBehaviour
{
    // Queue of PipeHandlers for tracking the pipes that need to be processed during flow.
    Queue<PipeHandler> queue;

    // Set of visited PipeHandlers to prevent processing of the same PipeHandler multiple times.
    HashSet<PipeHandler> visited;

    // Dictionary of positions to integer distances, used for distinguishing different waves of animation.
    Dictionary<Position, int> distances;

    // Boolean flags to track if flow has started and if a win condition has been reached.
    bool flowsStarted;
    bool isWon;

    // List of boolean flags to keep track of whether each flow has finished.
    List<bool> flowsFinished;

    // The LevelHandler for the current level.
    LevelHandler lh;

    // The GUIHandler for the current level.
    GUIHandler GUIHandler;

    void Start()
    {
        lh = GameObject.FindObjectOfType<LevelHandler>();
        GUIHandler = GetComponent<GUIHandler>();
        isWon = false;
        flowsStarted = false;
        flowsFinished = new List<bool>();
    }

    /// <summary>
    /// Updates the GameManager's state and checks for win conditions.
    /// </summary>
    public void Update()
    {
        // If all flows have started and finished,
        if (flowsStarted && flowsFinished.All(flowFinished => flowFinished))
        {
            // Set flag and check for liquid at level start and end points.
            flowsStarted = false;
            bool waterAtStart = LevelData.Starts.ContainsKey(Liquid.Water);
            bool lavaAtStart = LevelData.Starts.ContainsKey(Liquid.Lava);
            isWon = true;

            // If water is at the start and end points.
            if (waterAtStart)
                if (LevelData.Ends.ContainsKey(Liquid.Water))
                    foreach (var pos in LevelData.Ends[Liquid.Water])
                    {
                        var pipe = LevelData.GamePieces[pos.X, pos.Y];
                        isWon = visited.Contains(pipe);
                        if (!isWon) break;
                    }

            // If water is not won, show endgame menu.
            if (!isWon)
                GUIHandler.ShowEndGameMenu(isWon);
            else
            {
                // If lava is at the start and end points.
                if (lavaAtStart)
                    if (LevelData.Ends.ContainsKey(Liquid.Lava))
                        foreach (var pos in LevelData.Ends[Liquid.Lava])
                        {
                            var pipe = LevelData.GamePieces[pos.X, pos.Y];
                            isWon = visited.Contains(pipe);
                            if (!isWon) break;
                        }
                GUIHandler.ShowEndGameMenu(isWon);
            }
        }
    }

    /// <summary>
    /// StartFlow function initializes required variables to start a liquid (water/lava) flow and starts a coroutine for each instantiation of a liquid drop/
    /// </summary>
    public void StartFlow()
    {
        // Reset boolean flags
        flowsStarted = true;
        isWon = false;

        // Initialize data structures for flow
        flowsFinished = new List<bool>();
        distances = new Dictionary<Position, int>();
        queue = new Queue<PipeHandler>();
        visited = new HashSet<PipeHandler>();

        // Set end game scene and store game pieces
        GUIHandler.SetEndGameScene();
        lh.StoreGamePieces();

        // Check if starting point has water or lava
        bool waterAtStart = LevelData.Starts.ContainsKey(Liquid.Water);
        bool lavaAtStart = LevelData.Starts.ContainsKey(Liquid.Lava);

        // Counter to help distinguish different instances of liquid flows.
        int flowIndex = 0;

        // If water is at the starting point, instantiate water
        if (waterAtStart)
        {
            // For each starting point - start a new coroutine and keep track of flowsFinished status
            LevelData.Starts[Liquid.Water].ForEach(start =>
            {
                flowsFinished.Add(false);
                StartCoroutine(Flow(start, Liquid.Water, flowIndex));
                flowIndex++;
            });
        }

        // If lava is at the starting point, instantiate lava
        if (lavaAtStart)
        {
            // For each starting point - start a new coroutine and keep track of flowsFinished status
            LevelData.Starts[Liquid.Lava].ForEach(start =>
            {
                flowsFinished.Add(false);
                StartCoroutine(Flow(start, Liquid.Lava, flowIndex));
                flowIndex++;
            });
        }
    }

    /// <summary>
    /// Used in Coroutine that uses BFS Traversal to fill all connected pipes from the StartPipe to
    /// the EndPipe location and checks if there is such a path
    /// </summary>
    IEnumerator Flow(Position startPos, Liquid flowLiquid, int flowIndex)
    {
        PipeHandler startPipe = LevelData.GamePieces[startPos.X, startPos.Y];
        PipeHandler previousPipe = startPipe;
        distances[startPipe.location] = 0;
        visited.Add(startPipe);
        queue.Enqueue(startPipe);

        // Loops the queue until all connected pipes have been visited
        while (queue.Count != 0)
        {
            var current = queue.Dequeue();

            // If the previous pipe is 1 level below current, then the whole level
            // has been visited and wait for next wave (used for wave animation)
            if (distances[previousPipe.location] < distances[current.location])
                if (flowLiquid == Liquid.Water)
                    yield return new WaitForSeconds(0.5f);
                else
                    yield return new WaitForSeconds(1f);

            Pipe currentType = current.pipeType;
            // Set filled Blue-Green/Red-Grey sprites for Start/End sprites
            if (current.location == startPipe.location || LevelData.Ends[flowLiquid].Exists(pos => pos == current.location))
            {
                var chosenStartEndSprite = currentType.Liquid == Liquid.Water ?
                    lh.filledBlueGreenPipeSprites[(int)currentType.Type] : lh.filledRedGreyPipeSprites[(int)currentType.Type];
                current.GetComponent<SpriteRenderer>().sprite = chosenStartEndSprite;
            }
            else // Otherwise set filled green/grey sprites
            {
                var chosenSprite = currentType.Liquid == Liquid.Water ?
                    lh.filledGreenPipeSprites[(int)currentType.Type] : lh.filledGreyPipeSprites[(int)currentType.Type];
                current.GetComponent<SpriteRenderer>().sprite = chosenSprite;
            }

            // Loop through each IO Dir of the Pipe
            for (int dir = 0; dir < current.IODirs.Length; dir++)
            {
                if (current.IODirs[dir]) // If the IO Port is available
                {
                    switch (dir) // Check the direction and continue there
                    {
                        case (int)Dir.UP:
                            // Check if we can move the water to the UP Pipe, check if it hasn't been already visited and 
                            // also the the liquid type must be the same, otherwise the liquid doesn't move through
                            if (current.upFree && !visited.Contains(current.up) && currentType.Liquid == current.up.pipeType.Liquid)
                            {
                                previousPipe = current; // Remember the previous Pipe for wave animation
                                distances[current.up.location] = distances[current.location] + 1;
                                visited.Add(current.up);
                                queue.Enqueue(current.up);
                            }
                            break;
                        case (int)Dir.RIGHT:
                            if (current.rightFree && !visited.Contains(current.right) && currentType.Liquid == current.right.pipeType.Liquid)
                            {
                                previousPipe = current;
                                distances[current.right.location] = distances[current.location] + 1;
                                visited.Add(current.right);
                                queue.Enqueue(current.right);
                            }
                            break;
                        case (int)Dir.DOWN:
                            if (current.downFree && !visited.Contains(current.down) && currentType.Liquid == current.down.pipeType.Liquid)
                            {
                                previousPipe = current;
                                distances[current.down.location] = distances[current.location] + 1;
                                visited.Add(current.down);
                                queue.Enqueue(current.down);
                            }
                            break;
                        case (int)Dir.LEFT:
                            if (current.leftFree && !visited.Contains(current.left) && currentType.Liquid == current.left.pipeType.Liquid)
                            {
                                previousPipe = current;
                                distances[current.left.location] = distances[current.location] + 1;
                                visited.Add(current.left);
                                queue.Enqueue(current.left);
                            }
                            break;
                    }
                }
            }
        }

        flowsFinished[flowIndex] = true;
    }
}