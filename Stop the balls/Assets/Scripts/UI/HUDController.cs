using TMPro;
using UnityEngine;

public class HUDController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] LevelManager levelManager;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.OnGameStateChange += HandleGameStateChange;
        levelManager.ScoreUpdated += UpdateScore;
    }

    private void HandleGameStateChange(GameManager.GameState currentState, GameManager.GameState previousState)
    {
        if (previousState == GameManager.GameState.GAME_OVER && currentState == GameManager.GameState.PREGAME)
        {
            UpdateScore(0);
        }
    }

    private void UpdateScore(int score)
    {
        scoreText.text = score.ToString();
    }
}
