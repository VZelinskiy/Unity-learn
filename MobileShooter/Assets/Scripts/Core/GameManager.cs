using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // статичний інстанс для глобального доступу
    public static GameManager Instance { get; private set; }

    [Header("References")]
    [SerializeField] private Health playerHealth;
    [SerializeField] private GameObject gameOverPanel;

    public event Action OnGameOverTriggered;

    private void Awake()
    {
        // реалізація паттерну Singleton
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        // ховаємо екран поразки на старті
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }

        // гарантуємо, що час у нормі при запуску сцени
        Time.timeScale = 1.0f;
    }

    private void OnEnable()
    {
        if (playerHealth  != null)
        {
            playerHealth.OnDie += HandlePlayerDeath;
        }
    }

    private void OnDisable()
    {
        if (playerHealth != null)
        {
            playerHealth.OnDie -= HandlePlayerDeath;
        }
    }

    private void HandlePlayerDeath()
    {
        Debug.Log("Game Over! Player has died.");

        // активуємо панель Game Over
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }

        // зупиняємо час у грі, щоб вороги і кулі завмерли
        Time.timeScale = 0;

        OnGameOverTriggered?.Invoke();
    }

    // метод який буде викликатися при натисканні кнопки "Restart" у UI
    public void RestartGame()
    {
        // обовязково повертаємо час у норму перед перезавантаженням сцени
        Time.timeScale = 1.0f;

        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }
}
