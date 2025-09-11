using Unity.XR.CoreUtils;
using UnityEngine;
using thread = System.Threading.Tasks;
using Photon.Pun;

public class Menu21Functions : MonoBehaviourPun
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

    public void Ridding()
    {
        if (Cart.GetComponent<MovementSpeed2>().isReady2 == true)
        {
            Cart.GetComponent<MovementSpeed2>().SetisFollowing2(true);
            Cart.GetComponent<MovementSpeed2>().isObserving2 = false;
        }
        else
        {

            photonView.RPC("StartRollerCoaster2", RpcTarget.AllBuffered);
            Cart.GetComponent<MovementSpeed2>().SetisFollowing2(true);
            Cart.GetComponent<MovementSpeed2>().isObserving2 = false;
        }
    }

    public void Observing()
    {
        Vector3 observinglocation3 = visualizationplataform.transform.position;
        if (Cart.GetComponent<MovementSpeed2>().isReady2 == true)
        {
            Cart.GetComponent<MovementSpeed2>().SetisFollowing2(false);
            Cart.GetComponent<MovementSpeed2>().isObserving2 = true;
            Player.transform.position = observinglocation3;
        }
        else
        {
            photonView.RPC("StartRollerCoaster2", RpcTarget.AllBuffered);
            Cart.GetComponent<MovementSpeed2>().SetisFollowing2(false);
            Cart.GetComponent<MovementSpeed2>().isObserving2 = true;
            Player.transform.position = observinglocation3;
        }
    }
    async public void OfflineRidding()
    {
        await thread.Task.Delay(3000);
        cartlocation = Cart.transform.position;
        Player.transform.position = cartlocation;
        Player.GetComponent<XROrigin>().RequestedTrackingOriginMode = XROrigin.TrackingOriginMode.Floor;
        Player.GetComponent<XROrigin>().RequestedTrackingOriginMode = XROrigin.TrackingOriginMode.Device;
        Cart.GetComponent<MovementSpeed2>().isObserving2 = false;
        Cart.GetComponent<MovementSpeed2>().isReady2 = true;
        //audioManager.SeleccionAudio(4, 0.9f, true);


    }

    async public void OfflineObserving()
    {
        Vector3 observinglocation = visualizationplataform.transform.position;
        Player.transform.position = observinglocation;
        Player.GetComponent<XROrigin>().RequestedTrackingOriginMode = XROrigin.TrackingOriginMode.Floor;
        Player.GetComponent<XROrigin>().RequestedTrackingOriginMode = XROrigin.TrackingOriginMode.Device;
        Cart.GetComponent<MovementSpeed2>().isObserving2 = true;
        await thread.Task.Delay(3000);
        Cart.GetComponent<MovementSpeed2>().isReady2 = true;
        //audioManager.SeleccionAudio(3, 0.4f, true);

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

    [PunRPC]
    public void StartRollerCoaster2()
    {
        Cart.GetComponent<MovementSpeed2>().isReady2 = true;
    }

}
