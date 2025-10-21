using System;
using UnityEngine;
using Photon.Pun;
using System.Collections.Generic;
using UnityEngine.Rendering;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine.ProBuilder.Shapes;
using System.Linq;

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
    private float LevelCurvesIterations = 0;
    private GameObject supportSurface;
    private GameObject s;
    private GameObject LinerenderObject;
    public Vector3 OriginPoint;

    public float scale = 100f;

    private float curveHeight = 10f; // Altura de la curva de nivel
                                     // Tolerancia para considerar un punto
                                     //Esta lista se usa para las curvas de nivel, ya que debe ser borrada cada vez que se calcula una nueva curva.
    public List<Vector3> intersectionPoints = new List<Vector3>();
    //Esta lista se usa para las gradientes, ya que debe conservar todos los puntos de intersección.
    public List<Vector3> intersectionPointsForGradient = new List<Vector3>();
    public List<GameObject> gradientObjects = new List<GameObject>();
    private bool isLevelCurvesReady = false;
    private bool LineRendererCurvesReady = false;


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

    void Update()
    {
        return;
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
        supportSurface.transform.localScale = new Vector3(scale, scale, scale);
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
        foreach (Vector3 v in intersectionPointsForGradient)
        {
            Vector2 pointXY = new Vector2(v.x, v.z); // suponiendo XZ como plano
            Vector2 grad = GradienteCalculator.CalculateScaledGradient(pointXY, surface);
            Vector3 end = v + new Vector3(grad.x, 0f, grad.y) * 0.50f;
            Vector3 newend = end + new Vector3(-100, 100003f, -100); // elevar un poco el punto para mejor visibilidad
            //Se crea un detector de gradiente en el punto de intersección para que solo las gradientes cercanas al jugador se activen.
            GameObject detector = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            detector.name = "GradientDetector";
            detector.transform.position = newend;
            detector.GetComponent<Renderer>().enabled = false;
            detector.GetComponent<Collider>().isTrigger = true;
            detector.GetComponent<Collider>().providesContacts = false;
            detector.transform.localScale = new Vector3(2, scale, 2);
            detector.AddComponent<GradientDetector>();


            CreateLineRendererObject(detector);
            LineRenderer lr = LinerenderObject.GetComponent<LineRenderer>();
            lr.positionCount = 2;
            Vector3 newv = v + new Vector3(-100, 100003f, -100); // elevar un poco el punto para mejor visibilidad
            lr.SetPosition(0, newv);
            lr.SetPosition(1, newend);
        }
    }

    [PunRPC]
    private void CreateLineRendererObject(GameObject detector)
    {
        LinerenderObject = new GameObject("CurveLineRenderer");
        LinerenderObject.transform.SetParent(this.transform);
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
        LinerenderObject.SetActive(false);
        gradientObjects.Add(LinerenderObject);
        detector.GetComponent<GradientDetector>().SetLine(LinerenderObject);
    }

    //-------------------------------------LevelCurves-------------------------------------

    public void ComputePoints(Mesh mesh)
    {
        intersectionPoints.Clear();
        SurfaceEvaluator surface = new SurfaceEvaluator(FunctionGenerator.GetComponent<FunctionGenerator>().WrittenFunction);
        Bounds bounds = mesh.bounds;
        int levelCurveIndex = 0;

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
        OriginPoint = new Vector3(-100, (float)surface.ComputeZ(0, 0) * scale + 100002f, -100);
        // Debug.Log("Límites del mesh - minY: " + minY + ", maxY: " + maxY + ", minX: " + minX + ", maxX: " + maxX + ", minZ: " + minZ + ", maxZ: " + maxZ);
        // Debug.Log("Su posición en el mund del limite inferior de y es: " + (new Vector3(0, bounds.min.y, 0)).y);



        for (float h = minY; h <= maxY + 1e-4f; h += 1f)
        {
            //Debug.Log("Altura actual de la curva: " + h);
            ComputePointsAtHeight(mesh, h);
            levelCurveIndex++;
        }
    }

    private void ComputePointsAtHeight(Mesh mesh, float curveHeight)
    {
        intersectionPoints.Clear();
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
            //Debug.Log("p1: " + p1.ToString() + " p2: " + p2.ToString() + " p3: " + p3.ToString());
            CheckEdge(p1, p2, curveHeight, ref contadorEpsilon);
            CheckEdge(p2, p3, curveHeight, ref contadorEpsilon);
            CheckEdge(p3, p1, curveHeight, ref contadorEpsilon);
        }
        if (intersectionPoints.Count > 1)
        {
            // Agrupar por cercanía
            List<List<Vector3>> groups = GroupPointsByProximity(intersectionPoints, 7.0f);

            int groupIndex = 0;
            foreach (List<Vector3> group in groups)
            {
                List<Vector3> orderedPoints = OrderPointsByProximity(group);
                GameObject lineObj = CreateLevelCurveLine(curveHeight);

                LineRenderer lineRenderer = lineObj.GetComponent<LineRenderer>();
                Debug.Log("La altura " + curveHeight + " Grupo " + groupIndex + " con " + (orderedPoints.Count - 1) + " puntos ordenados.");
                lineRenderer.positionCount = orderedPoints.Count;
                for (int i = 0; i < orderedPoints.Count-1; i++)
                {
                    if(orderedPoints[i] == Vector3.zero || orderedPoints[i + 1] == Vector3.zero)
                    {
                        Debug.Log("Punto nulo " + i + " en la lista de puntos ordenados, saltando.");
                        continue;
                    }
                    if (Vector3.Distance(orderedPoints[i], orderedPoints[i + 1]) > 7.0f)
                    {
                        Debug.Log("Saltando punto distante: " + orderedPoints[i] + " a " + orderedPoints[i + 1]);
                        continue;
                    }
                    else
                    {
                        lineRenderer.SetPosition(i, orderedPoints[i] + new Vector3(-100, 100000, -100)); // elevar un poco el punto para mejor visibilidad
                    }
                    if (i == orderedPoints.Count - 2 && Vector3.Distance(orderedPoints[i], orderedPoints[0]) <= 7.0f && orderedPoints[0] != Vector3.zero)
                    {
                        Debug.Log("Cerrando curva entre: " + orderedPoints[i] + " y " + orderedPoints[0]);
                        lineRenderer.SetPosition(i + 1, orderedPoints[0] + new Vector3(-100, 100000, -100));
                    }
                }
                //lineRenderer.SetPositions(orderedPoints.Select(p => p + new Vector3(-100, 100000, -100)).ToArray()); // elevar un poco el punto para mejor visibilidad
                groupIndex++;
            }
        }

        //Debug.Log("Contador: " + contadorEpsilon);
    }

    private void CheckEdge(Vector3 p1, Vector3 p2, float curveHeight, ref int contadorEpsilon)
    {
        float epsilon = 0.01f;
        float y1 = p1.y;
        float y2 = p2.y;

        // Caso 1: la arista cruza el plano
        if ((y1 < curveHeight && y2 > curveHeight) || (y1 > curveHeight && y2 < curveHeight))
        {
            //Debug.Log("Entró al caso 1");
            Vector3 dir = p2 - p1;
            float dy = dir.y;

            if (Mathf.Abs(dy) > 1e-6f) // evitar división por cero
            {
                //Debug.Log("Entró al caso 1.1");
                float yc = curveHeight - p1.y;
                float t = yc / dy; // interpolación [0,1]
                float x = p1.x + dir.x * t;
                float y = curveHeight;
                float z = p1.z + dir.z * t;

                Vector3 localPoint = new Vector3(x, y, z);
                intersectionPoints.Add(localPoint);
                intersectionPointsForGradient.Add(localPoint);

                Vector3 worldPoint = transform.TransformPoint(localPoint);
                s = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                s.AddComponent<PhotonView>();
                s.transform.position = (worldPoint);
                s.GetComponent<SphereCollider>().enabled = false;
                s.transform.localScale = Vector3.one * 0.2f; // Escala de la esfera
                //Debug.Log("Punto de intersección: " + localPoint.ToString());
            }
        }
        // Caso 2: uno de los extremos está en el plano
        else if (Mathf.Abs(p1.y - curveHeight) < epsilon)
        {
            //Debug.Log("Entró al caso 2");
            intersectionPoints.Add(p1);
            intersectionPointsForGradient.Add(p1);

            Vector3 worldP1 = transform.TransformPoint(p1);
            //Debug.Log("Punto de intersección: " + p1.ToString());
            s = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            s.AddComponent<PhotonView>();
            s.transform.position = worldP1;
            s.GetComponent<SphereCollider>().enabled = false;
            s.transform.localScale = Vector3.one * 0.2f; // Escala de la esfera
            contadorEpsilon++;
        }
        else if (Mathf.Abs(p2.y - curveHeight) < epsilon)
        {
            //Debug.Log("Entró al caso 2.1");
            intersectionPoints.Add(p2);
            intersectionPointsForGradient.Add(p2);

            Vector3 worldP2 = transform.TransformPoint(p2);
            //  Debug.Log("Punto de intersección: " + p2.ToString());
            s = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            s.AddComponent<PhotonView>();
            s.transform.position = worldP2;
            s.GetComponent<SphereCollider>().enabled = false;
            s.transform.localScale = Vector3.one * 0.2f; // Escala de la esfera
            contadorEpsilon++;
        }
    }

    //Se agrupan los puntos por proximidad, para evitar que el line renderer dibuje lineas cruzadas.
    private List<List<Vector3>> GroupPointsByProximity(List<Vector3> points, float maxDistance)
    {
    List<List<Vector3>> groups = new List<List<Vector3>>();
    HashSet<int> visited = new HashSet<int>();

    for (int i = 0; i < points.Count; i++)
    {
        if (visited.Contains(i)) continue;

        List<Vector3> group = new List<Vector3>();
        Queue<int> queue = new Queue<int>();
        queue.Enqueue(i);
        visited.Add(i);

        while (queue.Count > 0)
        {
            int current = queue.Dequeue();
            group.Add(points[current]);

            for (int j = 0; j < points.Count; j++)
            {
                if (visited.Contains(j)) continue;

                if (Vector3.Distance(points[current], points[j]) < maxDistance)
                {
                    queue.Enqueue(j);
                    visited.Add(j);
                }
            }
        }

        groups.Add(group);
    }

    return groups;
    }
    //Se ordenan los puntos por proximidad para dibujar el objeto curva de nivel.
    private List<Vector3> OrderPointsByProximity(List<Vector3> points)
    {
        
        if (points.Count <= 2)
            return points;

        List<Vector3> ordered = new List<Vector3>();
        HashSet<int> used = new HashSet<int>();

        // Empezamos desde el primer punto
        Vector3 current = points[0];
        ordered.Add(current);
        used.Add(0);

        // Repetimos hasta usar todos los puntos
        while (ordered.Count < points.Count)
        {
            float minDist = float.MaxValue;
            int nextIndex = -1;

            // Buscar el punto más cercano al actual que no haya sido usado
            for (int i = 0; i < points.Count; i++)
            {
                if (used.Contains(i)) continue;

                float dist = Vector3.Distance(current, points[i]);
                if (dist < minDist)
                {
                    minDist = dist;
                    nextIndex = i;
                }
            }

            // Si encontramos un punto cercano, lo agregamos
            if (nextIndex != -1)
            {
                current = points[nextIndex];
                ordered.Add(current);
                used.Add(nextIndex);
            }
            else
            {
                break; // No hay más puntos conectables
            }
        }

        return ordered;
    }

    public Mesh ScaleMesh(Mesh mesh)
    {
        Vector3[] vertices = mesh.vertices;
        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i] *= scale; // Escalar cada vértice por la variable scale
        }
        mesh.vertices = vertices;
        mesh.RecalculateBounds();
        return mesh;
    }
    //Metodo para dibujar la ascencion por gradiente, con este metodo se obtiene una lista de puntos que representan la linea, y que seben ser unidos por un line renderer.
    private void DrawGradientAscencion(SurfaceEvaluator surface)
    {
        float EPSILON = 0.05f;
        //Se crea una lista de puntos para la linea.
        List<Vector3> points = new List<Vector3>();
        Vector3 puntoinicial = new Vector3(1.5f, 0, 1.5f);
        points.Add(puntoinicial);
        Vector2 actual2D = new Vector2(puntoinicial.x, puntoinicial.z);
        //Se calcula la y del punto inicial en base a la formula de la funcion ingresada por el profesor.
        double y = surface.ComputeZ(actual2D.x, actual2D.y);
        Vector3 actual3D = new Vector3(actual2D.x, (float)y, actual2D.y);
        //Debug.Log("Punto inicial 3D corregido: " + actual3D);
        points.Add(actual3D);
        actual2D = new Vector2(actual3D.x, actual3D.z);

        //Se calcula el gradiente en el punto inicial.
        Vector2 gradiente = GradienteCalculator.CalculateNeutralGradient(actual2D, surface);
        //Se calcula el siguiente X Y Z
        double siguienteX = actual3D.x + gradiente.x * EPSILON;
        double siguienteZ = actual3D.z + gradiente.y * EPSILON;
        double siguienteY = surface.ComputeZ(siguienteX, siguienteZ);
        Vector3 siguiente3D = new Vector3((float)siguienteX, (float)siguienteY, (float)siguienteZ);
        while (siguiente3D.y > actual3D.y)
        {
            //Se añade el siguiente punto a la lista de puntos.
            //Debug.Log("Punto siguiente 3D: " + siguiente3D);
            points.Add(siguiente3D);
            actual3D = siguiente3D;
            //Se calcula el siguiente punto
            actual2D = new Vector2(actual3D.x, actual3D.z);
            gradiente = GradienteCalculator.CalculateNeutralGradient(actual2D, surface);
            siguienteX = actual3D.x + gradiente.x * EPSILON;
            siguienteZ = actual3D.z + gradiente.y * EPSILON;
            siguienteY = surface.ComputeZ(siguienteX, siguienteZ);
            siguiente3D = new Vector3((float)siguienteX, (float)siguienteY, (float)siguienteZ);
            //Revisar si el siguiente punto está fuera de los límites de la superficie
            if (siguiente3D.x < surfaceMinX || siguiente3D.x > surfaceMaxX || siguiente3D.z < surfaceMinZ || siguiente3D.z > surfaceMaxZ)
            {
                //Debug.Log("El siguiente punto está fuera de los límites de la superficie. Terminando la ascensión.");
                break;
            }
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
            Vector3 modifiedPoint = points[i];
            modifiedPoint.y /= 74f; // Ajustar la altura para que coincida con la escala del mesh
            lineRenderer.SetPosition(i, modifiedPoint + new Vector3(-100, 100000, -100)); // elevar un poco el punto para mejor visibilidad
            //Debug.Log("Punto " + i + ": " + points[i]);
        }

    }
    //Metodo para crear un objeto con line renderer para las curvas de nivel.
    public GameObject CreateLevelCurveLine(float LevelCurveNumber)
    {
        //Logica para dibujar un LineRenderer con los puntos de la curva de nivel.
        GameObject lineObject = new GameObject("LevelCurveLine");
        lineObject.name = "LevelCurveLine" + LevelCurveNumber;
        LineRenderer lineRenderer = lineObject.AddComponent<LineRenderer>();
        lineRenderer.startWidth = 0.5f;
        lineRenderer.endWidth = 0.5f;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = Color.white;
        lineRenderer.endColor = Color.white;
        lineRenderer.useWorldSpace = true;
        lineRenderer.shadowCastingMode = ShadowCastingMode.Off;
        lineRenderer.receiveShadows = false;

        return lineObject;
    }

    

}

