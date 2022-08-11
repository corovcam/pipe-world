using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        if (!endGameMenu.active)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                PauseGame();
            }
        }
        #pragma warning restore CS0618 // Type or member is obsolete
    }

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

    void GetBackToMainMenu()
    {
        PauseGame();
        SceneHandler.LoadMainMenuScene();
    }

    void RestartGame()
    {
        PauseGame();
        levelHandler.ResetLevel();
    }
}
