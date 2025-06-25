using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

public class DrawLine : MonoBehaviour
{
    [Header("Puntos de la línea")]
    [SerializeField]
    private Transform pointA; // Primer punto
    [SerializeField]
    private Transform pointB; // Segundo punto
    private LineRenderer lineRenderer;
    public int PreviousSphere = 0;
    public int NextSphere = 1;
    private int CurrentTangent = 0;
    private int SphereTangent = 1;
    public List<GameObject> sphereList = new List<GameObject>();
    private bool isUpdating = true;
    public bool FinishedLine = false;
    public bool FinishedTangent = false;
    public bool FinishedCentripetal = false;
    public bool FinishedNormal = false;
    [Header("Ortogonal")]
    [SerializeField]
    private DrawTangent ReferenceDrawTangent;
    [SerializeField]
    private DrawCentripetal ReferenceDrawCentripetal;
    [SerializeField]
    private DrawNormal ReferenceDrawNormal;

    
    
    public List<GameObject> ReadSphereList => sphereList;

    private List<GameObject> tangentList = new List<GameObject>();
    public List<GameObject> ReadTangentList => tangentList;
    private List <GameObject> centripetalList = new List<GameObject>();
    public List<GameObject> ReadCentripetalList => centripetalList;

    private List<GameObject> normalList = new List<GameObject>();
    public List<GameObject> ReadNormalList => normalList;

    void Start()
    {

        lineRenderer = GetComponent<LineRenderer>();  

        CreatePoint(0);
        //lineRenderer = GetComponent<LineRenderer>();

        createTangent();

        createCentripetal();

        createNormal();

        // lineRenderer.positionCount = 200;
        // lineRenderer.startWidth = 1f;
        // lineRenderer.endWidth = 1f;

    }

    void Update()
    {
        // if (isUpdating == false)
        // {
        //     return;
        // }}
        if (FinishedLine == false)
        {
            DrawingRollerCoaster(PreviousSphere, NextSphere);
            
        }
        else if (FinishedLine == true && FinishedTangent == false)
        {
            ReferenceDrawTangent.DrawingTangent2();
        }
        else if (FinishedTangent == true && FinishedCentripetal == false)
        {
            ReferenceDrawCentripetal.DrawingCentripetal();
        }
        else if (FinishedCentripetal == true && FinishedNormal == false)
        {
            ReferenceDrawNormal.DrawingNormal();
        }


        // if (PreviousTangent == 200 && tangentList.Count >= 2 && ReadyTangent == true)
        // {
        //     lineRenderer.SetPosition(PreviousTangent, tangentList[PreviousTangent].transform.position);
        //     lineRenderer.SetPosition(NextTangent, tangentList[NextTangent].transform.position);
        //     PreviousTangent ++;
        //     NextTangent ++;
        // }
        // else
        // {
        //     isUpdating = false;
        // }
     } 
    
    

        private Vector3 EvaluateFunction(double t)
        {
            double r = 200;
            double x = Math.Cos(t) * r;
            double z = Math.Sin(t) * r;
            double y = 30 * t;
                
            return new Vector3((float)x, (float)y, (float)z);
        }

        private void CreatePoint(double t)
        {
            int messageCounter = 0;
            int counter = 1; 
            while (counter <= 200)
            {
                //Creates the points in the scene
                GameObject point = new GameObject("Point" + counter);
                //GameObject point = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                //point.name = "Point" + counter;
                //Positions the point with the function specified
                point.transform.position = EvaluateFunction(t);
                //Adds the point to the list
                sphereList.Add(point);
                t += 0.05;
                counter ++;
                messageCounter ++;
                }
         }

        private void createTangent()
        {
            int CurrentPoint = 1;
            int PreviousPoint = 0;
            int NextPoint = 2;
            for (int i = 1; i <= 200 && NextPoint < sphereList.Count; i++)
            {
                GameObject point = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                Vector3 V1 = sphereList[NextPoint].transform.position;
                Vector3 V2 = sphereList[PreviousPoint].transform.position;
                Vector3 tangent = (V1 - V2).normalized;
                Vector3 scaledTangent = tangent * 80f; // Escalar la tangente
                point.transform.position = sphereList[CurrentPoint].transform.position + scaledTangent;
                point.transform.Translate(scaledTangent, Space.World);
                tangentList.Add(point);
                point.name = "Tangent" + i;

                PreviousPoint ++;
                CurrentPoint ++;
                NextPoint ++;


            }
        }

        private void createCentripetal()
        {
            int CurrentPoint = 1;
            int PreviousPoint = 0;
            int NextPoint = 2;
            for (int i = 1; i<= 200 && NextPoint < sphereList.Count; i++)
            {
                GameObject point = GameObject.CreatePrimitive(PrimitiveType.Cube);
                Vector3 V1 =  ((sphereList[NextPoint].transform.position) -  (sphereList[CurrentPoint].transform.position));
                Vector3 V2 =  ((sphereList[PreviousPoint].transform.position) -  (sphereList[CurrentPoint].transform.position));
                Vector3 centripetal = (V1 + V2).normalized * 80f;
                point.transform.position = sphereList[CurrentPoint].transform.position + centripetal;
                point.transform.Translate(centripetal, Space.World);
                centripetalList.Add(point);
                point.name = "Centripetal" + i;

                PreviousPoint = CurrentPoint;
                CurrentPoint = NextPoint;
                NextPoint ++;
            }
        }

        private void createNormal()
        {
            int position  = 0;
            for (int i = 1; i <= 200 && position < sphereList.Count; i++)
            {
                GameObject point = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                Vector3 V1 = tangentList[position].transform.position;
                Vector3 V2 = centripetalList[position].transform.position;
                Vector3 normal = Vector3.Cross(V1, V2).normalized * 80f;
                point.transform.position = sphereList[position].transform.position + normal;
                point.transform.Translate(normal, Space.World);
                normalList.Add(point);
                point.name = "Normal" + i;

    
                position ++;
            }
            
        }

        private void DrawingRollerCoaster(int PreviousLocation, int NextLocation)
        {
            // // Configuración inicial del LineRenderer
                
            lineRenderer.positionCount = 200;
            lineRenderer.startWidth = 1f;
            lineRenderer.endWidth = 1f;

            if (PreviousLocation < 200 && sphereList.Count >= 2 && NextLocation < sphereList.Count)
            {
                lineRenderer.SetPosition(PreviousLocation, sphereList[PreviousLocation].transform.position);
                lineRenderer.SetPosition(NextLocation, sphereList[NextLocation].transform.position);
                
            }
            

            if (NextLocation >= 200)
                {
                    FinishedLine = true;
                }
            else
                {
                    PreviousSphere ++;
                    NextSphere ++;
                }
          
        }
        private void DrawingTangent()
        {
            // // Configuración inicial del LineRenderer

            if (SphereTangent != 200 &&  tangentList.Count >= 2)
            {
                lineRenderer.SetPosition(SphereTangent, sphereList[SphereTangent].transform.position);
                lineRenderer.SetPosition(CurrentTangent, tangentList[CurrentTangent].transform.position);
                CurrentTangent ++;
                SphereTangent ++;
                if (SphereTangent >= 200)
                {
                    FinishedTangent = true;
                }
            }
        }


}