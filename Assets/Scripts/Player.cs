using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    Board board;

    [SerializeField]
    UI ui;

    bool started = false;

    Tetromino activeTetromino;
    Gravity gravity;
    Sound sound;
 //   int currentLevel; TODO?

    Vector2 touchPoint;
    bool rotated;
    bool moved;
    bool ignoreTouch;
    int touchSensitivity;

    private void Awake()
    {
//        currentLevel = 0;

        gravity = GetComponent<Gravity>();
        gravity.SetBoard(board);

        sound = GetComponent<Sound>();

        board.Ui = ui;
        board.Sound = sound;

    }

    void StartGame()
    {
        started = true;
        sound.PlayBGM();

        activeTetromino = board.GetNextTetromino();
        gravity.SetActiveTetromino(activeTetromino);

        touchSensitivity = ui.GetSensitivity();

        ui.StartGame();
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
//                currentLevel++;
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
                rotated = false;
                touchPoint = touch.position;
            }
            else if (!ignoreTouch) //if droped ignore till next touch
            {
                if(!rotated) //dont move if rotating
                TouchMoveLeftRight(touch);

                if (!moved && !rotated) //1 rotate per touch
                    TouchRotate(touch);

                else if (!moved && !rotated) //dont drop on move/rotate touch
                    TouchDrop(touch);
            }
        }

    }

    void TouchMoveLeftRight(Touch touch)
    {
        if (Mathf.Abs(touchPoint.x - touch.position.x) > Screen.width / touchSensitivity)
        {
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
            moved = true;
        }
    }

    void TouchRotate(Touch touch)
    {
        if (touch.position.y - touchPoint.y > Screen.height / touchSensitivity)
        {
            if (touch.position.x < Screen.width / 2)
            {
                RotateTetrominoRight();
            }
            else
            {
                RotateTetrominoLeft();
            }

            rotated = true;
        }
    }

    void TouchDrop(Touch touch)
    {
        if (touchPoint.y - touch.position.y > Screen.height / touchSensitivity)
        {
            gravity.HardDrop();
            ignoreTouch = true;
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
}
