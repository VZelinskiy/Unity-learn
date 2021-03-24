using UnityEngine;

public class GameSceneBackgroundMusic : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;

    private void Awake()
    {
        GameManager.Instance.OnGameStateChange += HandleGameStateChange;
    }

    private void HandleGameStateChange(GameManager.GameState currentState, GameManager.GameState previousState)
    {
        if (currentState == GameManager.GameState.RUNNING)
        {
            audioSource.Play();
        }
        else
        {
            audioSource.Pause();
        }
    }

    private void OnDestroy()
    {
        GameManager.Instance.OnGameStateChange -= HandleGameStateChange;
    }
}
