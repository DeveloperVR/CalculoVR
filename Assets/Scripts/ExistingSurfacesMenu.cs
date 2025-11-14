using System.Collections.Generic;
using Photon.Pun;
using Unity.XR.CoreUtils;
using UnityEngine;

public class ExistingSurfacesMenu : MonoBehaviourPun
{
    GameObject functionInstantiator;
    GameObject functionGenerator;
    GameObject treasureCreator;
    GameObject treasureManager;
    [SerializeField] GameObject TreasureManagerObject;
    [SerializeField] GameObject ThisMenu;
    [SerializeField] GameObject GoToSurfaceMenu;
    [SerializeField] GameObject Surface1;
    string Surface1Function = "25.6* (2.71)^(-((x - 25.6)^2 / 1.9^4 + (y - 20.2)^2 / 4.1^2))   +  7.6* (2.71)^(-((x - 20.1)^2 / 1.8^4 + (y - 17)^2 / 2.6^2))   +   15.8* (2.71)^(-((x - 8.3)^2 / 2.2^4 + (y - 30)^2 / 7.2^2))  + 8* (2.71)^(-((x - 14.2)^2 / 1.5^4 + (y - 26.8)^2 / 3.2^2))   +    9.5* (2.71)^(-((x - 6.6)^2 / 2.2^4 + (y -17.2)^2 / 5^2))   +   17.7* (2.71)^(-((x - 2.7)^2 / 1.9^4 + (y - 8.5)^2 / 3.4^2))  +  8.6* (2.71)^(-((x - 15.6)^2 / 2.5^4 + (y - 3.4)^2 / 5.5^2))  -  9.5* (2.71)^(-((x - 31.7)^2 / 2.7^4 + (y - 7.5)^2 / 5.3^2))  -5* (2.71)^(-((x - 21.8)^2 / 1.8^4 + (y - 10.7)^2 / 4.5^2)) -2* (2.71)^(-((x - 25.5)^2 / 2.5^4 + (y - 30)^2 / 3.2^2))";
    List<Vector3> treasurePositions1 = new List<Vector3>()
    {
        new Vector3(7f, 20f, 9.03f),
        new Vector3(14.9f, 12.9f, 0.67f),
        new Vector3(20f, 17f, 8.12f),
        new Vector3(25f, 9f, 25f),
        new Vector3(2.7f, 8.5f, 18f),
        new Vector3(16f, 27f, 5.18f)
    };
    string Surface2Function = "21* (2.71)^(-((x - 25)^2 / 2.2^4 + (y - 25.6)^2 / 4.46^2))  +  13.7* (2.71)^(-((x - 20.8)^2 / 1.8^4 + (y - 17.2)^2 / 4^2))  + 11* (2.71)^(-((x - 4.7)^2 / 1.7^4 + (y - 26.7)^2 / 2.7^2))  + 4* (2.71)^(-((x - 12.4)^2 / 1.34^4 + (y - 21.6)^2 / 1.87^2))  + 28* (2.71)^(-((x - 24.3)^2 / 1.5^4 + (y - 6.2)^2 / 2.7^2))     -   10* (2.71)^(-((x - 18.5)^2 / 1.8^4 + (y - 5.2)^2 / 4.3^2))  -  6.5* (2.71)^(-((x - 12.6)^2 / 1.7^4 + (y - 7.5)^2 / 5.3^2))  - 9* (2.71)^(-((x - 5.5)^2 / 1.8^4 + (y - 13)^2 / 5^2))   -  9.5* (2.71)^(-((x - 10.4)^2 / 1.8^4 + (y - 26.4)^2 / 3.2^2))";
    
    [SerializeField] GameObject Surface2;
    [SerializeField] GameObject Surface3;

    GameObject Player;
    private bool ActiveSurface;
    private List<GameObject> ActiveLineObjects = new List<GameObject>();
    private List<GameObject> ActiveGradientObjects = new List<GameObject>();

