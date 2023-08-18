using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public IdealSpawnSide idealSpawnSide;

    public List<Transform> allSpawnPoints;
    public List<Transform> leftSpawnPoints;
    public List<Transform> rightSpawnPoints;

    private NetworkRunnerHandler networkRunnerHandler;

    private void Start()
    {
        networkRunnerHandler = FindObjectOfType<NetworkRunnerHandler>();

        StartCoroutine(CheckForClosePlayersEverySecond());
    }

    public Transform GetSpawnPoint()
    {
        CheckForClosePlayers();

        if (idealSpawnSide == IdealSpawnSide.Left)
        {
            int index = Random.Range(0, leftSpawnPoints.Count);

            return leftSpawnPoints[index];

        }
        else if (idealSpawnSide == IdealSpawnSide.Right)
        {
            int index = Random.Range(0, rightSpawnPoints.Count);

            return rightSpawnPoints[index];
        }
        else
        {
            int index = Random.Range(0, allSpawnPoints.Count);

            return allSpawnPoints[index];
        }
    }

    public enum IdealSpawnSide
    {
        All,
        Left,
        Right
    }

    private void CheckForClosePlayers()
    {
        idealSpawnSide = IdealSpawnSide.All;

        foreach (PlayerRef player in networkRunnerHandler.networkRunner.ActivePlayers)
        {
            if (player != networkRunnerHandler.networkRunner.LocalPlayer)
            {
                NetworkObject playerObject = networkRunnerHandler.networkRunner.GetPlayerObject(player);

                if (playerObject != null)
                {
                    //Debug.Log(playerObject.transform.position);
                    if (playerObject.transform.position.z >= 0)
                    {
                        idealSpawnSide = IdealSpawnSide.Right;
                    }
                    else
                    {
                        idealSpawnSide = IdealSpawnSide.Left;
                    }
                }
            }
        }
    }

    IEnumerator CheckForClosePlayersEverySecond()
    {
        yield return new WaitForSeconds(1f);

        CheckForClosePlayers();

        StartCoroutine(CheckForClosePlayersEverySecond());
    }
}
