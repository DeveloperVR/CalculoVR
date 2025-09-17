using Photon.Pun;
using UnityEngine;

public class ChangeOfSkins : MonoBehaviourPun
{
    private GameObject Player;
    [SerializeField] private Material[] materials;
    [SerializeField] SkinsCounter skinsCounter;

    private void PaintPlayer(int numberOfPlayers)
    {
        GameObject PlayerModel = Player.transform.Find("Player").gameObject;

        if (PlayerModel == null)
        {
            Debug.LogError("Player model not found as a child of the XR Origin.");
            return;
        }

        switch (numberOfPlayers)
        {
            case 1:
                PlayerModel.GetComponent<MeshRenderer>().material = materials[0];
                Debug.Log("Material asignado: " + materials[0].name);
                break;
            case 2:
                PlayerModel.GetComponent<MeshRenderer>().material = materials[1];
                Debug.Log("Material asignado: " + materials[1].name);
                break;
            case 3:
                PlayerModel.GetComponent<MeshRenderer>().material = materials[2];
                Debug.Log("Material asignado: " + materials[2].name);
                break;
            case 4:
                PlayerModel.GetComponent<MeshRenderer>().material = materials[3];
                Debug.Log("Material asignado: " + materials[3].name);
                break;
            case 5:
                PlayerModel.GetComponent<MeshRenderer>().material = materials[4];
                Debug.Log("Material asignado: " + materials[4].name);
                break;
            default:
                Debug.LogWarning("Número de jugadores no soportado: " + numberOfPlayers);
                break;
        }
    }
    
    [PunRPC]
    private void RPC_PaintPlayer(int viewID, int skinIndex)
    {
        PhotonView targetView = PhotonView.Find(viewID);
        if (targetView != null)
        {
            GameObject targetPlayer = targetView.gameObject;
            GameObject PlayerModel = targetPlayer.transform.Find("Player").gameObject;
            if (PlayerModel != null)
            {
                PlayerModel.GetComponent<MeshRenderer>().material = materials[skinIndex];
                Debug.Log($"Painted player {viewID} with {materials[skinIndex].name}");
            }
        }
    }

    private void CheckAndAddPlayer()
    {
        int numberOfPlayers = skinsCounter.GetNumberOfPlayersConnected();
        numberOfPlayers++;
        photonView.RPC("SyncSkinsInfo", RpcTarget.AllBuffered, numberOfPlayers);
        int skinIndex = Mathf.Clamp(numberOfPlayers - 1, 0, materials.Length - 1);
        photonView.RPC("RPC_PaintPlayer", RpcTarget.AllBuffered, Player.GetPhotonView().ViewID, skinIndex);
        Debug.Log("Número de jugadores conectados actualizado: " + numberOfPlayers);

    }

    [PunRPC]
    private void SyncSkinsInfo(int numberOfPlayers)
    {
        skinsCounter.SetNumberOfPlayersConnected(numberOfPlayers);
    }
    
    public void SetPlayer()
    {
        Player = RCGameManager.Instance.LocalPlayer;
        if (Player == null)
        {
            Debug.LogError("XR Origin (XR Rig) not found in the scene.");
        }
        else
        {
            Debug.Log("XR Origin (XR Rig) found: " + Player.name);
            CheckAndAddPlayer();
        }
    }
}
