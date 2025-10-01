using System;
using UnityEngine;
using Photon.Pun;
using System.Collections.Generic;
using UnityEngine.Rendering;

public class FunctionInstantiator : MonoBehaviour
{
    private GameObject FunctionGenerator;
    public GameObject functionMesh;
    private String MeshPath;
    public string TruePath;
    [SerializeField] private string SavedPath;
    private String FunctionType;
    private GameObject newsurface;
    private float surfaceMinX, surfaceMaxX, surfaceMinZ, surfaceMaxZ, surfaceMinY, surfaceMaxY;
    private GameObject supportSurface;
    private GameObject s;
    private GameObject LinerenderObject;

    private float curveHeight = 10f; // Altura de la curva de nivel
                                     // Tolerancia para considerar un punto

    public List<Vector3> intersectionPoints = new List<Vector3>();
    public List<GameObject> gradientObjects = new List<GameObject>();


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
        createSupportSurface();
    }



    // Update is called once per frame
    public void CreateSurface()
    {
        newsurface = new GameObject("FunctionSurface");
        newsurface.transform.position = new Vector3(-100, 100000, -100);
        //newsurface.transform.localScale = new Vector3(50, 50, 50);
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

    public void createSupportSurface()
    {
        supportSurface = new GameObject("SupportSurface");
        supportSurface.transform.position = new Vector3(-100, 99999, -100);
        supportSurface.transform.localScale = new Vector3(50, 50, 50);
        supportSurface.AddComponent<MeshFilter>();
        supportSurface.AddComponent<MeshCollider>();
        supportSurface.AddComponent<PhotonView>();
        supportSurface.AddComponent<PhotonTransformView>();
    }

    public void SetMesh()
    {
        GradienteCalculator GradientCalculator = new();
        SurfaceEvaluator surface = new SurfaceEvaluator(FunctionGenerator.GetComponent<FunctionGenerator>().WrittenFunction);
        MeshFilter GeneratorMeshFilter = FunctionGenerator.GetComponent<MeshFilter>();
        Mesh GeneratorMesh = GeneratorMeshFilter.mesh;
        newsurface.GetComponent<MeshFilter>().mesh = GeneratorMesh;
        supportSurface.GetComponent<MeshFilter>().mesh.vertices = GeneratorMesh.vertices;
        newsurface.GetComponent<MeshCollider>().sharedMesh = GeneratorMesh;
        supportSurface.GetComponent<MeshCollider>().sharedMesh = GeneratorMesh;
        supportSurface.GetComponent<MeshCollider>().providesContacts = true;
        newsurface.GetComponent<MeshCollider>().providesContacts = false;
        Mesh scaledmesh = GeneratorMesh;
        scaledmesh = ScaleMesh(scaledmesh);
        ComputePoints(scaledmesh);

        foreach (Vector3 v in intersectionPoints)
        {
            Vector2 pointXY = new Vector2(v.x, v.z); // suponiendo XZ como plano
            Vector2 grad = GradienteCalculator.CalculateGradient(pointXY, surface);
            Vector3 end = v + new Vector3(grad.x, 0f, grad.y) * 0.5f;
            Vector3 newend = end + new Vector3(-100, 100000.2f, -100); // elevar un poco el punto para mejor visibilidad
            CreateLineRendererObject();
            LineRenderer lr = LinerenderObject.GetComponent<LineRenderer>();
            lr.positionCount = 2;
            Vector3 newv = v + new Vector3(-100, 100000.2f, -100); // elevar un poco el punto para mejor visibilidad
            lr.SetPosition(0, newv);
            lr.SetPosition(1, newend);
        }
        DrawGradientAscencion(surface);
    }

    [PunRPC]
    private void CreateLineRendererObject()
    {
        LinerenderObject = new GameObject("CurveLineRenderer");
        LinerenderObject.layer = LayerMask.NameToLayer("Gradients");
        LineRenderer lineRenderer = LinerenderObject.AddComponent<LineRenderer>();
        lineRenderer.positionCount = 0;
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = Color.red;
        lineRenderer.endColor = Color.red;
        lineRenderer.useWorldSpace = true;
        lineRenderer.shadowCastingMode = ShadowCastingMode.Off;
        lineRenderer.receiveShadows = false;
        //LinerenderObject.setActive(false);
        gradientObjects.Add(LinerenderObject);
    }

    //-------------------------------------LevelCurves-------------------------------------

    public void ComputePoints(Mesh mesh)
    {
        Debug.Log("Computando puntos de intersección con la curva de nivel en y = " + curveHeight);
        intersectionPoints.Clear();
        Bounds bounds = mesh.bounds;

        // Obtener los límites del mesh
        float minY = bounds.min.y;
        float maxY = bounds.max.y;
        float minZ = bounds.min.z;
        float maxZ = bounds.max.z;
        float minX = bounds.min.x;
        float maxX = bounds.max.x;

        // Ajustar los límites al espacio mundial
        minY = (new Vector3(0, bounds.min.y, 0)).y;
        maxY = (new Vector3(0, bounds.max.y, 0)).y;
        surfaceMinX = (new Vector3(bounds.min.x, 0, 0)).x;
        surfaceMaxX = (new Vector3(bounds.max.x, 0, 0)).x;
        surfaceMinZ = (new Vector3(0, 0, bounds.min.z)).z;
        surfaceMaxZ = (new Vector3(0, 0, bounds.max.z)).z;



        for (float h = minY; h <= maxY + 1e-4f; h += 3f)
        {
            Debug.Log("Altura actual de la curva: " + h);
            ComputePointsAtHeight(mesh, h);
        }

    }

    private void ComputePointsAtHeight(Mesh mesh, float curveHeight)
    {
        int contadorEpsilon = 0;
        Vector3[] vertices = mesh.vertices;
        int[] triangles = mesh.triangles;

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
            CheckEdge(p1, p2, curveHeight, ref contadorEpsilon);
            CheckEdge(p2, p3, curveHeight, ref contadorEpsilon);
            CheckEdge(p3, p1, curveHeight, ref contadorEpsilon);
        }

        Debug.Log("Contador: " + contadorEpsilon);
    }

    private void CheckEdge(Vector3 p1, Vector3 p2, float curveHeight, ref int contadorEpsilon)
    {
        float epsilon = 0.01f;
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
                s.AddComponent<PhotonView>();
                s.transform.position = (worldPoint);
                s.GetComponent<SphereCollider>().enabled = false;
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
            s.AddComponent<PhotonView>();
            s.transform.position = worldP1;
            s.GetComponent<SphereCollider>().enabled = false;
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
            s.AddComponent<PhotonView>();
            s.transform.position = worldP2;
            s.GetComponent<SphereCollider>().enabled = false;
            s.transform.localScale = Vector3.one * 0.2f; // Escala de la esfera
            contadorEpsilon++;
        }
    }

    public Mesh ScaleMesh(Mesh mesh)
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
    //Metodo para dibujar la ascencion por gradiente, con este metodo se obtiene una lista de puntos que representan la linea, y que seben ser unidos por un line renderer.
    private void DrawGradientAscencion(SurfaceEvaluator surface)
    {
        //Se crea una lista de puntos para la linea.
        List<Vector3> points = new List<Vector3>();
        Vector3 puntoinicial = new Vector3(-103, 100000, -103);
        Debug.Log("Punto inicial: " + puntoinicial);
        points.Add(puntoinicial);
        Vector2 actual2D = new Vector2(puntoinicial.x, puntoinicial.z);
        //Se calcula la y del punto inicial en base a la formula de la funcion ingresada por el profesor.
        double y = surface.ComputeZ(actual2D.x, actual2D.y);
        Vector3 actual3D = new Vector3(actual2D.x, (float)y, actual2D.y);
        Debug.Log("Punto inicial 3D corregido: " + actual3D);
        points.Add(actual3D);
        actual2D = new Vector2(actual3D.x, actual3D.z);
        
        //Se calcula el gradiente en el punto inicial.
        Vector2 gradiente = GradienteCalculator.CalculateGradient(actual2D, surface);
        //Se calcula el siguiente X Y Z
        double siguienteX = actual3D.x + gradiente.x * 0.1f;
        double siguienteZ = actual3D.z + gradiente.y * 0.1f;
        double siguienteY = surface.ComputeZ(siguienteX, siguienteZ);
        Vector3 siguiente3D = new Vector3((float)siguienteX, (float)siguienteY, (float)siguienteZ);
        while (siguiente3D.y > actual3D.y)
        {
            //Se añade el siguiente punto a la lista de puntos.
            Debug.Log("Punto siguiente 3D: " + siguiente3D);
            points.Add(siguiente3D);
            actual3D = siguiente3D;
            //Se calcula el siguiente punto
            actual2D = new Vector2(actual3D.x, actual3D.z);
            gradiente = GradienteCalculator.CalculateGradient(actual2D, surface);
            siguienteX = actual3D.x + (gradiente.x) * 0.1f;
            siguienteZ = actual3D.z + (gradiente.y) * 0.1f;
            siguienteY = surface.ComputeZ(siguienteX, siguienteZ);
            siguiente3D = new Vector3((float)siguienteX, (float)siguienteY, (float)siguienteZ);
            //Revisar si el siguiente punto está fuera de los límites de la superficie
            if (siguiente3D.x < surfaceMinX || siguiente3D.x > surfaceMaxX || siguiente3D.z < surfaceMinZ || siguiente3D.z > surfaceMaxZ || siguiente3D.y < surfaceMinY || siguiente3D.y > surfaceMaxY) break;
        }
        //Se crea el line renderer y se le asignan los puntos.
        GameObject lineObject = new GameObject("GradientAscensionLine");
        LineRenderer lineRenderer = lineObject.AddComponent<LineRenderer>();
        lineRenderer.positionCount = points.Count;
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = Color.blue;
        lineRenderer.endColor = Color.blue;
        lineRenderer.useWorldSpace = true;
        lineRenderer.shadowCastingMode = ShadowCastingMode.Off;
        lineRenderer.receiveShadows = false;
        for (int i = 0; i < points.Count; i++)
        {
            lineRenderer.SetPosition(i, points[i]);
            Debug.Log("Punto " + i + ": " + points[i]);
        }

    }

}

