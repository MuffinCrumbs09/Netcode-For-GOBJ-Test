using UnityEngine;
using UnityEngine.UI;

// Handles non-network main menu buttons
public class Buttons : MonoBehaviour
{
    [Header("Settings - Buttons")]
    [SerializeField] private Button quitButton;
    [SerializeField] private Button menuButton;
    [SerializeField] private Button cosButton;

    [Header("Settings - Canvas")]
    [SerializeField] private Canvas cosmeticCanvas;
    [SerializeField] private Canvas menuCanvas;

    private void Start()
    {
        // Setup buttons
        quitButton.onClick.AddListener(QuitGame);
        menuButton.onClick.AddListener(() => SwitchCanvas(cosmeticCanvas, menuCanvas));
        cosButton.onClick.AddListener(() => SwitchCanvas(menuCanvas, cosmeticCanvas));
    }
    
    // Quit application
    public void QuitGame()
    {
        Application.Quit();
    }

    public void SwitchCanvas(Canvas curCanvas, Canvas newCanvas)
    {
        curCanvas.gameObject.SetActive(false);
        newCanvas.gameObject.SetActive(true);
    }
}
