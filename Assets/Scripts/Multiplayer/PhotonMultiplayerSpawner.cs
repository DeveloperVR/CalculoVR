using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PhotonMultiplayerSpawner : MonoBehaviour
{

    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject testCubePrefab; // Assuming you have a prefab for the test cube
    // Start is called before the first frame update
    void Start()
    {
        SpawnPlayer(0); // ActorNumber starts at 1, so we subtract 1 to match array index
        SpawnTestCube(); // Spawning a test cube at the first spawn point
    }

    public void SpawnPlayer(int playerIndex)
    {
        if (playerIndex < 0 || playerIndex >= spawnPoints.Length)
        {
            Debug.LogError("Player index out of bounds for spawn points array.");
            return;
        }
        Transform spawnPoint = spawnPoints[playerIndex];
        PhotonNetwork.Instantiate(playerPrefab.name, spawnPoint.position, spawnPoint.rotation);
    }

    public void SpawnTestCube()
    {
        Transform spawnPoint = spawnPoints[1]; // Assuming you want to spawn at the second spawn point
        GameObject testCube = PhotonNetwork.Instantiate(testCubePrefab.name, spawnPoint.position, spawnPoint.rotation);
        testCube.transform.localScale = new Vector3(1, 1, 1);
    }
}
