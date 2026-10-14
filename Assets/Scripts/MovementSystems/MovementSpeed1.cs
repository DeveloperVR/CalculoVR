using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;

public class MovementSpeed1 : MonoBehaviourPun
{
    [SerializeField]
    private Cube self;
    [SerializeField]
    private DrawLine1 drawLine; // Referencia a la otra clase
    [SerializeField]
    private DrawTangent1 drawTangent;
    [SerializeField] private GameObject Player;
    [SerializeField] private TangentPool1 tangentPool;
    [SerializeField] private CentripetalPool1 centripetalPool;
    [SerializeField] private NormalPool1 normalPool;
    [SerializeField] private ObsculatorPool1 obsculatorPool;
    [SerializeField] private GameObject menu;
    [SerializeField] private Transform RidingPlaceFinalPosition;
    [SerializeField] private Transform ObservingMenuFinalPosition;
    [SerializeField] private XROriginRecentering centrado;
    private GameObject CartMovementManager;
    private AudioManager audioManager;
    [SerializeField] private AudioSource railing;

    [SerializeField] private GameObject VectorsModel;

    private List<GameObject> sphereList = new List<GameObject>();
    private List<GameObject> SphereListCopy = new List<GameObject>();
    private float initialHeight = 0f;
    public float initialVelocity = 10f;
    public float gravity = 9.81f;

    private float currentS = 0f;
    private Vector3 currentPosition;
    private float lastS = 0f;

    public float currentVelocity = 0f;
    private float lastVelocity = 0f;

    private float currentHeight = 0f;
    private Vector3 lastPosition;
    private int currentSphere = 0;
    private Vector3 moveDirection;
    private int lastSphere = 199; // Último índice de la lista de esferas
    public bool isReady = false;
    private float r = 200f; // Radio de la curva
    private List<Vector3> pathPoints = new List<Vector3>();
    private Vector3 initialPosition = Vector3.zero;
    public static int rotationSpeed = 3;
    [SerializeField] float boostEnergy = 50f;

    float totalLength = 0f;
    List<float> segmentLengths = new List<float>();
    List<float> cumulativeLengths = new List<float>();
    private int i = 0;
    public bool isObserving = false;
    private bool checkpoint = false;

    private Dictionary<GameObject, GameObject> sphereToTangent = new Dictionary<GameObject, GameObject>();

    private Quaternion PlayerCalculatedRotation;
    public bool isFollowing { get; private set; }
    private GameObject PlayerAfterCartPosition;

    void Awake()
    {
        MuteAudio();
    }
    void Start()
    {
        StartCoroutine(WaitForData());
        audioManager = FindObjectOfType<AudioManager>();
        PlayerAfterCartPosition = GameObject.Find("AfterCart1Place");
    }

    void Update()
    {
        int ping = PhotonNetwork.GetPing();
        Debug.Log("Latencia (Ping): " + ping + " ms");




        if (!isReady) return;
        ///////////////////////////////////////////////if the movement starts/////////////////////////////////////////////
        if (pathPoints.Count < 2) return;
        float deltaTime = Time.deltaTime;
        PlayingAudio();

        currentPosition = EvaluatePositionOnPath2(currentS);
        SetCurrentPosition(currentPosition);
        moveDirection = currentPosition - lastPosition;
        PlayerCalculatedRotation = Quaternion.LookRotation(moveDirection.normalized);
        SetPlayerCalculatedRotation(PlayerCalculatedRotation);
        CartCurrentMovement(currentPosition);

        if (isObserving == false && isFollowing == true)
        {
            PlayerMovementAndRotation(PlayerCalculatedRotation, currentVelocity, currentPosition);

            Vector3 targetPosition = currentPosition + Player.transform.forward * 2 + Vector3.up * 0.5f;
            VectorsModel.transform.position = targetPosition;
            VectorsModel.transform.rotation = PlayerCalculatedRotation * Quaternion.Euler(0f, 180f, 0f);
        }

        // Avanzar distancia S
        currentS = lastS + lastVelocity * deltaTime;

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



        // Mover objeto y rotarlo en la dirección del movimiento

        Quaternion lookRotation = Quaternion.LookRotation(moveDirection.normalized);
        lookRotation *= Quaternion.Euler(0f, 90f, 0f);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime * currentVelocity);


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
            audioManager.StopAudio();
            StopAudio();

