using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tetromino : MonoBehaviour
{
    public enum TetrominoShape
    {
        I, J, L, O, S, T, Z
    }

    public TetrominoShape tetrominoShape;
    public Color color;
    Vector2Int position;
    bool[,] blockMatrix;
    Block[] blocks;

    private void Awake()
    {
#if DEBUG
        Debug.Assert(Mathf.Approximately(transform.localScale.x, 1f) && Mathf.Approximately(transform.localScale.y, 1f), "Do not change tetromino scale");
#endif
        GenerateTetromino();
    }

    public Vector2Int[] getBlockPositions()
    {
        //block position = tetromino position + matrix position
        int matrixDimensions = (int)Mathf.Sqrt(blockMatrix.Length);
        Vector2Int[] positions = new Vector2Int[4];
        int count = 0;
        for (int i = 0; i < matrixDimensions; ++i)
        {
            for (int j = 0; i < matrixDimensions; ++i)
            {
                if (blockMatrix[i, j])
                {
                    positions[count] = new Vector2Int(i, j) + position;
                    count++;
                }
            }
        }

        return positions;
    }

    public void Place(Vector2Int position)
    {
        this.position = position;
    }

    public void MoveUp()
    {
        position.y += 1;
    }

    public void MoveDown()
    {
        position.y -= 1;
    }

    public void MoveLeft()
    {
        position.x -= 1;
    }

    public void MoveRight()
    {
        position.x += 1;
    }

    public void RotateLeft()
    {
        Util.RotateBoolMatrixCunterClockwise(blockMatrix);
    }

    public void RotateRight()
    {
        Util.RotateBoolMatrixClockwise(blockMatrix);
    }

    public void GenerateTetromino()
    {
        //TETROMINO SHAPES SRS guidelines
        switch (tetrominoShape)
        {
            case TetrominoShape.I:
                blockMatrix = new bool[4, 4];
                blockMatrix[2, 0] = true;
                blockMatrix[2, 1] = true;
                blockMatrix[2, 2] = true;
                blockMatrix[2, 3] = true;
                break;
            case TetrominoShape.J:
                blockMatrix = new bool[3, 3];
                blockMatrix[1, 0] = true;
                blockMatrix[1, 1] = true;
                blockMatrix[1, 2] = true;
                blockMatrix[0, 2] = true;
                break;
            case TetrominoShape.L:
                blockMatrix = new bool[3, 3];
                blockMatrix[1, 0] = true;
                blockMatrix[1, 1] = true;
                blockMatrix[1, 2] = true;
                blockMatrix[2, 2] = true;
                break;
            case TetrominoShape.O:
                blockMatrix = new bool[2, 2];
                blockMatrix[0, 0] = true;
                blockMatrix[0, 1] = true;
                blockMatrix[1, 0] = true;
                blockMatrix[1, 1] = true;
                break;
            case TetrominoShape.S:
                blockMatrix = new bool[3, 3];
                blockMatrix[0, 1] = true;
                blockMatrix[1, 1] = true;
                blockMatrix[1, 0] = true;
                blockMatrix[2, 0] = true;
                break;
            case TetrominoShape.T:
                blockMatrix = new bool[3, 3];
                blockMatrix[0, 1] = true;
                blockMatrix[1, 1] = true;
                blockMatrix[2, 1] = true;
                blockMatrix[1, 0] = true;
                break;
            case TetrominoShape.Z:
                blockMatrix = new bool[3, 3];
                blockMatrix[0, 0] = true;
                blockMatrix[1, 1] = true;
                blockMatrix[1, 1] = true;
                blockMatrix[2, 1] = true;
                break;
        }
    }

    public void PlaceBlocks()
    {
        if (blocks == null)
        {
            blocks = new Block[4];

            for (int i = 0; i < 4; ++i) //4 blocks per tetromino
            {
                blocks[i] = Instantiate(Globals.Block, transform);
                blocks[i].Initiate(color, Vector2.zero, Board.BlockSize);
            }
        }

        int matrixDimensions = (int)Mathf.Sqrt(blockMatrix.Length);

        int count = 0;

        //pivot = top left block
        for (int i = 0; i < matrixDimensions; ++i)
        {
            for (int j = 0; j < matrixDimensions; ++j)
            {
                if (blockMatrix[i, j])
                {
                    blocks[count].SetTransformPosition(new Vector3(i * Board.BlockSize.x, j * Board.BlockSize.y, 0f) + transform.position);
                    count++;
                }
            }
        }
    }
}
