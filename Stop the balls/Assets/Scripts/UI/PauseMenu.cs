using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public void HandleResumeClick()
    {
        GameManager.Instance.TogglePause();
    }

    public void HandleOptionsClick()
    {
        UIManager.Instance.SetActiveOptionsMenu(true);
        UIManager.Instance.SetActivePauseMenu(false);
    }

    public void HandleExitClick()
    {
        GameManager.Instance.RestartGame();
    }
}
