using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;

public class MovementVersion2 : MonoBehaviour
{
    [SerializeField]
    private Cube self;
    [SerializeField]
    private DrawLine1 drawLine; // Referencia a la otra clase
    private LineRenderer lr;
    [SerializeField]
    private DrawTangent1 drawTangent;
    [SerializeField] private GameObject player;
    [SerializeField] private TangentPool1 tangentPool;
    [SerializeField] private CentripetalPool1 centripetalPool;
    [SerializeField] private NormalPool1 normalPool;
    [SerializeField] private ObsculatorPool1 obsculatorPool;
    [SerializeField] private GameObject menu;
    [SerializeField] private Transform RidingMenuFinalPosition;
    [SerializeField] private Transform ObservingMenuFinalPosition;
    [SerializeField] private XROriginRecentering centrado;
    [SerializeField] private GameObject prefabTangent;
    private GameObject Tdrawer;
    [SerializeField] private GameObject prefabCentripetal;
    private GameObject Cdrawer;
    [SerializeField] private GameObject prefabNormal;
    private GameObject Ndrawer;
    private List<GameObject> SphereListCopy = new List<GameObject>();
    private float initialHeight = 0f;
    [SerializeField] private float initialVelocity = 10f;
    [SerializeField] private float gravity = 9.81f;

    private float currentS = 0f;
    private float lastS = 0f;
    private float nextS = 0f;

    public float currentVelocity = 0f;
    private float lastVelocity = 0f;

    private float currentHeight = 0f;
    private Vector3 lastPosition;
    private int currentSphere = 0;
    private int lastSphere = 199; // Último índice de la lista de esferas
    private bool isReady = false;
    private float r = 200f; // Radio de la curva
    private List<Vector3> pathPoints = new List<Vector3>();
    private Vector3 initialPosition = Vector3.zero;
    [SerializeField] private int rotationSpeed = 1;
    [SerializeField] float boostEnergy = 50f;

    float totalLength = 0f;
    List<float> segmentLengths = new List<float>();
    List<float> cumulativeLengths = new List<float>();
    private int i = 0;
    private bool Observing = false;
    private bool checkpoint = false;

    private Dictionary<GameObject, GameObject> sphereToTangent = new Dictionary<GameObject, GameObject>();

    void Start()
    {
        StartCoroutine(WaitForData());
        Tdrawer = Instantiate(prefabTangent);
        Cdrawer = Instantiate(prefabCentripetal);
        Ndrawer = Instantiate(prefabNormal);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            isReady = true;
            
        }
        if (!isReady)
        { 
            Debug.Log("IsReady no activado ");
            return;
        }
            
        if (pathPoints.Count < 2) return;
        float deltaTime = Time.deltaTime;

        // Avanzar distancia S
        currentS = lastS + lastVelocity * deltaTime;
        Debug.Log("Current S: " + currentS);


        // Obtener posición actual en la curva según S
        Vector3 currentPosition = EvaluateFunction(currentS);
        Debug.Log("Current Position: " + currentPosition);
        Vector3 nextPosition = EvaluateFunction(currentS + 0.005f);
        Debug.Log("Next Position: " + nextPosition);

        // Actualizar altura y velocidad usando conservación de energía
        currentHeight = currentPosition.y;

        if (currentVelocity < 0.5f && initialHeight < currentHeight)
        {
            initialVelocity += boostEnergy;
        }
        // Solo si además estás subiendo (no bajando)

        float energy = 2f * gravity * (initialHeight - currentHeight) + initialVelocity * initialVelocity;
        energy = Mathf.Max(0f, energy);
        currentVelocity = Mathf.Sqrt(energy);

        nextS = currentS + currentVelocity * deltaTime;





