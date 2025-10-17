using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SurfacesPauseMenu : MonoBehaviour
{
    private GameObject Player;
    [SerializeField] private Transform cam;

    private GameObject FunctionSurface;
    private GameObject MainPlane;
    [SerializeField] private GameObject VRMainMenu;
    [SerializeField] private GameObject DesktopMainMenu;

    private bool isMenuActive = false;

    public InputActionReference PanelButtonXR;
    public InputActionReference PanelButtonDesktop;
    private string TypePlayer;

    void Awake()
    {
        PanelButtonXR.action.Enable();
        PanelButtonDesktop.action.Enable();
    }

    private void OnEnable()
    {
        PanelButtonXR.action.performed += CheckVRInput;
        PanelButtonDesktop.action.performed += CheckDesktopInput;
    }
    void Start()
    {
        MainPlane = GameObject.Find("SpawnPoint");
    }

    private void LateUpdate()
    {
        if (isMenuActive && TypePlayer == "VR")
        {
            Vector3 forward = cam.forward;
            Vector3 canvasPosition = cam.position + forward * 5;
            VRMainMenu.transform.position = canvasPosition;
            VRMainMenu.transform.LookAt(cam);
            VRMainMenu.transform.Rotate(0, 180, 0); // Rotate to face the camera
        }
    }

    public void CheckVRInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            OpenPanel();
        }
    }

    public void CheckDesktopInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            OpenPanel();
        }
    }

    public void OpenPanel()
    {   if (TypePlayer == "VR")
        { 
            if (!isMenuActive)
            {
                isMenuActive = true;
                VRMainMenu.SetActive(true);

            }
            else
            {
                isMenuActive = false;
                VRMainMenu.SetActive(false);
            }
        }
        else if (TypePlayer == "Desktop")
        {
            if (!isMenuActive)
            {
                isMenuActive = true;
                DesktopMainMenu.SetActive(true);
            }
            else
            {
                isMenuActive = false;
                DesktopMainMenu.SetActive(false);
            }
        }
    }
    //-----------------------FUNCIONALIDADES DE BOTONES-----------------------
    public void BacktoOriginalPosition()
    {
        FunctionSurface = GameObject.Find("FunctionSurface");
        Debug.Log("Back to Original Position on the surface");
        Player.transform.position = FunctionSurface.transform.position + new Vector3(0, 3, 0);
        if (TypePlayer == "VR")
        {
            VRMainMenu.SetActive(false);
        }
        else if (TypePlayer == "Desktop")
        {
            DesktopMainMenu.SetActive(false);
        }
    }

    public void BacktoMainMenu()
    {
        Debug.Log("Back to Main Menu");
        Vector3 TargetPosition = MainPlane.transform.position;
        Player.transform.position = TargetPosition + new Vector3(0, 2, 0);
        if (TypePlayer == "VR")
        {
            VRMainMenu.SetActive(false);
        }
        else if (TypePlayer == "Desktop")
        {
            DesktopMainMenu.SetActive(false);
        }
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game");
        Application.Quit();
    }
    //-----------------------FUNCIONALIDADES DE CAMBIO DE PLAYER-----------------------
    public void SetPlayer(string type)
    {
        TypePlayer = type;
        if (TypePlayer == "VR")
        {
            SetVRPlayer();
            Debug.Log("VR Player set.");
        }
        else if (TypePlayer == "Desktop")
        {
            SetDesktopPlayer();
            Debug.Log("Desktop Player set.");
        }
    }
    public void SetDesktopPlayer()
    {
         Player = FRGameManager.Instance.LocalPlayer;
        if (Player == null)
        {
            Debug.LogError("Player not found in the scene.");
        }
        else
        {
            Debug.Log("Player found: " + Player.name);
        }
    }
    
    public void SetVRPlayer()
    {
        Player = FRGameManager.Instance.LocalPlayer;
        if (Player == null)
        {
            Debug.LogError("XR Origin (XR Rig) not found in the scene.");
        }
        else
        {
            Debug.Log("XR Origin (XR Rig) found: " + Player.name);
            cam = Player.transform.Find("Camera Offset/Main Camera").transform;
            if (cam == null)
            {
                Debug.LogError("Camera not found under XR Origin (XR Rig).");
            }
            else
            {
                Debug.Log("Camera found: " + cam.name);
            }
        }
    }
}