using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif



public class FunctionGenerator : MonoBehaviour
{
    public tipoFuncion function; //La función que se pintará.

    //En Unity Z es lo que en cálculo es Y. Puede ser confuso, pero en este código B será el coeficiente de Z.
    public float A; //Coeficiente de X.
    public float B; //Coeficiente de Z.
    private GameObject FunctionInstantiator; //Objeto que instanciará la función.

    [Range(1, 100)] // Rango que aparecerá en el inspector.
    public int xSegments; // Número de vértices en X.
    [Range(1, 100)]
    public int ySegments; // Número de vértices en Y.

    public List<Vector3> verts; // Lista de vértices generados.

    public int[] triangles;

    private Mesh mesh;
    private MeshRenderer meshRenderer;
    public Material whiteMat;
    public Material blackMat;
    private string meshPath;
    [SerializeField] private string SavedMeshPath;
    private GameObject FunctionInstantiator;

    public string FunctionType { get; private set; }

    private void Start()
    {
        FunctionInstantiator = GameObject.Find("FunctionInstantiator");
        MakeFunction();
        
    }

    ///<summary>
    /// Código que genera la gráfica de la función.
    ///<summary>
    public void MakeFunction()
    {
        //Si el número de segmentos es par, se agrega uno más para que la gráfica sea simétrica
        if (xSegments % 2 == 0)
        {
            xSegments += 1;
        }
        if (ySegments % 2 == 0)
        {
            ySegments += 1;
        }
        MakeVertices(function);

        if (verts == null || verts.Count == 0)
        {
            Debug.LogFormat("No se crearon vértices");
            return; // Manejar una lista de vértices vacía.
        }

        triangles = GenerateTriangles(verts.Count); // orden en que se unirán los vértices para crear los triángulos.

        mesh = new Mesh();
        meshRenderer = GetComponent<MeshRenderer>();
        GetComponent<MeshFilter>().mesh = mesh; // El objeto que tenga este código debe tener un MeshFilter.

        UpdateMesh();
        PaintMesh();
        SaveMesh();
        SavePrefab();
        Debug.Log("Enviando el Mesh: " + SavedMeshPath + " al Instantiator");
        FunctionInstantiator.GetComponent<FunctionInstantiator>().SetMesh(SavedMeshPath);
    }

    public void MakeVertices(tipoFuncion function)
    {
        //Se crea un Switch para cuando se deseen agregar más funciones
        switch (function)
        {
            case tipoFuncion.HyperbolicParaboloid:
                verts = HyperbolicParaboloid();
                break;
            case tipoFuncion.EllipticParaboloid:
                verts = EllipticParaboloid();
                break;
            default:
                break;
        }
    }

    //Estos métodos se usarán para llamar desde el botón
    public void SetFunctionToHyperbolicParaboloid()
    {
        function = tipoFuncion.HyperbolicParaboloid;
        setFunctionType("HyperbolicParaboloid");
        MakeFunction();
    }
    public void SetFunctionToEllipticParaboloid()
    {
        function = tipoFuncion.EllipticParaboloid;
        setFunctionType("EllipticParaboloid");
        MakeFunction();
    }

    /// <summary>
    /// Este método retorna los vertices necesarios para construir el paraboloide hiperbólico.
    /// </summary>
    public List<Vector3> HyperbolicParaboloid()
    {
        List<Vector3> verts = new List<Vector3>();
        float stepX = 1.0f / (xSegments - 1); // Esta es la distancia entre vértices en x.
        float stepY = 1.0f / (ySegments - 1); // Esta es la distancia entre vértices en y.

        // Se comienza en la mitad negativa del Rango, y va hasta la mitad positiva. Es decir, para un rango de 100. se comienza en -50 y va hasta 50.
        for (int y = -ySegments / 2; y <= (ySegments / 2); y++)
        {
            for (int x = -xSegments / 2; x <= (xSegments / 2); x++)
            {
                float xPos = x * stepX;
                float zPos = y * stepY; // z se trata como y.
                float yPos = (A * xPos * xPos) + (-B * zPos * zPos);// y se trata como z. Esta es la ecuación del paraboloide hiperbólico
                verts.Add(new Vector3(xPos, yPos, zPos));
            }
        }
        Debug.Log("Vértices totales: " + verts.Count);
        return verts;
    }

    /// <summary>
    /// Este método retorna los vertices necesarios para construir el paraboloide elíptico.
    /// </summary>
    public List<Vector3> EllipticParaboloid()
    {
        List<Vector3> verts = new List<Vector3>();
        float stepX = 1.0f / (xSegments - 1); // Esta es la distancia entre vértices en x.
        float stepY = 1.0f / (ySegments - 1); // Esta es la distancia entre vértices en y.

        // Se comienza en la mitad negativa del Rango, y va hasta la mitad positiva. Es decir, para un rango de 100. se comienza en -50 y va hasta 50.
        for (int y = -ySegments / 2; y <= (ySegments / 2); y++)
        {
            for (int x = -xSegments / 2; x <= (xSegments / 2); x++)
            {
                float xPos = x * stepX;
                float zPos = y * stepY; // z se trata como y.
                float yPos = (A * xPos * xPos) + (B * zPos * zPos);// y se trata como z. Esta es la ecuación del paraboloide elíptico
                verts.Add(new Vector3(xPos, yPos, zPos));
            }
        }
        Debug.Log("Vértices totales: " + verts.Count);
        return verts;
    }

