using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Pun.Demo.Cockpit;
using UnityEngine;

public class CartMovementManager : MonoBehaviourPun
{
    [SerializeField] private GameObject cart;
    [SerializeField] private GameObject Player;
    [SerializeField] public bool isMoving { get; private set; } = false;

    public void SetPlayer()
    {
        Player = RCGameManager.Instance.LocalPlayer;
        if (Player == null)
        {
            Debug.LogError("XR Origin (XR Rig) not found in the scene.");
        }
    }

    public void FollowCartMovement()
    {
        while (isMoving == true)
        {
            if (cart != null && Player != null)
            {
                Player.transform.position = cart.transform.position;
                Player.transform.rotation = Quaternion.Slerp(Player.transform.rotation, cart.GetComponent<MovementSpeed1>().getPlayerRotation(), cart.GetComponent<MovementSpeed1>().getRotationSpeed() * Time.deltaTime * cart.GetComponent<MovementSpeed1>().getCurrentVelocity());
            }
            else
            {
                Debug.LogError("Cart or Player is not set.");
            }
        }

    }
    
    [PunRPC]
    public void SetisMoving(bool value)
    {
        isMoving = value;
    }


}
