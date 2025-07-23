using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class TeleportPlayerToSurface : MonoBehaviourPun
{
    private GameObject Player;
    private GameObject Surface;

    [PunRPC]
    public void TeleportPlayer()
    {
        Surface = GameObject.Find("FunctionSurface");
        Player.transform.position = Surface.transform.position + new Vector3(0, 3, 0); // Adjust the Y position to be above the surface
        Player.transform.rotation = Quaternion.identity; // Reset rotation to default
    }

    public void SetPlayer()
    {
        Player = FRGameManager.Instance.LocalPlayer;
        if (Player == null)
        {
            Debug.LogError("XR Origin (XR Rig) not found in the scene.");
        }
        else
        {
            Debug.Log("XR Origin (XR Rig) found: " + Player.name);
        }
    }

    public void callTeleport()
    { 
        photonView.RPC("TeleportPlayer", RpcTarget.AllBuffered);
    }
}
