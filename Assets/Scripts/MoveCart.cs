using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCart : MonoBehaviour
{

    [SerializeField]
    DrawLine DrawLine;
    private List<GameObject> SphereListCopy = new List<GameObject>();
    [SerializeField]
    private float speed = 2.0f;
    private int currentSphere = 0;

    private float T = 0f;

    private int LastSphere = 199;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(WaitForData());
      
    }

    void Update()
    {
        MovingCart();
    }

    IEnumerator WaitForData()
    {
        while (DrawLine == null || DrawLine.ReadSphereList.Count == 0)
        {
            yield return null;
        }

        SphereListCopy = new List<GameObject>(DrawLine.ReadSphereList);

         if (SphereListCopy.Count > 0)
        {
            currentSphere = SphereListCopy.Count - 1;
            transform.position = SphereListCopy[LastSphere].transform.position;
        }
    }

    private void MovingCart()
    {
        if (SphereListCopy.Count < 2) return;

        Transform StartPoint = SphereListCopy[currentSphere].transform;
        Transform EndPoint = SphereListCopy[currentSphere - 1].transform;

        T += Time.deltaTime * speed / Vector3.Distance(StartPoint.position, EndPoint.position);
        transform.position = Vector3.Lerp(StartPoint.position, EndPoint.position, T);
        transform.forward = (EndPoint.position - StartPoint.position).normalized;

        if (T > 1f)
        {
            T = 0f;
            currentSphere --;
            if (currentSphere >= SphereListCopy.Count - 1)
            {
                currentSphere = 0;
                enabled = false;
            }
        }
    }

    


}
