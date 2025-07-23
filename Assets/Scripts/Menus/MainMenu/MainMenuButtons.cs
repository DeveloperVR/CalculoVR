using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButtons : MonoBehaviour
{

    [SerializeField] private LoadingScene LoadingScreen;
    private string test;
    private string mode;
    [SerializeField] private GameObject TestCanvas;
    [SerializeField] private GameObject ModeCanvas;
    [SerializeField] private GameObject ConfirmationCanvas;
    private GameObject ConnectionManagerObject;

    private void Awake()
    {
        TestCanvas.SetActive(true);
        ModeCanvas.SetActive(false);
        ConfirmationCanvas.SetActive(false);
    }

    public void Start()
    {
        
    }
    //Funtionality to pick what test to initiate----------------------------------------------------------//
    public void RollerCoaster()
    {
        test = "RollerCoaster";
        TestCanvas.SetActive(false);
        ModeCanvas.SetActive(true);
    }

    public void FreeRoam()
    {
        test = "FreeRoam";
        TestCanvas.SetActive(false);
        ModeCanvas.SetActive(true);
    }

    public void Boat()
    {
        test = "Boat";
        TestCanvas.SetActive(false);
        ModeCanvas.SetActive(true);
    }
    //Funtionality to pick the mode, singleplayer or multiplayer----------------------------------------------------------//
    public void Mode1()
    {
        mode = "SinglePlayer";
        ModeCanvas.SetActive(false);
        ConfirmationCanvas.SetActive(true);
    }

    public void Mode2()
    {
        mode = "MultiPlayer";
        ModeCanvas.SetActive(false);
        ConfirmationCanvas.SetActive(true);
    }

    public void confirm()
    {
        if (test == "RollerCoaster" && mode == "SinglePlayer")
        {
            LoadingScreen.LoadScene("RollerCoaster_SinglePlayer");
        }

        if (test == "RollerCoaster" && mode == "MultiPlayer")
        {
            LoadingScreen.LoadScene("RollerCoaster_MultiPlayer");
            ConnectionManager.Instance.SetRoomName("RollerCoaster_Multiplayer");

        }
        if (test == "FreeRoam" && mode == "SinglePlayer")
        {
            LoadingScreen.LoadScene("SurfacesApp_SinglePlayer");
        }
        if (test == "FreeRoam" && mode == "MultiPlayer")
        {
            LoadingScreen.LoadScene("SurfacesApp_MultiPlayer");
            ConnectionManager.Instance.SetRoomName("SurfacesApp_Multiplayer");
        }
    }

    public void cancel()
    {
        TestCanvas.SetActive(true);
        ModeCanvas.SetActive(false);
        ConfirmationCanvas.SetActive(false);
    }
}
