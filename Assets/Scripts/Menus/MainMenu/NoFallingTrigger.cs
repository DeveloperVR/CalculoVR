using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoFallingTrigger : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] GameObject respawnPoint;
    GameObject Surface;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameObject player = other.gameObject;
            {
                // Teleport the player to the respawn point
                player.transform.position = Surface.transform.position + new Vector3(0, 3, 0);
            }
        }
    }

    public void SetSpawnPoint(Vector3 newRespawnPosition)
    {
        respawnPoint.transform.position = newRespawnPosition;
    }

    public void SetSurface(GameObject surface)
    {
        Surface = surface;
    }

}
