using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoFallingTrigger : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] private Transform respawnPoint;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameObject player = other.gameObject;
            {
                // Teleport the player to the respawn point
                player.transform.position = respawnPoint.position;
                player.transform.rotation = respawnPoint.rotation; // Optional: Reset rotation
            }
        }
    }

    private void SetSpawnPoint(Transform newRespawnPoint)
    {
        respawnPoint = newRespawnPoint;
    }

}
