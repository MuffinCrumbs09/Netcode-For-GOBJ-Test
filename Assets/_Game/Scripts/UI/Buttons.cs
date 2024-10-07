using UnityEngine;
using UnityEngine.UI;

public class Buttons : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private Button quitGame;

    private void Start()
    {
        quitGame.onClick.AddListener(QuitGame);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
