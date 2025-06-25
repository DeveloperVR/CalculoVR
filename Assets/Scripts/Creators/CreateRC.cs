using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

public class CreateRC : MonoBehaviour
{
    [Header("Puntos de la línea")]
    [SerializeField]
    private Transform pointA; // Primer punto
    [SerializeField]
    private Transform pointB; // Segundo punto
    [SerializeField]
    private Drawers drawers;
    private int indicator = 0;
    private int PreviousSphere = 0;
    private int NextSphere = 1;
    public List<GameObject> sphereList = new List<GameObject>();
    private bool FinishedLine = false;
    public List<GameObject> ReadSphereList => sphereList;
    // Start is called before the first frame update
    void Start()
    {
        CreatePoint(0);

    }

    // Update is called once per frame
    void Update()
    {
        if (FinishedLine == false)
        {
            indicator = drawers.DrawingRollerCoaster(PreviousSphere, NextSphere, indicator);
            PreviousSphere = NextSphere;
            NextSphere++;
            Debug.Log("El valor de PreviousSphere es: " + PreviousSphere);
            Debug.Log("El valor de NextSphere es: " + NextSphere); 
            Debug.Log("El valor de FinishedLine es: " + FinishedLine);
            if (indicator == 1)
            {
                Debug.Log("La línea ha terminado de dibujarse.");
                FinishedLine = true;
            }

        }
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
}
