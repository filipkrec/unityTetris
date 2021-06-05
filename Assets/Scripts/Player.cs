using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Board board;

    Tetromino activeTetromino;
    Gravity gravity;
    int currentLevel;

    private void Awake()
    {
        currentLevel = 0;
        gravity = GetComponent<Gravity>();
    }

    private void Start()
    {
        activeTetromino = board.GetNextTetromino();
        gravity.SetActiveTetromino(activeTetromino);
    }

    // Update is called once per frame
    void Update()
    {
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
            activeTetromino.MoveRight();
        else
        {
            activeTetromino.SetPosition();
            if (gravity.SoftDroped)
                gravity.ResetSoftDropTimer();
        }
    }

    void MoveTetrominoRight()
    {
        activeTetromino.MoveRight();

        if (board.CheckFieldsTaken(activeTetromino.getBlockPositions()))
            activeTetromino.MoveLeft();
        else
        {
            activeTetromino.SetPosition();
            if (gravity.SoftDroped)
                gravity.ResetSoftDropTimer();
        }
    }

    void RotateTetrominoRight()
    {
        activeTetromino.RotateRight();

        if (board.CheckFieldsTaken(activeTetromino.getBlockPositions()))
            activeTetromino.RotateLeft();
        else
        {
            activeTetromino.PlaceBlocksToMatrix();
            if (gravity.SoftDroped)
                gravity.ResetSoftDropTimer();
        }
    }
    void RotateTetrominoLeft()
    {
        activeTetromino.RotateLeft();

        if (board.CheckFieldsTaken(activeTetromino.getBlockPositions()))
            activeTetromino.RotateRight();
        else
        {
            activeTetromino.PlaceBlocksToMatrix();
            if (gravity.SoftDroped)
                gravity.ResetSoftDropTimer();
        }
    }
}
