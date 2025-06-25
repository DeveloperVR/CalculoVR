using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateNewLine : MonoBehaviour
{

    private LineRenderer lineRenderer;
    [SerializeField] private GameObject spherelist;
    private List<GameObject> points = new List<GameObject>();
    int contador = 0;
    // Start is called before the first frame update
    void Start()
    {
        points = spherelist.GetComponent<DrawLine1>().sphereList;
    }

    // Update is called once per frame
    void Update()
    {
         if (points.Count < 2)
        {
            return;
        }
        lineRenderer.SetPosition(contador, points[contador].transform.position);
        if (contador >= points.Count - 1)
        contador++;
    }
}
