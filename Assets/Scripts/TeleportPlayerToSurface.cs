using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportPlayerToSurface : MonoBehaviour
{
    private GameObject Player;
    private GameObject Surface;


    void Start()
    {
        Player = GameObject.Find("XR Origin (XR Rig)");
        if (Player == null)
        {
            Debug.LogError("Local Player not found in the scene.");
            return;
        }
    }
    public void TeleportPlayer()
    {
        Surface = GameObject.Find("MeshTest");
        Player.transform.position = Surface.transform.position + new Vector3(0, 1, 0); // Adjust the Y position to be above the surface
        Player.transform.rotation = Quaternion.identity; // Reset rotation to default
    }
}
