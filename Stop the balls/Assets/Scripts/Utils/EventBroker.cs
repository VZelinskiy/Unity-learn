using System;
using UnityEngine;

public class EventBroker
{
    public static event Action<BallController, BallController, bool> LaunchedBallCollsionWithQueue;
    public static event Action GameOver;
    //public static event Action<Vector3> BallIsLaunched;

    public static void CallLaunchedBallCollsionWithQueue(BallController launchedBall, BallController ballInQueue, bool isCollisionFront)
    {
        LaunchedBallCollsionWithQueue?.Invoke(launchedBall, ballInQueue, isCollisionFront);
    }

    public static void CallGameOver()
    {
        GameOver?.Invoke();
    }

    /*public static void CallBallIsLaunched(Vector3 direction)
    {
        BallIsLaunched?.Invoke(direction);
    }*/
}
