using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] Image[] heartImages;
    
    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.OnGameStateChange += HandleGameStateChange;
        EventBroker.UpdateScoreForFood += UpdateScore;
        EventBroker.LifeLost += HideHeart;
        EventBroker.LifeGain += ShowHeart;
    }

    private void HandleGameStateChange(GameManager.GameState currentState, GameManager.GameState previousState)
    {
        if (previousState == GameManager.GameState.GAME_OVER && currentState == GameManager.GameState.PREGAME)
        {
            ShowAllHearts();
            UpdateScore(0);
        }
    }

    private void UpdateScore(int score)
    {
        scoreText.text = score.ToString();
    }

    private void HideHeart(int imageIndex)
    {
        heartImages[imageIndex].gameObject.SetActive(false);
    }

    private void ShowHeart(int imageIndex)
    {
        heartImages[imageIndex].gameObject.SetActive(true);
    }

    private void ShowAllHearts()
    {
        for (int i = 0; i < heartImages.Length; i++)
        {
            heartImages[i].gameObject.SetActive(true);
        }
    }
}