//-------------------------------------Gradiente Calculator-------------------------------------

public class GradienteCalculator
{

    public static float epsilon = 0.01f;

    //Se calcula el gradiente en un punto dado, usando la formula del gradiente.
    public static Vector2 CalculateNormalizedGradient(Vector2 point, SurfaceEvaluator
     surface)
    {
        float x1 = point.x;
        float z1 = point.y;

        float x2 = x1 + epsilon;
        float z2 = z1 + epsilon;

        double dyx = surface.ComputeZ(x2, z1) - surface.ComputeZ(x1, z1);
        double dyz = surface.ComputeZ(x1, z2) - surface.ComputeZ(x1, z1);

        float dydx = (float)(dyx / epsilon);
        float dydz = (float)(dyz / epsilon);

        Vector2 gradient = new Vector2(dydx, dydz).normalized;
        return gradient;

    }

    //Se calcula el gradiente escalado en un punto dado, usando la formula del gradiente.
    public static Vector2 CalculateScaledGradient(Vector2 point, SurfaceEvaluator
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
        Vector2 ScaledGradient = gradient * 10f; // Escalar el gradiente para mejor visibilidad
        return ScaledGradient;

    }

    public static Vector2 CalculateNeutralGradient(Vector2 point, SurfaceEvaluator
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

        Vector2 gradient = new Vector2(dzdx, dzdy);
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