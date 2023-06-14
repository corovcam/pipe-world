---
layout: default
---

## [GUIHandler.cs](https://github.com/corovcam/pipe-world/blob/main/Assets/Scripts/GUIHandler.cs)

`GUIHandler` handles the GUI elements and logic for the pipe-world game. It displays timers, scores, end game menus, and has buttons to control game flow.

The `CountdownTimer` coroutine decreases a timer by 1 second intervals until it reaches 0, at which point it shows an end game menu. This menu displays if the player won or lost, shows their total score, and has buttons to restart the level or quit to the main menu. 

The `CalculateTotalScore` method determines a score between 0 to 10,000 based on the time left and level difficulty. This score is shown in the end game menu.

The `RestartGame` method restarts the level, resets variables, and restarts the timer. The `GetBackToMainMenu` method loads the main menu scene.

The `SetEndGameScene` method prepares the end game menu to be shown. The `AccelerateFlow` method speeds up the game when the "skip" button is pressed.

This script works with the `LevelHandler` script to control level generation and logic. It pairs timers, scoring, and end game menus with the core level gameplay. The skip button could be used for gameplay testing.

Overall this script provides essential GUI and gameplay control for the pipe-world game. It displays feedback to the player, allows level restarting and menu navigation, and can accelerate gameplay if needed. This script works cohesively with `LevelHandler` and `PauseControl` to form the full gameplay experience.


1. **What other scripts does this GUIHandler script interact with?**

    This script interacts with the LevelHandler, GameManager, PauseControl, and SceneHandler scripts.

2. **What happens if the timer reaches 0?**

    If the timer reaches 0, the ShowEndGameMenu method is called with isWon set to false. This shows the end game menu and displays a "You Lost" message, setting the total score to 0.

3. **Why is the MAXIMUM_SCORE constant used?**

    The MAXIMUM_SCORE constant is used in the CalculateTotalScore method to determine the player's score. It calculates a percentage of the maximum score based on the time remaining, so MAXIMUM_SCORE represents the total possible points. A higher score is earned by completing the level with more time remaining.