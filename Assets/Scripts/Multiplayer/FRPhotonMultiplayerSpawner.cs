using System.Collections;
using UnityEngine;
using Photon.Pun;

public class FRPhotonMultiplayerSpawner : MonoBehaviour
{

    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private GameObject VRplayerPrefab;
    [SerializeField] private GameObject NonVRplayerPrefab;
    private bool hasSpawned = false;
    private GameObject PlayerEvent;
    private GameObject RigSwitcherObject;
    // Start is called before the first frame update

    void Start()
    {
        StartCoroutine(WaitForConnectionReady());
        RigSwitcherObject = GameObject.Find("RigSwitcher");
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
        if (RigSwitcherObject.GetComponent<RigSwitcherMultiplayer>().getisXRActivated())
        {
            PhotonNetwork.Instantiate(VRplayerPrefab.name, spawnPoint.position, spawnPoint.rotation);
            hasSpawned = true;
            PlayerEvent = GameObject.Find("PlayerSpawnEvent");
            PlayerEvent.GetComponent<FreeRoamPlayerSpawnEvent>().playerSpawned = true;
            PlayerEvent.GetComponent<FreeRoamPlayerSpawnEvent>().isVRPlayer = true;
        }
        else
        {
            PhotonNetwork.Instantiate(NonVRplayerPrefab.name, spawnPoint.position, spawnPoint.rotation);
            hasSpawned = true;
            PlayerEvent = GameObject.Find("PlayerSpawnEvent");
            PlayerEvent.GetComponent<FreeRoamPlayerSpawnEvent>().playerSpawned = true;
            PlayerEvent.GetComponent<FreeRoamPlayerSpawnEvent>().isNonVRPlayer = true;
        }
        

    }

    IEnumerator WaitForConnectionReady()
    {
        while (!PhotonNetwork.InRoom && !PhotonNetwork.IsConnectedAndReady)
        {
            yield return null;
        }
        yield return new WaitForSeconds(1f);
        SpawnPlayer(0); // Spawn player after connection is ready
    }

}
