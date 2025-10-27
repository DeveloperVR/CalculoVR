using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureCreator : MonoBehaviour
{
    Vector3 position;

    [SerializeField] GameObject treasurePrefab;
    
    public void CreateTreasureAtPosition(Vector3 pos)
    {
        position = pos;
        Instantiate(treasurePrefab, position, Quaternion.identity);
    }
}