    [PunRPC]
    public void ProcessSurface1()
    {
        if (ActiveSurface == true)
        {
            Surface2.SetActive(false);
            ActiveLineObjects.ForEach(Destroy);
            ActiveLineObjects.Clear();
            ActiveGradientObjects.ForEach(Destroy);
            ActiveGradientObjects.Clear();
        }
        ActiveSurface = true;
        List<GameObject> SurfaceItems = new List<GameObject>();
        Surface1.GetChildGameObjects(SurfaceItems);
        ThisMenu.SetActive(false);
        Surface1.SetActive(true);
        functionInstantiator = GameObject.Find("FunctionInstantiator");
        functionGenerator = GameObject.Find("FunctionGenerator");
        treasureCreator = GameObject.Find("TreasureCreator");
        Mesh surfaceMesh = Surface1.GetComponent<MeshFilter>().mesh;
        functionGenerator.GetComponent<FunctionGenerator>().SetFunctionString(Surface1Function);
        surfaceMesh = functionInstantiator.GetComponent<FunctionInstantiator>().ScaleMesh(surfaceMesh, 2.0f);
        Surface1.GetComponent<MeshFilter>().mesh = surfaceMesh;
        Surface1.GetComponent<MeshCollider>().sharedMesh = surfaceMesh;

        ActiveLineObjects = functionInstantiator.GetComponent<FunctionInstantiator>().ComputePoints(surfaceMesh, 2f);
        ActiveGradientObjects = functionInstantiator.GetComponent<FunctionInstantiator>().Gradients();

        Vector3 Origin = Surface1.transform.position;

        foreach (GameObject item in SurfaceItems)
        {
            Vector3 direction = item.transform.position - Origin;
            item.transform.position = Origin + direction * 2.0f;
            Vector3 pos = item.transform.position;
            Vector3 rayStart = new Vector3(pos.x, pos.y + 50f, pos.z);
            Ray ray = new Ray(rayStart, Vector3.down);

            if (Physics.Raycast(ray, out RaycastHit hit, 200f))
            {
                // Mover el objeto justo encima del nuevo mesh
                Debug.Log($"Colisión detectada para {item.name} en {hit.point}");
                item.transform.position = hit.point + Vector3.up * 0.01f; // pequeño offset para evitar clipping
            }
            else
            {
                Debug.LogWarning($"No se detectó el mesh debajo de {item.name}");
            }
        }
        // Cambiar el minimapa por la primera pista
        GameObject.Find("DesktopMiniMap").GetComponent<DesktopMiniMap>().SetClueSprite(0);
        GameObject.Find("WristMenu").GetComponent<WristMenu>().SetClueSprite(0);
        TreasureManagerObject.GetComponent<TreasureManager>().SetTotalTreasures(6);
        TreasureManagerObject.GetComponent<TreasureManager>().SetSurfaceRoutine(1);
        GoToSurfaceMenu.SetActive(true);
        GoToSurfaceMenu.GetComponent<TeleportPlayerToSurface>().SetSurface(Surface1);
        

    }

    [PunRPC]
    public void ProcessSurface2()
    {
        if (ActiveSurface == true)
        {
            Surface1.SetActive(false);
            ActiveLineObjects.ForEach(Destroy);
            ActiveLineObjects.Clear();
            ActiveGradientObjects.ForEach(Destroy);
            ActiveGradientObjects.Clear();
        }
        ActiveSurface = true;
        List<GameObject> SurfaceItems = new List<GameObject>();
        Surface2.GetChildGameObjects(SurfaceItems);
        ThisMenu.SetActive(false);
        Surface2.SetActive(true);
        functionInstantiator = GameObject.Find("FunctionInstantiator");
        functionGenerator = GameObject.Find("FunctionGenerator");
        treasureCreator = GameObject.Find("TreasureCreator");
        Mesh surfaceMesh = Surface2.GetComponent<MeshFilter>().mesh;
        functionGenerator.GetComponent<FunctionGenerator>().SetFunctionString(Surface2Function);
        surfaceMesh = functionInstantiator.GetComponent<FunctionInstantiator>().ScaleMesh(surfaceMesh, 2.0f);
        Surface2.GetComponent<MeshFilter>().mesh = surfaceMesh;
        surfaceMesh.RecalculateBounds();
        surfaceMesh.RecalculateNormals();
        Surface2.GetComponent<MeshCollider>().sharedMesh = null;
        Surface2.GetComponent<MeshCollider>().sharedMesh = surfaceMesh;

        ActiveLineObjects = functionInstantiator.GetComponent<FunctionInstantiator>().ComputePoints(surfaceMesh, 1f);
        ActiveGradientObjects = functionInstantiator.GetComponent<FunctionInstantiator>().Gradients();

        Vector3 Origin = Surface2.transform.position;

        foreach (GameObject item in SurfaceItems)
        {
            Vector3 direction = item.transform.position - Origin;
            item.transform.position = Origin + direction * 2.0f;
            Vector3 pos = item.transform.position;
            Vector3 rayStart = new Vector3(pos.x, pos.y + 50f, pos.z);
            Ray ray = new Ray(rayStart, Vector3.down);

            if (Physics.Raycast(ray, out RaycastHit hit, 200f))
            {
                // Mover el objeto justo encima del nuevo mesh
                Debug.Log($"Colisión detectada para {item.name} en {hit.point}");
                item.transform.position = hit.point + Vector3.up * 0.01f; // pequeño offset para evitar clipping
            }
            else
            {
                Debug.LogWarning($"No se detectó el mesh debajo de {item.name}");
            }
        }
        TreasureManagerObject.GetComponent<TreasureManager>().SetTotalTreasures(11);
        TreasureManagerObject.GetComponent<TreasureManager>().SetSurfaceRoutine(2);
        GoToSurfaceMenu.SetActive(true);
        GoToSurfaceMenu.GetComponent<TeleportPlayerToSurface>().SetSurface(Surface2);
        
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

    public void OnlineSurface1Call()
    {
        photonView.RPC("ProcessSurface1", RpcTarget.AllBuffered);
    }

    public void OnlineSurface2Call()
    {
        photonView.RPC("ProcessSurface2", RpcTarget.AllBuffered);
    }    

}
