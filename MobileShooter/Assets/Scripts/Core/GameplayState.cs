using UnityEngine;

public class GameplayState: IState
{
    private readonly GameManager _gameManager;

    public GameplayState(GameManager gameManager)
    {
        _gameManager = gameManager;
    }

    public void Enter()
    {
        Time.timeScale = 1.0f;

        if (_gameManager.GameOverPanel != null) _gameManager.GameOverPanel.SetActive(false);
        if (_gameManager.VictoryPanel != null) _gameManager.VictoryPanel.SetActive(false);

        Debug.Log("Entered Gameplay State");
    }

    public void Exit()
    {
        
    }

    public void Update()
    {
        
    }
}
