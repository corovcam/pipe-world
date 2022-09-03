using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Handles Pause mechanic in the game as well as Pause menu and buttons
/// </summary>
public class PauseControl : MonoBehaviour
{
    public GameObject pauseMenu;
    public LevelHandler levelHandler;

    public Toggle toggle;
    public GameObject toggleImgGO;

    public Sprite toggleImgOnSprite;
    public Sprite toggleImgOffSprite;

    public Button restartBtn;
    public Button quitBtn;
    public Button resumeBtn;
    public Button pauseBtn;

    public GameObject endGameMenu;

    Image switchImage;

    public static bool GameIsPaused { get; set; }

    // Fired at the very start of script loading
    void Awake()
    {
        switchImage = toggleImgGO.GetComponent<Image>();
        toggle.onValueChanged.AddListener(OnSwitchToggle);
        if (toggle.isOn)
            OnSwitchToggle(isAudioOn: true);

        restartBtn.onClick.AddListener(RestartGame);
        quitBtn.onClick.AddListener(GetBackToMainMenu);
        resumeBtn.onClick.AddListener(PauseGame);
        pauseBtn.onClick.AddListener(PauseGame);
    }

    void Update()
    {
        #pragma warning disable CS0618 // Type or member is obsolete
        // Determine if the endGameMenu is visible, if not then the game can be paused
        if (!endGameMenu.active)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                PauseGame();
            }
        }
        #pragma warning restore CS0618 // Type or member is obsolete
    }

    /// <summary>
    /// Default pausing function that freezes all interactions, sets/unsets GameIsPaused flag
    /// and makes the PauseMenu visible
    /// </summary>
    public void PauseGame()
    {
        GameIsPaused = !GameIsPaused;

        if (GameIsPaused)
        {
            Time.timeScale = 0f;
            pauseBtn.enabled = false;
            pauseMenu.SetActive(true);
        }
        else
        {
            Time.timeScale = 1;
            pauseBtn.enabled = true;
            pauseMenu.SetActive(false);
        }
    }

    /// <summary>
    /// Delegate that is fired when the Pause Toggle in PauseMenu is clicked on
    /// </summary>
    /// <param name="isAudioOn">The toggle value</param>
    void OnSwitchToggle(bool isAudioOn)
    {
        if (isAudioOn)
        {
            AudioListener.pause = !isAudioOn;
            switchImage.sprite = toggleImgOnSprite;
        }
        else
        {
            AudioListener.pause = !isAudioOn;
            switchImage.sprite = toggleImgOffSprite;
        }
    }

    void OnDestroy()
    {
        toggle.onValueChanged.RemoveListener(OnSwitchToggle);
        restartBtn.onClick.RemoveListener(RestartGame);
        quitBtn.onClick.RemoveListener(GetBackToMainMenu);
        resumeBtn.onClick.RemoveListener(PauseGame);
        pauseBtn.onClick.RemoveListener(PauseGame);
    }

    /// <summary>
    /// Get back to Main Menu from the Pause Menu screen
    /// </summary>
    void GetBackToMainMenu()
    {
        PauseGame(); // The game is paused, so it needs to be resumed again
        SceneHandler.LoadMainMenuScene();
    }

    /// <summary>
    /// Restart game from the Pause Menu screen
    /// </summary>
    void RestartGame()
    {
        PauseGame();
        levelHandler.ResetLevel();
    }
}
