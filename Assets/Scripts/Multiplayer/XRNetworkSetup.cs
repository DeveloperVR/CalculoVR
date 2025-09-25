using Photon.Pun;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Inputs;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;


public class CXRNetworkSetup : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private GameObject leftHand;
    [SerializeField] private GameObject leftHandStabilized;
    [SerializeField] private GameObject rightHand;
    [SerializeField] private GameObject rightHandStabilized;
    [SerializeField] private GameObject LocomotionSystem;
    [SerializeField] private GameObject skin;
    [SerializeField] private CharacterControllerDriver characterControllerDriver;
    [SerializeField] private XRInputModalityManager xrInputModalityManager;
    [SerializeField] private XRGazeAssistance xrGazeAssistance;
    [SerializeField] private OnButtonPress onButtonPress;
    [SerializeField] private GameObject LeftSimulatedHand;
    [SerializeField] private GameObject RightSimulatedHand;

    void Start()
    {

        PhotonView photonView = GetComponent<PhotonView>();
        if (photonView.IsMine)
        {
            mainCamera.gameObject.SetActive(true);
            leftHand.SetActive(true);
            rightHand.SetActive(true);
            LocomotionSystem.SetActive(true);
            rightHandStabilized.SetActive(true);
            leftHandStabilized.SetActive(true);
            LeftSimulatedHand.GetComponent<MeshRenderer>().enabled = false;
            RightSimulatedHand.GetComponent<MeshRenderer>().enabled = false;
            

            characterControllerDriver.enabled = true;
            xrInputModalityManager.enabled = true;
            xrGazeAssistance.enabled = true;
            onButtonPress.enabled = true;

            if (SceneManager.GetActiveScene().name == "SurfacesApp_Multiplayer")
            {
                FRGameManager.Instance.SetLocalPlayer(this.gameObject);
            }
            else if (SceneManager.GetActiveScene().name == "RollerCoaster_MultiPlayer")
            {
                RCGameManager.Instance.SetLocalPlayer(this.gameObject);
            }

        }
        else
        {
            skin.SetActive(true);
        }
    }

}
