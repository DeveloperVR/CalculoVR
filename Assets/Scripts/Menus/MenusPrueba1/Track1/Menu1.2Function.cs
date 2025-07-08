using UnityEngine;
using Unity.XR.CoreUtils;


public class Menu12Function : MonoBehaviour
{
    [SerializeField] private GameObject User;
    [SerializeField] private GameObject puntoinicial;
    [SerializeField] private XROriginRecentering centrado;
    private AudioManager audioManager;
    void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
    }

    public void ReturnPark()
    {
        User.transform.position = puntoinicial.transform.position;
        gameObject.SetActive(false);
        User.GetComponent<XROrigin>().RequestedTrackingOriginMode = XROrigin.TrackingOriginMode.Floor;
        User.GetComponent<XROrigin>().RequestedTrackingOriginMode = XROrigin.TrackingOriginMode.Device;
        centrado.RecenterToHeadset();
        audioManager.StopAudio();
        audioManager.SeleccionAudio(0, 0.8f, false);
    }
}
