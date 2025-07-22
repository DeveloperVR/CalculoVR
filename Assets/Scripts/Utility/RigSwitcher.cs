using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Management;

public class RigSwitcher : MonoBehaviour
{

    [SerializeField] private GameObject xrRig;
    [SerializeField] private GameObject nonVrRig;
    [SerializeField] private GameObject crosshair;
    void Start()
    {
        var xrManager = XRGeneralSettings.Instance?.Manager;
        checking();
    }



    private void checking()
    {
        var xrManager = XRGeneralSettings.Instance?.Manager;

        bool isXRActive = xrManager &&
                          xrManager.isInitializationComplete &&
                          xrManager.activeLoader != null;

        if (isXRActive)
        {
            Debug.Log("XR Rig is active.");
            xrRig.SetActive(true);
            nonVrRig.SetActive(false);
            crosshair.SetActive(false);
        }
        else
        {
            Debug.Log("Non-VR Rig is active.");
            xrRig.SetActive(false);
            nonVrRig.SetActive(true);
            crosshair.SetActive(true);
        }
    }
}
