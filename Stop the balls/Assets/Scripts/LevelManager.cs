using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public event Action<int> ScoreUpdated;

    [SerializeField] QueueController queueController;

    private int totalPoints;
    private readonly int ballValue = 1;

    // Start is called before the first frame update
    void Start()
    {
        queueController.BallsDestroyed += BallsDestroyedHandler;
    }

    private void BallsDestroyedHandler(int destroyedBallsCount)
    {
        totalPoints += (ballValue * destroyedBallsCount);
        ScoreUpdated?.Invoke(totalPoints);
    }
}
