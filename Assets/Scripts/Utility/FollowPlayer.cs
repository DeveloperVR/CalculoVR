using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    private GameObject Player;
    private bool isFollowing = false;

    void Update()
    {
        if (isFollowing && Player != null)
        {
            Vector3 offset = new Vector3(0, 25, 0); // Adjust the offset as needed
            transform.position = Player.transform.position + offset;
        }
    }
    public void SetPlayer()
    {
        Player = FRGameManager.Instance.LocalPlayer;
        if (Player == null)
        {
            Debug.LogError("XR Origin (XR Rig) not found in the scene.");
        }
        else
        {
            Debug.Log("XR Origin (XR Rig) found: " + Player.name);
            isFollowing = true;
        }
    }
}
