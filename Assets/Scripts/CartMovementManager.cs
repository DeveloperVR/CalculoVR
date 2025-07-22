using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Pun.Demo.Cockpit;
using UnityEngine;

public class CartMovementManager : MonoBehaviourPun
{
    public static CartMovementManager Instance { get; private set; }
    private GameObject cart;
    [SerializeField] private GameObject Player;
    public bool isMoving { get; private set; } = false;
    public bool isFollowing { get; private set; } = false;
    private GameObject PlayerAfterCartPosition;

    public void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }
    public void Start()
    {
        PlayerAfterCartPosition = GameObject.Find("AfterCartPlace");
        if (PlayerAfterCartPosition == null)
        {
            Debug.LogError("PlayerAfterCartPlace not found in the scene.");
        }
        cart = GameObject.Find("Carrito1");
        SetPlayer();

    }
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

    void Update()
    {
        if (isMoving && isFollowing)
        {
            FollowCartMovement();
        }
    }

    [PunRPC]
    public void SetisMoving(bool value)
    {
        isMoving = value;
        Debug.Log("Cart movement state changed: " + isMoving);
    }

    public void SetisFollowing(bool value)
    {
        isFollowing = value;
        Debug.Log("Cart following state changed: " + isFollowing);
    }

    [PunRPC]
    public void TakeOffPlayer()
    {
        Player.transform.position = PlayerAfterCartPosition.transform.position;
        Debug.Log("Player taken off the cart and moved to PlayerAfterCartPlace position.");

    }

}
