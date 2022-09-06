using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Player Input interaction with the game. Game Controls.
/// Pointer movement: Arrows, WASD.
/// Rotate Active Pipe: R.
/// Start Flow: F.
/// </summary>
public class Player : MonoBehaviour
{
    private GameManager gm;
    private LevelHandler levelHandler;

    void Start()
    {
        gm = gameObject.GetComponent<GameManager>();
        levelHandler = GameObject.FindObjectOfType<LevelHandler>();
    }

    void Update()
    {
        if (!PauseControl.GameIsPaused && !GUIHandler.IsEndGame)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
            {
                levelHandler.MoveActiveTileUp();
            }
            if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
            {
                levelHandler.MoveActiveTileDown();
            }
            if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
            {
                levelHandler.MoveActiveTileRight();
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
            {
                levelHandler.MoveActiveTileLeft();
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                levelHandler.RotateActiveTile();
            }
            if (Input.GetKeyDown(KeyCode.F))
            {
                gm.StartFlow();
            }
        }
    }
}
