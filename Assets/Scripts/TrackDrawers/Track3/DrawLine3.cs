using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;
using ExitGames.Client.Photon.StructWrapping;

public class DrawLine3 : MonoBehaviour
{
   [Header("Puntos de la línea")]
    private LineRenderer lineRenderer;
    public int PreviousSphere3 = 0;
    public int NextSphere3 = 1;
    private int CurrentTangent = 0;
    private int SphereTangent = 1;
    public List<GameObject> sphereList3 = new List<GameObject>();
    private bool isUpdating = true;
    public bool FinishedLine2 = false;
    public bool FinishedTangent2 = false;
    public bool FinishedCentripetal2 = false;
    public bool FinishedNormal2 = false;
    public int NumberOfPoints = 2000;
    [Header("Ortogonal")]
    [SerializeField]
    private DrawTangent1 ReferenceDrawTangent;
    [SerializeField]
    private DrawCentripetal1 ReferenceDrawCentripetal;
    [SerializeField]
    private DrawNormal1 ReferenceDrawNormal;
    [SerializeField] private GameObject prefabSphere;
    [SerializeField] private GameObject sphereparent;
    [SerializeField] private GameObject prefabObsculator;


    public List<GameObject> tangentList3 = new List<GameObject>();
    
    public List<GameObject> centripetalList3 = new List<GameObject>();
   
    public List<GameObject> normalList3 = new List<GameObject>();
    
    public List<GameObject> storedCircles3 = new List<GameObject>();
    public List<GameObject> DrawersCircles3 = new List<GameObject>();

    void Start()
    {

        lineRenderer = GetComponent<LineRenderer>();

        lineRenderer.positionCount = NumberOfPoints;
        lineRenderer.startWidth = 0.5f;
        lineRenderer.endWidth = 0.5f;

        sphereList3 = sphereparent.GetComponent<ContadorPrefabsHijos3>().children3;
        //CreatePoint(0);
        createTangent();
        asocciateTangents();

        createCentripetal();
        asocciateCentripetal();

        createNormal();
        asocciateNormal();

        CreateOsculatingCircle();

        CreateDrawers();

    }

