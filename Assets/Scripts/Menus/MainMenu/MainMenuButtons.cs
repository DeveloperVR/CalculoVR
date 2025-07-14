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

    private void Awake()
    {
        TestCanvas.SetActive(true);
        ModeCanvas.SetActive(false);
        ConfirmationCanvas.SetActive(false);
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
<<<<<<< Updated upstream
            LoadingScreen.LoadScene(1);
=======
            LoadingScreen.LoadScene("RollerCoaster_SinglePlayer");
        }

        if (test == "RollerCoaster" && mode == "MultiPlayer")
        {
            connectionManager.GetComponent<ConnectionManager>().SetRoomName("RollerCoaster_MultiPlayer");
            LoadingScreen.LoadScene("RollerCoaster_MultiPlayer");
<<<<<<< Updated upstream
<<<<<<< Updated upstream
>>>>>>> Stashed changes
=======
>>>>>>> Stashed changes
=======
>>>>>>> Stashed changes
        }
    }

    public void cancel()
    {
        TestCanvas.SetActive(true);
        ModeCanvas.SetActive(false);
        ConfirmationCanvas.SetActive(false);
    }
}
