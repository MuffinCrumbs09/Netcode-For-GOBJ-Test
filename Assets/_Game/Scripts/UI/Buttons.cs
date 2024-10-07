using UnityEngine;
using UnityEngine.UI;

// Handles non-network main menu buttons
public class Buttons : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private Button quitGame;

    private void Start()
    {
        // Setup buttons
        quitGame.onClick.AddListener(QuitGame);
    }
    
    // Quit application
    public void QuitGame()
    {
        Application.Quit();
    }
}
