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
        if(Input.GetMouseButtonDown(0))
        {
            MoveTetrominoDown();
        }
    }
    
    void MoveTetrominoDown()
    {
        activeTetromino.MoveDown();
        if (board.CheckFieldsTaken(activeTetromino.getBlockPositions()))
        {
            activeTetromino.MoveUp();
        }

        board.placeTetromino(activeTetromino);
    }


}
