using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneHandler
{
    public static void LoadMainMenuScene()
    {
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }

    public static void LoadLevelSelectScene()
    {
        SceneManager.LoadScene("LevelSelect", LoadSceneMode.Single);
    }

    public static void LoadArcadeGameScene()
    {
        GUIHandler.IsEndGame = false;
        PauseControl.GameIsPaused = false;
        LevelData.IsArcadeMode = true;
        LevelData.BoardSize = 10;
        LevelData.TimeLimit = 30;
        SceneManager.LoadScene("Game", LoadSceneMode.Single);
    }

    public static void LoadLevel(int levelNumber)
    {
        GUIHandler.IsEndGame = false;
        PauseControl.GameIsPaused = false;
        LevelData.IsArcadeMode = false;
        LevelData.LevelNumber = levelNumber;
        LevelData.BoardSize = 10;
        LevelData.TimeLimit = 20;
        SceneManager.LoadScene("Game", LoadSceneMode.Single);
    }
}
