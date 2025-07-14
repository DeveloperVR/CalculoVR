using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class CXRNetworkSetup : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private GameObject leftHand;
    [SerializeField] private GameObject rightHand;
    [SerializeField] private GameObject LocomotionSystem;


    void Awake()
    { 

    }
    void Start()
    {

        PhotonView photonView = GetComponent<PhotonView>();
        if (photonView.IsMine)
        {
            mainCamera.gameObject.SetActive(true);
            leftHand.SetActive(true);
            rightHand.SetActive(true);
            LocomotionSystem.SetActive(true);
        }
    }

}
