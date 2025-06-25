using Unity.XR.CoreUtils;
using UnityEngine;
using thread = System.Threading.Tasks;

public class Menu31Functions : MonoBehaviour
{

    [SerializeField] private GameObject Player;
    [SerializeField] private GameObject Cart;
    private Vector3 cartlocation;
    [SerializeField] private GameObject visualizationplataform;
    private AudioManager audioManager;

    // Start is called before the first frame update

    void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
    }
    async public void Ridding()
    {
        await thread.Task.Delay(3000);
        cartlocation = Cart.transform.position;
        Player.transform.position = cartlocation;
        Player.GetComponent<XROrigin>().RequestedTrackingOriginMode = XROrigin.TrackingOriginMode.Floor;
        Player.GetComponent<XROrigin>().RequestedTrackingOriginMode = XROrigin.TrackingOriginMode.Device;
        Cart.GetComponent<MovementSpeed3>().Observing3 = false;
        Cart.GetComponent<MovementSpeed3>().isReady3 = true;
        //audioManager.SeleccionAudio(4, 0.9f, true);
    }

    async public void Observing()
    {
        Vector3 observinglocation = visualizationplataform.transform.position;
        Player.transform.position = observinglocation;
        Player.GetComponent<XROrigin>().RequestedTrackingOriginMode = XROrigin.TrackingOriginMode.Floor;
        Player.GetComponent<XROrigin>().RequestedTrackingOriginMode = XROrigin.TrackingOriginMode.Device;
        Cart.GetComponent<MovementSpeed3>().Observing3 = true;
        await thread.Task.Delay(3000);
        Cart.GetComponent<MovementSpeed3>().isReady3 = true;
        //audioManager.SeleccionAudio(3, 0.4f, true);
    }


}