            //Determines where to put the button at the end of the ride
            PlacePlayerAtEnding();
            PlaceEndingButton();
            isObserving = false;
            checkpoint = false;
            centrado.RecenterToHeadset();
            Debug.Log("isMoving set to false");
            SetisFollowing(false);
            Debug.Log("isFollowing set to false");
            VectorsModel.transform.position = new Vector3(0, -20, 0);
            //photonView.RPC("TakeOffPlayer", RpcTarget.AllBuffered);


            return;
        }
        // Preparar para siguiente frame
        lastS = currentS;
        lastPosition = currentPosition;
        lastVelocity = currentVelocity;
        i++;
    }

    public void PlayerMovementAndRotation(Quaternion playerRotation, float playerCurrentVelocity, Vector3 currentPosition)
    {
        Player.transform.position = currentPosition;
        Debug.Log("Player position updated to: " + Player.transform.position);
        Quaternion PlayerFinalRotation = Quaternion.Slerp(Player.transform.rotation, playerRotation, rotationSpeed * Time.deltaTime * playerCurrentVelocity);
        Player.transform.rotation = PlayerFinalRotation;
        //Debug.Log("Player position and rotation updated. Position: " + Player.transform.position + " Rotation: " + Player.transform.rotation);
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

        initialHeight = pathPoints[0].y;
        currentHeight = initialHeight;
        currentVelocity = initialVelocity;

        ComputePathData();

    }

    [PunRPC]
    public void CartCurrentMovement(Vector3 position)
    {
        transform.position = position;
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
        if (other.TryGetComponent(out puntostrigger trigger))
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
        if (other.TryGetComponent(out puntostrigger trigger))
        {
            trigger.HideDrawer();
        }
    }

    void PlayingAudio()
    {
        if (railing != null && !railing.isPlaying)
        {
            railing.loop = true; // Asegúrate de que el audio se repita
            railing.volume = 0.3f; // Ajusta el volumen según sea necesario
            railing.Play();


        }
    }
    void StopAudio()
    {
        railing.Stop();
    }

    public void MuteAudio()
    {
        if (railing != null)
        {
            railing.volume = 0f;
        }
    }

    public void UnmuteAudio()
    {
        if (railing != null)
        {
            railing.volume = 0.3f;
        }
    }
    [PunRPC]
    public void SetCurrentPosition(Vector3 position)
    {
        transform.position = position;
        currentPosition = position;
    }

    public float getCurrentVelocity() => currentVelocity;

    [PunRPC]
    public void SetPlayerCalculatedRotation(Quaternion rotation)
    {
        PlayerCalculatedRotation = rotation;
    }
    public float getRotationSpeed() => rotationSpeed;

    public void SetPlayer()
    {
        Player = RCGameManager.Instance.LocalPlayer;
        if (Player == null)
        {
            Debug.LogError("XR Origin (XR Rig) not found in the scene.");
        }
        else
        {
            Debug.Log("MovementSpeed 1 XR Origin (XR Rig) found: " + Player.name);
            UnmuteAudio();
        }
    }

    public void SetOfflinePlayer()
    {
        Player = GameObject.Find("XR Player");
        Debug.Log("XR Player found: " + Player.name);
    }

    public void SetCartMovementManager()
    {
        CartMovementManager = RCGameManager.Instance.CartMovementManager;
        if (CartMovementManager == null)
        {
            Debug.LogError("CartMovementManager not found in the scene.");
        }
        else
        {
            Debug.Log("CartMovementManager set successfully: " + CartMovementManager.name);
        }
    }

    public void SetIsReady(bool value)
    {
        isReady = value;
        Debug.Log("Is ready set to: " + value);
    }

    public void SetisFollowing(bool value)
    {
        isFollowing = value;
        Debug.Log("Is following set to: " + value);
    }

    public void TakeOffPlayer()
    {
        Player.transform.position = PlayerAfterCartPosition.transform.position;
        Debug.Log("Player taken off the cart and moved to PlayerAfterCartPlace position.");

    }

    public void PlaceEndingButton()
    {
        if (isObserving == true || isFollowing == true)
        {
            menu.gameObject.SetActive(true);
            menu.transform.position = Player.transform.position + Player.transform.forward * 2 + Vector3.up * 0.5f;
        }
    }

    public void PlacePlayerAtEnding()
    {
        if (isObserving == true)
        {
            return;
        }
        else if (isFollowing == true)
        { 
            Player.transform.position =  RidingPlaceFinalPosition.position;
            Player.transform.rotation = RidingPlaceFinalPosition.rotation;
        }
    }
}
