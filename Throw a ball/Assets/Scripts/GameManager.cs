using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public bool isGameActive;

    [SerializeField] Button restartButton;
    [SerializeField] TextMeshProUGUI timeText;

    private float gameTime;

    private void Start()
    {
        isGameActive = true;
        gameTime = 15;
        StartCoroutine(Timer());
    }

    IEnumerator Timer()
    {
        while (gameTime > 0)
        {
            yield return new WaitForSeconds(1);
            gameTime -= 1;
            timeText.text = "Time: " + gameTime;
        }
        GameOver();
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GameOver()
    {
        restartButton.gameObject.SetActive(true);
        isGameActive = false;
    }
}
