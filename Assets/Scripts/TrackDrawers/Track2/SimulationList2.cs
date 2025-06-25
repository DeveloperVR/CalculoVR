using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SimulationLists2 : MonoBehaviour
{
    [SerializeField]
    private GameObject self;
    [SerializeField]
    private DrawLine2 drawLine; // Referencia a la otra clase
    [SerializeField]
    private List<GameObject> SphereListCopy = new List<GameObject>();
    public List<GameObject> ScaledTangents2 = new List<GameObject>();
    public List<GameObject> ScaledCentripeta2 = new List<GameObject>();
    private float initialHeight = 0f;
    public float initialVelocity2 = 10f;
    public float gravity2 = 9.81f;

    private float currentS = 0f;
    private float lastS = 0f;

    public float currentVelocity2 = 0f;
    private float lastVelocity = 0f;

    private float currentHeight = 0f;
    private Vector3 lastPosition;
    private int currentSphere = 0;
    private int lastSphere = 199; // Último índice de la lista de esferas
    public bool isReady2 = false;
    public bool finished2 = false;
    private float r = 200f; // Radio de la curva
    private List<Vector3> pathPoints = new List<Vector3>();
    private Vector3 initialPosition = Vector3.zero;
    private int rotationSpeed = 3;
    [SerializeField] float boostEnergy = 50f;

    float totalLength = 0f;
    List<float>segmentLengths = new List<float>();
    List<float>cumulativeLengths = new List<float>();
    private int i = 0;

    private Dictionary<GameObject, GameObject> sphereToTangent = new Dictionary<GameObject, GameObject>();


    void Start()
    {
        StartCoroutine(WaitForData());
    }

    void Update()
    {
        if (!isReady2) return;
        if (pathPoints.Count < 2) return;
        float deltaTime = Time.deltaTime;

        // Avanzar distancia S
        currentS = lastS + lastVelocity * deltaTime;


        // Obtener posición actual en la curva según S
        Vector3 currentPosition = EvaluatePositionOnPath2(currentS);

        // Actualizar altura y velocidad usando conservación de energía
        currentHeight = currentPosition.y;

        if (currentVelocity2 < 0.5f && initialHeight < currentHeight)
        {
            initialVelocity2 += boostEnergy;
        }
            // Solo si además estás subiendo (no bajando)
            
        float energy = 2f * gravity2 * (initialHeight - currentHeight) + initialVelocity2 * initialVelocity2;
        energy = Mathf.Max(0f, energy);
        currentVelocity2 = Mathf.Sqrt(energy);

        

        // Mover objeto y rotarlo en la dirección del movimiento
        transform.position = currentPosition;
        Vector3 moveDirection = currentPosition - lastPosition;
        Quaternion lookRotation = Quaternion.LookRotation(moveDirection.normalized);
        Quaternion playerRotation = Quaternion.LookRotation(moveDirection.normalized);
        lookRotation *=Quaternion.Euler(0f, 90f, 0f);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);

        

        // Preparar para siguiente frame
        lastS = currentS;
        lastPosition = currentPosition;
        lastVelocity = currentVelocity2;

        if (currentPosition == pathPoints[pathPoints.Count - 1])
        {
            finished2 = true;
        }

        if (finished2)
        self.gameObject.SetActive(false);

    }


    IEnumerator WaitForData()
    {
        while (drawLine == null || drawLine.sphereList2.Count == 0)
        {
            yield return null;
        }

        SphereListCopy = new List<GameObject>(drawLine.sphereList2);

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

        initialHeight = pathPoints[0].y;
        currentHeight = initialHeight;
        currentVelocity2 = initialVelocity2;


        ComputePathData();

        isReady2 = true;
           
    }

    // Interpolación lineal entre puntos del camino para encontrar posición según "S"



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

    Vector3 EvaluatePositionOnPath2(float s)
    {
        s = Mathf.Clamp(s, 0f, totalLength);

        for (int i = 1; i < cumulativeLengths.Count; i++)
        {
            if (s <= cumulativeLengths[i])
            {
                float segStart = cumulativeLengths[i - 1];
                float segEnd = cumulativeLengths[i];
                float t = (s - segStart) / (segEnd - segStart);
                return Vector3.Lerp(pathPoints[i - 1], pathPoints[i], t);
            }
        }

        return pathPoints[pathPoints.Count - 1];
    }

    void OnTriggerEnter(Collider other)
    {
       
        if (other.TryGetComponent(out puntostrigger2 trigger))
        {
            GameObject Scaledt = trigger.triggerPoint(currentVelocity2);
            GameObject Scaledc = trigger.scaling(currentVelocity2);
            ScaledTangents2.Add(Scaledt);
        }
          
        
    }
}