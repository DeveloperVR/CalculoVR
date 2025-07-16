using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

public class FunctionInstantiator : MonoBehaviour
{
    private GameObject FunctionGenerator;
    public GameObject functionMesh;
    private String MeshPath;
    public string TruePath;
    private String FunctionType;
    void Start()
    {
        FunctionGenerator = GameObject.Find("FunctionGenerator");
        if (FunctionGenerator == null)
        {
            Debug.LogError("FunctionGenerator not found in the scene.");
            return;
        }
        FunctionType = FunctionGenerator.GetComponent<FunctionGenerator>().FunctionType;
    }

    // Update is called once per frame
    public void CreateSurface()
    {
        //Creates the GameObject for the surface and sets the components that it needs
        GameObject newsurface = new GameObject("Surface");
        newsurface.transform.position = new Vector3(-100, -100, -100);
        newsurface.transform.localScale = new Vector3(50, 50, 50);
        newsurface.AddComponent<MeshFilter>();
        newsurface.AddComponent<MeshRenderer>();
        newsurface.AddComponent<MeshCollider>();
        Mesh loadedMesh = Resources.Load<Mesh>("Meshes/HyperbolicParaboloid");
        Debug.Log("Loaded mesh from path: " + TruePath);
        if (loadedMesh == null)
        {
            Debug.LogError("Failed to load mesh.");
            return;
        }
        newsurface.GetComponent<MeshFilter>().mesh = loadedMesh;
    }

    public void SetMeshPath(string path)
    {
        MeshPath = path;
        string ShortPath = Path.GetFileNameWithoutExtension(MeshPath);
        TruePath = "Meshes/" + ShortPath;
        Debug.Log("Mesh path set to: " + TruePath);

    }

    public void StartCreation()
    {
        CreateSurface();

    }
}
