using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.ProBuilder.Shapes;

public class MovementSpeed2 : MonoBehaviourPun
{
    [SerializeField]
    private Cube self;
    [SerializeField]
    private DrawLine2 drawLine; // Referencia a la otra clase
    [SerializeField] private GameObject player;
    [SerializeField] private TangentPool2 tangentPool;
    [SerializeField] private CentripetalPool2 centripetalPool;
    [SerializeField] private NormalPool2 normalPool;
    [SerializeField] private ObsculatorPool2 obsculatorPool;
    [SerializeField] private GameObject menu;
    [SerializeField] private Transform RidingMenuFinalPosition;
    [SerializeField] private Transform ObservingMenuFinalPosition;
    [SerializeField] private XROriginRecentering centrado;
    private AudioManager audioManager;
    [SerializeField] private AudioSource railing;
    private List<GameObject> sphereList2 = new List<GameObject>();
    private List<GameObject> SphereList2sphereList2Copy = new List<GameObject>();
    private float initialHeight = 0f;
    public float initialVelocity2 = 10f;
    public float gravity2 = 9.81f;

    private float currentS = 0f;
    private float lastS = 0f;

    public float currentVelocity2 = 0f;
    private float lastVelocity = 0f;

    private float currentHeight = 0f;
    private Vector3 lastPosition;
    public bool isReady2 = false;
    private float r = 200f; // Radio de la curva
    private List<Vector3> pathPoints = new List<Vector3>();
    private Vector3 initialPosition = Vector3.zero;
    [SerializeField] private int rotationSpeed = 3;
    [SerializeField] float boostEnergy = 50f;

    float totalLength = 0f;
    List<float> segmentLengths = new List<float>();
    List<float> cumulativeLengths = new List<float>();
    private int i = 0;

    private bool isMasterCall2 = false;

    public bool isObserving2 = false;
    private bool checkpoint = false;
    private Vector3 currentPosition;
    private Vector3 moveDirection;
    private Quaternion PlayerCalculatedRotation;

    public bool isFollowing2 { get; private set; }
    public bool isMoving2 { get; private set; } = false;


    private Dictionary<GameObject, GameObject> sphereToTangent = new Dictionary<GameObject, GameObject>();

    void Start()
    {
        StartCoroutine(WaitForData());
        audioManager = FindObjectOfType<AudioManager>();
    }

    void Update()
    {
        if (isMasterCall2)
        {
            currentPosition = EvaluatePositionOnPath2(currentS);
            photonView.RPC("SetCurrentPosition2", RpcTarget.AllBuffered, currentPosition);
            moveDirection = currentPosition - lastPosition;
            PlayerCalculatedRotation = Quaternion.LookRotation(moveDirection.normalized);
            photonView.RPC("SetPlayerCalculatedRotation2", RpcTarget.AllBuffered, PlayerCalculatedRotation);
            photonView.RPC("CartCurrentMovement2", RpcTarget.AllBuffered, currentPosition);
        }
        if (isObserving2 == false && isFollowing2 == true)
        {
            PlayerMovementAndRotation2(PlayerCalculatedRotation, currentVelocity2, currentPosition);
        }
        if (!isReady2) return;
        ////////////////////////////////////////////////////if the movement starts/////////////////////////////////////////////
        if (pathPoints.Count < 2) return;
        float deltaTime = Time.deltaTime;
        PlaySound();

        // Avanzar distancia S
        currentS = lastS + lastVelocity * deltaTime;


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
        Quaternion lookRotation = Quaternion.LookRotation(moveDirection.normalized);
        lookRotation *= Quaternion.Euler(0f, 90f, 0f);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime * currentVelocity2);


