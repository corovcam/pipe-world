using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Handles Game scene GUI Components, End Game Menu, Total Score calculaction 
/// and Timer and its mechanism
/// </summary>
public class GUIHandler : MonoBehaviour
{
    /// <summary>
    /// Used for Total Score calculation
    /// </summary>
    public const int MAXIMUM_SCORE = 10000;

    public GameObject endGameMenu;
    public GameObject endGameText;
    public TMP_Text totalScore;
    public LevelHandler levelHandler;

    public Button restartButton;
    public Button quitButton;
    public Button pauseButton;
    public Button skipButton;
    public Button startFlowButton;

    public TMP_Text timerText;

    // After the Flow starts EndGame starts
    public static bool IsEndGame { get; set; } = false;

    public bool isDebug;
    [SerializeField]
    [Range(5, 30)]
    int defaultTimeLimit = 20; // To edit in Editor
    int currentTime;

    GameManager gm;

    void Awake()
    {
        gm = GetComponent<GameManager>();
        restartButton.onClick.AddListener(RestartGame);
        quitButton.onClick.AddListener(GetBackToMainMenu);
        skipButton.onClick.AddListener(AccelerateFlow);
        startFlowButton.onClick.AddListener(gm.StartFlow);
    }

    void Start()
    {
        if (isDebug)
            LevelData.TimeLimit = defaultTimeLimit;
        timerText.text = LevelData.TimeLimit.ToString();
        StartCoroutine("CountdownTimer");
    }

    /// <summary>
    /// Coroutine that starts counting down the Timer every second until it reaches 0 
    /// after which it's Game Over
    /// </summary>
    IEnumerator CountdownTimer()
    {
        currentTime = LevelData.TimeLimit;
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
        startFlowButton.onClick.RemoveListener(gm.StartFlow);
    }

    /// <summary>
    /// A popup GUI that shows the End Game menu with the Total Score, Restart and Quit buttons
    /// </summary>
    /// <param name="isWon">Used to determine if the player won or lost the game. 
    /// The End Game Menu changes accordingly.</param>
    public void ShowEndGameMenu(bool isWon)
    {
        pauseButton.enabled = false;
        skipButton.gameObject.SetActive(false);
        // Pauses the game to prevent GUI interaction and player Input
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
            totalScore.text = "0"; // If the player looses the remaining Timer is unnecessary
        }

        endGameMenu.SetActive(true);
    }

    /// <summary>
    /// Resumes the Game, restores defaults, restarts and shuffles the current level 
    /// </summary>
    void RestartGame()
    {
        if (PauseControl.GameIsPaused) // Resumes the game
        {
            PauseControl.GameIsPaused = false;
            Time.timeScale = 1;
        }
        endGameMenu.SetActive(false);

        IsEndGame = false;
        pauseButton.enabled = true;
        startFlowButton.enabled = true;
        levelHandler.ResetLevel();
        timerText.text = LevelData.TimeLimit.ToString();
        StartCoroutine("CountdownTimer");
    }

    /// <summary>
    /// Resumes the Game, restores defaults and changes scene to MainMenu
    /// </summary>
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

    /// <summary>
    /// Setup the EndGame routine and sets IsEndGame
    /// </summary>
    public void SetEndGameScene()
    {
        IsEndGame = true;
        StopCoroutine("CountdownTimer");
        skipButton.gameObject.SetActive(true);
        startFlowButton.enabled = false;
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
