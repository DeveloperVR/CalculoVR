using System;
using System.IO;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using Photon.Pun;
using System.Collections.Generic;

public class FunctionInstantiator : MonoBehaviour
{
    private GameObject FunctionGenerator;
    public GameObject functionMesh;
    private String MeshPath;
    public string TruePath;
    [SerializeField] private string SavedPath;
    private String FunctionType;
    private GameObject newsurface;
    private GameObject s;

    private float curveHeight = 10f; // Altura de la curva de nivel
    private float epsilon = 0.001f; // Tolerancia para considerar un punto

    public List<Vector3> intersectionPoints = new List<Vector3>();


    void Start()
    {
        s = new GameObject();
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
        Mesh scaledmesh = GeneratorMesh;
        scaledmesh = scaleMesh(scaledmesh);
        ComputePoints(scaledmesh);
    }

    public void ComputePoints(Mesh mesh)
    {
        Debug.Log("Computando puntos de intersección con la curva de nivel en y = " + curveHeight);
        intersectionPoints.Clear();
        int contadorEpsilon = 0;

        Vector3[] vertices = mesh.vertices;
        int[] triangles = mesh.triangles;

        // Recorremos los triángulos de la malla
        for (int i = 0; i < triangles.Length; i += 3)
        {
            // Cada triángulo tiene 3 vértices
            Vector3 p1 = (vertices[triangles[i]]);
            Vector3 p2 = (vertices[triangles[i + 1]]);
            Vector3 p3 = (vertices[triangles[i + 2]]);

            // Revisar las aristas del triángulo
            Debug.Log("Altura curva: " + curveHeight);
            Debug.Log("Mesh bounds local Y: " + mesh.bounds.min.y + " -> " + mesh.bounds.max.y);
            Debug.Log("Mesh bounds world Y: " + (transform.position.y + mesh.bounds.min.y) + " -> " + (transform.position.y + mesh.bounds.max.y));
            CheckEdge(p1, p2, ref contadorEpsilon);
            CheckEdge(p2, p3, ref contadorEpsilon);
            CheckEdge(p3, p1, ref contadorEpsilon);
        }

        Debug.Log("Contador: " + contadorEpsilon);
    }

    private void CheckEdge(Vector3 p1, Vector3 p2, ref int contadorEpsilon)
    {
        float y1 = p1.y;
        float y2 = p2.y;

        // Caso 1: la arista cruza el plano
        if ((y1 < curveHeight && y2 > curveHeight) || (y1 > curveHeight && y2 < curveHeight))
        {
            Debug.Log("Entró al caso 1");
            Vector3 dir = p2 - p1;
            float dy = dir.y;

            if (Mathf.Abs(dy) > 1e-6f) // evitar división por cero
            {
                Debug.Log("Entró al caso 1.1");
                float yc = curveHeight - p1.y;
                float t = yc / dy; // interpolación [0,1]
                float x = p1.x + dir.x * t;
                float y = curveHeight;
                float z = p1.z + dir.z * t;

                Vector3 localPoint = new Vector3(x, y, z);
                intersectionPoints.Add(localPoint);

                Vector3 worldPoint = transform.TransformPoint(localPoint);
                s = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                s.transform.position = (worldPoint);
                s.transform.localScale = Vector3.one * 0.2f; // Escala de la esfera
                Debug.Log("Punto de intersección: " + localPoint.ToString());
            }
        }
        // Caso 2: uno de los extremos está en el plano
        else if (Mathf.Abs(p1.y - curveHeight) < epsilon)
        {
            Debug.Log("Entró al caso 2");
            intersectionPoints.Add(p1);

            Vector3 worldP1 = transform.TransformPoint(p1);
            Debug.Log("Punto de intersección: " + p1.ToString());
            s = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            s.transform.position = worldP1;
            s.transform.localScale = Vector3.one * 0.2f; // Escala de la esfera
            contadorEpsilon++;
        }
        else if (Mathf.Abs(p2.y - curveHeight) < epsilon)
        {
            Debug.Log("Entró al caso 2.1");
            intersectionPoints.Add(p2);

            Vector3 worldP2 = transform.TransformPoint(p2);
            Debug.Log("Punto de intersección: " + p2.ToString());
            s = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            s.transform.position = worldP2;
            s.transform.localScale = Vector3.one * 0.2f; // Escala de la esfera
            contadorEpsilon++;
        }
    }
    
    public Mesh scaleMesh(Mesh mesh)
    {
        Vector3[] vertices = mesh.vertices;
        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i] *= 50f; // Escalar a 50 veces
        }
        mesh.vertices = vertices;
        mesh.RecalculateBounds();
        return mesh;
    }
}