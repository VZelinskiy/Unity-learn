using System;
using UnityEngine;

public class EventBroker
{
    public static event Action<BallController, BallController, bool> LaunchedBallCollsionWithQueue;
    public static event Action GameOver;
    public static event Action<BallController> NewBallIWasSpawned;

    public static void CallLaunchedBallCollsionWithQueue(BallController launchedBall, BallController ballInQueue, bool isCollisionFront)
    {
        LaunchedBallCollsionWithQueue?.Invoke(launchedBall, ballInQueue, isCollisionFront);
    }

    public static void CallGameOver()
    {
        GameOver?.Invoke();
    }

    public static void CallNewBallIWasSpawned(BallController spawnedBall)
    {
        NewBallIWasSpawned?.Invoke(spawnedBall);
    }
}
