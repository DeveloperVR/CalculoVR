using UnityEngine;

public class ParkAudio : MonoBehaviour
{
    private GameObject Player;
    [SerializeField] private AudioSource controlAudio;


    public void SetPlayer()
    {
        Player = RCGameManager.Instance.LocalPlayer;
        if (Player == null)
        {
            Debug.LogError("XR Origin (XR Rig) not found in the scene.");
        }
        else
        {
            Debug.Log("XR Origin (XR Rig) found: " + Player.name);
            if (!controlAudio.isPlaying)
            {
                controlAudio.Play();
                controlAudio.volume = 0.3f;
            }
            else
            {
                Debug.Log("Audio is already playing.");
            }
        }
    }

    public void Awake()
    {
        controlAudio.volume = 0.0f;
    }
}
