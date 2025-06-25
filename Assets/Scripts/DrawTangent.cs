using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrawTangent : MonoBehaviour
{
    private int CurrentTangent = 0;
    [SerializeField]
    private LineRenderer lineRenderer;
    private int SphereTangent = 1;
    private bool FinishedTangent = false;

    private List<GameObject> sphereList;
    private List<GameObject> tangentList;
    [SerializeField]
    private DrawLine drawLine; // Referencia a la otra clase

    void Start()
    {
        // Obtiene el LineRenderer del GameObject
        lineRenderer.positionCount = 200;
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;

        // Encuentra la instancia de DrawLine en la escena

        if (drawLine != null)
        {
            sphereList = drawLine.sphereList;
            tangentList = drawLine.ReadTangentList;
        }
        else
        {
            Debug.LogError("DrawLine no encontrado en la escena.");
        }
    }

    public void DrawingTangent2()
    {
        if (drawLine == null)
        {
            Debug.LogError("DrawLine no ha sido inicializado.");
            return;
        }

        if (SphereTangent < sphereList.Count && 
        SphereTangent < tangentList.Count && 
        !FinishedTangent)
        {

        int idx = SphereTangent * 2;

        if (lineRenderer.positionCount < idx + 2)
        {
            lineRenderer.positionCount = idx + 2;
        }

            lineRenderer.SetPosition(idx, sphereList[SphereTangent].transform.position);
            lineRenderer.SetPosition(idx+1, tangentList[SphereTangent].transform.position);
            CurrentTangent++;
            SphereTangent++;

            if (SphereTangent >= 200 || SphereTangent >= tangentList.Count || SphereTangent >= tangentList.Count)
            {
                drawLine.FinishedTangent = true;
            }
        
        }
    }
}
