using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class WristMenu : MonoBehaviour
{
    private bool isWristUIDisplayed = false;
    [SerializeField]
    private GameObject wristUI;
    public GameObject miniMap;
    public InputActionReference ActivateMiniMap;
    private GameObject Player;
    public RenderTexture miniMapTexture;
    public List<Sprite> Surface1Clues;
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

    public void SetMiniMapSprite()
    {
        miniMap.GetComponent<RawImage>().texture = miniMapTexture;
    }

     public void SetClueSprite(int clueIndex)
    {
        if (clueIndex >= 0 && clueIndex < Surface1Clues.Count-1)
        {
            miniMap.GetComponent<RawImage>().texture = Surface1Clues[clueIndex].texture;
        }
        else
        {
            Debug.LogWarning("Clue index out of range.");
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
                wristUI.transform.localPosition = new Vector3(0, 0f, 0f);
                wristUI.transform.localRotation = Quaternion.identity * Quaternion.Euler(0, 0, 0); // Adjust position as needed
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
