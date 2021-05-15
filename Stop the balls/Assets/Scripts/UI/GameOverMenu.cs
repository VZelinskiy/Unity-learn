using UnityEngine;
using UnityEngine.UI;

public class GameOverMenu : MonoBehaviour
{
    [SerializeField] Button exitButton;

    public void HandleExitClick()
    {
        GameManager.Instance.RestartGame();
    }
}