    void Update()
    {
        if (isUpdating == false)
        {
             return;
        }
        if (FinishedLine2 == false)
        {
             DrawingRollerCoaster(PreviousSphere3, NextSphere3);
            
        }
        // else if (FinishedLine2 == true && FinishedTangent2 == false)
        // {
        //     ReferenceDrawTangent.DrawingTangent2();
        // }
        // else if (FinishedTangent2 == true && FinishedCentripetal2 == false)
        // {
        //     ReferenceDrawCentripetal.DrawingCentripetal();
        // }
        // else if (FinishedCentripetal2 == true && FinishedNormal 2== false)
        // {
        //     ReferenceDrawNormal.DrawingNormal();
        // }


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
            if (t >= 0 && t <= 1)
            {
                double x = 33 * t - 3 * Math.Pow(t, 3);
                double z = 0;
                double y = 90 * Math.Pow(t, 2) - 60 * Math.Pow(t, 3);
                    
                return new Vector3((float)x, (float)y, (float)z);
            }
            else if (t >= 1 && t <= 2)
            {
                double x = -0.02 * Math.Pow(t - 1, 3) + 0.03 * Math.Pow(t - 1, 2) + 10 * (t - 1) + 30;
                double z = 0;
                double y = 30;
                    
                return new Vector3((float)x, (float)y, (float)z);
            }
            else if (t >= 2 && t <= 3)
            {
                double x = 40 + 20 * Math.Cos(Math.PI * t - 5 * Math.PI / 2);
                double z = 20 + 20 * Math.Sin(Math.PI * t - 5 * Math.PI / 2);
                double y = 30;

                return new Vector3((float)x, (float)y, (float)z);
            }
            else if (t >= 3 && t <= 4)
            {
                double x = 40 - 80 * (t - 3);
                double z = 40;
                double y = 15 * Math.Exp(-0.2 * 7 * Math.PI / 5 * (t - 3)) * (Math.Cos(7 * Math.PI * (t - 3)) + 1);

                return new Vector3((float)x, (float)y, (float)z);
            }
            else if (t >= 4 && t <= 5)
            {
                double x = 10 * Math.Cos(Math.PI / 2 * ((t - 4) + 1)) - 40;
                double z = 10 * Math.Sin(Math.PI / 2 * ((t - 4) + 1)) + 30;
                double y = 0;

                return new Vector3((float)x, (float)y, (float)z);
            }
            else if (t >= 5 && t <= 6)
            {
                double x = -50;
                double z = 30 - (3 * Math.Pow((1 - (t - 5)), 2) * (t - 5) * 4 + 3 * (1 - (t - 5)) * Math.Pow((t - 5), 2) * 12 + Math.Pow((t - 5), 3) * 15);
                double y = 3 * (1 - (t - 5)) * Math.Pow((t - 5), 2) * 15 + Math.Pow((t - 5), 3) * 15;
                
                return new Vector3((float)x, (float)y, (float)z);
            }
            else if (t >= 6 && t <= 7)
            {
                double x = -45 + 5 * Math.Cos(Math.PI + (Math.PI / 2) * (t - 6));
                double z = 15 + 5 * Math.Sin(Math.PI + (Math.PI / 2) * (t - 6));
                double y = 15;

                return new Vector3((float)x, (float)y, (float)z);
            }
            else if (t >= 7 && t <= 8)
            {
                double x = -45 + 20 * (t - 7);
                double z = 10;
                double y = 15;

                return new Vector3((float)x, (float)y, (float)z);
            } 
            else if (t >= 8 && t <= 9)
            {
                double x = -25 + 5 * Math.Pow(t - 8, 3) + 6 * (t - 8) * Math.Pow(1 - (t - 8), 2);
                double z = 10;
                double y = 45 * (t - 8) * Math.Pow(1 - (t - 8), 2) + 15 * Math.Pow(1 - (t - 8), 3);

                return new Vector3((float)x, (float)y, (float)z);
            }
            else if (t >= 9 && t <= 10)
            {
                double x = -20 + 10 * (t - 9);
                double z = 10;
                double y = 0;

                return new Vector3((float)x, (float)y, (float)z);
            }
            else if (t >= 10 && t <= 11)
            {
                double x = -10 + 3 * Math.Pow(1 - (t - 10), 2) * (t - 10) * 8 + 3 * (1 - (t - 10)) * Math.Pow(t - 10, 2) * 8;
                double z = 10 - 5 * (t - 10);
                double y = 3 * (1 - (t - 10)) * Math.Pow(t - 10, 2) * 10 + Math.Pow(t - 10, 3) * 10;

                return new Vector3((float)x, (float)y, (float)z);
            }
            else if (t >= 11 && t <= 12)
            {
                double x = -10 - (3 * Math.Pow(t - 11, 2) * (1 - (t - 11)) * 8 + 3 * (t - 11) * Math.Pow(1 - (t - 11), 2) * 8);
                double z = 5 * (1 - (t - 11));
                double y = 3 * (t - 11) * Math.Pow(1 - (t - 11), 2) * 10 + Math.Pow(1 - (t - 11), 3) * 10;

                return new Vector3((float)x, (float)y, (float)z);
            }
            else if (t >= 12 && t <= 13)
            {
                double x = -10 + 10 * (t - 12);
                double z = 0;
                double y = 0;

                return new Vector3((float)x, (float)y, (float)z);
            }
            else{
                return new Vector3(0, 0, 0);
            }

        }

    // private void CreatePoint(double t)
    // {
    //     int messageCounter = 0;
    //     int counter = 1;
    //     while (counter <= NumberOfPoints)
    //     {
    //         //Creates the points in the scene
    //         GameObject point = Instantiate(prefabSphere);
    //         point.transform.SetParent(sphereparent.transform);
    //         point.name = "T3Points" + counter;
    //         //Positions the point with the function specified
    //         point.transform.position = EvaluateFunction(t);
    //         //Adds the point to the list
    //         sphereList3.Add(point);
    //         t += 0.005;
    //         counter++;
    //         messageCounter++;
    //     }

