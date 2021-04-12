using System;

public class EventBroker
{
    public static event Action<BallController, BallController, bool> LaunchedBallCollsionWithQueue;

    public static void CallLaunchedBallCollsionWithQueue(BallController launchedBall, BallController ballInQueue, bool isCollisionFront)
    {
        LaunchedBallCollsionWithQueue?.Invoke(launchedBall, ballInQueue, isCollisionFront);
    }
}
