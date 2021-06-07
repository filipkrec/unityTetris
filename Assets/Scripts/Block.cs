using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    Vector2Int boardPos;

    private void Awake()
    {
#if DEBUG
        Debug.Assert(Mathf.Approximately(transform.localScale.x, 1f) && Mathf.Approximately(transform.localScale.y, 1f), "Do not change block scale");
#endif
    }

    public Vector2Int BoardPos
    {
        get => boardPos;
        set
        {
            boardPos = value;
            Hide(boardPos.y >= Board.fieldRows); //hide for invis rows
        }

    }
    public void Initiate(Color color, Vector2 pos, Vector2 size)
    {
        SetColor(color);
        SetTransformPosition(pos);
        SetBlockSize(size);
        Hide(false);
    }

    public void SetColor(Color color)
    {
        GetComponent<SpriteRenderer>().color = color;
    }

    public void SetTransformPosition(Vector2 pos)
    {
        transform.position = pos;
    }

    public void ShiftDown()
    {
        boardPos.y -= 1;
        transform.position -= new Vector3(0f, Board.BlockSize.y,0f);
    }

    public void MoveTransformposition(Vector2 move)
    {
        transform.position += (Vector3)move;
    }

    public void SetBlockSize(Vector2 size)
    {
        GetComponent<SpriteRenderer>().size = size;
    }

    public void Hide(bool hide)
    {
        if (hide)
            GetComponent<SpriteRenderer>().enabled = false;
        else
            GetComponent<SpriteRenderer>().enabled = true;
    }

    public void MoveBoardPosition(int x, int y)
    {
        boardPos.x += x;
        boardPos.y += y;
        Hide(boardPos.y >= Board.fieldRows);
    }
}
