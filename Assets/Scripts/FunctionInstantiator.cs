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
    [SerializeField] private string SavedPath;
    private String FunctionType;
    private GameObject newsurface;
    void Start()
    {
        FunctionGenerator = GameObject.Find("FunctionGenerator");
        if (FunctionGenerator == null)
        {
            Debug.LogError("FunctionGenerator not found in the scene.");
            return;
        }
        FunctionType = FunctionGenerator.GetComponent<FunctionGenerator>().FunctionType;
        CreateSurface();
    }

    // Update is called once per frame
    public void CreateSurface()
    {
        newsurface = new GameObject("FunctionSurface");
        newsurface.transform.position = new Vector3(-100, -100, -100);
        newsurface.transform.localScale = new Vector3(50, 50, 50);
        newsurface.AddComponent<MeshFilter>();
        newsurface.AddComponent<MeshRenderer>();
        newsurface.AddComponent<MeshCollider>();
        Material mat = Resources.Load<Material>("Materials/Surfaces Material");
        Material mat2 = Resources.Load<Material>("Materials/Surfaces Material");
        if (mat != null)
        {
            newsurface.GetComponent<MeshRenderer>().materials = new Material[] { mat, mat2 };
        }
        else
        {
            Debug.LogError("Material not found!");
        }
        
    }

    public void SetMesh(string path)
    {
        MeshPath = path;
        string ShortPath = Path.GetFileNameWithoutExtension(MeshPath);
        TruePath = "Meshes/" + ShortPath;
        SavedPath = TruePath;
        Debug.Log("Mesh path set to: " + TruePath);
        if (SavedPath == null || SavedPath == "")
        {
            Debug.LogError("Mesh path is not set. Please set the mesh path before creating the surface.");
            return;
        }
        Mesh loadedMesh = Resources.Load<Mesh>(SavedPath);
        newsurface.GetComponent<MeshFilter>().mesh = loadedMesh;
        Debug.Log("Loaded mesh from path: " + SavedPath);
        if (loadedMesh == null)
        {
            Debug.LogError("Failed to load mesh.");
            return;
        }

    }
}
