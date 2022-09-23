using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages core water flowing mechanic after Flow start is triggered
/// </summary>
public class GameManager : MonoBehaviour
{
    Queue<PipeHandler> queue;
    HashSet<PipeHandler> visited; // Used to determine if the given Pipe has been visited yet or not
    Dictionary<Position, int> distances; // Used to distinguish different waves for animation
    bool isWon = false;

    PipeHandler startPipe;
    LevelHandler lh;
    GUIHandler GUIHandler;

    void Start()
    {
        lh = GameObject.FindObjectOfType<LevelHandler>();
        GUIHandler = GetComponent<GUIHandler>();
        startPipe = LevelData.GamePieces[LevelData.StartPipe.X, LevelData.StartPipe.Y]
            .GetComponent<PipeHandler>();
    }

    /// <summary>
    /// Starts the flow of water from the StartPipe location
    /// </summary>
    public void StartFlow()
    {
        isWon = false;
        distances = new Dictionary<Position, int>();
        queue = new Queue<PipeHandler>();
        visited = new HashSet<PipeHandler>();

        StartCoroutine("Flow");
    }

    /// <summary>
    /// Used in Couroutine that uses BFS Traversal to fill all connected pipes from the StartPipe to
    /// the EndPipe location and checks if there is such a path
    /// </summary>
    IEnumerator Flow()
    {
        GUIHandler.SetEndGameScene();

        PipeHandler previousPipe = startPipe;
        distances[LevelData.StartPipe] = 0;
        visited.Add(startPipe);
        queue.Enqueue(startPipe);

        // Loops the queue until all connected pipes have been visited
        while (queue.Count != 0)
        {
            var current = queue.Dequeue();

            // If the previous pipe is 1 level below current, then the whole level
            // has been visited and wait for next wave (used for wave animation)
            if (distances[previousPipe.location] < distances[current.location])
                yield return new WaitForSeconds(0.5f);

            Pipe currentType = current.pipeType;
            // Set filled Blue-Green/Red-Grey sprites for Start/End sprites
            if (current.location == LevelData.StartPipe || current.location == LevelData.EndPipe)
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


            // If we filled/visited the EndPipe then mark it and continue filling
            if (current.location == LevelData.EndPipe)
                isWon = true;

            // Loop through each IO Dir of the Pipe
            for (int dir = 0; dir < current.IODirs.Length; dir++)
            {
                if (current.IODirs[dir]) // If the IO Port is available
                {
                    switch (dir) // Check the direction and continue there
                    {
                        case (int)Dir.UP:
                            // Check if we can move the water to the UP Pipe and check if it hasn't
                            // been already visited
                            if (current.upFree && !visited.Contains(current.up))
                            {
                                previousPipe = current; // Remember the previous Pipe for wave animation
                                distances[current.up.location] = distances[current.location] + 1;
                                visited.Add(current.up);
                                queue.Enqueue(current.up);
                            }
                            break;
                        case (int)Dir.RIGHT:
                            if (current.rightFree && !visited.Contains(current.right))
                            {
                                previousPipe = current;
                                distances[current.right.location] = distances[current.location] + 1;
                                visited.Add(current.right);
                                queue.Enqueue(current.right);
                            }
                            break;
                        case (int)Dir.DOWN:
                            if (current.downFree && !visited.Contains(current.down))
                            {
                                previousPipe = current;
                                distances[current.down.location] = distances[current.location] + 1;
                                visited.Add(current.down);
                                queue.Enqueue(current.down);
                            }
                            break;
                        case (int)Dir.LEFT:
                            if (current.leftFree && !visited.Contains(current.left))
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

        //Debug.Log(isWon);
        GUIHandler.ShowEndGameMenu(isWon); // The Flow is terminated
    }
}