using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoFallingTrigger : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] private Vector3 respawnPosition;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameObject player = other.gameObject;
            {
                // Teleport the player to the respawn point
                player.transform.position = respawnPosition;
                player.transform.rotation = Quaternion.identity; // Optional: Reset rotation
            }
        }
    }

    public void SetSpawnPoint(Vector3 newRespawnPosition)
    {
        respawnPosition = newRespawnPosition;
    }

}
