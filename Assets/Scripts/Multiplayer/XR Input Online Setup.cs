// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class XRInputOnlineSetup : MonoBehaviour
// {
//     [SerializeField] private GameObject XRInteraction;
//     [SerializeField] private GameObject XRInput;
//     [SerializeField] private GameObject eventSystem;

//     private void Start()
//     {
//         XRInteraction = GameObject.Find("XR Interaction Manager");
//         XRInput = GameObject.Find("Input Action Manager");
//         eventSystem = GameObject.Find("EventSystem");
//     }

//     public void DeActivateInputs()
//     { 
//         if (XRInteraction != null)
//         {
//             XRInteraction.SetActive(false);
//         }
//         if (XRInput != null)
//         {
//             XRInput.SetActive(false);
//         }
//         if (eventSystem != null)
//         {
//             eventSystem.SetActive(false);
//         }
//     }
// }
