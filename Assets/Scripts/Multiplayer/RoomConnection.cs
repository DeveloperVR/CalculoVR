using UnityEngine;

public class RoomConnection : MonoBehaviour
{
    void Start()
    {
        ConnectionManager.Instance.JoinRoom(ConnectionManager.Instance.GetRoomName());
    }
}
