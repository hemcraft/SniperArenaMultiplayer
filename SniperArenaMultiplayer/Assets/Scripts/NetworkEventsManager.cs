using Fusion;
using Fusion.Sockets;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkEventsManager : MonoBehaviour, INetworkRunnerCallbacks
{
    public NetworkObject NetworkPlayerManagerPrefab;

    private NetworkObject networkPlayerManager;
    private SpawnManager spawnManager;
    private UserInterface userInterface;

    private void Start()
    {
        spawnManager = FindObjectOfType<SpawnManager>();
        userInterface = FindObjectOfType<UserInterface>();
    }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        Debug.Log("OnPlayerJoined");

        if (runner.LocalPlayer.PlayerId == player.PlayerId)
        {
            Debug.Log(runner.LocalPlayer.PlayerId);
            networkPlayerManager = runner.Spawn(
                    NetworkPlayerManagerPrefab,
                    spawnManager.GetSpawnPoint().position,
                    Quaternion.identity,
                    player);

            runner.SetPlayerObject(player, networkPlayerManager);

            networkPlayerManager.gameObject.name = "NetworkPlayerManager - " + player.PlayerId + " -> " +
                        DateTime.UtcNow.Hour.ToString() + ":" + DateTime.UtcNow.Minute.ToString() + ":" + DateTime.UtcNow.Second.ToString();

            networkPlayerManager.GetComponent<NetworkPlayerRig>().SetEditorName();
            userInterface.ShowMessage("Joined session\nWaiting for players");
        }
        else
        {
            userInterface.ShowMessage("New Player joined");
        }
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        Debug.Log("OnPlayerLeft");

        userInterface.EnemyPlayerLeft(runner);
        userInterface.ShowMessage("Player left");
    }

    public void OnConnectedToServer(NetworkRunner runner)
    {
        Debug.LogWarning("OnConnectedToServer");
    }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {
        Debug.LogWarning("OnConnectFailed");
    }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {
        Debug.LogWarning("OnConnectRequest");
    }

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
    {
        Debug.LogWarning("OnCustomAuthenticationResponse");
    }

    public void OnDisconnectedFromServer(NetworkRunner runner)
    {
        Debug.LogWarning("OnDisconnectedFromServer");
    }

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {
        Debug.LogWarning("OnHostMigration");
    }

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        //Debug.LogWarning("OnInput");
    }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    {
        Debug.LogWarning("OnInputMissing");
    }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data)
    {
        Debug.LogWarning("OnReliableDataReceived");
    }

    public void OnSceneLoadDone(NetworkRunner runner)
    {
        Debug.LogWarning("OnSceneLoadDone");
    }

    public void OnSceneLoadStart(NetworkRunner runner)
    {
        Debug.LogWarning("OnSceneLoadStart");
    }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
        Debug.LogWarning("OnSessionListUpdated");
    }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {
        Debug.Log("OnShutdown");
    }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {
        Debug.LogWarning("OnUserSimulationMessage");
    }
}
