using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] Button resumeButton;
    [SerializeField] Button optionsButton;
    [SerializeField] Button exitButton;

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
