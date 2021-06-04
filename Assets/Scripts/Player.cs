using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Board board;

    Tetromino activeTetromino;

    private void Start()
    {
        activeTetromino = board.GetNextTetromino();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            MoveTetrominoRight();
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            MoveTetrominoLeft();
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            MoveTetrominoDown();
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            RotateTetrominoRight();
        }
    }
    
    void MoveTetrominoDown()
    {
        activeTetromino.MoveDown();

        if (board.CheckFieldsTaken(activeTetromino.getBlockPositions()))
            activeTetromino.MoveUp();
        else 
            activeTetromino.SetPosition();
    }

    void MoveTetrominoLeft()
    {
        activeTetromino.MoveLeft();

        if (board.CheckFieldsTaken(activeTetromino.getBlockPositions()))
            activeTetromino.MoveRight();
        else 
            activeTetromino.SetPosition();
    }

    void MoveTetrominoRight()
    {
        activeTetromino.MoveRight();

        if (board.CheckFieldsTaken(activeTetromino.getBlockPositions()))
            activeTetromino.MoveLeft();
        else 
            activeTetromino.SetPosition();
    }

    void RotateTetrominoRight()
    {
        activeTetromino.RotateRight();

        if (board.CheckFieldsTaken(activeTetromino.getBlockPositions()))
            activeTetromino.RotateLeft();
        else
            activeTetromino.PlaceBlocksToMatrix();
    }
    void RotateTetrominoLeft()
    {
        activeTetromino.RotateLeft();

        if (board.CheckFieldsTaken(activeTetromino.getBlockPositions()))
            activeTetromino.RotateRight();
        else
            activeTetromino.PlaceBlocksToMatrix();
    }
}
