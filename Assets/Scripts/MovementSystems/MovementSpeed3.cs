using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;

public class MovementSpeed3 : MonoBehaviour
{
    [SerializeField]
    private Cube self;
    [SerializeField]
    private DrawLine3 drawLine; // Referencia a la otra clase
    [SerializeField] private GameObject player;
    [SerializeField] private TangentPool3 tangentPool;
    [SerializeField] private CentripetalPool3 centripetalPool;
    [SerializeField] private NormalPool3 normalPool;
    [SerializeField] private ObsculatorPool3 obsculatorPool;
    [SerializeField] private GameObject menu;
    [SerializeField] private Transform RidingMenuFinalPosition;
    [SerializeField] private Transform ObservingMenuFinalPosition;
    [SerializeField] private XROriginRecentering centrado;
    private AudioManager audioManager;
    [SerializeField] private AudioSource railing;
    private List<GameObject> SphereList3sphereList3Copy = new List<GameObject>();
    private float initialHeight = 0f;
    public float initialVelocity3 = 10f;
    public float gravity3 = 9.81f;

    private float currentS = 0f;
    private float lastS = 0f;

    public float currentVelocity3 = 0f;
    private float lastVelocity = 0f;

    private float currentHeight = 0f;
    private Vector3 lastPosition;
    public bool isReady3 = false;
    private float r = 200f; // Radio de la curva
    private List<Vector3> pathPoints = new List<Vector3>();
    private Vector3 initialPosition = Vector3.zero;
    [SerializeField] private int rotationSpeed = 3;
    [SerializeField] float boostEnergy = 50f;

    float totalLength = 0f;
    List<float> segmentLengths = new List<float>();
    List<float> cumulativeLengths = new List<float>();
    private int i = 0;
    public bool Observing3 = false;
    private bool checkpoint = false;

    private Dictionary<GameObject, GameObject> sphereToTangent = new Dictionary<GameObject, GameObject>();

    void Start()
    {
        StartCoroutine(WaitForData());
        audioManager = FindObjectOfType<AudioManager>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            isReady3 = true;
        }
        if (!isReady3) return;
        if (pathPoints.Count < 2) return;
        float deltaTime = Time.deltaTime;
        PlayAudio();

        // Avanzar distancia S
        currentS = lastS + lastVelocity * deltaTime;


        // Obtener posición actual en la curva según S
        Vector3 currentPosition = EvaluatePositionOnPath2(currentS);

        // Actualizar altura y velocidad usando conservación de energía
        currentHeight = currentPosition.y;

        if (currentVelocity3 < 0.5f)
        {
            initialVelocity3 += boostEnergy;
        }
        // Solo si además estás subiendo (no bajando)

        float energy = 2f * gravity3 * (initialHeight - currentHeight) + initialVelocity3 * initialVelocity3;
        energy = Mathf.Max(0f, energy);
        currentVelocity3 = Mathf.Sqrt(energy);



        // Mover objeto y rotarlo en la dirección del movimiento
        transform.position = currentPosition;
        Vector3 moveDirection = currentPosition - lastPosition;
        Quaternion lookRotation = Quaternion.LookRotation(moveDirection.normalized);
        Quaternion playerRotation = Quaternion.LookRotation(moveDirection.normalized);
        lookRotation *= Quaternion.Euler(0f, 90f, 0f);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime * rotationSpeed);
        if (Observing3 == false)
        {
            player.transform.position = currentPosition;
            player.transform.rotation = Quaternion.Slerp(player.transform.rotation, playerRotation, rotationSpeed * Time.deltaTime * currentVelocity3);
        }

        if (currentPosition == pathPoints[pathPoints.Count - 1])
        { checkpoint = true; }
        if (currentPosition == pathPoints[pathPoints.Count - 1] && checkpoint == true)
        {
            isReady3 = false;
            i = 0;
            initialHeight = pathPoints[0].y;
            currentHeight = initialHeight;
            currentVelocity3 = initialVelocity3;
            currentS = 0f;
            lastS = 0f;
            lastVelocity = 0f;
            lastPosition = pathPoints[0];
            audioManager.StopAudio();
            StopAudio();
            if (Observing3 == true)
            {
                menu.transform.position = ObservingMenuFinalPosition.transform.position;
            }
            else
            {
                menu.gameObject.transform.position = RidingMenuFinalPosition.transform.position;
            }
            Observing3 = false;
            menu.gameObject.SetActive(true);
            checkpoint = false;
            centrado.RecenterToHeadset();
            return;
        }





        // Preparar para siguiente frame
        lastS = currentS;
        lastPosition = currentPosition;
        lastVelocity = currentVelocity3;
        i++;
    }

    IEnumerator WaitForData()
    {
        while (drawLine == null || drawLine.sphereList3.Count == 0)
        {
            yield return null;
        }

        SphereList3sphereList3Copy = new List<GameObject>(drawLine.sphereList3);

        // if (SphereList3sphereList3Copy.Count > 0)
        // {
        //     currentSphere = SphereList3sphereList3Copy.Count - 1;
        //     transform.position = SphereList3sphereList3Copy[lastSphere].transform.position;
        // }

        // for (int i = 0; i >= sphereList3.Count ; i++)
        // {
        //     pathPoints.Add(SphereList3sphereList3Copy[i].transform.position);
        //     if (i == 1999){
        //         break;
        //     }
        // }
        for (int i = 0; i < SphereList3sphereList3Copy.Count && i < 2600; i++)
        {
            pathPoints.Add(SphereList3sphereList3Copy[i].transform.position);
        }

        if (pathPoints.Count > 0)
        {
            initialPosition = pathPoints[0];
            transform.position = initialPosition;
        }

        initialHeight = pathPoints[0].y;
        currentHeight = initialHeight;
        currentVelocity3 = initialVelocity3;

        ComputePathData();

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
        if (other.TryGetComponent(out puntostrigger3 trigger))
        {
            GameObject tangente = trigger.GetTangentPosition();
            tangentPool.addAndUsePool(other.gameObject.transform.position, tangente.transform.position);
            GameObject centripetal = trigger.GetCentripetalPosition();
            centripetalPool.addAndUsePool(other.gameObject.transform.position, centripetal.transform.position);
            GameObject normal = trigger.GetNormalPosition();
            normalPool.addAndUsePool(other.gameObject.transform.position, normal.transform.position);
            trigger.ShowDrawer();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out puntostrigger3 trigger))
        {
            trigger.HideDrawer();
        }
    }

    private void PlayAudio()
    {
        if (railing != null && !railing.isPlaying)
        {
            railing.loop = true;
            railing.volume = 0.3f;
            railing.Play();
        }
    }
    private void StopAudio()
    {
        railing.Stop();
    }

    private void muteSound()
    {
        if (railing != null)
        {
            railing.volume = 0f;
        }
    }

    private void unmuteSound()
    {
        if (railing != null)
        {
            railing.volume = 0.3f;
        }
    }

    public void SetPlayer()
    {
        player = RCGameManager.Instance.LocalPlayer;
        if (player == null)
        {
            Debug.LogError("XR Origin (XR Rig) not found in the scene.");
        }
        else
        {
            Debug.Log("XR Origin (XR Rig) found: " + player.name);
        }
    }

    public void SetOfflinePlayer()
    {
        player = GameObject.Find("XR Player");
        Debug.Log("XR Player found: " + player.name);
    }

    
}