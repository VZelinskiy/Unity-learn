using System;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public event Action<bool> OnMainMenuFadeComplete;

    private void Start()
    {
        GameManager.Instance.OnGameStateChange += HandleGameStateChange;
    }

    private void HandleGameStateChange(GameManager.GameState currentState, GameManager.GameState previousState)
    {
        if (previousState == GameManager.GameState.PREGAME && currentState == GameManager.GameState.RUNNING)
        {
            UIManager.Instance.SetActiveMainMenu(false);
            UIManager.Instance.SetActiveMenuCamera(false);
            OnMainMenuFadeComplete?.Invoke(true);
        }
        if (previousState != GameManager.GameState.PREGAME && currentState == GameManager.GameState.PREGAME)
        {
            UIManager.Instance.SetActiveMainMenu(true);
            OnMainMenuFadeComplete?.Invoke(false);
            UIManager.Instance.SetActiveMenuCamera(true);
        }
    }

    public void HandleStartClick()
    {
        GameManager.Instance.StartGame();
    }

    public void HandleOptionsClick()
    {
        UIManager.Instance.SetActiveOptionsMenu(true);
        UIManager.Instance.SetActiveMainMenu(false);
    }

    public void HandleExitClick()
    {
        GameManager.Instance.QuitGame();
    }
}