    // }
        private void createTangent()
    {
        int CurrentPoint = 1;
        int PreviousPoint = 0;
        int NextPoint = 2;
        int counter = 1;
        for (int i = 1; i <= NumberOfPoints && NextPoint < sphereList3.Count; i++)
        {
            GameObject point = new GameObject("Point" + counter);
            point.name = "Track2tangent" + counter;
            //GameObject point = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            Vector3 V1 = sphereList3[NextPoint].transform.position;
            Vector3 V2 = sphereList3[PreviousPoint].transform.position;
            Vector3 tangent = (V1 - V2).normalized;
            Vector3 scaledTangent = tangent; // Escalar la tangente
            point.transform.position = sphereList3[CurrentPoint].transform.position + scaledTangent;
            point.transform.Translate(scaledTangent, Space.World);
            tangentList3.Add(point);
            //point.name = "Tangent" + i;

            PreviousPoint++;
            CurrentPoint++;
            NextPoint++;
            counter++;
        }

    }

    private void asocciateTangents()
    {
        for (int i = 1; i < sphereList3.Count && i - 1 < tangentList3.Count; i++)
        {
            sphereList3[i].GetComponent<puntostrigger3>().tangent = tangentList3[i - 1];
        }
    }
    //sphereList[i].Getcomponent<puntostrigger3>().circlePointsList = 

    private void createCentripetal()
    {
        int CurrentPoint = 1;
        int PreviousPoint = 0;
        int NextPoint = 2;
        int counter = 1;
        for (int i = 1; i <= NumberOfPoints && NextPoint < sphereList3.Count; i++)
        {
            GameObject point = new GameObject("Track2Centripetal" + counter);
            //GameObject point = GameObject.CreatePrimitive(PrimitiveType.Cube);
            Vector3 V1 = ((sphereList3[NextPoint].transform.position) - (sphereList3[CurrentPoint].transform.position)).normalized;
            Vector3 V2 = ((sphereList3[PreviousPoint].transform.position) - (sphereList3[CurrentPoint].transform.position)).normalized;
            Vector3 centripetal = (V1 + V2).normalized * 5;
            point.transform.position = sphereList3[CurrentPoint].transform.position + centripetal;
            point.transform.Translate(centripetal, Space.World);
            centripetalList3.Add(point);
            //point.name = "Centripetal" + i;

            PreviousPoint = CurrentPoint;
            CurrentPoint = NextPoint;
            NextPoint++;
            counter++;
        }
    }

    private void asocciateCentripetal()
    {
        for (int i = 1; i < sphereList3.Count && i - 1 < centripetalList3.Count; i++)
        {
            sphereList3[i].GetComponent<puntostrigger3>().centripetal = centripetalList3[i - 1];
        }
    }

    private void createNormal()
    {
        int position = 0;
        int counter = 1;
        for (int i = 1; i <= NumberOfPoints && position < sphereList3.Count - 2; i++)
        {

            if (position >= sphereList3.Count)
            {
                position = 1998;
            }
            GameObject point = new GameObject("Track2Normal" + counter);
            //GameObject point = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            Vector3 V1 = tangentList3[position].transform.position;
            Vector3 V2 = centripetalList3[position].transform.position;
            Vector3 normal = Vector3.Cross(V1, V2).normalized;
            point.transform.position = sphereList3[position].transform.position + normal;
            point.transform.Translate(normal, Space.World);
            normalList3.Add(point);
            //point.name = "Normal" + i;


            position++;

            counter++;
        }


    }

    private void asocciateNormal()
    {
        for (int i = 1; i < sphereList3.Count && i - 1 < normalList3.Count; i++)
        {
            sphereList3[i].GetComponent<puntostrigger3>().normal = normalList3[i - 1];
        }
    }

