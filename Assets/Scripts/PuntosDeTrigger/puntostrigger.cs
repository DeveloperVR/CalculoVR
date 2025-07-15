using System.Collections.Generic;
using UnityEngine;

public class puntostrigger : MonoBehaviour
{

    public GameObject tangent;

    public List<GameObject> circlePointsList1;

    public GameObject centripetal;
    public GameObject normal;
    public GameObject CDrawer;
    public float radius = 1.0f;

    private bool Activated = false;
    private bool Activated2 = false;


    //Se asignan las variables para el escalado
    public GameObject triggerPoint(float currentVelocity)
    {
        if (Activated)
        {
            return tangent;
        }
        if (tangent == null)
        {

            return null;
        }
        Vector3 t = tangent.transform.position;
        Vector3 s = transform.position;
        Vector3 Direction = (t - s).normalized;
        float distancia = Vector3.Distance(s, t);
        float escala = (currentVelocity / 4) * distancia;
        Vector3 tangentDirection = s + Direction * escala;
        tangent.transform.position = tangentDirection;
        Activated = true;
        return tangent;
    }

    public GameObject scaling(float currentVelocity)
    {
        //Debug.Log("Entro al escalado de la fuerza centripeta");
        if (Activated2)
        {
            return centripetal;
        }
        if (centripetal == null)
        {
            return null;
        }
        if (radius == 0 || radius == 1)
        {
            //Debug.Log("El radio es cero");
            return null;
        }
        //Debug.Log("El punto centripetal antes del escalado es: " + centripetal.transform.position);
        Vector3 t = centripetal.transform.position;
        Vector3 s = transform.position;
        Vector3 Direction = (t - s).normalized;
        float distancia = Vector3.Distance(s, t);
        float escala = (distancia * ((currentVelocity * currentVelocity) / radius));
        Vector3 centripetalDirection = s + Direction * escala;
        centripetal.transform.position = centripetalDirection;
        Activated2 = true;
        //Debug.Log("El punto centripetal después del escalado es: " + centripetal.transform.position);
        return centripetal;
    }


    public GameObject GetTangentPosition()
    {
        return tangent;
    }
    public GameObject GetCentripetalPosition()
    {
        return centripetal;
    }
    public List<GameObject> GetCirclePointsList()
    {
        return circlePointsList1;
    }

    public GameObject GetNormalPosition()
    {
        return normal;
    }

    public void ShowDrawer()
    {
        Debug.Log("Entro al GetDrawer");
        if (CDrawer != null)
        {
            CDrawer.SetActive(true);
        }
        else
        {
        }
    }
    
    public void HideDrawer()
    {
        if (CDrawer != null)
        {
            CDrawer.SetActive(false);
        }
    }
}
