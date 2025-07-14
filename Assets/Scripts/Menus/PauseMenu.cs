using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuCanvas;
    [SerializeField] private GameObject SettingsCanvas;
    [SerializeField] private Transform cam;

    private GameObject LoadingScreen;

    public InputActionReference pauseButton;
    private bool isPaused = false;

    private void Awake()
    {
        pauseButton.action.Enable();
        LoadingScreen = GameObject.Find("LoadingScreen");
    }

    private void OnEnable()
    {
        pauseButton.action.performed += Pausing;
    }

    // private void OnDisable()
    // {
    //     pauseButton.action.performed -= Pausing;
    // }

    private void LateUpdate()
    {
        if (isPaused)
        {
            Vector3 forward = cam.forward;
            Vector3 canvasPosition = cam.position + forward * 5;
            mainMenuCanvas.transform.position = canvasPosition;
            mainMenuCanvas.transform.LookAt(cam);
            mainMenuCanvas.transform.Rotate(0, 180, 0); // Rotate to face the camera
        }
    }



    private void OnDestroy()
    {

    }
    public void Pausing(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            DisplayPauseMenu();
        }
    }

    public void DisplayPauseMenu()
    {
        if (isPaused)
        {
            mainMenuCanvas.SetActive(false);
            isPaused = false;
            //Time.timeScale = 1f; // Resumes the game
        }
        else if (!isPaused)
        {
            mainMenuCanvas.SetActive(true);
            isPaused = true;
            //Time.timeScale = 0f; // Pauses the game
        }

    }
    //--------------------------------FUNCIONALIDAD DE BOTONES DEL MENU---------------------------------------//

    public void ResumeGame()
    {
        mainMenuCanvas.SetActive(false);
        isPaused = false;
        //Time.timeScale = 1f; // Resumes the game
    }

    public void MenuReturn()
    {
        LoadingScene loadingScene = LoadingScreen.GetComponent<LoadingScene>();
        loadingScene.LoadScene("MainMenu");
        //Time.timeScale = 1f; // Resumes the game
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void SettingsMenu()
    {

        mainMenuCanvas.SetActive(false);
        SettingsCanvas.SetActive(true);
        SettingsCanvas.GetComponent<SettingsMenu>().SetSettings(true);
    }
}