    /// <summary>
    /// Este método retorna un arreglo de los índices de los vértices ordenados por los triángulos con los que se generará el Mesh.
    /// <summary>
    private int[] GenerateTriangles(int numVertices)
    {
        int[] tris = new int[numVertices * 6]; // dos triángulos por segmento, cada triángulo con 3 vértices.

        int index = 0;
        for (int y = 0; y < ySegments - 1; y++)
        {
            for (int x = 0; x < xSegments - 1; x++)
            {
                int bottomLeft = y * xSegments + x;
                int bottomRight = bottomLeft + 1;
                int topLeft = bottomLeft + xSegments;
                int topRight = topLeft + 1;

                // **Solo superficie superior**
                // Primer triángulo (abajo-izquierda a arriba-izquierda a abajo-derecha)
                tris[index++] = bottomLeft;
                tris[index++] = topLeft;
                tris[index++] = bottomRight;

                // Segundo triángulo (abajo-derecha a arriba-izquierda a arriba-derecha)
                tris[index++] = bottomRight;
                tris[index++] = topLeft;
                tris[index++] = topRight;
            }
        }

        Debug.Log("Triángulos: " + tris.Length);
        return tris;
    }

    void UpdateMesh()
    {
        mesh.Clear();
        mesh.vertices = verts.ToArray();
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }

    void PaintMesh()
    {
        mesh.subMeshCount = 2;
        List<int> submesh1 = new List<int>();
        List<int> submesh2 = new List<int>();
        for (int i = 0; i < triangles.Length; i += 3)
        {
            // Se intercalan los colores
            if (i % 2 == 0)
            {
                submesh1.Add(triangles[i]);
                submesh1.Add(triangles[i + 1]);
                submesh1.Add(triangles[i + 2]);
            }
            else
            {
                submesh2.Add(triangles[i]);
                submesh2.Add(triangles[i + 1]);
                submesh2.Add(triangles[i + 2]);
            }
        }

        // Convierte las listas en arreglos.
        int[] submesh1Array = submesh1.ToArray();
        int[] submesh2Array = submesh2.ToArray();

        // Agrega los triangulos a los SubMesh
        mesh.SetTriangles(submesh1Array, 0);
        mesh.SetTriangles(submesh2Array, 1);


        // Assign materials to submeshes (assuming alternating white/black pattern)
        meshRenderer.sharedMaterials = new Material[] { whiteMat, blackMat };
    }

    public void SaveMesh()
    {
#if UNITY_EDITOR
        if (!Directory.Exists("Assets/Meshes"))
            AssetDatabase.CreateFolder("Assets", "Meshes");
        string meshPath;
        switch (function)
        {
            case tipoFuncion.HyperbolicParaboloid:
                meshPath = "Assets/Resources/Meshes/HyperbolicParaboloid.mesh";
                break;
            case tipoFuncion.EllipticParaboloid:
                meshPath = "Assets/Resources/Meshes/EllipticParaboloid.mesh";
                break;
            default:
                meshPath = "Assets/Resources/Meshes/FailedMesh.mesh";
                break;
        }
        AssetDatabase.CreateAsset(mesh, meshPath);
        FunctionInstantiator.GetComponent<FunctionInstantiator>().SetMesh(meshPath);
#endif
    }

    public void SavePrefab()
    {
#if UNITY_EDITOR
        if (!Directory.Exists("Assets/Prefabs"))
            AssetDatabase.CreateFolder("Assets", "Prefabs");
        string localPath;
        switch (function)
        {
            case tipoFuncion.HyperbolicParaboloid:
                localPath = "Assets/Prefabs/"+ FunctionType +".prefab";
                break;
            case tipoFuncion.EllipticParaboloid:
                localPath = "Assets/Prefabs/"+ FunctionType +".prefab";
                break;
            default:
                localPath = "Assets/Prefabs/FailedPrefab.prefab";
                break;
        }


        // Solo usar esto si se quieren guardar prefabs diferentes.
        //localPath = AssetDatabase.GenerateUniqueAssetPath(localPath);

        bool prefabSuccess;
        PrefabUtility.SaveAsPrefabAsset(gameObject, localPath, out prefabSuccess);
        if (prefabSuccess == true)
            Debug.Log("Prefab guardado con éxito");
        else
            Debug.Log("Error al guardar el prefab" + prefabSuccess);
#endif
    }
    
    public void setFunctionType(string type)
    {
        FunctionType = type;
    }
}

public enum tipoFuncion
    {
        HyperbolicParaboloid,
        EllipticParaboloid
    }
