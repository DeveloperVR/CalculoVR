using Photon.Pun;
using Unity.XR.CoreUtils;
using UnityEngine;
using thread = System.Threading.Tasks;

public class Menu11Functions : MonoBehaviourPun
{

    [SerializeField] private GameObject Player;
    [SerializeField] private GameObject Cart;
    private Vector3 cartlocation;
    [SerializeField] private GameObject visualizationplataform;
    private AudioManager audioManager;
    private GameObject CartMovementManager;




    void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
    }

    public void SetCartMovementManager()
    {
        CartMovementManager = RCGameManager.Instance.CartMovementManager;
        if (CartMovementManager == null)
        {
            Debug.LogError("CartMovementManager not found in RCGameManager.");
        }
    }
    async public void Ridding()
    {

        if (CartMovementManager != null)
        {
            Debug.Log("isMoving state is: " + CartMovementManager.GetComponent<CartMovementManager>().isMoving);
            if (CartMovementManager.GetComponent<CartMovementManager>().isMoving == true)
            {
                Debug.Log("Cart is already moving, setting player position.");
                Cart.GetComponent<MovementSpeed1>().SetisFollowing(true);
            }
            else if (PhotonNetwork.IsMasterClient)
            {
                Debug.Log("Master client is setting up the cart movement.");
                await thread.Task.Delay(3000);
                cartlocation = Cart.transform.position;
                Player.transform.position = cartlocation;
                Player.GetComponent<XROrigin>().RequestedTrackingOriginMode = XROrigin.TrackingOriginMode.Floor;
                Player.GetComponent<XROrigin>().RequestedTrackingOriginMode = XROrigin.TrackingOriginMode.Device;
                Cart.GetComponent<MovementSpeed1>().Observing = false;
                Cart.GetComponent<MovementSpeed1>().SetIsReady(true);
                Cart.GetComponent<MovementSpeed1>().SetisFollowing(true);
                CartMovementManager.GetComponent<CartMovementManager>().photonView.RPC("SetisMoving", RpcTarget.AllBuffered, true);
                //audioManager.SeleccionAudio(4, 0.3f, true);
            }
            else
            {
                Cart.GetComponent<MovementSpeed1>().photonView.RPC("SetIsReady", RpcTarget.MasterClient, true);
                CartMovementManager.GetComponent<CartMovementManager>().photonView.RPC("SetisMoving", RpcTarget.AllBuffered, true);
                Cart.GetComponent<MovementSpeed1>().SetisFollowing(true);
            }

        }

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
    
    public void SetPlayer()
    { 
        Player = RCGameManager.Instance.LocalPlayer;
        if (Player == null)
        {
            Debug.LogError("XR Origin (XR Rig) not found in the scene.");
        }
    }
    


}
