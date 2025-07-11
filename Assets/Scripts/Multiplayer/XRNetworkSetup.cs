using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private GameObject leftHand;
    [SerializeField] private GameObject rightHand;

    void Start()
    {

        PhotonView photonView = GetComponent<PhotonView>();
        if (photonView.IsMine)
        {
            mainCamera.gameObject.SetActive(false);
            leftHand.SetActive(false);
            rightHand.SetActive(false);
        }
        else
        {
            mainCamera.gameObject.SetActive(true);
            leftHand.SetActive(true);
            rightHand.SetActive(true);
        }
    }

}