    private void CreateOsculatingCircle()
    {
        int sidesInCircle = 32;


        for (int i = 1; i <= NumberOfPoints && i < sphereList3.Count - 2; i++)
        {

            Vector3 currentPoint = sphereList3[i].transform.position;
            Vector3 nextPoint = sphereList3[i + 1].transform.position;
            Vector3 previousPoint = sphereList3[i - 1].transform.position;
            float a = Vector3.Distance(currentPoint, previousPoint);
            float b = Vector3.Distance(currentPoint, nextPoint);
            float c = Vector3.Distance(previousPoint, nextPoint);
            float s = (a + b + c) / 2;
            float area = Mathf.Sqrt(s * (s - a) * (s - b) * (s - c));
            if (area > 1e-6f)
            {
                float radius = a * b * c / (4 * area);
                sphereList3[i].GetComponent<puntostrigger3>().radius = radius;
                Vector3 dir1 = (nextPoint - currentPoint).normalized;
                Vector3 dir2 = (previousPoint - currentPoint).normalized;
                Vector3 centripetal = dir1 + dir2;
                if (centripetal.magnitude >= 0.001f)
                {
                    centripetal.Normalize();
                    Vector3 center = currentPoint + (radius * centripetal);
                    Vector3 xDirection = (nextPoint - center).normalized;
                    Vector3 yDirection = (nextPoint - currentPoint).normalized;
                    Vector3[] circlePoints = new Vector3[sidesInCircle + 1];
                    float angleStep = 2 * Mathf.PI / sidesInCircle;

                    for (int j = 0; j <= sidesInCircle; j++)
                    {
                        GameObject Circlepoint = new GameObject("Track2CirclePoint" + j);
                        float angle = j * angleStep;
                        Vector3 point = center + radius * (Mathf.Cos(angle) * xDirection + Mathf.Sin(angle) * yDirection);
                        Circlepoint.transform.position = point;
                        sphereList3[i].GetComponent<puntostrigger3>().circlePointsList.Add(Circlepoint);
                        storedCircles3.Add(Circlepoint);
                    }
                }

            }
        }
    }


    private void CreateDrawers()
    {

        for (int i = 0; i < sphereList3.Count - 1; i++)
        {

            if (sphereList3[i].GetComponent<puntostrigger3>().circlePointsList.Count != 0)
            {

                GameObject drawer = Instantiate(prefabObsculator);
                drawer.name = "Track2drawer" + i;
                DrawersCircles3.Add(drawer);
                sphereList3[i].GetComponent<puntostrigger3>().CDrawer = drawer;
                for (int j = 1; j < sphereList3[i].GetComponent<puntostrigger3>().circlePointsList.Count - 1; j++)
                {

                    GameObject point = sphereList3[i].GetComponent<puntostrigger3>().circlePointsList[j - 1];
                    GameObject point2 = sphereList3[i].GetComponent<puntostrigger3>().circlePointsList[j];
                    LineRenderer lr = drawer.GetComponent<LineRenderer>();

                    lr.startWidth = 0.4f;
                    lr.endWidth = 0.4f;
                    lr.positionCount = sphereList3[i].GetComponent<puntostrigger3>().circlePointsList.Count - 1;
                    if (lr != null)
                    {

                        lr.SetPosition(j - 1, point.transform.position);
                        lr.SetPosition(j, point2.transform.position);
                    }
                }
                drawer.SetActive(false);

            }

        }

    }
    int contador = 0;
    private void DrawingRollerCoaster(int PreviousLocation, int NextLocation)
    {
        if (sphereList3.Count < 2)
        {
            return;
        }
        lineRenderer.SetPosition(contador, sphereList3[contador].transform.position);
        if (contador >= sphereList3.Count - 1)
        {
            FinishedLine2 = true;
        }
        contador++;


        // if (PreviousLocation < NumberOfPoints && sphereList.Count >= 2 && NextLocation < sphereList.Count)
        // {
        //     lineRenderer.SetPosition(PreviousLocation, sphereList[PreviousLocation].transform.position);
        //     lineRenderer.SetPosition(NextLocation, sphereList[NextLocation].transform.position);
        //     Debug.Log("entró al nucleo " + contador + "veces");
        //     contador ++;
        //     Debug.Log("punto previo " + PreviousLocation);
        //     Debug.Log("siguiente punto" + NextLocation);

        // }


        // if (NextLocation >= NumberOfPoints)
        //     {
        //         FinishedLine2 = true;
        //     }
        // else
        //     {
        //         PreviousSphere ++;
        //         NextSphere ++;
        //     }

    }
    private void DrawingTangent()
    {
        // // Configuración inicial del LineRenderer

        if (SphereTangent != NumberOfPoints && tangentList3.Count >= 2)
        {
            lineRenderer.SetPosition(SphereTangent, sphereList3[SphereTangent].transform.position);
            lineRenderer.SetPosition(CurrentTangent, tangentList3[CurrentTangent].transform.position);
            CurrentTangent++;
            SphereTangent++;
            if (SphereTangent >= NumberOfPoints)
            {
                FinishedTangent2 = true;
            }
        }
    }

    private bool StartTravel(bool isReady)
    {
        isReady = true;
        return isReady;
    }


}

