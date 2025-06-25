using UnityEngine;
using Unity.XR.CoreUtils;

public class MainMenuFunctions : MonoBehaviour
{

    [SerializeField] private GameObject Self;
    [SerializeField] private GameObject Player;
    [SerializeField] private GameObject Cart1;
    [SerializeField] private GameObject Cart2;
    [SerializeField] private GameObject Cart3;
    private Vector3 cartlocation;
    [SerializeField] private GameObject EntradaPista1;
    [SerializeField] private GameObject EntradaPista2;
    [SerializeField] private GameObject EntradaPista3;
    private AudioManager audioManager;

    // Start is called before the first frame update


    public void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
        audioManager.SeleccionAudio(0, 0.9f, false);

    }

    public void Trackc1()
    {

        audioManager.SeleccionAudio(2, 0.9f, false);
        cartlocation = Cart1.transform.position;
        Player.transform.position = EntradaPista1.transform.position;
        Player.GetComponent<XROrigin>().RequestedTrackingOriginMode = XROrigin.TrackingOriginMode.Floor;
        Player.GetComponent<XROrigin>().RequestedTrackingOriginMode = XROrigin.TrackingOriginMode.Device;

    }

    public void Trackc2()
    {
        audioManager.SeleccionAudio(2, 0.9f, false);
        cartlocation = Cart2.transform.position;
        Player.transform.position = EntradaPista2.transform.position;
        Player.GetComponent<XROrigin>().RequestedTrackingOriginMode = XROrigin.TrackingOriginMode.Floor;
        Player.GetComponent<XROrigin>().RequestedTrackingOriginMode = XROrigin.TrackingOriginMode.Device;

    }

    public void Trackc3()
    {
        audioManager.SeleccionAudio(2, 0.9f, false);
        cartlocation = Cart3.transform.position;
        Player.transform.position = EntradaPista3.transform.position;
        Player.GetComponent<XROrigin>().RequestedTrackingOriginMode = XROrigin.TrackingOriginMode.Floor;
        Player.GetComponent<XROrigin>().RequestedTrackingOriginMode = XROrigin.TrackingOriginMode.Device;

    }

    public void EnterPark()
    {
        audioManager.SeleccionAudio(1, 0.9f, false);
        Player.GetComponent<XROrigin>().RequestedTrackingOriginMode = XROrigin.TrackingOriginMode.Floor;
        Player.GetComponent<XROrigin>().RequestedTrackingOriginMode = XROrigin.TrackingOriginMode.Device;

    }


}
