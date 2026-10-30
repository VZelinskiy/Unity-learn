using UnityEngine;

public class GameOverState: IState
{
    private readonly GameManager _gameManager;

    public GameOverState(GameManager gameManager)
    {
        _gameManager = gameManager;
    }

    public void Enter()
    {
        Time.timeScale = 0;

        if (_gameManager.GameOverPanel != null)
        {
            _gameManager.GameOverPanel.SetActive(true);
        }

        Debug.Log("Entered GameOver State");
    }

    public void Exit()
    {
        
    }

    public void Update()
    {
        
    }
}
