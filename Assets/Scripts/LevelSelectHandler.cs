using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelectHandler : MonoBehaviour
{
    public void SelectLevelWithNumber(int number)
    {
        SceneHandler.LoadLevel(number);
    }
}
