using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Simulation;

public class VisibleHandsMovement : MonoBehaviourPun
{
    [SerializeField] private GameObject HandInteractor;
    [SerializeField] private GameObject HandModel;

    void Update()
    {
        photonView.RPC("moveHands", RpcTarget.AllBuffered);
    }

    [PunRPC]
    private void moveHands()
    {
        HandModel.transform.position = HandInteractor.transform.position;
        HandModel.transform.rotation = HandInteractor.transform.rotation;
    }
}
