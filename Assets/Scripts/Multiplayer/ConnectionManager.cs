using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class ConnectionManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private List<string> roomNames;
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Master Server");
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
    
    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        roomNames.Remove(PhotonNetwork.CurrentRoom.Name);
        Debug.Log("Left the room");
    }
}
