using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ConnectionManager : MonoBehaviourPunCallbacks
{
<<<<<<< Updated upstream
    [SerializeField]  private 
=======
    [SerializeField] private List<string> roomNames;
    [SerializeField] private LoadingScene LoadingScreen; // Assuming you have a LoadingScene script to handle scene loading
    public static ConnectionManager Instance;
    private GameObject DebugAudio;
    private string roomName;

    void Awake()
    {
        if (Instance == null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
    }
<<<<<<< Updated upstream
<<<<<<< Updated upstream
>>>>>>> Stashed changes
=======
>>>>>>> Stashed changes
=======
>>>>>>> Stashed changes
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    { 
        Debug.Log("Connected to Master Server");
        PhotonNetwork.JoinLobby();
    }
<<<<<<< Updated upstream
=======

    public void JoinRoom(string roomName)
    {
        var (roomOptions, typedLobby) = SetupRoomSetting();
        PhotonNetwork.JoinOrCreateRoom(roomName, roomOptions, typedLobby);
    }

    private (RoomOptions, TypedLobby) SetupRoomSetting()
    {
        RoomOptions roomOptions = new()
        {
            MaxPlayers = 6,
            IsVisible = true,
            IsOpen = true
        };

        TypedLobby typedLobby = TypedLobby.Default;
        return (roomOptions, typedLobby);
    }

    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        roomNames.Add(PhotonNetwork.CurrentRoom.Name);
        Debug.Log($"Room created: {PhotonNetwork.CurrentRoom.Name}");
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        Debug.Log("Joined lobby");
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        roomNames.Add(PhotonNetwork.CurrentRoom.Name);
        Debug.Log($"Joined room: {PhotonNetwork.CurrentRoom.Name}");
        DebugAudio.GetComponent<DebugAudio>().SuccesAudio(); // Play success audio
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        roomNames.Remove(PhotonNetwork.CurrentRoom.Name);
        Debug.Log("Left the room");
    }

    public string GetRoomName() => roomName;

    public void SetRoomName(string name)
    {
        roomName = name;
    }
<<<<<<< Updated upstream
<<<<<<< Updated upstream
>>>>>>> Stashed changes
=======
>>>>>>> Stashed changes
=======
>>>>>>> Stashed changes
}
