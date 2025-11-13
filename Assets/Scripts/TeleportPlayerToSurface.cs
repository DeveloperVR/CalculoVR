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

    //[PunRPC]
    public void TeleportPlayer()
    {
        Surface = GameObject.FindWithTag("Superficie1");
        Player.transform.position = functionInstantiator.OriginPoint;
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
        TeleportPlayer();
    }

    public void standardTeleport()
    {
        Player.transform.position = predeterminedTelepor.transform.position;
        PredeterminedSurfacesMenu.SetActive(true);
    }
}
