using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ExistingSurfacesMenu : MonoBehaviour
{
    GameObject functionInstantiator;
    [SerializeField] GameObject ThisMenu;
    [SerializeField] GameObject Surface1;
    [SerializeField] GameObject Surface2;
    [SerializeField] GameObject Surface3;

    GameObject Player;

    public void ProcessSurface(GameObject surface)
    {
        ThisMenu.SetActive(false);
        functionInstantiator = GameObject.Find("FunctionInstantiator");
        Mesh surfaceMesh = surface.GetComponent<MeshFilter>().mesh;
        functionInstantiator.GetComponent<FunctionInstantiator>().ComputePoints(surfaceMesh);
    }
    
    public void SetPlayer()
    {
        Player = FRGameManager.Instance.LocalPlayer;
        if (Player == null)
        {
            Debug.LogError("XR Origin (XR Rig) not found in the scene.");
        }
        else
        {
            Debug.Log("XR Origin (XR Rig) found: " + Player.name);
            ThisMenu.SetActive(true);
        }
    }
    

}
