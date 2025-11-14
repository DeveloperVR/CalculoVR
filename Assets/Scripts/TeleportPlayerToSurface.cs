using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Unity.VisualScripting;

public class TeleportPlayerToSurface : MonoBehaviourPun
{
    [SerializeField] private GameObject Player;
    [SerializeField] private GameObject Surface;
    [SerializeField] private FunctionInstantiator functionInstantiator;
    [SerializeField] private GameObject predeterminedTelepor;
    [SerializeField] private GameObject PredeterminedSurfacesMenu;
    private bool isDesktopPlayer = false;

    //[PunRPC]
    public void TeleportPlayer()
    {
        Surface = GameObject.FindWithTag("Superficie1");
        Player.transform.position = functionInstantiator.OriginPoint;
        Player.transform.rotation = Quaternion.identity; // Reset rotation to default
    }

    public void SetPlayer(bool desktop)
    {
        Player = FRGameManager.Instance.LocalPlayer;
        if (Player == null)
        {
            Debug.LogError("XR Origin (XR Rig) not found in the scene.");
        }
        else
        {
            Debug.Log("XR Origin (XR Rig) found: " + Player.name);
            isDesktopPlayer = desktop;
        }
    }

    public void callTeleport()
    {
        TeleportPlayer();
    }
    
    public void SetSurface(GameObject surface)
    {
        Surface = surface;
    }

    public void standardTeleport()
    {
        Player.transform.position = Surface.transform.position + Vector3.up * 2f;
        PredeterminedSurfacesMenu.SetActive(true);
        ResetDesktopPlayer();
    }

    private void ResetDesktopPlayer()
    {
        if (isDesktopPlayer)
        {
            Player.GetComponent<DektopNetworkSetup>().photonView.RPC("ResetSkinToAll", RpcTarget.AllBuffered);
        }
    }
}
