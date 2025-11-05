using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureCreator : MonoBehaviour
{
    

    [SerializeField] GameObject treasurePrefab;

    List<Vector3> treasurePositions1 = new List<Vector3>()
    {
        new Vector3(7f, 20f, 9.03f),
        new Vector3(14.9f, 12.9f, 0.67f),
        new Vector3(20f, 17f, 8.12f),
        new Vector3(25f, 9f, 25f),
        new Vector3(2.7f, 8.5f, 18f),
        new Vector3(16f, 27f, 5.18f)
    };
    List<Vector3> treasurePositions2 = new List<Vector3>()
    {
        new Vector3(5f, 0f, 15f),
        new Vector3(20f, 0f, 25f),
        new Vector3(30f, 0f, 10f)
    };

    public GameObject CreateTreasureAtPosition(Vector3 pos)
    {
        pos = new Vector3(pos.x - 100, pos.y + 100000, pos.z - 100); // Adjust y position to be above the surface
        GameObject treasure = Instantiate(treasurePrefab, pos, Quaternion.identity);
        return treasure;
    }

    public Vector3 CalculatePosition(string functionString, float x, float y)
    {
        SurfaceEvaluator evaluator = new SurfaceEvaluator(functionString);
        double z = evaluator.ComputeZ(x, y);
        return new Vector3(x, (float)z, y);
    }

    public List<GameObject> PlaceTreasures(List<Vector3> treasurePositions)
    {
        List<GameObject> createdTreasures = new List<GameObject>();
        foreach (Vector3 pos in treasurePositions)
        {
            createdTreasures.Add(CreateTreasureAtPosition(pos));
        }
        return createdTreasures;
    }

    public List<GameObject> CalculateAndPlaceTreasures(string functionString, List<Vector3> positionsXY)
    {
        List<GameObject> createdTreasures = new List<GameObject>();
        foreach (Vector3 pos in positionsXY)
        {
            Vector3 fullPos = CalculatePosition(functionString, pos.x, pos.z);
            createdTreasures.Add(CreateTreasureAtPosition(fullPos));
        }
        return createdTreasures;
    }
}