        // Mover objeto y rotarlo en la dirección del movimiento
        transform.position = currentPosition;
        Vector3 moveDirection = currentPosition - lastPosition;
        Quaternion lookRotation = Quaternion.LookRotation(moveDirection.normalized);
        Quaternion playerRotation = Quaternion.LookRotation(moveDirection.normalized);
        lookRotation *= Quaternion.Euler(0f, 90f, 0f);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime * currentVelocity);
        if (Observing == false)
        {
            player.transform.position = currentPosition;
            player.transform.rotation = Quaternion.Slerp(player.transform.rotation, playerRotation, rotationSpeed * Time.deltaTime * currentVelocity);
        }


        if (currentPosition == pathPoints[pathPoints.Count - 1])
        { checkpoint = true; }
        if (currentPosition == pathPoints[pathPoints.Count - 1] && checkpoint == true)
        {
            isReady = false;
            i = 0;
            initialHeight = pathPoints[0].y;
            currentHeight = initialHeight;
            currentVelocity = initialVelocity;
            currentS = 0f;
            lastS = 0f;
            lastVelocity = 0f;
            lastPosition = pathPoints[0];
            if (Observing == true)
            {
                menu.transform.position = ObservingMenuFinalPosition.transform.position;
            }
            else
            {
                menu.gameObject.transform.position = RidingMenuFinalPosition.transform.position;
            }
            Observing = false;
            menu.gameObject.SetActive(true);
            checkpoint = false;
            centrado.RecenterToHeadset();
            return;
        }
        // Preparar para siguiente frame
        lastS = currentS;
        lastPosition = currentPosition;
        lastVelocity = currentVelocity;
        i++;

        calculateT(currentPosition, nextPosition, currentVelocity);
    }

    IEnumerator WaitForData()
    {
        while (drawLine == null || drawLine.sphereList.Count == 0)
        {
            yield return null;
        }

        SphereListCopy = new List<GameObject>(drawLine.sphereList);

        // if (SphereListCopy.Count > 0)
        // {
        //     currentSphere = SphereListCopy.Count - 1;
        //     transform.position = SphereListCopy[lastSphere].transform.position;
        // }

        // for (int i = 0; i >= sphereList.Count ; i++)
        // {
        //     pathPoints.Add(SphereListCopy[i].transform.position);
        //     if (i == 1999){
        //         break;
        //     }
        // }
        for (int i = 0; i < SphereListCopy.Count && i < 2000; i++)
        {
            pathPoints.Add(SphereListCopy[i].transform.position);
        }

        if (pathPoints.Count > 0)
        {
            initialPosition = pathPoints[0];
            transform.position = initialPosition;
        }

        Vector3 currentPosition = EvaluateFunction(0);
        initialHeight = currentPosition.y;
        currentHeight = initialHeight;
        currentS = 0f;
        lastS = 0f;
        lastVelocity = initialVelocity;

        ComputePathData();

    }


    void ComputePathData()
    {
        cumulativeLengths.Add(0f);
        for (int i = 1; i < pathPoints.Count; i++)
        {
            float len = Vector3.Distance(pathPoints[i - 1], pathPoints[i]);
            segmentLengths.Add(len);
            totalLength += len;
            cumulativeLengths.Add(totalLength);
        }

        initialHeight = pathPoints[0].y;
    }


    private Vector3 EvaluateFunction(double t)
    {
        if (t >= 0 && t <= 1)
        {
            double x = 5 * t;
            double z = 0;
            double y = 0;

            return new Vector3((float)x, (float)y, (float)z);
        }
        else if (t >= 1 && t <= 2)
        {
            double x = 5 + 6 * (t - 1) + 12 * Math.Pow(t - 1, 2) - 8 * Math.Pow(t - 1, 3);
            double z = 0;
            double y = 10 * Math.PI * (3 * Math.Pow(t - 1, 2) - 2 * Math.Pow(t - 1, 3));

            return new Vector3((float)x, (float)y, (float)z);
        }
        else if (t >= 2 && t <= 3)
        {
            double x = 15 + 5 * Math.Cos(Math.PI / 2 * (t + 1));
            double z = 5 + 5 * Math.Sin(Math.PI / 2 * (t + 1));
            double y = 10 * Math.PI;

            return new Vector3((float)x, (float)y, (float)z);
        }
        else if (t >= 3 && t <= 4)
        {
            double x = 20;
            double z = 5 + 10 * (t - 3);
            double y = 10 * Math.PI;

            return new Vector3((float)x, (float)y, (float)z);
        }
        else if (t >= 4 && t <= 5)
        {
            double x = 15 + 5 * Math.Cos(Math.PI / 2 * (t - 4));
            double z = 15 + 5 * Math.Sin(Math.PI / 2 * (t - 4));
            double y = 10 * Math.PI;

            return new Vector3((float)x, (float)y, (float)z);
        }
        else if (t >= 5 && t <= 6)
        {
            double x = 90 - 15 * t;
            double z = 20;
            double y = 10 * Math.PI;

            return new Vector3((float)x, (float)y, (float)z);
        }
        else if (t >= 6 && t <= 7)
        {
            double x = 20 * Math.Sin(10 * Math.PI * (7 - t));
            double z = 20 * Math.Cos(10 * Math.PI * (7 - t));
            double y = 10 * Math.PI * (7 - t);

            return new Vector3((float)x, (float)y, (float)z);
        }
        else if (t >= 7 && t <= 8)
        {
            double x = -10 * (t - 7);
            double z = 20;
            double y = 0;

            return new Vector3((float)x, (float)y, (float)z);
        }
        else if (t >= 8 && t <= 9)
        {
            double x = -10 + 10 * Math.Cos(Math.PI * (t - 7.5));
            double z = 10 + 10 * Math.Sin(Math.PI * (t - 7.5));
            double y = 0;

            return new Vector3((float)x, (float)y, (float)z);
        }
        else if (t >= 9 && t <= 10)
        {
            double x = -10 + 10 * (t - 9);
            double z = 0;
            double y = 0;

            return new Vector3((float)x, (float)y, (float)z);
        }
        else
        {
            return new Vector3(0, 0, 0); // Valor por defecto si t no está en el rango esperado
        }

    }
    public void IsReadySetter(bool value)
    {
        isReady = value;
        Debug.Log("IsReady: " + isReady);
    }

    public void ObservingSetter(bool value)
    {
        Observing = value;
    }

    void calculateT(Vector3 currentPoint, Vector3 NextPoint, float velocity)
    {

        Vector3 tangent = (NextPoint - NextPoint).normalized;
        Vector3 start = currentPoint + Vector3.up * 0.2f;

        Tdrawer.transform.localScale = new Vector3(32f, velocity / 2f, 32f);
        Tdrawer.transform.position = start + tangent * (velocity / 2f);
        Tdrawer.transform.rotation = Quaternion.LookRotation(tangent);
        Tdrawer.transform.Rotate(90f, 0f, 0f);
        

    }
}