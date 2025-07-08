using Unity.XR.CoreUtils;
using UnityEngine;
using thread = System.Threading.Tasks;

public class Menu11Functions : MonoBehaviour
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
        Cart.GetComponent<MovementSpeed1>().Observing = false;
        Cart.GetComponent<MovementSpeed1>().isReady = true;
        //audioManager.SeleccionAudio(4, 0.3f, true);


    }

    async public void Observing()
    {
        Vector3 observinglocation = visualizationplataform.transform.position;
        Player.transform.position = observinglocation;
        Player.GetComponent<XROrigin>().RequestedTrackingOriginMode = XROrigin.TrackingOriginMode.Floor;
        Player.GetComponent<XROrigin>().RequestedTrackingOriginMode = XROrigin.TrackingOriginMode.Device;
        Cart.GetComponent<MovementSpeed1>().Observing = true;
        await thread.Task.Delay(3000);
        Cart.GetComponent<MovementSpeed1>().isReady = true;
        //audioManager.SeleccionAudio(3, 0.4f, true);
    }

    async public void Ridding2()
    {
        await thread.Task.Delay(3000);
        cartlocation = Cart.transform.position;
        Player.transform.position = cartlocation;
        Player.GetComponent<XROrigin>().RequestedTrackingOriginMode = XROrigin.TrackingOriginMode.Floor;
        Player.GetComponent<XROrigin>().RequestedTrackingOriginMode = XROrigin.TrackingOriginMode.Device;
        MovementVersion2 movement = Cart.GetComponent<MovementVersion2>();
        if (movement != null)
        {
            movement.ObservingSetter(true);
            movement.IsReadySetter(true);
        }
        else
        {
            Debug.LogWarning("MovementVersion2 no está en el objeto Cart.");
        }


    }

    async public void Observing2()
    {
        Vector3 observinglocation = visualizationplataform.transform.position;
        Player.transform.position = observinglocation;
        Player.GetComponent<XROrigin>().RequestedTrackingOriginMode = XROrigin.TrackingOriginMode.Floor;
        Player.GetComponent<XROrigin>().RequestedTrackingOriginMode = XROrigin.TrackingOriginMode.Device;
        Cart.GetComponent<MovementVersion2>().ObservingSetter(true);
        await thread.Task.Delay(3000);
        Cart.GetComponent<MovementVersion2>().IsReadySetter(true);
    }
    


}
