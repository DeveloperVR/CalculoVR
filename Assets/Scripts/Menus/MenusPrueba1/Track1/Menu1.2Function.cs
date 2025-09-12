using UnityEngine;
using Unity.XR.CoreUtils;


public class Menu12Function : MonoBehaviour
{
    [SerializeField] private GameObject Player;
    [SerializeField] private GameObject puntoinicial;
    [SerializeField] private XROriginRecentering centrado;
    private AudioManager audioManager;
    void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
    }

    public void ReturnPark()
    {
        Player.transform.position = puntoinicial.transform.position;
        gameObject.SetActive(false);
        Player.GetComponent<XROrigin>().RequestedTrackingOriginMode = XROrigin.TrackingOriginMode.Floor;
        Player.GetComponent<XROrigin>().RequestedTrackingOriginMode = XROrigin.TrackingOriginMode.Device;
        centrado.RecenterToHeadset();
        audioManager.StopAudio();
    }

    public void SetPlayer()
    {
        Player = RCGameManager.Instance.LocalPlayer;
        if (Player == null)
        {
            Debug.LogError("XR Origin (XR Rig) not found in the scene.");
        }
    }
    
    public void SetOfflinePlayer()
    {
        Player = GameObject.Find("XR Player");
        Debug.Log("XR Player found: " + Player.name);
    }
}
