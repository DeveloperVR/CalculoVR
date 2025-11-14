
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class DektopNetworkSetup : MonoBehaviourPun
{
    [SerializeField] private MonoBehaviour movementScript;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private GameObject CameraHolder;
    [SerializeField] private Camera cam;
    [SerializeField] private GameObject Orientation;
    [SerializeField] private GameObject CameraPos;
    [SerializeField] private GameObject StandardPlayer;
    //[SerializeField] private GameObject ExternalPlayerModel;
    [SerializeField] private GameObject MiniMapSprite;
    [SerializeField] private GameObject Skin;
    // Start is called before the first frame update
    void Start()
    {
        PhotonView photonView = GetComponent<PhotonView>();
        if (photonView.IsMine)
        {   
            rb.isKinematic = false;
            movementScript.enabled = true;
            CameraHolder.SetActive(true);
            cam.gameObject.SetActive(true);
            Orientation.SetActive(true);
            CameraPos.SetActive(true);
            StandardPlayer.SetActive(true);
            FRGameManager.Instance.SetLocalPlayer(this.gameObject);
        }
        else
        {
            rb.isKinematic = true;
            movementScript.enabled = false;
            Orientation.SetActive(false);
            CameraPos.SetActive(false);
            CameraHolder.SetActive(false);
            StandardPlayer.SetActive(true);
            //ExternalPlayerModel.SetActive(true);
            MiniMapSprite.SetActive(false);
        }

    }

    [PunRPC]
    public void ResetSkinToAll()
    {
        Skin.transform.localPosition = new Vector3(0, 0, 0);
    }
}
