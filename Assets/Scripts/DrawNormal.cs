using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawNormal : MonoBehaviour
{
    private int currentNormal = 0;
    [SerializeField]
    private LineRenderer lineRenderer;
    private int sphereNormal = 1;
    private bool finishedNormal = false;

    private List<GameObject> sphereList;
    private List<GameObject> normalList;
    [SerializeField]
    private DrawLine drawLine; // Referencia a la otra clase


    // Start is called before the first frame update
    void Start()
    {
        lineRenderer.positionCount = 200;
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;

        if (drawLine != null)
        {
            sphereList = drawLine.sphereList;
            normalList = drawLine.ReadNormalList;
        }
        else
        {
            Debug.LogError("DrawLine no encontrado en la escena.");
        }
    }
    
    // Update is called once per frame
    public void DrawingNormal()
    {
        if (drawLine == null)
        {
            Debug.LogError("DrawLine no ha sido inicializado.");
            return;
        }

        if (sphereNormal < sphereList.Count && 
        sphereNormal < normalList.Count && 
        !finishedNormal)
        {

        int idx = sphereNormal * 2;

        if (lineRenderer.positionCount < idx + 2)
        {
            lineRenderer.positionCount = idx + 2;
        }

            lineRenderer.SetPosition(idx, sphereList[sphereNormal].transform.position);
            lineRenderer.SetPosition(idx+ 1, normalList[sphereNormal].transform.position);
            currentNormal++;
            sphereNormal++;

            if (sphereNormal >= sphereList.Count || sphereNormal >= normalList.Count)
            {
                drawLine.FinishedNormal = true;
                finishedNormal = true;
            }
        }
    }
}
