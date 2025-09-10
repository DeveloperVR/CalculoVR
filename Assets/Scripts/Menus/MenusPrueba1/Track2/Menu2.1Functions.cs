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

    async public void Ridding()
    {
        if (Cart.GetComponent<MovementSpeed2>().isMoving2 == true)
        {
            Cart.GetComponent<MovementSpeed2>().SetisFollowing2(true);
            return;
        }
        else if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("Master client is setting up the cart movement.");
            await thread.Task.Delay(3000);
            Player.GetComponent<XROrigin>().RequestedTrackingOriginMode = XROrigin.TrackingOriginMode.Floor;
            Player.GetComponent<XROrigin>().RequestedTrackingOriginMode = XROrigin.TrackingOriginMode.Device;
            Cart.GetComponent<MovementSpeed2>().isObserving2 = false;
            Cart.GetComponent<MovementSpeed2>().isReady2 = true;
            Cart.GetComponent<MovementSpeed2>().SetisFollowing2(true);
            Cart.GetComponent<MovementSpeed2>().photonView.RPC("SetisMoving2", RpcTarget.AllBuffered, true);
            Cart.GetComponent<MovementSpeed2>().SetisMasterCall2(true);
        }
        else
        {
            Cart.GetComponent<MovementSpeed2>().photonView.RPC("SetisReady2", RpcTarget.MasterClient, true);
            Cart.GetComponent<MovementSpeed2>().SetisFollowing2(true);
            Cart.GetComponent<MovementSpeed2>().photonView.RPC("SetisMasterCall2", RpcTarget.MasterClient, true);
            Cart.GetComponent<MovementSpeed2>().photonView.RPC("SetisMoving2", RpcTarget.AllBuffered, true);
            Cart.GetComponent<MovementSpeed2>().isObserving2 = false;
        }
        
    }

    async public void Observing()
    {
        Vector3 observinglocation = visualizationplataform.transform.position;
        if (Cart.GetComponent<MovementSpeed2>().isMoving2 == true)
        {
            Player.transform.position = observinglocation;
            Cart.GetComponent<MovementSpeed2>().isObserving2 = true;
            return;
        }
        else if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("Master client is setting up observing mode.");
            Player.transform.position = observinglocation;
            await thread.Task.Delay(3000);
            Player.GetComponent<XROrigin>().RequestedTrackingOriginMode = XROrigin.TrackingOriginMode.Floor;
            Player.GetComponent<XROrigin>().RequestedTrackingOriginMode = XROrigin.TrackingOriginMode.Device;
            Cart.GetComponent<MovementSpeed2>().SetIsReady2(true);
            Cart.GetComponent<MovementSpeed2>().SetisFollowing2(true);
            Cart.GetComponent<MovementSpeed2>().isObserving2 = true;
            Cart.GetComponent<MovementSpeed2>().SetisMasterCall2(true);
            Cart.GetComponent<MovementSpeed2>().photonView.RPC("SetIsMoving", RpcTarget.AllBuffered, true);
        }
        else
        {
            Player.transform.position = observinglocation;
            Cart.GetComponent<MovementSpeed2>().SetisFollowing2(false);
            Cart.GetComponent<MovementSpeed2>().isObserving2 = true;
            Cart.GetComponent<MovementSpeed2>().photonView.RPC("SetIsReady2", RpcTarget.MasterClient, true);
            Cart.GetComponent<MovementSpeed2>().photonView.RPC("SetisMasterCall2", RpcTarget.MasterClient, true);
            Cart.GetComponent<MovementSpeed2>().photonView.RPC("SetisMoving2", RpcTarget.AllBuffered, true);
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


}
