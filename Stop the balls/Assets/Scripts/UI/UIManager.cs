using System;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    public event Action<bool> OnMainMenuFadeComplete;

    [SerializeField] MainMenu mainMenu;
    [SerializeField] PauseMenu pauseMenu;
    [SerializeField] OptionsMenu optionsMenu;
    [SerializeField] GameOverMenu gameOverMenu;
    [SerializeField] Camera mainMenuCamera;

    void Start()
    {
        GameManager.Instance.OnGameStateChange += HandleGameStateChange;
        mainMenu.OnMainMenuFadeComplete += HandleMainMenuFadeComplete;
    }

    private void HandleMainMenuFadeComplete(bool isFadeOut)
    {
        OnMainMenuFadeComplete?.Invoke(isFadeOut);
    }

    private void HandleGameStateChange(GameManager.GameState currentState, GameManager.GameState previousState)
    {
        pauseMenu.gameObject.SetActive(currentState == GameManager.GameState.PAUSED);
        gameOverMenu.gameObject.SetActive(currentState == GameManager.GameState.GAME_OVER);
    }

    public void SetActiveOptionsMenu(bool isActive)
    {
        optionsMenu.gameObject.SetActive(isActive);
    }

    public void SetActiveMainMenu(bool IsActive)
    {
        mainMenu.gameObject.SetActive(IsActive);
    }

    public void SetActivePauseMenu(bool isActive)
    {
        pauseMenu.gameObject.SetActive(isActive);
    }

    public void SetActiveMenuCamera(bool isActive)
    {
        mainMenuCamera.gameObject.SetActive(isActive);
    }

    public bool IsOptionMenuActive()
    {
        return optionsMenu.isActiveAndEnabled;
    }
}
