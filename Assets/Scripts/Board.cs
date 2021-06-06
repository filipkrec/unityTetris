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
    int rowsClearedThisLevel;
    bool grabNext;

    public List<Tetromino> tetrominoPrefabs;
    public SpriteRenderer boardBackground;
    public SpriteRenderer previewBoard;
    public UI ui;

    static Vector2 blockSize;
    Vector2 fieldWorldSpaceSize;
    Vector2 fieldBotLeft;
    List<Block> placedBlocks;

    Tetromino previewTetromino;

    public TextMeshProUGUI scoreTxt;

    public static Vector2 BlockSize { get => blockSize; }
    public int Score { get => score; }
    public int RowsClearedThisLevel { get => rowsClearedThisLevel; }
    public bool GrabNext { get => grabNext; }

    private void Awake()
    {
        board = new bool[columns , rows];

        BoxCollider2D fieldBox = GetComponent<BoxCollider2D>();
        placedBlocks = new List<Block>();

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
        foreach(Block block in tetromino.Blocks)
        {
            board[block.BoardPos.x, block.BoardPos.y] = true;
            block.transform.SetParent(transform);
            placedBlocks.Add(block);
            if (block.BoardPos.y >= fieldRows)
                Lose();
        }

        Destroy(tetromino.gameObject);

        ScoreBoard();

        grabNext = true;
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

        rowsClearedThisLevel += clearedRows;

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
            scoreTxt.text = score.ToString();
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
                else if (j == columns - 1 && board[j, i])
                {
                    ShiftRowsDown(i);
                    --i;
                }
            }
        }
    }

    void ShiftRowsDown(int fromRow)
    {
        List<Block> toRemove = new List<Block>();
        foreach(Block block in placedBlocks)
        {
            if (block.BoardPos.y == fromRow)
            {
                toRemove.Add(block);
            }
            else if (block.BoardPos.y > fromRow)
                block.ShiftDown();
        }

        foreach(Block block in toRemove)
        {
            placedBlocks.Remove(block);
            Destroy(block.gameObject);
        }


        for(int i = fromRow; i < fieldRows; ++i)
        {
            for (int j = 0; j < columns; ++j)
            {
                board[j, i] = board[j, i + 1];
            }
        }
    }

    public Tetromino GetNextTetromino()
    {
        grabNext = false;

        if (previewTetromino == null)
            SpawnPreviewTetromino();

        Tetromino toReturn = previewTetromino;
        SetPreviewTetrominoToBoard();

        SpawnPreviewTetromino();

        return toReturn;
    }

    void SetPreviewTetrominoToBoard()
    {
        previewTetromino.SetBoardPosition(new Vector2Int(Random.Range(previewTetromino.SpawnColMin, previewTetromino.SpawnColMax + 1), rows - 1));
        previewTetromino.SetToBoard(boardBackground, fieldBotLeft);
    }

    public bool NextLevel()
    {
        if (rowsClearedThisLevel >= 10) //10 lines = level up
        {
            rowsClearedThisLevel = 0;
            return true;
        }

        return false;
    }

    void SpawnPreviewTetromino()
    {
        previewTetromino = Instantiate(tetrominoPrefabs[Random.Range(0, tetrominoPrefabs.Count)]);
        previewTetromino.SetToPreview(previewBoard, 2f);
    }

    void Lose()
    {
        ui.Lose(score);
    }
}
