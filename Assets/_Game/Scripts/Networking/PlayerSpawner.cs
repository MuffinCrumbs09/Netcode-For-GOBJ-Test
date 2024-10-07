using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSpawner : NetworkBehaviour
{
    [SerializeField] private GameObject player;

    private void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    public override void OnNetworkSpawn()
    {
        NetworkManager.Singleton.SceneManager.OnLoadEventCompleted += SceneLoaded;
    }

    private void SceneLoaded(string sceneName, LoadSceneMode loadSceneMode, List<ulong> clientsCompleted, List<ulong> clientsTimedOut)
    {
        if(IsHost && sceneName == "SampleScene")
        {
            foreach (ulong id in clientsCompleted)
            {
                GameObject _player = Instantiate(player);
                _player.GetComponent<NetworkObject>().SpawnAsPlayerObject(id, true);
                RenamePlayerRpc();
            }
        }
    }

    [Rpc(SendTo.Everyone)]
    public void RenamePlayerRpc()
    {
        GameObject[] _players = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject _player in _players)
        {
            ulong id = _player.GetComponent<NetworkBehaviour>().OwnerClientId;
            _player.name = string.Format("Player {0}", id);
        }
    }
}
