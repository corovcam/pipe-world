using System;
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
        
        LevelData.lvlData = Resources.Load<TextAsset>("Level" + levelNumber.ToString()).text.Split(Environment.NewLine);
        LevelData.BoardSize = int.Parse(LevelData.lvlData[0]);
        LevelData.TimeLimit = int.Parse(LevelData.lvlData[1]);
        SceneManager.LoadScene("Game", LoadSceneMode.Single);
    }
}
