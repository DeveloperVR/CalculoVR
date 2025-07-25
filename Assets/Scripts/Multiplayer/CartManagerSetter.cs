using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartManagerSetter : MonoBehaviour
{
    GameObject Spawner;
    private GameObject[] Menus = new GameObject[3];
    void Start()
    {
        Spawner = GameObject.Find("PhotonConnectionManager");
        //StartCoroutine(WaitForSpawner());
    }

    IEnumerator WaitForSpawner()
    {
        while (Spawner.GetComponent<PhotonMultiplayerSpawner>().CartManagerHasSpawned == false || RCGameManager.Instance.CartMovementManager == null)
        {
            yield return null; // Wait for the CartMovementManager to be spawned
        }
        SetMenus(); // Set the menus after the CartMovementManager has been spawned
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
}
