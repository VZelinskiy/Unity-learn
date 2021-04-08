using System;

public class EventBroker
{
    public static event Action<BallController, BallController> LaunchedBallCollsionWithQueue;

    public static void CallLaunchedBallCollsionWithQueue(BallController launchedBall, BallController ballInQueue)
    {
        LaunchedBallCollsionWithQueue?.Invoke(launchedBall, ballInQueue);
    }
}
