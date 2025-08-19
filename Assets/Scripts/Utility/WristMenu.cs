using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WristMenu : MonoBehaviour
{
    private bool isWristUIDisplayed = false;
    [SerializeField]
    private GameObject wristUI;
    public InputActionReference ActivateMiniMap;
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
            wristUI.SetActive(false);
            isWristUIDisplayed = false;
        }
        else if (!isWristUIDisplayed)
        {
            wristUI.SetActive(true);
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
}
