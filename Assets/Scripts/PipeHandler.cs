using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Dir
{
    UP, RIGHT, DOWN, LEFT
}

public class PipeHandler : MonoBehaviour
{
	public bool[] IODirs;
	public float speed;
    [SerializeField]
	float rotation;

	public int tileType;
    public Position location;
	public bool upFree = false;
    public bool downFree = false;
    public bool rightFree = false;
	public bool leftFree = false;
    public PipeHandler up, right, down, left;

    LevelHandler levelHandler;

	void Start()
	{
        levelHandler = GameObject.Find("Grid").GetComponent<LevelHandler>();
		rotation = 0;

        location.X = (int)gameObject.transform.parent.localPosition.x;
        location.Y = (int)gameObject.transform.parent.localPosition.y;

        if (location.Y + 1 < LevelData.BoardSize)
        {
			up = LevelData.GamePieces[location.X, location.Y + 1];
        }

        if (location.Y != 0)
        {
            down = LevelData.GamePieces[location.X, location.Y - 1]; ;
        }

        if (location.X + 1 < LevelData.BoardSize)
        {
            right = LevelData.GamePieces[location.X + 1, location.Y]; ;
        }

        if (location.X != 0)
        {
            left = LevelData.GamePieces[location.X - 1, location.Y]; ;
        }
    }

	void Update()
	{
		if (transform.rotation.eulerAngles.z != rotation)
		{
			transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, rotation), speed);
		}
	}

	void OnMouseDown()
	{
        if (!PauseControl.GameIsPaused && !GUIHandler.IsEndGame)
        {
            RotatePiece();
            levelHandler.SetActiveTile(gameObject);
        }
	}

	public void RotatePiece()
	{
        levelHandler.PlayPipeRotationAudio();

		rotation += 90;
		if (rotation == 360)
			rotation = 0;

		RotateIODirections();

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

	public void RotateIODirections()
	{
		bool storedUp = IODirs[0];

		for (int i = 0; i < IODirs.Length - 1; i++)
		{
			IODirs[i] = IODirs[i + 1];
		}
		IODirs[3] = storedUp;
	}

    // Updates bools to check if water can be pumped
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
