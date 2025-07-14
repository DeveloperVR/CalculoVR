using System.Collections;
using UnityEngine;
using Photon.Pun;

public class PhotonMultiplayerSpawner : MonoBehaviour
{

    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private GameObject playerPrefab;
<<<<<<< Updated upstream
<<<<<<< Updated upstream
<<<<<<< Updated upstream
    // Start is called before the first frame update
    void Start()
    {

=======
    [SerializeField] private GameObject testCubePrefab; // Assuming you have a prefab for the test cube
    private bool hasSpawned = false;
    private bool CubeHasSpawned = false;
    void Start()
    {
        StartCoroutine(WaitForConnectionReady());
>>>>>>> Stashed changes
=======
    [SerializeField] private GameObject testCubePrefab; // Assuming you have a prefab for the test cube
    private bool hasSpawned = false;
    private bool CubeHasSpawned = false;
    void Start()
    {
        StartCoroutine(WaitForConnectionReady());
>>>>>>> Stashed changes
=======
    [SerializeField] private GameObject testCubePrefab; // Assuming you have a prefab for the test cube
    private bool hasSpawned = false;
    private bool CubeHasSpawned = false;
    void Start()
    {
        StartCoroutine(WaitForConnectionReady());
>>>>>>> Stashed changes
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
    }
<<<<<<< Updated upstream
=======

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

    IEnumerator WaitForConnectionReady()
    {
        while (!PhotonNetwork.InRoom && !PhotonNetwork.IsConnectedAndReady)
        {
            yield return null;
        }
        yield return new WaitForSeconds(0.1f);
        SpawnPlayer(0); // Spawn player after connection is ready
        SpawnTestCube(); // Spawn test cube after connection is ready
    }
<<<<<<< Updated upstream
<<<<<<< Updated upstream
>>>>>>> Stashed changes
=======
>>>>>>> Stashed changes
=======
>>>>>>> Stashed changes
}
