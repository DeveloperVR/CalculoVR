using System;
using System.IO;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using Photon.Pun;

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
        newsurface.transform.position = new Vector3(-100, 100000, -100);
        newsurface.transform.localScale = new Vector3(50, 50, 50);
        newsurface.AddComponent<MeshFilter>();
        newsurface.AddComponent<MeshRenderer>();
        newsurface.AddComponent<MeshCollider>();
        newsurface.AddComponent<PhotonView>();
        newsurface.AddComponent<PhotonTransformView>();
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

    public void SetMesh()
    {
        MeshFilter GeneratorMeshFilter = FunctionGenerator.GetComponent<MeshFilter>();
        Mesh GeneratorMesh = GeneratorMeshFilter.mesh;
        newsurface.GetComponent<MeshFilter>().mesh = GeneratorMesh;
        newsurface.GetComponent<MeshCollider>().sharedMesh = GeneratorMesh;
        newsurface.GetComponent<MeshCollider>().providesContacts = true;
    }
}