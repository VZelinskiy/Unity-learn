using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // статичний інстанс для глобального доступу
    public static GameManager Instance { get; private set; }

    [Header("References")]
    [SerializeField] private Health playerHealth;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject victoryPanel;

    public GameObject GameOverPanel => gameOverPanel;
    public GameObject VictoryPanel => victoryPanel;

    public event Action OnGameOverTriggered;
    public event Action OnVictoryTriggered;

    private StateMachine stateMachine;
    private IState gameplayState;
    private IState gameOverState;
    private IState victoryState;

    private void Awake()
    {
        // реалізація паттерну Singleton
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        //// ховаємо обилві панелі на старті
        //if (gameOverPanel != null) gameOverPanel.SetActive(false);
        //if (victoryPanel != null) victoryPanel.SetActive(false);

        //// гарантуємо, що час у нормі при запуску сцени
        //Time.timeScale = 1.0f;

        // ініціалазуємо машину станів
        stateMachine = new StateMachine();
        gameplayState = new GameplayState(this);
        gameOverState = new GameOverState(this);
        victoryState = new VictoryState(this);
    }

    private void Start()
    {
        // запускаємо гру в стані Gameplay
        stateMachine.Initialize(gameplayState);
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

    private void Update()
    {
        stateMachine.Update();
    }

    private void HandlePlayerDeath()
    {
        //Debug.Log("Game Over! Player has died.");

        //// активуємо панель Game Over
        //if (gameOverPanel != null)
        //{
        //    gameOverPanel.SetActive(true);
        //}

        //// зупиняємо час у грі, щоб вороги і кулі завмерли
        //Time.timeScale = 0;

        OnGameOverTriggered?.Invoke();
        stateMachine.ChangeState(gameOverState);
    }

    // метод який буде викликатися при натисканні кнопки "Restart" у UI

    public void TriggerVictory()
    {
        //Debug.Log("Victory! all waves complited.");

        //if (victoryPanel != null)
        //{
        //    victoryPanel.SetActive(true);
        //}

        //Time.timeScale = 0;
        OnVictoryTriggered?.Invoke();
        stateMachine.ChangeState(victoryState);
    }

    public void RestartGame()
    {
        //Time.timeScale = 1.0f;

        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }
}
