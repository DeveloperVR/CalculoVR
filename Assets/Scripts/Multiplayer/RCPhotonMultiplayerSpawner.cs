using System.Collections;
using UnityEngine;
using Photon.Pun;

public class PhotonMultiplayerSpawner : MonoBehaviour
{

    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject testCubePrefab; // Assuming you have a prefab for the test cube
    [SerializeField] private GameObject CartMovementManagerPrefab; // Assuming this is the cart movement manager prefab
    private bool hasSpawned = false;
    private bool CubeHasSpawned = false;
    private bool CartManagerHasSpawned = false;
    private GameObject PlayerEvent;
    // Start is called before the first frame update

    void Start()
    {
        StartCoroutine(WaitForConnectionReady());
    }

    public void SpawnPlayer(int playerIndex)
    {
        if (hasSpawned)
        {
            Debug.LogWarning("Player has already been spawned.");
            return;
        }
        if (playerIndex < 0 || playerIndex >= spawnPoints.Length)
        {
            Debug.LogError("Player index out of bounds for spawn points array.");
            return;
        }
        Transform spawnPoint = spawnPoints[playerIndex];
        PhotonNetwork.Instantiate(playerPrefab.name, spawnPoint.position, spawnPoint.rotation);
        hasSpawned = true;
        PlayerEvent = GameObject.Find("PlayerSpawnEvent");
        PlayerEvent.GetComponent<PlayerSpawnEvent>().playerSpawned = true;

    }


    public void SpawnTestCube()
    {
        if (CubeHasSpawned)
        {
            Debug.LogWarning("Test cube has already been spawned.");
            return;
        }
        Transform spawnPoint = spawnPoints[1]; // Assuming you want to spawn at the second spawn point
        GameObject testCube = PhotonNetwork.Instantiate(testCubePrefab.name, spawnPoint.position, spawnPoint.rotation);
        testCube.transform.localScale = new Vector3(1, 1, 1);
    }

    public void SpawnMovementManager()
    {
        if (CartManagerHasSpawned)
        {
            Debug.LogWarning("CartMovementManager has already been spawned.");
            return;
        }
        Transform spawnPoint = spawnPoints[1]; // Assuming you want to spawn at the third spawn point
        GameObject cartManager = PhotonNetwork.Instantiate(CartMovementManagerPrefab.name, spawnPoint.position, spawnPoint.rotation);
    }

    IEnumerator WaitForConnectionReady()
    {
        while (!PhotonNetwork.InRoom && !PhotonNetwork.IsConnectedAndReady)
        {
            yield return null;
        }
        yield return new WaitForSeconds(1f);
        SpawnPlayer(0); // Spawn player after connection is ready
        SpawnTestCube(); // Spawn test cube after connection is ready
        SpawnMovementManager(); // Spawn cart movement manager after connection is ready
    }

}
