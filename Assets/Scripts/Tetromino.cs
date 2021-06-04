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
    Vector2 originPos;
    bool[,] blockMatrix;
    Block[] blocks;

    int spawnColMin;
    int spawnColMax;

    public int SpawnColMin { get => spawnColMin; }
    public int SpawnColMax { get => spawnColMax; }

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
        Vector2Int[] positions = new Vector2Int[4];
        int count = 0;
        foreach(Block block in blocks)
        {
            positions[count] = block.BoardPos;
            count++;
        }

        return positions;
    }

    public void SetBoardPosition(Vector2Int position)
    {
        this.position = position;
    }

    public void SetOriginPosition(Vector2 tetrominoSpawnPos)
    {
        originPos = tetrominoSpawnPos;
    }

    public void SetPosition()
    {
        transform.position = originPos + position * Board.BlockSize;
    }

    public void MoveUp()
    {
        position.y += 1;
        foreach (Block block in blocks)
        {
            block.MoveBoardPosition(0, 1);
        }
    }

    public void MoveDown()
    {
        position.y -= 1;
        foreach (Block block in blocks)
        {
            block.MoveBoardPosition(0, -1);
        }
    }

    public void MoveLeft()
    {
        position.x -= 1;
        foreach (Block block in blocks)
        {
            block.MoveBoardPosition(-1, 0);
        }
    }

    public void MoveRight()
    {
        position.x += 1;
        foreach(Block block in blocks)
        {
            block.MoveBoardPosition(1, 0);
        }
    }

    public void RotateLeft()
    {
        Util.RotateBoolMatrixCunterClockwise(blockMatrix);
        SetBoardPositionsToMatrix();
    }

    public void RotateRight()
    {
        Util.RotateBoolMatrixClockwise(blockMatrix);
        SetBoardPositionsToMatrix();
    }

    public void GenerateTetromino()
    {
        //[00][10][20]
        //[01][11][21]
        //[02][12][22]
        //TETROMINO SHAPES SRS guidelines
        switch (tetrominoShape)
        {
            case TetrominoShape.I:
                blockMatrix = new bool[4, 4];
                blockMatrix[2, 0] = true;
                blockMatrix[2, 1] = true;
                blockMatrix[2, 2] = true;
                blockMatrix[2, 3] = true;
                spawnColMin = -2;
                spawnColMax = Board.columns - 2;
                break;
            case TetrominoShape.J:
                blockMatrix = new bool[3, 3];
                blockMatrix[1, 0] = true;
                blockMatrix[1, 1] = true;
                blockMatrix[1, 2] = true;
                blockMatrix[0, 2] = true;
                spawnColMin = 0;
                spawnColMax = Board.columns - 2;
                break;
            case TetrominoShape.L:
                blockMatrix = new bool[3, 3];
                blockMatrix[1, 0] = true;
                blockMatrix[1, 1] = true;
                blockMatrix[1, 2] = true;
                blockMatrix[2, 2] = true;
                spawnColMin = 0;
                spawnColMax = Board.columns - 1;
                break;
            case TetrominoShape.O:
                blockMatrix = new bool[2, 2];
                blockMatrix[0, 0] = true;
                blockMatrix[0, 1] = true;
                blockMatrix[1, 0] = true;
                blockMatrix[1, 1] = true;
                spawnColMin = 0;
                spawnColMax = Board.columns - 1;
                break;
            case TetrominoShape.S:
                blockMatrix = new bool[3, 3];
                blockMatrix[0, 1] = true;
                blockMatrix[1, 1] = true;
                blockMatrix[1, 0] = true;
                blockMatrix[2, 0] = true;
                spawnColMin = 0;
                spawnColMax = Board.columns - 2;
                break;
            case TetrominoShape.T:
                blockMatrix = new bool[3, 3];
                blockMatrix[0, 1] = true;
                blockMatrix[1, 1] = true;
                blockMatrix[2, 1] = true;
                blockMatrix[1, 0] = true;
                spawnColMin = 0;
                spawnColMax = Board.columns - 2;
                break;
            case TetrominoShape.Z:
                blockMatrix = new bool[3, 3];
                blockMatrix[0, 0] = true;
                blockMatrix[1, 0] = true;
                blockMatrix[1, 1] = true;
                blockMatrix[2, 1] = true;
                spawnColMin = 0;
                spawnColMax = Board.columns - 3;
                break;
        }
    }

    public void PlaceBlocksToMatrix()
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
                    blocks[count].SetTransformPosition(new Vector3(i * Board.BlockSize.x, -j * Board.BlockSize.y, 0f) + transform.position);

                    count++;
                }
            }
        }
    }

    public void SetBoardPositionsToMatrix()
    {
        int matrixDimensions = (int)Mathf.Sqrt(blockMatrix.Length);
        int count = 0;

        for (int i = 0; i < matrixDimensions; ++i)
        {
            for (int j = 0; j < matrixDimensions; ++j)
            {
                if (blockMatrix[i, j])
                {
                    blocks[count].BoardPos = position + new Vector2Int(i, -j);
                    count++;
                }
            }
        }
    }
}
