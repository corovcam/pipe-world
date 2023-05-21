using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Manages core water flowing mechanic after Flow start is triggered
/// </summary>
public class GameManager : MonoBehaviour
{
    Queue<PipeHandler> queue;
    HashSet<PipeHandler> visited; // Used to determine if the given Pipe has been visited yet or not
    Dictionary<Position, int> distances; // Used to distinguish different waves for animation
    bool flowsStarted;
    bool isWon;
    List<bool> flowsFinished;

    LevelHandler lh;
    GUIHandler GUIHandler;

    void Start()
    {
        lh = GameObject.FindObjectOfType<LevelHandler>();
        GUIHandler = GetComponent<GUIHandler>();
        isWon = false;
        flowsStarted = false;
        flowsFinished = new List<bool>();
    }

    void Update()
    {
        if (flowsStarted && flowsFinished.All(flowFinished => flowFinished))
        {
            flowsStarted = false;
            bool waterAtStart = LevelData.Starts.ContainsKey(Liquid.Water);
            bool lavaAtStart = LevelData.Starts.ContainsKey(Liquid.Lava);
            isWon = true;

            if (waterAtStart)
                if (LevelData.Ends.ContainsKey(Liquid.Water))
                    foreach (var pos in LevelData.Ends[Liquid.Water])
                    {
                        var pipe = LevelData.GamePieces[pos.X, pos.Y];
                        isWon = visited.Contains(pipe);
                        if (!isWon) break;
                    }

            if (!isWon) 
                GUIHandler.ShowEndGameMenu(isWon);
            else
            {
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
    /// Starts the flow of water from the StartPipe location
    /// </summary>
    public void StartFlow()
    {
        flowsStarted = true;
        isWon = false;
        flowsFinished = new List<bool>();
        distances = new Dictionary<Position, int>();
        queue = new Queue<PipeHandler>();
        visited = new HashSet<PipeHandler>();

        GUIHandler.SetEndGameScene();

        bool waterAtStart = LevelData.Starts.ContainsKey(Liquid.Water);
        bool lavaAtStart = LevelData.Starts.ContainsKey(Liquid.Lava);

        int flowIndex = 0;
        if (waterAtStart)
            LevelData.Starts[Liquid.Water].ForEach(start => {
                flowsFinished.Add(false);
                StartCoroutine(Flow(start, Liquid.Water, flowIndex));
                flowIndex++;
            });
        if (lavaAtStart)
            LevelData.Starts[Liquid.Lava].ForEach(start => {
                flowsFinished.Add(false);
                StartCoroutine(Flow(start, Liquid.Lava, flowIndex));
                flowIndex++;
            });
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


            //// If we filled/visited the EndPipe then mark it and continue filling
            //if (LevelData.Ends.ContainsKey(flowLiquid))
            //    if (LevelData.Ends[flowLiquid].Exists(pos => pos == current.location))
            //        isWon = true;

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