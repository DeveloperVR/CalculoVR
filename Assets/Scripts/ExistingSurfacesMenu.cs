using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExistingSurfacesMenu : MonoBehaviour
{
    GameObject functionInstantiator;
    GameObject ThisMenu;
    GameObject Surface1;
    GameObject Surface2;
    GameObject Surface3;

    public void ProcessSurface(GameObject surface)
    {
        ThisMenu = GameObject.Find("ExistingSurfacesMenu");
        ThisMenu.SetActive(false);
        functionInstantiator = GameObject.Find("FunctionInstantiator");
        Mesh surfaceMesh = surface.GetComponent<MeshFilter>().mesh;
        functionInstantiator.GetComponent<FunctionInstantiator>().ComputePoints(surfaceMesh);    
    }
}
