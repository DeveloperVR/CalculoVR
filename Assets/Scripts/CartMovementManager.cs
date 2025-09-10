using System.Collections;
using Photon.Pun;
using UnityEngine;

public class CartMovementManager : MonoBehaviourPun
{
    private GameObject cart;
    [SerializeField] private GameObject Player;
    public bool isMoving = false;
    public bool isFollowing { get; private set; } = false;
    private GameObject PlayerAfterCartPosition;
    private GameObject[] Menus = new GameObject[3];
    private GameObject PlayerEvent;

    public void Start()
    {
        // if (!photonView.IsMine)
        // {
        //     return;
        // }
        // PlayerEvent = GameObject.Find("PlayerSpawnEvent");
        // PlayerAfterCartPosition = GameObject.Find("AfterCartPlace");
        // if (PlayerAfterCartPosition == null)
        // {
        //     Debug.LogError("PlayerAfterCartPlace not found in the scene.");
        // }
        // cart = GameObject.Find("Carrito1");
        // StartCoroutine(WaitForPlayerSpawn());
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

}
