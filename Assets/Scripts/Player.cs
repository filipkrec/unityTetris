using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    Board board;
    bool started = false;

    Tetromino activeTetromino;
    Gravity gravity;
    Sound sound;
    int currentLevel;

    Vector2 touchPoint;
    bool spun;
    bool moved;
    bool ignoreTouch;
    int touchSensitivity;

    private void Awake()
    {
        currentLevel = 0;
        gravity = GetComponent<Gravity>();
        sound = GetComponent<Sound>();
        Globals.paused = true;
        touchSensitivity = PlayerPrefs.GetInt("Sensitivity");

        if (touchSensitivity == 0)
            touchSensitivity = 10;
    }

    void StartGame()
    {
        started = true;
        sound.PlayBGM();
        activeTetromino = board.GetNextTetromino();
        gravity.SetActiveTetromino(activeTetromino);
        GetComponent<UI>().StartGame();
        Globals.paused = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!started 
            &&
#if DEBUG
            Input.GetMouseButtonDown(0) ||
#endif
            Input.touchCount > 0
            )
            StartGame();

        if (Globals.paused)
            return;

        if (board.GrabNext)
        {
            activeTetromino = board.GetNextTetromino();
            gravity.SetActiveTetromino(activeTetromino);

            if (board.NextLevel())
            {
                currentLevel++;
                gravity.IncreaseGs();
            }
        }
        ProcessTouchInput();
#if DEBUG
        ProcessPcInput();
#endif
    }

#if DEBUG
    void ProcessPcInput()
    {

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            MoveTetrominoRight();
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            MoveTetrominoLeft();
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            gravity.HardDrop();
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            RotateTetrominoRight();
        }

    }
#endif

    void ProcessTouchInput()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                moved = false;
                ignoreTouch = false;
                spun = false;
                touchPoint = touch.position;
            }
            else if (!ignoreTouch)
            {
                //Left Right
                if (Mathf.Abs(touchPoint.x - touch.position.x) > Screen.width / touchSensitivity)
                {
                    moved = true;
                    if (touch.position.x < touchPoint.x)
                    {
                        touchPoint.x = touchPoint.x - Screen.width / touchSensitivity;
                        MoveTetrominoLeft();
                    }
                    else
                    {
                        touchPoint.x = touch.position.x + Screen.width / touchSensitivity;
                        MoveTetrominoRight();
                    }
                }

                //Rotate
                if (!spun && !moved && touch.position.y - touchPoint.y > Screen.height / touchSensitivity)
                {
                    spun = true;
                    //move once per 1/6 of the screen, move twice if > 3 moves per sec, move 3 times if over 4 moves per sec

                    if (touch.position.x < Screen.width / 2)
                    {
                        RotateTetrominoRight();
                    }
                    else
                    {
                        RotateTetrominoLeft();
                    }
                }

                if(!spun && !moved && touchPoint.y - touch.position.y > Screen.height / touchSensitivity)
                {
                    gravity.HardDrop();
                    ignoreTouch = true;
                }
            }
        }

    }


    void MoveTetrominoLeft()
    {
        activeTetromino.MoveLeft();

        if (board.CheckFieldsTaken(activeTetromino.getBlockPositions()))
        {
            activeTetromino.MoveRight();
            sound.PlayError();
        }
        else
        {
            sound.PlayMove();
            activeTetromino.SetPosition();
            if (gravity.SoftDroped)
                gravity.ResetSoftDropTimer();
        }
    }

    void MoveTetrominoRight()
    {
        activeTetromino.MoveRight();

        if (board.CheckFieldsTaken(activeTetromino.getBlockPositions()))
        {
            activeTetromino.MoveLeft();
            sound.PlayError();
        }
        else
        {
            sound.PlayMove();
            activeTetromino.SetPosition();
            if (gravity.SoftDroped)
                gravity.ResetSoftDropTimer();
        }
    }

    void RotateTetrominoRight()
    {
        activeTetromino.RotateRight();

        if (board.CheckFieldsTaken(activeTetromino.getBlockPositions()))
        {
            activeTetromino.RotateLeft();
            sound.PlayError();
        }
        else
        {
            sound.PlayMove();
            activeTetromino.PlaceBlocksToMatrix();
            if (gravity.SoftDroped)
                gravity.ResetSoftDropTimer();
        }
    }
    void RotateTetrominoLeft()
    {
        activeTetromino.RotateLeft();

        if (board.CheckFieldsTaken(activeTetromino.getBlockPositions()))
        {
            activeTetromino.RotateRight();
            sound.PlayError();
        }
        else
        {
            sound.PlayMove();
            activeTetromino.PlaceBlocksToMatrix();
            if (gravity.SoftDroped)
                gravity.ResetSoftDropTimer();
        }
    }

    public void ChangeTouchSensitivity(System.Single sensitivity)
    {
        touchSensitivity = (int)sensitivity;
        PlayerPrefs.SetInt("Sensitivity", touchSensitivity);
    }
}
