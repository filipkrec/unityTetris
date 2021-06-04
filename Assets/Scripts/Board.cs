using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Board : MonoBehaviour
{
    bool[,] board;
    public const int columns = 10;
    public const int rows = 23; // 2 invis rows for spawn
    public const int fieldRows = rows - 2;

    int score;

    public List<Tetromino> tetrominoPrefabs;

    static Vector2 blockSize;
    Vector2 fieldWorldSpaceSize;
    Vector2 fieldBotLeft;

    Tetromino previewTetromino;

    public TextMeshProUGUI scoreTxt;

    public static Vector2 BlockSize { get => blockSize; }
    public int Score { get => score; }

    private void Awake()
    {
        board = new bool[columns , rows];

        BoxCollider2D fieldBox = GetComponent<BoxCollider2D>();

#if DEBUG 
        if (fieldBox != null)
#endif
        {
            fieldWorldSpaceSize = new Vector2(fieldBox.size.x * transform.lossyScale.x, fieldBox.size.y * transform.lossyScale.y);
            blockSize = new Vector2(fieldWorldSpaceSize.x / columns, fieldWorldSpaceSize.y / (rows - 2)); //top 2 rows not in fieldSpace

            fieldBotLeft = new Vector2(transform.position.x - fieldWorldSpaceSize.x / 2 + blockSize.x/2 - fieldBox.offset.x * transform.lossyScale.x, 
                transform.position.y - fieldWorldSpaceSize.y / 2 + blockSize.y / 2 + fieldBox.offset.y * transform.lossyScale.y); 
        }
    }

    public bool CheckFieldsTaken(Vector2Int[] fields)
    {
        foreach(Vector2Int field in fields)
        {
            if (field.x >= columns || field.x < 0 || field.y < 0 || board[field.x, field.y])
                return true;
        }

        return false;
    }

    public void PlaceTetromino(Tetromino tetromino)
    { 
        foreach(Vector2Int pos in tetromino.getBlockPositions())
        {
            board[pos.x, pos.y] = true;
        }

        ScoreBoard();
    } 

    void ScoreBoard()
    {
        int clearedRows = 0;
        for(int i = 0; i < fieldRows; ++i)
        {
            for(int j = 0; j < columns; ++j)
            {
                if (!board[j, i])
                {
                    break;
                }
                else if (j == columns - 1 && board[j, i])
                {
                    clearedRows++;
                }
            }
        }

        switch (clearedRows)
        {
            case 0:
                break;
            case 1:
                score += 100;
                break;
            case 2:
                score += 400;
                break;
            case 3:
                score += 1000;
                break;
            case 4: 
                score += 3000;
                break;
            default:
                break;
        }

        if(clearedRows > 0)
        {
            RearangeRows();
        }

    }

    void RearangeRows()
    {
        for(int i = 0; i < fieldRows; ++i)
        {
            for(int j = 0; j < columns; ++j)
            {
                if (!board[j, i])
                {
                    break;
                }
                else
                {
                    ShiftRowsDown(i);
                }
            }
        }
    }

    void ShiftRowsDown(int fromRow)
    {
        for(int i = fromRow; i < fieldRows; ++i)
        {
            for (int j = 0; j < columns; ++j)
            {
                board[j, i] = board[j, i - 1];
            }
        }
    }

    public Tetromino GetNextTetromino()
    {
        if(previewTetromino == null)
            previewTetromino = Instantiate(tetrominoPrefabs[Random.Range(0,tetrominoPrefabs.Count)]);

        Tetromino toReturn = previewTetromino;
        SpawnTetromino(toReturn);

        previewTetromino = Instantiate(tetrominoPrefabs[Random.Range(0, tetrominoPrefabs.Count)]);

        return toReturn;
    }

    void SpawnTetromino(Tetromino tetromino)
    {
        tetromino.SetBoardPosition(new Vector2Int(Random.Range(tetromino.SpawnColMin, tetromino.SpawnColMax), rows - 1));
        tetromino.SetOriginPosition(fieldBotLeft);
        tetromino.SetPosition();
        tetromino.PlaceBlocksToMatrix();
        tetromino.SetBoardPositionsToMatrix();
    }
}
