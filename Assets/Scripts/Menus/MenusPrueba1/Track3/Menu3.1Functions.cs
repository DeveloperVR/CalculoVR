using Unity.XR.CoreUtils;
using UnityEngine;
using thread = System.Threading.Tasks;
using Photon.Pun;

public class Menu31Functions : MonoBehaviourPun
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
    public void StartPlayerAndRollerCoaster3()
    {
        // Cart.GetComponent<MovementSpeed3>().isReady3 = true;
        // Cart.GetComponent<MovementSpeed3>().Observing3 = false;
        // Cart.GetComponent<MovementSpeed3>().isFollowing3 = true;
    }

    [PunRPC]
    public void StartOnlyRollerCoaster3()
    {
        // Cart.GetComponent<MovementSpeed3>().isReady3 = true;
        // Cart.GetComponent<MovementSpeed3>().isObserving3 = false;
        // Cart.GetComponent<MovementSpeed3>().isFollowing3 = false;
    }

    [PunRPC]
    public void CartAlreadyMoving3()
    {
        // Cart.GetComponent<MovementSpeed3>().isObserving3 = false;
        // Cart.GetComponent<MovementSpeed3>().isFollowing3 = true;
    }

    [PunRPC]
    public void StartOnObserver()
    {
        // Cart.GetComponent<MovementSpeed3>().isReady3 = true;
        // Cart.GetComponent<MovementSpeed3>().isObserving3 = true;
        // Cart.GetComponent<MovementSpeed3>().isFollowing3 = false;
    }

    [PunRPC]
    public void CartAlreadyMovingOnObserver()
    {
        // Cart.GetComponent<MovementSpeed3>().isObserving3 = true;
        // Cart.GetComponent<MovementSpeed3>().isFollowing3 = false;
    }

}
