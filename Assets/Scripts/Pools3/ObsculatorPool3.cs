using UnityEngine;
using System.Threading.Tasks;
using System.Collections.Generic;

public class ObsculatorPool3 : MonoBehaviour
{
    [Header("Asignaciones")]
    [SerializeField] private GameObject cart;
    [SerializeField] private GameObject prefabObsculator;
    [SerializeField] private DrawLine3 drawLine;



    [Header("Pool settings")]
    [SerializeField] private int poolSize = 20;
    public List<GameObject> drawerPool3 = new List<GameObject>();
    private int currentDrawerIndex = 0;
    private int currentIndex = 0;
    async void Start()
    {
        await Task.Delay(4000);
        for (int i = 0; i < poolSize; i++)
        {
            GameObject drawer = Instantiate(prefabObsculator);
            drawer.name = "obsculatordrawer2 " + i;
            drawer.transform.SetParent(transform);
            drawer.SetActive(false);
            drawerPool3.Add(drawer);
        }
    }

    void UseDrawer(List<GameObject> list)
    {
        GameObject drawer = drawerPool3[0];
        drawer.SetActive(true);

        LineRenderer lr = drawer.GetComponent<LineRenderer>();
        lr.startWidth = 0.1f;
        lr.endWidth = 0.1f;
        if (lr != null)
        {
            lr.positionCount = 34;
            for (int i = 0; i < poolSize; i++)
            {
                if(i <= list.Count - 1)
                {
                    lr.SetPosition(i, list[i].transform.position);
                    lr.SetPosition(i+1, list[i+1].transform.position);
                }
                
            }
        }

        currentDrawerIndex++;
        if (currentDrawerIndex >= poolSize)
            currentDrawerIndex = 0;
    }

    // void ShowDrawers(GameObject drawer)
    // { 
    //     for (int i = 0, i < poolSize; i++)
    //     {
    //         [i].SetActive(true);
    //     }
    // }

    // public void addAndUsePool(List<GameObject> list)
    // {
    //     UseDrawer(list);
    //     currentIndex++;
    //     if (currentIndex >= poolSize)
    //         currentIndex = 0;

    // }  
}
