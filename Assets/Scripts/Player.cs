using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Board board;
    bool started = false;

    Tetromino activeTetromino;
    Gravity gravity;
    Sound sound;
    int currentLevel;

    private void Awake()
    {
        currentLevel = 0;
        gravity = GetComponent<Gravity>();
        sound = GetComponent<Sound>();
        Globals.paused = true;
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
        if (!started && Input.GetMouseButtonDown(0))
            StartGame();

        if (Globals.paused)
            return;

        if(board.GrabNext)
        {
            activeTetromino = board.GetNextTetromino();
            gravity.SetActiveTetromino(activeTetromino);

            if (board.NextLevel())
            {
                currentLevel++;
                gravity.IncreaseGs();
            }
        }

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
