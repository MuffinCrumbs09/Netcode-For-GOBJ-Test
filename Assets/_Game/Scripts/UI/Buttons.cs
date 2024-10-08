using UnityEngine;
using UnityEngine.UI;

// Handles non-network main menu buttons
public class Buttons : MonoBehaviour
{
    [Header("Settings - Buttons")]
    [SerializeField] private Button quitButton;
    [SerializeField] private Button menuButton;

    [Header("Settings - Canvas")]
    [SerializeField] private Canvas cosmeticCanvas;
    [SerializeField] private Canvas menuCanvas;

    private void Start()
    {
        // Setup buttons
        quitButton.onClick.AddListener(QuitGame);
        menuButton.onClick.AddListener(MainMenu);
    }
    
    // Quit application
    public void QuitGame()
    {
        Application.Quit();
    }

    public void MainMenu()
    {
        cosmeticCanvas.gameObject.SetActive(false);
        menuCanvas.gameObject.SetActive(true);
    }
}