        if (currentPosition == pathPoints[pathPoints.Count - 1])
        { checkpoint = true; }
        if (currentPosition == pathPoints[pathPoints.Count - 1] && checkpoint == true)
        {
            isReady2 = false;
            i = 0;
            initialHeight = pathPoints[0].y;
            currentHeight = initialHeight;
            currentVelocity2 = initialVelocity2;
            currentS = 0f;
            lastS = 0f;
            lastVelocity = 0f;
            lastPosition = pathPoints[0];
            audioManager.StopAudio();
            StopSound();

            photonView.RPC("PlaceEndingButton2", RpcTarget.AllBuffered);
            isObserving2 = false;
            menu.gameObject.SetActive(true);
            checkpoint = false;
            centrado.RecenterToHeadset();
            photonView.RPC("SetisMoving2", RpcTarget.AllBuffered, false);
            photonView.RPC("SetisFollowing2", RpcTarget.AllBuffered, false);
            return;
        }



        // Preparar para siguiente frame
        lastS = currentS;
        lastPosition = currentPosition;
        lastVelocity = currentVelocity2;
        i++;
    }

    IEnumerator WaitForData()
    {
        while (drawLine == null || drawLine.sphereList2.Count == 0)
        {
            yield return null;
        }

        SphereList2sphereList2Copy = new List<GameObject>(drawLine.sphereList2);

        // if (SphereList2sphereList2Copy.Count > 0)
        // {
        //     currentSphere = SphereList2sphereList2Copy.Count - 1;
        //     transform.position = SphereList2sphereList2Copy[lastSphere].transform.position;
        // }

        // for (int i = 0; i >= sphereList2.Count ; i++)
        // {
        //     pathPoints.Add(SphereList2sphereList2Copy[i].transform.position);
        //     if (i == 1999){
        //         break;
        //     }
        // }
        for (int i = 0; i < SphereList2sphereList2Copy.Count && i < 2000; i++)
        {
            pathPoints.Add(SphereList2sphereList2Copy[i].transform.position);
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
        if (other.TryGetComponent(out puntostrigger2 trigger))
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
        if (other.TryGetComponent(out puntostrigger2 trigger))
        {
            trigger.HideDrawer();
        }
    }

    private void PlaySound()
    {
        if (railing != null && !railing.isPlaying)
        {
            railing.loop = true;
            railing.volume = 0.3f;
            railing.Play();
        }
    }

    private void StopSound()
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

    [PunRPC]
    public void SetCurrentPosition2(Vector3 position)
    {
        transform.position = position;
        currentPosition = position;
    }

    [PunRPC]
    public void SetPlayerCalculatedRotation2(Quaternion rotation)
    {
        PlayerCalculatedRotation = rotation;
    }

    [PunRPC]
    public void CartCurrentMovement2(Vector3 position)
    {
        transform.position = position;
    }

    public void PlayerMovementAndRotation2(Quaternion playerRotation, float playerCurrentVelocity, Vector3 currentPosition)
    {
        player.transform.position = currentPosition;
        Quaternion PlayerFinalRotation = Quaternion.Slerp(player.transform.rotation, playerRotation, rotationSpeed * Time.deltaTime * playerCurrentVelocity);
        player.transform.rotation = PlayerFinalRotation;
        //Debug.Log("Player position and rotation updated. Position: " + Player.transform.position + " Rotation: " + Player.transform.rotation);
    }

    [PunRPC]
    public void SetIsReady2(bool value)
    {
        isReady2 = value;
        Debug.Log("Is ready set to: " + value);
    }

    [PunRPC]
    public void SetisMoving2(bool value)
    {
        isMoving2 = value;
        Debug.Log("Cart movement state changed: " + isMoving2);
    }

    [PunRPC]
    public void SetisFollowing2(bool value)
    {
        isFollowing2 = value;
        Debug.Log("Is following set to: " + value);
    }

    [PunRPC]
    public void SetisMasterCall2(bool value)
    {
        isMasterCall2 = value;
        Debug.Log("isMasterCall2 set to: " + isMasterCall2);
    }
    
    [PunRPC]
    public void PlaceEndingButton2()
    {
        if (isObserving2 == true)
        {
            menu.transform.position = ObservingMenuFinalPosition.transform.position;
        }
        else
        {

            menu.gameObject.transform.position = RidingMenuFinalPosition.transform.position;
        }
    }
}