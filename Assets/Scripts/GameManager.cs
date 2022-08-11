using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    Queue<PipeHandler> queue;
    HashSet<PipeHandler> visited;
    Dictionary<Position, int> distances;
    bool isWon = false;

    PipeHandler startPipe;
    LevelHandler levelHandler;
    GUIHandler GUIHandler;

    // Start is called before the first frame update
    void Start()
    {
        levelHandler = GameObject.FindObjectOfType<LevelHandler>();
        GUIHandler = GetComponent<GUIHandler>();
        startPipe = LevelData.GamePieces[LevelData.StartPipe.X, LevelData.StartPipe.Y].GetComponent<PipeHandler>();
    }

    public void StartFlow()
    {
        isWon = false;
        distances = new Dictionary<Position, int>();
        queue = new Queue<PipeHandler>();
        visited = new HashSet<PipeHandler>();

        StartCoroutine(Flow());
    }

    IEnumerator Flow()
    {
        GUIHandler.SetEndGameScene();

        PipeHandler previousPipe = startPipe;
        distances[LevelData.StartPipe] = 0;
        visited.Add(startPipe);
        queue.Enqueue(startPipe);
        while (queue.Count != 0)
        {
            var current = queue.Dequeue();

            if (distances[previousPipe.location] < distances[current.location])
                yield return new WaitForSeconds(0.5f);

            current.GetComponent<SpriteRenderer>().sprite = levelHandler.filledPipeSprites[current.tileType];

            if (current.location == LevelData.EndPipe)
            {
                isWon = true;
            }

            for (int dir = 0; dir < current.IODirs.Length; dir++)
            {
                if (current.IODirs[dir])
                {
                    switch (dir)
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