//-------------------------------------Gradiente Calculator-------------------------------------

public class GradienteCalculator
{

    public static float epsilon = 0.01f;

    public static Vector2 CalculateGradient(Vector2 point, SurfaceEvaluator
     surface)
    {
        float x1 = point.x;
        float y1 = point.y;

        float x2 = x1 + epsilon;
        float y2 = y1 + epsilon;

        double dzx = surface.ComputeZ(x2, y1) - surface.ComputeZ(x1, y1);
        double dzy = surface.ComputeZ(x1, y2) - surface.ComputeZ(x1, y1);

        float dzdx = (float)(dzx / epsilon);
        float dzdy = (float)(dzy / epsilon);

        Vector2 gradient = new Vector2(dzdx, dzdy).normalized;
        return gradient;

    }
}

//-------------------------------------Gradient ascension-------------------------------------

//-------------------------------------Surface Evaluator-------------------------------------

public class SurfaceEvaluator
{
    private Node ast;
    public SurfaceEvaluator(string writtenFunction)
    {
        Tokenizer tokenizer = new Tokenizer(writtenFunction);
        Parser parser = new Parser(tokenizer);
        ast = parser.Parse();
    }

    public double ComputeZ(double x, double z)
    {
        var variables = new Dictionary<string, double>
        {
            { "x", x },
            { "y", z }
        };
        return ast.Evaluate(variables);
    }
}