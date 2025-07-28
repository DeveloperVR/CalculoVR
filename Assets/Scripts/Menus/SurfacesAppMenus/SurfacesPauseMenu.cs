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
    [SerializeField] private GameObject MainMenu;

    private bool isMenuActive = false;

    public InputActionReference pauseButton;
    void Start()
    {
        MainPlane = GameObject.Find("Plane");
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void LateUpdate()
    {
        if (isMenuActive)
        {
            // Keep the player in the menu position
            Player.transform.position = MainMenu.transform.position + new Vector3(0, 2, 0);
        }
    }
//-----------------------FUNCIONALIDADES DE BOTONES-----------------------
    public void BacktoOriginalPosition()
    {
        FunctionSurface = GameObject.Find("FunctionSurface");
        Player.transform.position = FunctionSurface.transform.position + new Vector3(0, 3, 0);
    }

    public void BacktoMainMenu()
    {
        Vector3 TargetPosition = MainPlane.transform.position;
        Player.transform.position = TargetPosition + new Vector3(0, 2, 0);
    }

    public void QuitGame()
    {
        Application.Quit();
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
        }
    }
}
