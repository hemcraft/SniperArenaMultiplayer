using Fusion;
using Fusion.Sockets;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetworkRunnerHandler : MonoBehaviour
{
    public NetworkRunner NetworkRunnerPrefab;

    public NetworkRunner networkRunner;

    private void Start()
    {
        Debug.LogError("Start Server");
        StartServer();
    }

    public void StartServer()
    {
        networkRunner = Instantiate(NetworkRunnerPrefab);
        networkRunner.name = "NetworkRunnerServer";

        var clientTask = IniTializeNetworkRunner(
            networkRunner,
            GameMode.Shared,
            NetAddress.Any(),
            SceneManager.GetActiveScene().buildIndex,
            null);
    }

    protected virtual Task IniTializeNetworkRunner(NetworkRunner runner, GameMode gamemode, NetAddress adress, SceneRef scene, Action<NetworkRunner> initialized)
    {
        var sceneObjectProvider = runner.GetComponents(typeof(MonoBehaviour)).OfType<INetworkSceneManager>().FirstOrDefault();

        if (sceneObjectProvider == null)
        {
            sceneObjectProvider = runner.gameObject.AddComponent<NetworkSceneManagerDefault>();
        }

        runner.ProvideInput = true;

        return runner.StartGame(new StartGameArgs 
        { 
            GameMode = gamemode,
            Address = adress,
            Scene = scene,
            PlayerCount = 2,
            SessionName = null,
            Initialized = initialized,
            SceneManager = sceneObjectProvider
        });
    }
}
