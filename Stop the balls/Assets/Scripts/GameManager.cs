using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public enum GameState
    {
        PREGAME,
        RUNNING,
        PAUSED,
        GAME_OVER
    }

    public event Action<GameState, GameState> OnGameStateChange;

    [SerializeField] GameObject[] systemPrefabs;
    [SerializeField] string gameSceneName;

    private List<AsyncOperation> loadOperations;
    private List<GameObject> instancedSystemPrefabs;
    private GameState currentGameState = GameState.PREGAME;
    private string currentSceneName = string.Empty;
    private const float gameOverDelay = 0.5f;

    public GameState CurrentGameState
    {
        get { return currentGameState; }
    }

    // Start is called before the first frame update
    private void Start()
    {
        DontDestroyOnLoad(gameObject);

        instancedSystemPrefabs = new List<GameObject>();
        loadOperations = new List<AsyncOperation>();

        InstantiateSystemPrefab();

        UIManager.Instance.OnMainMenuFadeComplete += HandleMainMenuFadeComplete;
    }

    private void Update()
    {
        if (currentGameState == GameState.PREGAME)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.Escape) && !UIManager.Instance.IsOptionMenuActive())
        {
            TogglePause();
        }
    }

    private void UpdateState(GameState state)
    {
        GameState previousGameState = currentGameState;
        currentGameState = state;

        switch (currentGameState)
        {
            case GameState.PREGAME:
                Time.timeScale = 1;
                break;
            case GameState.RUNNING:
                Time.timeScale = 1;
                break;
            case GameState.PAUSED:
                Time.timeScale = 0;
                break;
            case GameState.GAME_OVER:
                Time.timeScale = 0;
                break;
            default:
                break;
        }

        OnGameStateChange?.Invoke(currentGameState, previousGameState);
    }

    private void HandleMainMenuFadeComplete(bool isFadeOut)
    {
        if (!isFadeOut)
        {
            UnloadScene(currentSceneName);
        }
    }

    private void InstantiateSystemPrefab()
    {
        GameObject prefabInstance;
        for (int i = 0; i < systemPrefabs.Length; i++)
        {
            prefabInstance = Instantiate(systemPrefabs[i]);
            instancedSystemPrefabs.Add(prefabInstance);
        }
    }

    private void LoadScene(string sceneName)
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        if (asyncOperation == null)
        {
            Debug.LogError("[GameManager] Unable to load scene " + sceneName);
            return;
        }

        asyncOperation.completed += OnLoadOperationComplete;
        loadOperations.Add(asyncOperation);
        currentSceneName = sceneName;
    }

    private void UnloadScene(string sceneName)
    {
        AsyncOperation asyncOperation = SceneManager.UnloadSceneAsync(sceneName);
        if (asyncOperation == null)
        {
            Debug.LogError("[GameManager] Unable to unload scene " + sceneName);
            return;
        }
    }

    private void OnLoadOperationComplete(AsyncOperation asyncOperation)
    {
        if (loadOperations.Contains(asyncOperation))
        {
            loadOperations.Remove(asyncOperation);
            if (loadOperations.Count == 0)
            {
                UpdateState(GameState.RUNNING);
            }
        }
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        for (int i = 0; i < instancedSystemPrefabs.Count; i++)
        {
            Destroy(instancedSystemPrefabs[i]);
        }
        instancedSystemPrefabs.Clear();
    }

    public void StartGame()
    {
        LoadScene(gameSceneName);
    }

    public void RestartGame()
    {
        UpdateState(GameState.PREGAME);
    }

    public IEnumerator GameOver()
    {
        yield return new WaitForSeconds(gameOverDelay);
        UpdateState(GameState.GAME_OVER);
    }

    public void TogglePause()
    {
        UpdateState(CurrentGameState == GameState.RUNNING ? GameState.PAUSED : GameState.RUNNING);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
