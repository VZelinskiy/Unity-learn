using UnityEngine;

public class VictoryState: IState
{
    private readonly GameManager _gameManager;

    public VictoryState(GameManager gameManager)
    {
        _gameManager = gameManager;
    }

    public void Enter()
    {
        Time.timeScale = 0;

        if (_gameManager.VictoryPanel != null)
        {
            _gameManager.VictoryPanel.SetActive(true);
        }

        Debug.Log("Entered Victory State");
    }

    public void Exit()
    {
        
    }

    public void Update()
    {
        
    }
}
