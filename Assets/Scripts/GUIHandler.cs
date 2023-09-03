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
    /// Used for Total Score calculation, can be changed for custom calculations
    /// </summary>
    public const int MAXIMUM_SCORE = 10000;

    public GameObject endGameMenu;
    public GameObject endGameText;
    public TMP_Text totalScore;
    public LevelHandler levelHandler;

    public Button restartButton;
    public Button quitButton;
    public Button nextLevelButton;
    public Button pauseButton;
    public Button skipButton;
    public Button startFlowButton;

    public TMP_Text timerText;

    // After the Flow starts EndGame starts
    public static bool IsEndGame { get; set; } = false;

    public bool isDebug;
    [SerializeField]
    [Range(5, 30)]
    int defaultTimeLimit = 20; // To be edited in Editor
    int currentTime;

    GameManager gm;

    void Awake()
    {
        gm = GetComponent<GameManager>();
        restartButton.onClick.AddListener(RestartGame);
        quitButton.onClick.AddListener(GetBackToMainMenu);
        nextLevelButton.onClick.AddListener(GoToNextLevel);
        skipButton.onClick.AddListener(AccelerateFlow);
        startFlowButton.onClick.AddListener(gm.StartFlow);
    }

    void Start()
    {
        if (isDebug)
            LevelData.TimeLimit = defaultTimeLimit;

        bool isLastLevel = LevelData.LevelNumber == 
            (LevelData.IsFreeWorldMode ? SceneHandler.FreeWorldLevelCount : SceneHandler.LevelSelectLevelCount);
        if (LevelData.IsArcadeMode || isLastLevel)
            nextLevelButton.gameObject.SetActive(false);

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
    /// A popup GUI that shows the End Game menu with the Total Score, Restart, Quit and Next Level buttons
    /// </summary>
    /// <param name="isWon">Used to determine if the player won or lost the game. 
    /// The End Game Menu changes accordingly.</param>
    public void ShowEndGameMenu(bool isWon)
    {
        gm.StopAllCoroutines();
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
    public void RestartGame()
    {
        if (PauseControl.GameIsPaused) // Resumes the game
        {
            PauseControl.GameIsPaused = false;
            Time.timeScale = 1;
        }
        endGameMenu.SetActive(false);

        IsEndGame = false;
        skipButton.gameObject.SetActive(false);
        pauseButton.enabled = true;
        startFlowButton.enabled = true;
        levelHandler.ResetLevel();
        timerText.text = LevelData.TimeLimit.ToString();

        if (LevelData.IsFreeWorldMode)
        {
            SceneHandler.LoadLevel(LevelData.LevelNumber);
        }

        StartCoroutine("CountdownTimer"); // Starts the CountdownTimer coroutine for the new game
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
    /// Navigation to the next level
    /// </summary>
    void GoToNextLevel()
    {
        if (PauseControl.GameIsPaused)
        {
            PauseControl.GameIsPaused = false;
            Time.timeScale = 1;
        }
        endGameMenu.SetActive(false);

        SceneHandler.LoadLevel(LevelData.LevelNumber + 1);
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

    /// <summary>
    /// Basic Total Score calculation using set maximum metric
    /// </summary>
    /// <returns>Total Score string</returns>
    string CalculateTotalScore()
    {
        // Minimum Score: 0
        // Maximum Score: 10000
        double weight = LevelData.IsArcadeMode ? 1.0 : LevelSelectHandler.MaxTimeLimit / (double)LevelData.TimeLimit;
        double notNormalizedScore = currentTime / (double)LevelData.TimeLimit / weight;
        int score = Mathf.RoundToInt((float)(notNormalizedScore * MAXIMUM_SCORE));
        return score.ToString();
    }

    /// <summary>
    /// Used for flow acceleration using the "Skip" button
    /// </summary>
    void AccelerateFlow()
    {
        Time.timeScale = 4;
    }
}
