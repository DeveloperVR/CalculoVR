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
    public void Ridding()
    {
        if (Cart.GetComponent<MovementSpeed1>().isReady == true)
        {
            Cart.GetComponent<MovementSpeed1>().SetisFollowing(true);
            Cart.GetComponent<MovementSpeed1>().isObserving = false;
        }
        else
        {

            photonView.RPC("StartRollerCoaster", RpcTarget.AllBuffered);
            Cart.GetComponent<MovementSpeed1>().SetisFollowing(true);
            Cart.GetComponent<MovementSpeed1>().isObserving = false;
        }
    }

    public void Observing()
    {
        Vector3 observinglocation = visualizationplataform.transform.position;
        if (Cart.GetComponent<MovementSpeed1>().isReady == true)
        {
            Cart.GetComponent<MovementSpeed1>().SetisFollowing(false);
            Cart.GetComponent<MovementSpeed1>().isObserving = true;
            Player.transform.position = observinglocation;
        }
        else
        {
            photonView.RPC("StartRollerCoaster", RpcTarget.AllBuffered);
            Cart.GetComponent<MovementSpeed1>().SetisFollowing(false);
            Cart.GetComponent<MovementSpeed1>().isObserving = true;
            Player.transform.position = observinglocation;
        }
    }

    async public void OfflineRiding()
    {
        await thread.Task.Delay(3000);
        cartlocation = Cart.transform.position;
        Player.transform.position = cartlocation;
        Player.GetComponent<XROrigin>().RequestedTrackingOriginMode = XROrigin.TrackingOriginMode.Floor;
        Player.GetComponent<XROrigin>().RequestedTrackingOriginMode = XROrigin.TrackingOriginMode.Device;
        Cart.GetComponent<OfflineMovementSpeed1>().OfflineObserving = false;
        Cart.GetComponent<OfflineMovementSpeed1>().OfflineisReady = true;
    }



    async public void OfflineObserving()
    {
        Vector3 observinglocation = visualizationplataform.transform.position;
        Player.transform.position = observinglocation;
        Player.GetComponent<XROrigin>().RequestedTrackingOriginMode = XROrigin.TrackingOriginMode.Floor;
        Player.GetComponent<XROrigin>().RequestedTrackingOriginMode = XROrigin.TrackingOriginMode.Device;
        Cart.GetComponent<OfflineMovementSpeed1>().OfflineObserving = true;
        await thread.Task.Delay(3000);
        Cart.GetComponent<OfflineMovementSpeed1>().OfflineisReady = true;
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

    public void SetOfflinePlayer()
    {
        Player = GameObject.Find("XR Player");
        Debug.Log("XR Player found: " + Player.name);
    }
    
    [PunRPC]
    public void StartRollerCoaster()
    {
        Cart.GetComponent<MovementSpeed1>().isReady = true;
    }
}
