using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Direction enum
/// </summary>
public enum Dir
{
    UP, RIGHT, DOWN, LEFT
}

/// <summary>
/// Handles individual information about a Pipe (location, rotation) and its surrounding Pipes
/// </summary>
public class PipeHandler : MonoBehaviour
{
    /// <summary>
    /// 2D bool Array that is set in the Pipe Prefab. 1 means this Dir is available IO and 0 means its unavailable.
    /// Indexes: 0 is up, 1 is right, 2 is down, 3 is left.
    /// </summary>
	public bool[] IODirs; 
	public float speed; // Determines the speed at which the Rotate animation plays
    [SerializeField]
	float rotation; // 0 is up, 90 is right, 180 is down, 270 is left

    public Pipe pipeType; // Determines the tile/pipe type
    public Position location;
    // Determines if the corresponding neighbouring Tile is free for the flow to continue
	public bool upFree = false;
    public bool downFree = false;
    public bool rightFree = false;
	public bool leftFree = false;
    // References to neighbouring PipeHandlers
    public PipeHandler up, right, down, left;

    LevelHandler levelHandler;

	void Start()
	{
        levelHandler = GameObject.Find("Grid").GetComponent<LevelHandler>();
		rotation = 0;

        // Initializes the neighbouring PipeHandlers at the start of the script instance
        UpdateNeighbouringPipeHandlers();
    }

	void Update()
	{
        // Handles the rotation animation using the Main Loop
		if (transform.rotation.eulerAngles.z != rotation)
		{
			transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, rotation), speed);
		}
	}

    public void UpdateNeighbouringPipeHandlers()
    {
        location.X = (int)gameObject.transform.parent.localPosition.x;
        location.Y = (int)gameObject.transform.parent.localPosition.y;

        up = null;
        right = null;
        down = null;
        left = null;

        if (location.Y + 1 < LevelData.BoardSize)
        {
            up = LevelData.GamePieces[location.X, location.Y + 1];
        }

        if (location.Y != 0)
        {
            down = LevelData.GamePieces[location.X, location.Y - 1];
        }

        if (location.X + 1 < LevelData.BoardSize)
        {
            right = LevelData.GamePieces[location.X + 1, location.Y];
        }

        if (location.X != 0)
        {
            left = LevelData.GamePieces[location.X - 1, location.Y];
        }
    }

    /// <summary>
    /// Fires when this pipe was clicked on, rotates the pipe and sets new ActiveTile to this tile
    /// </summary>
	void OnMouseDown()
	{
        if (!PauseControl.GameIsPaused && !GUIHandler.IsEndGame)
        {
            RotatePiece();
            levelHandler.SetActiveTile(gameObject);
        }
	}

    /// <summary>
    /// Rotates the pipe and sets new IOs, available neighbouring pipes, and updates the neighbouring pipes as well
    /// </summary>
	public void RotatePiece()
	{
        levelHandler.PlayPipeRotationAudio();

        // Rotation is updated clockwise in a circular manner
		rotation += 90;
		if (rotation == 360)
			rotation = 0;

		RotateIODirections();

        ProcessNearbyPipeChanges(setNearbyPipeHandlers: false);
    }

    /// <summary>
    /// Rotates the IO Dir flags in a clockwise manner.
    /// </summary>
	public void RotateIODirections()
	{
		bool storedUp = IODirs[0];

		for (int i = 0; i < IODirs.Length - 1; i++)
		{
			IODirs[i] = IODirs[i + 1];
		}
		IODirs[3] = storedUp;
	}

    public void ProcessNearbyPipeChanges(bool setNearbyPipeHandlers)
    {
        if (setNearbyPipeHandlers)
        {
            UpdateNeighbouringPipeHandlers();

            if (up != null)
                up.UpdateNeighbouringPipeHandlers();
            if (down != null)
                down.UpdateNeighbouringPipeHandlers();
            if (left != null)
                left.UpdateNeighbouringPipeHandlers();
            if (right != null)
                right.UpdateNeighbouringPipeHandlers();
        }

        UpdateAvailableSides();

        if (up != null)
            up.UpdateAvailableSides();
        if (down != null)
            down.UpdateAvailableSides();
        if (left != null)
            left.UpdateAvailableSides();
        if (right != null)
            right.UpdateAvailableSides();
    }

    /// <summary>
    /// Used to determine if the water can flow to the corresponding Dirs and updates the Free flags
    /// </summary>
    public void UpdateAvailableSides()
    {
        upFree = false;
        downFree = false;
        rightFree = false;
        leftFree = false;

        for (int dirIndex = 0; dirIndex < IODirs.Length; dirIndex++)
        {
            if (IODirs[dirIndex])
            {
                switch (dirIndex)
                {
                    case (int)Dir.UP:
                        if (up != null)
                        {
                            if (up.IODirs[(int)Dir.DOWN])
                            {
                                upFree = true;
                            }
                        }
                        break;
                    case (int)Dir.RIGHT:
                        if (right != null)
                        {
                            if (right.IODirs[(int)Dir.LEFT])
                            {
                                rightFree = true;
                            }
                        }
                        break;
                    case (int)Dir.DOWN:
                        if (down != null)
                        {
                            if (down.IODirs[(int)Dir.UP])
                            {
                                downFree = true;
                            }
                        }
                        break;
                    case (int)Dir.LEFT:
                        if (left != null)
                        {
                            if (left.IODirs[(int)Dir.RIGHT])
                            {
                                leftFree = true;
                            }
                        }
                        break;
                }
            }
        }
    }
}
