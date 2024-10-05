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

public class RelayManager : MonoBehaviour
{
    [SerializeField] Button hostButton;
    [SerializeField] Button joinButton;
    [SerializeField] Button startButton;
    [SerializeField] TMP_InputField joinInput;
    [SerializeField] TextMeshProUGUI codeText;

    [SerializeField] Canvas mainMenu;
    [SerializeField] Canvas inLobby;

    private string joinCode;

    private async void Start()
    {
        await UnityServices.InitializeAsync();

        await AuthenticationService.Instance.SignInAnonymouslyAsync();

        hostButton.onClick.AddListener(CreateRelay);
        joinButton.onClick.AddListener(() => JoinRelay(joinInput.text));

        startButton.onClick.AddListener(StartGame);
    }

    private void Update()
    {
        if (GetComponent<NetworkManager>().IsHost && startButton != null)
        {
            startButton.gameObject.SetActive(true);
        }
    }

    async void CreateRelay()
    {
        Allocation allocation = await RelayService.Instance.CreateAllocationAsync(3);
        joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
        if(codeText != null)
            codeText.text = joinCode;

        var relayServerData = new RelayServerData(allocation, "dtls");

        NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);

        NetworkManager.Singleton.StartHost();
        InLobby();
    }

    async void JoinRelay(string joinCode)
    {
        var joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode);
        var relayServerData = new RelayServerData(joinAllocation, "dtls");
        NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);

        NetworkManager.Singleton.StartClient();

        InLobby();
    }

    private void InLobby()
    {
        mainMenu.gameObject.SetActive(false);
        inLobby.gameObject.SetActive(true);
    }

    public void StartGame()
    {
        NetworkManager.Singleton.SceneManager.LoadScene("SampleScene", LoadSceneMode.Single);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Main Menu") return;
        if (codeText != null) return;

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
