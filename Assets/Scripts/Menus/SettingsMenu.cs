using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsMenu : MonoBehaviour
{

    [SerializeField] private GameObject mainMenuCanvas;
    [SerializeField] private GameObject settingsCanvas;
    [SerializeField] private GameObject muteButton;
    [SerializeField] private GameObject unmuteButton;
    [SerializeField] private Transform cam;
    [SerializeField] private AudioListener audioListener;

    private bool isSettingsActive = false;
    private bool isMuted = false;
    // Start is called before the first frame update
    void Start()
    {
        audioListener = FindObjectOfType<AudioListener>();
        if (audioListener == null)
        {
            Debug.LogError("AudioListener not found in the scene.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isSettingsActive)
        {
            Vector3 forward = cam.forward;
            Vector3 canvasPosition = cam.position + forward * 5;
            settingsCanvas.transform.position = canvasPosition;
            settingsCanvas.transform.LookAt(cam);
            settingsCanvas.transform.Rotate(0, 180, 0); // Rotate to face the camera  
        }

    }



    public void Muting()
    {
        if (!isMuted)
        {
            MuteListener();
            isMuted = true;
            Debug.Log("Audio Muted");
            return;
        }
        UnmuteListener();
        isMuted = false;
        Debug.Log("Audio Unmuted");
    }

    public void BackToMainMenu()
    {
        isSettingsActive = false;
        mainMenuCanvas.SetActive(true);
        settingsCanvas.SetActive(false);
    }

    public void SetSettings(bool state)
    {
        isSettingsActive = state;
    }

    public void TurnOffSettings()
    {
        isSettingsActive = false;
        settingsCanvas.SetActive(false);
    }

    private void MuteListener()
    {
        audioListener.enabled = false;
        muteButton.GetComponent<SpriteSwap>().SwapSprite(1);
    }

    private void UnmuteListener()
    {
        audioListener.enabled = true;
        muteButton.GetComponent<SpriteSwap>().SwapSprite(0);

    }
    
    public void SetOfflineCam()
    {
        GameObject Player = GameObject.Find("XR Player");
        cam = Player.transform.Find("Camera Offset/Main Camera");
    }
}
