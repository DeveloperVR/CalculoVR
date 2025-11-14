using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class TreasureManager : MonoBehaviourPun
{

    //Aqui se manejan los tesoros, tanto la logica simple para la segunda supercicie, que es tenerlos todos activos al mismo tiempo, como la lógica algo mas compleja de la primera supercicie.
    private int totalTreasures = 0;
    private int collectedTreasures = 0;
    [SerializeField] private List<GameObject> Treasures = new List<GameObject>();
    private int treasureIndex = 0;
    private int SurfaceRoutine;
    [SerializeField] private DesktopMiniMap desktopMiniMap;
    [SerializeField] private WristMenu wristMenu;
    // Start is called before the first frame update

    public void SetTotalTreasures(int total)
    {
        totalTreasures = total;
    }


    public void RegisterTreasure(GameObject treasure)
    {
        Treasures.Add(treasure);
    }

    public void resetValues()
    {
        totalTreasures = 0;
        collectedTreasures = 0;
        treasureIndex = 0;
        Treasures.Clear();
    }
    public void SetSurfaceRoutine(int surface)
    {
        SurfaceRoutine = surface;
    }
    
    public void PickUpManagement()
    {
        if (SurfaceRoutine == 1)
        {
            photonView.RPC("Routine1", RpcTarget.AllBuffered);
        }
        else if (SurfaceRoutine == 2)
        {
            photonView.RPC("routine2", RpcTarget.AllBuffered);
        }
    }

    [PunRPC]
    private void Routine1()
    {
        if (treasureIndex+2 >= totalTreasures)
        {
            Debug.Log("All treasures collected on surface 1!");
            // Add any additional logic for when all treasures are collected
            Treasures[0].SetActive(true);
            Treasures[treasureIndex].SetActive(false);
            resetValues();
            SetSurfaceRoutine(0);
            desktopMiniMap.SetMiniMapSprite();
            wristMenu.SetMiniMapSprite();
            return;
        }
        if(treasureIndex+1 != totalTreasures)
        {
            desktopMiniMap.SetClueSprite(treasureIndex+1);
            wristMenu.SetClueSprite(treasureIndex+1);
        }
        
        
        Treasures[treasureIndex].SetActive(false);
        if(treasureIndex + 1 < totalTreasures) Treasures[treasureIndex+1].SetActive(true);
        treasureIndex++;
        collectedTreasures++;
    }

    [PunRPC]
    private void routine2()
    {
        collectedTreasures++;
        if (collectedTreasures >= totalTreasures)
        {
            Debug.Log("All treasures collected on surface 2!");
            // Add any additional logic for when all treasures are collected
            SetSurfaceRoutine(0);
            resetValues();
        }
    }
}