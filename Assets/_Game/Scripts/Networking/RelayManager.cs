using TMPro;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

// Handles joining, leaving and creating lobbies
public class RelayManager : MonoBehaviour
{
    [Header("Settings - Buttons")]
    [SerializeField] Button hostButton;
    [SerializeField] Button joinButton;
    [SerializeField] Button startButton;
    [SerializeField] Button quitButton;

    [Header("Settings - Text")]
    [SerializeField] TMP_InputField joinInput;
    [SerializeField] TextMeshProUGUI codeText;

    [Header("Settings - Canvas")]
    [SerializeField] Canvas mainMenu;
    [SerializeField] Canvas inLobby;

    private string joinCode;

    private async void Start()
    {
        // Init Unity Gaming Services
        await UnityServices.InitializeAsync();

        // Sigin in players anonymously
        await AuthenticationService.Instance.SignInAnonymouslyAsync();

        // Setup networking buttons
        hostButton.onClick.AddListener(CreateRelay);
        joinButton.onClick.AddListener(() => JoinRelay(joinInput.text));
        quitButton.onClick.AddListener(QuitLobby);

        // Setup start button
        startButton.onClick.AddListener(StartGame);
    }

    private void Update()
    {
        // if host and button is hidden, show button
        if (GetComponent<NetworkManager>().IsHost && startButton != null)
        {
            startButton.gameObject.SetActive(true);
        }
    }

    // Creates a relay lobby
    async void CreateRelay()
    {
        // Request an allocation on the relay serves, with expected max peers (3 peers 1 host)
        Allocation allocation = await RelayService.Instance.CreateAllocationAsync(3);

        // Gets the join code from the lobby
        joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);

        // Shows the code on screen
        if(codeText != null)
            codeText.text = joinCode;

        // Creates and sets the relay server using our allocation and connection type
        var relayServerData = new RelayServerData(allocation, "dtls");
        NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);

        // Starts the server as a host
        NetworkManager.Singleton.StartHost();

        InLobby();
    }

    // Joins a relay lopbby
    async void JoinRelay(string joinCode)
    {
        // Gets the lobby allocation, gets their relayServerData and joins based on it
        var joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode);
        var relayServerData = new RelayServerData(joinAllocation, "dtls");
        NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);

        // Starts/Joins the server as a client
        NetworkManager.Singleton.StartClient();

        InLobby();
    }

    // Switches Canvas
    private void InLobby()
    {
        mainMenu.gameObject.SetActive(false);
        inLobby.gameObject.SetActive(true);
    }

    //Loads the game scene
    public void StartGame()
    {
        NetworkManager.Singleton.SceneManager.LoadScene("SampleScene", LoadSceneMode.Single);
    }

    // Quits you out of the lobby
    public void QuitLobby()
    {
        // Quits the lobby and destroys local NetworkManager
        Destroy(NetworkManager.Singleton.gameObject);
        NetworkManager.Singleton.Shutdown();

        // Loads main menu (Creates new local NetworkManager)
        SceneManager.LoadScene(0);
    }

    // Tries to display code on scene load
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // If main menu or codeText is set, return
        if (scene.name == "Main Menu") return;
        if (codeText != null) return;

        // Try display code, if not catch the error
        try
        {
            codeText = FindAnyObjectByType<TextMeshProUGUI>();
            codeText.text = joinCode;
        }
        catch(Exception e)
        {
            Debug.LogException(e);
            Debug.Log("Unable to find codeText");
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
