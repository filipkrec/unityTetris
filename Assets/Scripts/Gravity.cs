using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravity : MonoBehaviour
{
    [SerializeField]
    Board board;

    Tetromino activeTetromino;

    float Gs;

    float dropTimer;
    float softDropDelayTimer;

    float dropTime;
    float softDropDelay;
    bool softDroped;

    const int resets = 10;
    int currentResets;

    public bool SoftDroped { get => softDroped; }

    public void Awake()
    {
        dropTimer = 0f;
        softDropDelayTimer = 0f;
        Gs = 0.015f;
        RecalculateTimers();
        softDroped = false;
    }

    public void SetActiveTetromino(Tetromino tetromino)
    {
        activeTetromino = tetromino;
    }

    private void Update()
    {
        if (Globals.paused)
            return;

        dropTimer += Time.deltaTime;

        if (dropTimer > dropTime)
        {
            Drop();
        }

        if (softDroped)
        {
            CheckSoftDrop();
        }
    }
    
    void Drop()
    {
        dropTimer = 0f;
        activeTetromino.MoveDown();
        if (board.CheckFieldsTaken(activeTetromino.getBlockPositions()))
        {
            activeTetromino.MoveUp();
            if (!softDroped)
                SoftDrop();
        }
        else
        {
            activeTetromino.SetPosition();
            if (softDroped)
                softDroped = false;
        }
    }

    void CheckSoftDrop()
    {
        softDropDelayTimer += Time.deltaTime;
        if (softDropDelayTimer > softDropDelay)
        {
            softDropDelayTimer = 0f;
            activeTetromino.MoveDown();
            if (board.CheckFieldsTaken(activeTetromino.getBlockPositions()))
            {
                activeTetromino.MoveUp();
                board.PlaceTetromino(activeTetromino);
            }
            else
            {
                activeTetromino.SetPosition();
                softDroped = false;
            }
        }
    }

    public void IncreaseGs()
    {
        //Double until 1G, from then on increment by 1 until 20
        if (Gs < 1f)
            Gs *= 2;
        else if (Gs < 2f)
            Gs = 1f;
        else if (Gs < 20)
            Gs++;

        RecalculateTimers();
    }

    void RecalculateTimers()
    {
        dropTime = 1f / (Gs * 60); // G = 1 cell per frame = 1 cell / 60
        softDropDelay = 5 * dropTime;  
        softDropDelay = softDropDelay > 2f ? 1f : softDropDelay < 1f ? 1f : softDropDelay; //min 1f , max 2f
    }

    void SoftDrop()
    {
        softDroped = true;
        currentResets = resets;
    }

    public void ResetSoftDropTimer()
    {
        if (currentResets > 0)
        {
            softDropDelayTimer = 0f;
            currentResets--;
        }
    }

    public void HardDrop()
    {
        while (!board.CheckFieldsTaken(activeTetromino.getBlockPositions()))
        {
            activeTetromino.MoveDown();
        }

        activeTetromino.MoveUp();
        activeTetromino.SetPosition();
        board.PlaceTetromino(activeTetromino);
    }
}
