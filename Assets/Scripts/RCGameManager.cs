using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class RCGameManager : MonoBehaviour
{
    public static RCGameManager Instance { get; private set; }
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
