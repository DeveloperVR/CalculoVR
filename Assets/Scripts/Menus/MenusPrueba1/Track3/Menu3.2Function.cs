using UnityEngine;
using Unity.XR.CoreUtils;


public class Menu32Function : MonoBehaviour
{
    [SerializeField] private GameObject Player;
    [SerializeField] private GameObject puntoinicial;
    [SerializeField] private XROriginRecentering centrado;
    [SerializeField] private GameObject Cart;
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
    
    public void ReturnParkAndDeactivateObserving()
    { 
        Cart.GetComponent<MovementSpeed3>().isObserving3 = false;
        Player.transform.position = puntoinicial.transform.position;
        gameObject.SetActive(false);
        Player.GetComponent<XROrigin>().RequestedTrackingOriginMode = XROrigin.TrackingOriginMode.Floor;
        Player.GetComponent<XROrigin>().RequestedTrackingOriginMode = XROrigin.TrackingOriginMode.Device;
        centrado.RecenterToHeadset();
        audioManager.StopAudio();
    }
}
