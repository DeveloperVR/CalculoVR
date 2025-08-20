using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WristMenu : MonoBehaviour
{
    private bool isWristUIDisplayed = false;
    [SerializeField]
    private GameObject wristUI;
    public GameObject miniMap;
    public InputActionReference ActivateMiniMap;
    private GameObject Player;
    private void Awake()
    {
        ActivateMiniMap.action.Enable();
        ActivateMiniMap.action.performed += MinimapButtonPressed;
        InputSystem.onDeviceChange += OnDeviceChange;
    }

    public void MinimapButtonPressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            DisplayWristUI();
        }
    }

    void Update()
    {

    }

    public void DisplayWristUI()
    {
        if (isWristUIDisplayed)
        {
            miniMap.SetActive(false);
            isWristUIDisplayed = false;
        }
        else if (!isWristUIDisplayed)
        {
            miniMap.SetActive(true);
            isWristUIDisplayed = true;
        }
    }

    private void OnDeviceChange(InputDevice device, InputDeviceChange change)
    {
        switch (change)
        {
            case InputDeviceChange.Disconnected:
                ActivateMiniMap.action.Disable();
                ActivateMiniMap.action.performed -= MinimapButtonPressed;
                break;
            case InputDeviceChange.Reconnected:
                ActivateMiniMap.action.Enable();
                ActivateMiniMap.action.performed += MinimapButtonPressed;
                break;
        }
    }

    public void SetPlayerWristMenu()
    {
        if (Player != null)
        {
            GameObject leftController = GameObject.Find("Left Controller");
            if (leftController == null)
            {
                Debug.LogError("Left Controller not found in the scene.");
            }
            else
            {
                wristUI.transform.SetParent(leftController.transform);
                wristUI.transform.localPosition = new Vector3(0.1f, 0.1f, 0.2f); // Adjust position as needed
            }
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
            SetPlayerWristMenu();
        }
    }
}
