using System.Collections;
using Photon.Pun;
using UnityEngine;

public class CartMovementManager : MonoBehaviourPun
{
    private GameObject cart;
    [SerializeField] private GameObject Player;
    public bool isMoving { get; private set; } = false;
    public bool isFollowing { get; private set; } = false;
    private GameObject PlayerAfterCartPosition;
    private GameObject[] Menus = new GameObject[3];
    private GameObject PlayerEvent;
    private GameObject Spawner;

    public void Start()
    {
        if (!photonView.IsMine)
        {
            return;
        }
        PlayerEvent = GameObject.Find("PlayerSpawnEvent");
        Spawner = GameObject.Find("PhotonConnectionManager");
        PlayerAfterCartPosition = GameObject.Find("AfterCartPlace");
        if (PlayerAfterCartPosition == null)
        {
            Debug.LogError("PlayerAfterCartPlace not found in the scene.");
        }
        cart = GameObject.Find("Carrito1");
        StartCoroutine(WaitForPlayerSpawn());
        //StartCoroutine(WaitForSpawner());
    }
    public void SetPlayer()
    {
        Player = RCGameManager.Instance.LocalPlayer;
        if (Player == null)
        {
            Debug.LogError("XR Origin (XR Rig) not found in the scene.");
        }
        else
        {
            Debug.Log("Player set successfully: " + Player.name);
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

    public void SetMenus()
    {
        Menus[0] = GameObject.Find("MENUS/MENUPISTA1/Menu1Track1");
        if (Menus[0] != null)
        {
            Menus[0].GetComponent<Menu11Functions>().SetCartMovementManager();
        }
        else
        {
            Debug.LogError("Menu1Track1 not found in the scene.");
        }
    }

    IEnumerator WaitForPlayerSpawn()
    {
        while (PlayerEvent.GetComponent<PlayerSpawnEvent>().playerSpawned == false)
        {
            yield return null; // Wait for the player to be spawned
        }

        SetPlayer(); // Set the player after they have spawned
    }

    IEnumerator WaitForSpawner()
    {
        while (Spawner.GetComponent<PhotonMultiplayerSpawner>().CartManagerHasSpawned == false)
        {
            yield return null; // Wait for the CartMovementManager to be spawned
        }
        SetMenus(); // Set the menus after the CartMovementManager has been spawned
    }

}
