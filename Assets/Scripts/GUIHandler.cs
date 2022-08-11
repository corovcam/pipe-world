using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GUIHandler : MonoBehaviour
{
    public const int MAXIMUM_SCORE = 10000;

    public GameObject endGameMenu;
    public GameObject endGameText;
    public TMP_Text totalScore;
    public LevelHandler levelHandler;

    public Button restartButton;
    public Button quitButton;
    public Button pauseButton;
    public Button skipButton;

    public TMP_Text timerText;

    public static bool IsEndGame { get; set; } = false;

    [SerializeField]
    [Range(5, 30)]
    int defaultTimeLimit = 20;
    int currentTime;

    void Awake()
    {
        restartButton.onClick.AddListener(RestartGame);
        quitButton.onClick.AddListener(GetBackToMainMenu);
        skipButton.onClick.AddListener(AccelerateFlow);
    }

    void Start()
    {
        timerText.text = defaultTimeLimit.ToString();
        StartCoroutine("CountdownTimer");
    }

    IEnumerator CountdownTimer()
    {
        currentTime = defaultTimeLimit;
        while (currentTime != 0)
        {
            yield return new WaitForSeconds(1);
            currentTime -= 1;
            timerText.text = currentTime.ToString();
        }
        ShowEndGameMenu(isWon: false);
        StopCoroutine("CountdownTimer");
    }

    void OnDestroy()
    {
        restartButton.onClick.RemoveListener(RestartGame);
        quitButton.onClick.RemoveListener(GetBackToMainMenu);
        skipButton.onClick.RemoveListener(AccelerateFlow);
    }

    public void ShowEndGameMenu(bool isWon)
    {
        pauseButton.enabled = false;
        skipButton.gameObject.SetActive(false);
        Time.timeScale = 0f;
        PauseControl.GameIsPaused = true;

        if (isWon)
        {
            endGameText.name = "You Won";
            endGameText.GetComponent<TMP_Text>().text = "YOU WON!";
            totalScore.text = CalculateTotalScore();
            levelHandler.PlayWinningAudio();
        }
        else
        {
            endGameText.name = "You Lost";
            endGameText.GetComponent<TMP_Text>().text = "YOU LOST!";
            totalScore.text = "0";
        }

        endGameMenu.SetActive(true);
    }

    void RestartGame()
    {
        if (PauseControl.GameIsPaused)
        {
            PauseControl.GameIsPaused = false;
            Time.timeScale = 1;
        }
        endGameMenu.SetActive(false);

        IsEndGame = false;
        pauseButton.enabled = true;
        levelHandler.ResetLevel();
        timerText.text = defaultTimeLimit.ToString();
        StartCoroutine("CountdownTimer");
    }

    void GetBackToMainMenu()
    {
        if (PauseControl.GameIsPaused)
        {
            PauseControl.GameIsPaused = false;
            Time.timeScale = 1;
        }
        endGameMenu.SetActive(false);

        SceneHandler.LoadMainMenuScene();
    }

    public void SetEndGameScene()
    {
        IsEndGame = true;
        StopCoroutine("CountdownTimer");
        skipButton.gameObject.SetActive(true);
    }

    string CalculateTotalScore()
    {
        // Minimum Score: 0
        // Maximum Score: 10000
        int score = Mathf.RoundToInt(currentTime / (float)defaultTimeLimit * MAXIMUM_SCORE);
        return score.ToString();
    }

    void AccelerateFlow()
    {
        Time.timeScale = 4;
    }
}
