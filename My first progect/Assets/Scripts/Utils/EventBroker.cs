using System;

public class EventBroker
{
    public static event Action<int> UpdateScoreForFood;
    public static event Action<int> LifeLost;
    public static event Action<int> LifeGain;

    public static void CallUpdateScoreForFood(int value)
    {
        UpdateScoreForFood?.Invoke(value);
    }

    public static void CallLifeLost(int lives)
    {
        LifeLost?.Invoke(lives);
    }

    public static void CallLifeGain(int lives)
    {
        LifeGain?.Invoke(lives);
    }
}
