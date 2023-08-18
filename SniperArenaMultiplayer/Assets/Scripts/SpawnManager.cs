using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public List<Transform> allSpawnPoints;

    private void Start()
    {
        
    }

    public Transform GetSpawnPoint()
    {
        int index = Random.Range(0, allSpawnPoints.Count);

        return allSpawnPoints[index];
    }
}
