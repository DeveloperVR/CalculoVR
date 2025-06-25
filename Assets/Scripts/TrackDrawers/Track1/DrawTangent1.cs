using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.UI;

public class DrawTangent1 : MonoBehaviour
{
    private int CurrentTangent = 0;
    [SerializeField]
    private LineRenderer lineRenderer;
    private int SphereTangent = 1;
    private bool FinishedTangent = false;

    private List<GameObject> sphereList;
    private List<GameObject> tangentList;
    private List<GameObject> TangentDrawersList = new List<GameObject>();
    [SerializeField]
    private DrawLine1 drawLine; // Referencia a la otra clase
    [SerializeField]
    private GameObject prefabTangent;
    private int DrawerIndex = 0;

    void Start()
    {
        // for (int i = 0; i < 200; i++)
        // {
        //     GameObject newTangent = Instantiate(prefabTangent);
        //     newTangent.name = "clone" + i;

        //     TangentDrawersList.Add(newTangent);
        // }
        // Obtiene el LineRenderer del GameObject
        lineRenderer.positionCount = 2000;
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;

        // Encuentra la instancia de DrawLine en la escena

        if (drawLine != null)
        {
            sphereList = drawLine.sphereList;
            tangentList = drawLine.tangentList;
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

            if (SphereTangent >= 2000 || SphereTangent >= tangentList.Count || SphereTangent >= tangentList.Count)
            {
                drawLine.FinishedTangent = true;
            }
        
        }
    }

    public void DrawersPool(Vector3 start, Vector3 end)
    {
        GameObject drawer = TangentDrawersList[DrawerIndex];

        drawer.SetActive(true);

        LineRenderer lr = drawer.GetComponent<LineRenderer>();

        if (lr != null)
        {
            lr.positionCount = 2;
            lr.SetPosition(0, start);
            lr.SetPosition(1, end);
        }

        DrawerIndex++;
        if (DrawerIndex >= 200)
            DrawerIndex = 0;
        }   
    
}
