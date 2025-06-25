using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawCentripetal1 : MonoBehaviour
{
    private int currentCentripetal = 0;
    [SerializeField]
    private LineRenderer lineRenderer;
    private int sphereCentripetal= 1;
    private bool FinishedCentripetal = false;

    private List<GameObject> sphereList;
    private List<GameObject> centripetalList;
    [SerializeField]
    private DrawLine1 drawLine; // Referencia a la otra clase

    void Start()
    {
        // Obtiene el LineRenderer del GameObject
        lineRenderer.positionCount = 2000;
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;

        // Encuentra la instancia de DrawLine en la escena

        if (drawLine != null)
        {
            sphereList = drawLine.sphereList;
            centripetalList = drawLine.sphereList;
        }
        else
        {
            Debug.LogError("DrawLine no encontrado en la escena.");
        }
    }

    public void DrawingCentripetal()
    {
        if (drawLine == null)
        {
            Debug.LogError("DrawLine no ha sido inicializado.");
            return;
        }

        if (sphereCentripetal < sphereList.Count && 
        sphereCentripetal < centripetalList.Count && 
        !FinishedCentripetal)
        {

        int idx = sphereCentripetal * 2;

        if (lineRenderer.positionCount < idx + 2)
        {
            lineRenderer.positionCount = idx + 2;
        }
            lineRenderer.SetPosition(idx, sphereList[sphereCentripetal].transform.position);
            lineRenderer.SetPosition(idx+ 1, centripetalList[sphereCentripetal].transform.position);
            currentCentripetal++;
            sphereCentripetal++;

            if (sphereCentripetal >= 2000 || sphereCentripetal >= centripetalList.Count || sphereCentripetal >= centripetalList.Count)
            {
                drawLine.FinishedCentripetal = true;
            }
        
        }
    }
}
