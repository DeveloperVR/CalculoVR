using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class CentripetalPool3 : MonoBehaviour
{
    [Header("Asignaciones")]
    [SerializeField] private GameObject cart;
    [SerializeField] private GameObject prefabCentripetal;
    [SerializeField] private DrawLine1 drawLine;
    [SerializeField] private GameObject CentArrowPrefab;
    

    [Header("Pool settings")]
    [SerializeField] private int poolSize = 20;
    public List<GameObject> drawerPool3 = new List<GameObject>();
    public List<GameObject> AltdrawerPool3 = new List<GameObject>();
    private int currentDrawerIndex = 0;
    private int currentIndex = 0;
    async void Start()
    {
        await Task.Delay(2000);
        for (int i = 0; i < poolSize; i++)
        {
            GameObject drawer = Instantiate(prefabCentripetal);
            drawer.name = "centripetaldrawer3" + i;
            drawer.transform.SetParent(transform);
            drawer.SetActive(false);
            drawerPool3.Add(drawer);
        }

        for (int i = 0; i < poolSize; i++)
        {
            GameObject drawer = Instantiate(CentArrowPrefab);
            drawer.name = "Altcentripetaldrawer3" + i;
            drawer.transform.SetParent(transform);
            drawer.SetActive(false);
            AltdrawerPool3.Add(drawer);
        }
    }
  

    

    void UseDrawer (Vector3 start, Vector3 end, int index)
    {
        GameObject drawer = drawerPool3[currentDrawerIndex];
        drawer.SetActive(true);

        LineRenderer lr = drawer.GetComponent<LineRenderer>();
        lr.startWidth = 0.1f;
        lr.endWidth = 0.1f;
        if (lr != null)
        {
            lr.positionCount = 2;
            lr.SetPosition(0, start);
            lr.SetPosition(1, end);
        }

        currentDrawerIndex++;
        if (currentDrawerIndex >= poolSize)
            currentDrawerIndex = 0;
    }
    
    void AlternativeDrawer(Vector3 start, Vector3 end)
    {
        GameObject arrow = AltdrawerPool3[currentDrawerIndex];
        arrow.SetActive(true);
        Vector3 direccion = end - start;
        float distance = direccion.magnitude;

        arrow.transform.position = start;

        arrow.transform.rotation = Quaternion.LookRotation(-direccion);

        arrow.transform.localScale = new Vector3(32, 32, distance);
        if (arrow.transform.localScale.z > 15)
        {
            arrow.transform.localScale = new Vector3(32, 32, 15);
            arrow.transform.position = start;
        }

        currentDrawerIndex++;
        if (currentDrawerIndex >= poolSize)
            currentDrawerIndex = 0;
    }

    public void addAndUsePool(Vector3 poolingObjectPosition, Vector3 tangentPosition)
    {

        drawerPool3[currentIndex].transform.position = poolingObjectPosition;
        AlternativeDrawer(poolingObjectPosition, tangentPosition);
        currentIndex++;
        if (currentIndex >= poolSize)
            currentIndex = 0;

    }  
}