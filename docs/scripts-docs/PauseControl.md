---
layout: default
---

## [PauseControl.cs](https://github.com/corovcam/pipe-world/blob/main/Assets/Scripts/PauseControl.cs)

The `PauseControl` class is responsible for handling the pausing mechanic in the game, as well as the pause menu and buttons. The class contains various GUI components such as the pause menu, help dialog, toggle button, and several buttons for restarting, quitting, and resuming the game. 

The `PauseGame()` method is the primary function that freezes all interactions and sets the `GameIsPaused` public static flag. It makes the pause menu visible if the game is paused, and resumes the game if it is unpaused. 

The `OnSwitchToggle()` method is a delegate that fires when the pause toggle in the pause menu is clicked on. It takes the toggle value as a parameter and toggles the audio listener on and off accordingly. 

The `ShowHelpDialog()` method shows the help dialog when the help button is clicked on. 

The `GetBackToMainMenu()` method is used to get back to the main menu from the pause menu screen, and the `RestartGame()` method is used to restart the game from the pause menu screen. 

This class is used in conjunction with the `GUIHandler` class to manage the GUI components of the game scene. Together, they provide the player with the necessary options and mechanics to pause, restart, and quit the game, as well as adjust the audio settings and access the help dialog. 

Here's an example of how to use the `PauseControl` class to pause the game when the "Escape" key is pressed:

```csharp
void Update()
{
    if (Input.GetKeyDown(KeyCode.Escape))
    {
        PauseControl.PauseGame();
    }
}
``` 

### Questions & Answers

1. **What happens if the endGameMenu is showing and the player presses the pause button?**

    The game will not pause since pausing is disabled when the endGameMenu is active.

2. **Why is the AudioListener pause state controlled through a toggle?**

    To give the player the option to mute or unmute the audio from the pause menu.

3. **What other scripts does this PauseControl script interact with?**

    It interacts with GUIHandler, LevelHandler, and GameManager.