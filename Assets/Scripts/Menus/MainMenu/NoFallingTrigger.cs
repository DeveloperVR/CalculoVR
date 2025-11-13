using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoFallingTrigger : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] GameObject respawnPoint;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameObject player = other.gameObject;
            {
                // Teleport the player to the respawn point
                player.transform.position = respawnPoint.transform.position;
                player.transform.rotation = Quaternion.identity; // Optional: Reset rotation
            }
        }
    }

    public void SetSpawnPoint(Vector3 newRespawnPosition)
    {
        respawnPoint.transform.position = newRespawnPosition;
    }

}
