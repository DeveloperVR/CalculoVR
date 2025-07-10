using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class ConnectionManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private List<string> roomNames;
    [SerializeField] private LoadingScene LoadingScreen; // Assuming you have a LoadingScene script to handle scene loading
    private GameObject DebugAudio;
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
        DebugAudio = GameObject.Find("DebugAudio");
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Master Server");
        DebugAudio.GetComponent<DebugAudio>().SuccesAudio();
        PhotonNetwork.JoinLobby();
    }

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

        TypedLobby typedLobby = new("Custom Typed Lobby", LobbyType.Default);
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
        LoadingScreen.LoadScene(PhotonNetwork.CurrentRoom.Name); // Assuming you have a LoadingScreen script to handle scene loading
    }
    
    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        roomNames.Remove(PhotonNetwork.CurrentRoom.Name);
        Debug.Log("Left the room");
    }
}
