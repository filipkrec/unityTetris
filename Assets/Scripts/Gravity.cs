using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravity : MonoBehaviour
{
    public Board board;
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
        Gs = 0.02f;
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

        if (softDroped)
        {
            softDropDelayTimer++;
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
        dropTime = 1f / (Gs * 20f); // 1G = 1 full drop in 1 sec = 1/20 s drop time
        softDropDelay = 0.5f / (10f / Gs) > 2f ? 2f : 0.5f / (10f / Gs); // 0.5s at 10Gs, longest 2s
    }

    void SoftDrop()
    {
        currentResets = resets;
    }

    public void ResetSoftDropTimer()
    {
        softDropDelayTimer = 0f;
        currentResets--;
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
