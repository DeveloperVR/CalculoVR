using UnityEngine;


public class RCGameManager : MonoBehaviour
{
    public static RCGameManager Instance { get; private set; }
    public GameObject LocalPlayer { get; private set; }
    public GameObject CartMovementManager { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
    }
    public void SetLocalPlayer(GameObject player)
    {
        LocalPlayer = player;
    }

    public void SetCartMovementManager(GameObject cartManager)
    {
        CartMovementManager = cartManager;
        Debug.Log("CartMovementManager set in RCGameManager.");
    }
}