using Photon.Pun;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Inputs;
using UnityEngine.SceneManagement;

public class CartManagerSetup : MonoBehaviour
{
    private void Start()
    {
        PhotonView pv = GetComponent<PhotonView>();
        Debug.Log($"[CartManager] Start() called by Player {PhotonNetwork.LocalPlayer.ActorNumber}, IsMine: {pv.IsMine}");
        if (pv.IsMine)
        {
            RCGameManager.Instance.SetCartMovementManager(gameObject);
        }
    }
}