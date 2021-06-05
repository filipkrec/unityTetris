using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    SpriteRenderer sprite;
    Vector2Int boardPos;

    public Vector2Int BoardPos
    {
        get => boardPos;
        set
        {
            boardPos = value;
            Hide(boardPos.y >= Board.fieldRows);
        }

    }
    public void Initiate(Color color, Vector2 pos, Vector2 size, bool hidden = false)
    {
        sprite = GetComponent<SpriteRenderer>();

#if DEBUG
        Debug.Assert(sprite != null);
#endif

        SetColor(color);
        SetTransformPosition(pos);
        SetBlockSize(size);
        Hide(hidden);
    }

    public void SetColor(Color color)
    {
        sprite.color = color;
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
        sprite.size = size;
    }

    public void Hide(bool hide)
    {
        if (hide)
            sprite.enabled = false;
        else
            sprite.enabled = true;
    }

    public void MoveBoardPosition(int x, int y)
    {
        boardPos.x += x;
        boardPos.y += y;
        Hide(boardPos.y >= Board.fieldRows);
    }
}
