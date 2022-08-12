using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages core water flowing mechanic after Flow start
/// </summary>
public class GameManager : MonoBehaviour
{
    Queue<PipeHandler> queue;
    HashSet<PipeHandler> visited;
    Dictionary<Position, int> distances;
    bool isWon = false;

    PipeHandler startPipe;
    LevelHandler levelHandler;
    GUIHandler GUIHandler;

    void Start()
    {
        levelHandler = GameObject.FindObjectOfType<LevelHandler>();
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

        StartCoroutine(Flow());
    }

    /// <summary>
    /// Couroutine that uses BFS Traversal to fill all connected pipes from the StartPipe to
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

            // Set red pipe sprites for start/end pipes
            if (current.location == LevelData.StartPipe || current.location == LevelData.EndPipe)
            {
                current.GetComponent<SpriteRenderer>().sprite 
                    = levelHandler.filledRedPipeSprites[current.tileType];
            }
            else // Otherwise set green sprites
            {
                current.GetComponent<SpriteRenderer>().sprite
                    = levelHandler.filledPipeSprites[current.tileType];
            }

            // If we filled/visited the EndPipe then mark it and continue filling
            if (current.location == LevelData.EndPipe)
                isWon = true;

            for (int dir = 0; dir < current.IODirs.Length; dir++)
            {
                if (current.IODirs[dir]) // If the IO Port is available
                {
                    switch (dir) // Check the direction and continue there
                    {
                        case (int)Dir.UP:
                            if (current.upFree && !visited.Contains(current.up))
                            {
                                previousPipe = current;
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

        Debug.Log(isWon);
        GUIHandler.ShowEndGameMenu(isWon);
    }
}