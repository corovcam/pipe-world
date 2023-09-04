using System;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Used to handle the load of new Scenes in the game and to configure the static default values.
/// </summary>
public static class SceneHandler
{
    public static int LevelSelectLevelCount = Resources.LoadAll("LevelSelectLevels", typeof(TextAsset)).Length;
    public static int FreeWorldLevelCount = Resources.LoadAll("FreeWorldLevels", typeof(TextAsset)).Length;
    static bool[] tutorialsPlayed = new bool[3];

    public static void LoadMainMenuScene()
    {
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }

    public static void LoadLevelSelectScene(bool isFreeWorldMode)
    {
        LevelData.IsFreeWorldMode = isFreeWorldMode;
        SceneManager.LoadScene("LevelSelect", LoadSceneMode.Single);
    }

    public static void LoadArcadeGameScene()
    {
        GUIHandler.IsEndGame = false;
        PauseControl.GameIsPaused = false;
        LevelData.IsFreeWorldMode = false;
        LevelData.IsArcadeMode = true;
        LevelData.Starts = new();
        LevelData.Ends = new();
        LevelData.defaultStart = null;
        LevelData.defaultEnd = null;
        LevelData.GamePieces = null;
        LevelData.BoardSize = 10;
        LevelData.TimeLimit = LevelData.Difficulty == Difficulty.Normal ? 40 : 
            (LevelData.Difficulty == Difficulty.Hard ? 30 : 60);

        if (tutorialsPlayed[1])
            SceneManager.LoadScene("Game", LoadSceneMode.Single);
        else
        {
            tutorialsPlayed[1] = true;
            SceneManager.LoadScene("Tutorial", LoadSceneMode.Single);
        }
    }

    public static void LoadLevel(int levelNumber)
    {
        GUIHandler.IsEndGame = false;
        PauseControl.GameIsPaused = false;
        LevelData.IsArcadeMode = false;
        LevelData.LevelNumber = levelNumber;
        LevelData.Starts = new();
        LevelData.Ends = new();
        LevelData.defaultStart = null;
        LevelData.defaultEnd = null;
        LevelData.GamePieces = null;

        if (LevelData.IsFreeWorldMode)
            LevelData.lvlData = Resources.Load<TextAsset>("FreeWorldLevels/Level" + levelNumber.ToString())
                .text.Split(Environment.NewLine);
        else
            LevelData.lvlData = Resources.Load<TextAsset>("LevelSelectLevels/Level" + levelNumber.ToString())
                .text.Split(Environment.NewLine);

        LevelData.BoardSize = int.Parse(LevelData.lvlData[0]);

        int[] difficulties = Array.ConvertAll(LevelData.lvlData[1].Split(';'), int.Parse);
        LevelData.TimeLimit = LevelData.Difficulty == Difficulty.Normal ? difficulties[1] :
            (LevelData.Difficulty == Difficulty.Hard ? difficulties[2] : difficulties[0]);

        if ((LevelData.IsFreeWorldMode && tutorialsPlayed[2]) || (!LevelData.IsFreeWorldMode && tutorialsPlayed[0]))
            SceneManager.LoadScene("Game", LoadSceneMode.Single);
        else
        {
            if (LevelData.IsFreeWorldMode)
            {
                tutorialsPlayed[2] = true;
                SceneManager.LoadScene("Tutorial", LoadSceneMode.Single);
            }
            else
            {
                tutorialsPlayed[0] = true;
                SceneManager.LoadScene("Tutorial", LoadSceneMode.Single);
            }
        }
    }
}
