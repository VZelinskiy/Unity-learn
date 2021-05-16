using System;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public event Action<int> ScoreUpdated;

    [SerializeField] QueueController queueController;
    [SerializeField] AudioSource backgroundMusic;

    private int totalPoints;
    private readonly int ballValue = 1;

    // Start is called before the first frame update
    void Start()
    {
        queueController.BallsDestroyed += BallsDestroyedHandler;
        GameManager.Instance.OnGameStateChange += OnGameStateChangeHandler;
    }

    private void OnGameStateChangeHandler(GameManager.GameState currentState, GameManager.GameState previousState)
    {
        if (currentState == GameManager.GameState.RUNNING)
        {
            backgroundMusic.Play();
        }
        else
        {
            backgroundMusic.Pause();
        }
    }

    private void BallsDestroyedHandler(int destroyedBallsCount)
    {
        totalPoints += (ballValue * destroyedBallsCount);
        ScoreUpdated?.Invoke(totalPoints);
    }

    private void OnDestroy()
    {
        queueController.BallsDestroyed -= BallsDestroyedHandler;
        GameManager.Instance.OnGameStateChange -= OnGameStateChangeHandler;
    }
}
