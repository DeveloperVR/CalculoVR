using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementSpeed : MonoBehaviour
{
    [SerializeField]
    private DrawLine drawLine; // Referencia a la otra clase
    private List<GameObject> sphereList = new List<GameObject>();
    private List<GameObject> SphereListCopy = new List<GameObject>();
    private float initialHeight = 0f;
    private float initialVelocity = 0f;
    public float gravity = 9.81f;

    private float currentS = 0f;
    private float lastS = -1326.66f;

    private float currentVelocity = 0f;
    private float lastVelocity = 0f;

    private float currentHeight = 0f;
    private int currentSphere = 0;
    private int lastSphere = 199; // Último índice de la lista de esferas
    private bool isReady = false;
    private float r = 200f; // Radio de la curva
    private List<Vector3> pathPoints = new List<Vector3>();
    private Vector3 initialPosition = Vector3.zero;

    void Start()
    {
        StartCoroutine(WaitForData());
    }

    void Update()
    {
        if (!isReady) return;
        if (pathPoints.Count < 2) return;
        float deltaTime = Time.deltaTime;

        // Avanzar distancia S
        currentS = lastS + lastVelocity * deltaTime;

        // Obtener posición actual en la curva según S
        Vector3 currentPosition = EvaluatePositionOnPath(currentS);

        // Actualizar altura y velocidad usando conservación de energía
        currentHeight = currentPosition.y;
        currentVelocity = Mathf.Sqrt(2f * gravity * (initialHeight - currentHeight) + initialVelocity * initialVelocity);

        // Mover objeto
        transform.position = currentPosition;

        // Preparar para siguiente frame
        lastS = currentS;
        lastVelocity = currentVelocity;
    }

    IEnumerator WaitForData()
    {
        while (drawLine == null || drawLine.ReadSphereList.Count == 0)
        {
            yield return null;
        }

        SphereListCopy = new List<GameObject>(drawLine.ReadSphereList);
        Debug.Log("El valor de SphereListCopy es: " + SphereListCopy.Count);

        // if (SphereListCopy.Count > 0)
        // {
        //     currentSphere = SphereListCopy.Count - 1;
        //     transform.position = SphereListCopy[lastSphere].transform.position;
        // }

        for (int i = SphereListCopy.Count - 1; i >= 0; i--)
        {
            pathPoints.Add(SphereListCopy[i].transform.position);
        }
        if (pathPoints.Count > 0)
        {
            initialPosition = pathPoints[0];
            transform.position = initialPosition;
            Debug.Log("El punto inicial con la lista invertida es: " + pathPoints[0]); // ← Esta ya está en el orden correcto
        }

        initialHeight = pathPoints[0].y;
        currentHeight = initialHeight;
        currentVelocity = initialVelocity;
        isReady = true;


        
           
    }

    // Interpolación lineal entre puntos del camino para encontrar posición según "S"
    Vector3 EvaluatePositionOnPath(float s)
    {
        float theta = s / r;
        return EvaluateFunctionGivenAngle(theta);
    }

    Vector3 EvaluateFunctionGivenAngle(float theta)
    {
        float r = 200f;
        float x = Mathf.Cos(theta) * r;
        float z = -Mathf.Sin(theta) * r;
        float y = -30f * theta;
        return new Vector3(x, y, z);
    }



    // Calcular la longitud total del camino
    float GetTotalPathLength()
    {
        float length = 0f;
        for (int i = 0; i < pathPoints.Count - 1; i++)
        {
            length += Vector3.Distance(pathPoints[i], pathPoints[i + 1]);
        }
        return length;
    }

}
