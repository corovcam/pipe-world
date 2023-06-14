---
layout: default
---

## [GameManager.cs](https://github.com/corovcam/pipe-world/blob/main/Assets/Scripts/GameManager.cs)

The `GameManager` class in `pipe-world` is responsible for managing the core water and lava flow mechanics in the game. It tracks pipes that need to be processed during flow and checks for win conditions. The class uses queues, dictionaries, and hashsets to manage the flow and to prevent processing the same pipe multiple times.

The `Start()` method initializes the necessary variables, including the LevelHandler (`lh`) and the GUIHandler. It also sets the initial values for the `isWon` and `flowsStarted` flags.

The `Update()` method checks if all flows have started and finished, then evaluates win conditions. If water or lava reaches the correct endpoint, the game is won, and the endgame menu is shown.

The `StartFlow()` method initializes the required variables for starting a liquid flow (water or lava) and starts a coroutine for each liquid drop. Depending on the starting point, it instantiates water or lava flows and adds them to the `flowsFinished` list.

The `Flow()` function is used in a coroutine that uses Breadth-First Search (BFS) traversal to fill all connected pipes from the start pipe to the end pipe location and checks if there is such a path. The function takes the starting position, liquid flow type, and flow index as arguments. The function initializes the distances and visited dictionaries, sets the initial values of distances and visited, and enqueues the start pipe into the queue. The function uses a while loop to dequeue pipes from the queue until all connected pipes have been visited. The function sets the filled sprites for the start and end sprites and the filled green/grey sprites for the other pipes. The function loops through each IO direction of the pipe and checks if the IO port is available. If the IO port is available, the function checks the direction and continues to move the liquid in that direction. If the liquid can be moved to the next pipe, the functionupdates the previousPipe for wave animation, sets the distance of the next pipe, adds it to the visited set, and enqueues it into the queue. Once all connected pipes have been visited, the function sets the flowsFinished flag to true for the corresponding flow index.

Example of starting a flow:

```csharp
GameManager gameManager = GetComponent<GameManager>();
gameManager.StartFlow();
```

The `GameManager` class is essential to the `Pipe World` game as it manages the core logic for the game, including the flow of water and lava, as well as win conditions.

### Questions & Answers

1. **What are the win conditions for the game?**

   The win condition is met when all flows (water and/or lava) have started and finished, and have reached their respective end points without any interruptions or mismatches in liquid type.

2. **How does the GameManager handle multiple instances of liquid (water/lava) flows?**

   The GameManager maintains separate coroutines for each instance of liquid flow, keeping track of their finished status in a `flowsFinished` list and using a `flowIndex` counter to distinguish between different instances.

3. **What is the purpose of the `distances` dictionary in the GameManager class?**

   The `distances` dictionary is used to store the integer distances of each visited pipe from the starting position. It is primarily used to distinguish different waves of animation during the flow of liquid through the pipes.