using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drawers : MonoBehaviour
{
    [Header("Creators")]
    [SerializeField]
    public CreateRC createRC;
    [SerializeField]
    private CreateTangent createTangent;
    [SerializeField]
    private CreateCentripetal createCentripetal;
    [SerializeField]
    private CreateNormal createNormal;

    private LineRenderer lineRenderer;

    private List<GameObject> RCList = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        List<GameObject> RCList = createRC.ReadSphereList;

        lineRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int DrawingRollerCoaster(int PreviousLocation, int NextLocation, int indicator)
        {
            // // Configuración inicial del LineRenderer
            
            lineRenderer.positionCount = 200;
            lineRenderer.startWidth = 1f;
            lineRenderer.endWidth = 1f;
            if (PreviousLocation < 200 && RCList.Count >= 2 && NextLocation < RCList.Count)
            {
                lineRenderer.SetPosition(PreviousLocation, RCList[PreviousLocation].transform.position);
                lineRenderer.SetPosition(NextLocation, RCList[NextLocation].transform.position);
                Debug.Log("El valor de PreviousLocation es: " + PreviousLocation);
                Debug.Log("El valor de NextLocation es: " + NextLocation);
                
            }
            

            if (NextLocation >= 200)
                {
                    indicator = 1;
                    Debug.Log("El valor de indicator es: " + indicator);
                    Debug.Log("Linea terminada");
                }
            else
                {
                    Debug.Log("Linea en proceso");
                }
            Debug.Log("El valor de PreviousSphere es: " + PreviousLocation);
            Debug.Log("El valor de NextSphere es: " + NextLocation);

            return indicator;
        }
}
