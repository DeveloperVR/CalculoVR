using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.CompilerServices;

#if UNITY_EDITOR
using UnityEditor;
#endif



public class FunctionGenerator : MonoBehaviour
{
    public tipoFuncion function; //La función que se pintará.

    private GameObject StringCaptured; //the GameObject that gets the string for the free resolver

    //En Unity Z es lo que en cálculo es Y. Puede ser confuso, pero en este código B será el coeficiente de Z.
    public float A; //Coeficiente de X.
    public float B; //Coeficiente de Z.
    private GameObject FunctionInstantiator; //Objeto que instanciará la función.

    [Range(1, 1000)] // Rango que aparecerá en el inspector.
    public int xSegments; // Número de vértices en X.
    [Range(1, 1000)]
    public int ySegments; // Número de vértices en Y.

    public List<Vector3> verts; // Lista de vértices generados.

    public int[] triangles;
    public List<Vector3> intersectionPoints = new List<Vector3>();
    private float curveHeight = 0.3f;
    private float epsilon = 0.01f;

    private Mesh mesh;
    //private MeshRenderer meshRenderer;
    public Material whiteMat;
    public Material blackMat;

    public string FunctionType { get; private set; }

    private string GlobalMeshPath;

    public string HyperbolicParaboloidPath = "Assets/Resources/Meshes/HyperbolicParaboloid.asset";
    public string EllipticParaboloidPath = "Assets/Resources/Meshes/EllipticParaboloid.asset";

    public string FreeMeshPath = "Assets/Resources/Meshes/FreeFunction.asset";

    public string WrittenFunction;
    private GameObject s;

    private void Start()
    {
        s = new GameObject();
        FunctionInstantiator = GameObject.Find("FunctionInstantiator");
        StringCaptured = GameObject.Find("EnterFunction");

        //MakeFunction();
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
        //meshRenderer = GetComponent<MeshRenderer>();
        GetComponent<MeshFilter>().mesh = mesh; // El objeto que tenga este código debe tener un MeshFilter.

        UpdateMesh();
        PaintMesh();
        SaveMesh();
        SavePrefab();
        FunctionInstantiator.GetComponent<FunctionInstantiator>().SetMesh();
        //ComputePoints(mesh);
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
            case tipoFuncion.OriginalFunction:
                verts = FreeFunctionResolver();
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

    public void SetFunctionToFreeFunction(string functionReceived)
    {
        StringCaptured = GameObject.Find("EnterFunction");
        function = tipoFuncion.OriginalFunction;
        setFunctionType("OrignalFunction");
        WrittenFunction = functionReceived;
        MakeFunction();
    }

    /// <summary>
    /// Este método retorna los vertices necesarios para construir el paraboloide hiperbólico.
    /// </summary>
    public List<Vector3> FreeFunctionResolver()
    {
        Debug.Log("Entró a free Resolver, activando Parser");
        List<Vector3> verts = new List<Vector3>();
        double stepX = 70.0 / (xSegments - 1);
        double stepY = 70.0 / (ySegments - 1);

        Tokenizer tokenizer = new Tokenizer(WrittenFunction);
        Parser parser = new Parser(tokenizer);
        Node ast = parser.Parse();
        for (int y = -ySegments / 2; y <= ySegments / 2; y++)
        {
            for (int x = -xSegments / 2; x <= xSegments / 2; x++)
            {
                double xPos = x * stepX;
                double zPos = y * stepY;
                Dictionary<string, double> variables = new Dictionary<string, double>
                {
                    { "x", xPos },
                    { "y", zPos }
                };

                double yPos = ast.Evaluate(variables);

                verts.Add(new Vector3((float)xPos, (float)yPos, (float)zPos));
            }
        }
        GlobalMeshPath = FreeMeshPath;
        return verts;
    }
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
        GlobalMeshPath = HyperbolicParaboloidPath;
        LevelCurvesCreator levelCurvesCreator = GetComponent<LevelCurvesCreator>();
        levelCurvesCreator.GenerateLevelCurve(verts, xSegments, ySegments);
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
        GlobalMeshPath = EllipticParaboloidPath;
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
        //meshRenderer.sharedMaterials = new Material[] { whiteMat, blackMat };
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
                meshPath = "Assets/Resources/Meshes/HyperbolicParaboloid.asset";
                break;
            case tipoFuncion.EllipticParaboloid:
                meshPath = "Assets/Resources/Meshes/EllipticParaboloid.asset";
                break;
            case tipoFuncion.OriginalFunction:
                meshPath = "Assets/Resources/Meshes/FreeFunction.asset";
                break;
            default:
                meshPath = "Assets/Resources/Meshes/FailedMesh.asset";
                break;
        }
        AssetDatabase.CreateAsset(mesh, meshPath);
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
                localPath = "Assets/Prefabs/HyperbolicParaboloid.prefab";
                break;
            case tipoFuncion.EllipticParaboloid:
                localPath = "Assets/Prefabs/EllipticParaboloid.prefab";
                break;
            case tipoFuncion.OriginalFunction:
                localPath = "Assets/Prefabs/FreeFunction.prefab";
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
                s.transform.localScale = Vector3.one * 0.5f; // Escala de la esfera
                Debug.Log("Punto de intersección: " + localPoint.ToString());
            }
        }
        // Caso 2: uno de los extremos está en el plano
        else if (Mathf.Abs(p1.y - curveHeight) < epsilon)
        {
            Debug.Log("Entró al caso 2");
            intersectionPoints.Add(p1);
            Debug.Log("Punto de intersección: " + p1.ToString());
            s = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            s.transform.position = new Vector3(p1.x - 100, p1.y + 100000, p1.z - 100);
            s.transform.localScale = Vector3.one * 0.5f; // Escala de la esfera
            contadorEpsilon++;
        }
        else if (Mathf.Abs(p2.y - curveHeight) < epsilon)
        {
            Debug.Log("Entró al caso 2.1");
            intersectionPoints.Add(p2);
            Debug.Log("Punto de intersección: " + p2.ToString());
            s = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            s.transform.position = new Vector3(p2.x - 100, p2.y + 100000, p2.z - 100);
            s.transform.localScale = Vector3.one * 0.5f; // Escala de la esfera
            contadorEpsilon++;
        }
    }


}

public enum tipoFuncion
{
    HyperbolicParaboloid,
    EllipticParaboloid,
        
    OriginalFunction
}
