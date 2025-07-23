using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FRGameManager : MonoBehaviour
{
    public static FRGameManager Instance { get; private set; }
    public GameObject LocalPlayer { get; private set; }

    void Awake()
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
    public void SetLocalPlayer(GameObject player)
    {
        LocalPlayer = player;
    }
}
