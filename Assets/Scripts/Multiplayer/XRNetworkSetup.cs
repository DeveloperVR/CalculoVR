using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Inputs;

public class CXRNetworkSetup : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private GameObject leftHand;
    [SerializeField] private GameObject leftHandStabilized;
    [SerializeField] private GameObject rightHand;
    [SerializeField] private GameObject rightHandStabilized;
    [SerializeField] private GameObject LocomotionSystem;
    [SerializeField] private GameObject skin;
    [SerializeField] private CharacterControllerDriver characterControllerDriver;
    [SerializeField] private XRInputModalityManager xrInputModalityManager;
    [SerializeField] private XRGazeAssistance xrGazeAssistance;
    [SerializeField] private OnButtonPress onButtonPress;


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
            rightHandStabilized.SetActive(true);
            leftHandStabilized.SetActive(true);

            characterControllerDriver.enabled = true;
            xrInputModalityManager.enabled = true;
            xrGazeAssistance.enabled = true;
            onButtonPress.enabled = true;
        }
        else
        {
            skin.SetActive(true);
        }
    }

}
