using UnityEngine;

public class ExistingSurfacesMenu : MonoBehaviour
{
    GameObject functionInstantiator;
    GameObject functionGenerator;
    [SerializeField] GameObject ThisMenu;
    [SerializeField] GameObject Surface1;
    string Surface1Function = "25.6* (2.71)^(-((x - 25.6)^2 / 1.9^4 + (y - 20.2)^2 / 4.1^2))   +  7.6* (2.71)^(-((x - 20.1)^2 / 1.8^4 + (y - 17)^2 / 2.6^2))   +   15.8* (2.71)^(-((x - 8.3)^2 / 2.2^4 + (y - 30)^2 / 7.2^2))  + 8* (2.71)^(-((x - 14.2)^2 / 1.5^4 + (y - 26.8)^2 / 3.2^2))   +    9.5* (2.71)^(-((x - 6.6)^2 / 2.2^4 + (y -17.2)^2 / 5^2))   +   17.7* (2.71)^(-((x - 2.7)^2 / 1.9^4 + (y - 8.5)^2 / 3.4^2))  +  8.6* (2.71)^(-((x - 15.6)^2 / 2.5^4 + (y - 3.4)^2 / 5.5^2))  -  9.5* (2.71)^(-((x - 31.7)^2 / 2.7^4 + (y - 7.5)^2 / 5.3^2))  -5* (2.71)^(-((x - 21.8)^2 / 1.8^4 + (y - 10.7)^2 / 4.5^2)) -2* (2.71)^(-((x - 25.5)^2 / 2.5^4 + (y - 30)^2 / 3.2^2))";
    string Surface2Function = "21* (2.71)^(-((x - 25)^2 / 2.2^4 + (y - 25.6)^2 / 4.46^2))  +  13.7* (2.71)^(-((x - 20.8)^2 / 1.8^4 + (y - 17.2)^2 / 4^2))  + 11* (2.71)^(-((x - 4.7)^2 / 1.7^4 + (y - 26.7)^2 / 2.7^2))  + 4* (2.71)^(-((x - 12.4)^2 / 1.34^4 + (y - 21.6)^2 / 1.87^2))  + 28* (2.71)^(-((x - 24.3)^2 / 1.5^4 + (y - 6.2)^2 / 2.7^2))     -   10* (2.71)^(-((x - 18.5)^2 / 1.8^4 + (y - 5.2)^2 / 4.3^2))  -  6.5* (2.71)^(-((x - 12.6)^2 / 1.7^4 + (y - 7.5)^2 / 5.3^2))  - 9* (2.71)^(-((x - 5.5)^2 / 1.8^4 + (y - 13)^2 / 5^2))   -  9.5* (2.71)^(-((x - 10.4)^2 / 1.8^4 + (y - 26.4)^2 / 3.2^2))";
    [SerializeField] GameObject Surface2;
    [SerializeField] GameObject Surface3;

    GameObject Player;

    public void ProcessSurface1(GameObject surface)
    {
        ThisMenu.SetActive(false);
        surface.SetActive(true);
        functionInstantiator = GameObject.Find("FunctionInstantiator");
        functionGenerator = GameObject.Find("FunctionGenerator");
        Mesh surfaceMesh = surface.GetComponent<MeshFilter>().mesh;
        functionGenerator.GetComponent<FunctionGenerator>().SetFunctionString(Surface1Function);
        functionInstantiator.GetComponent<FunctionInstantiator>().ComputePoints(surfaceMesh);
        functionInstantiator.GetComponent<FunctionInstantiator>().Gradients();
    }

    public void ProcessSurface2(GameObject surface)
    {
        ThisMenu.SetActive(false);
        surface.SetActive(true);
        functionInstantiator = GameObject.Find("FunctionInstantiator");
        functionGenerator = GameObject.Find("FunctionGenerator");
        Mesh surfaceMesh = surface.GetComponent<MeshFilter>().mesh;
        functionGenerator.GetComponent<FunctionGenerator>().SetFunctionString(Surface2Function);
        functionInstantiator.GetComponent<FunctionInstantiator>().ComputePoints(surfaceMesh);
        functionInstantiator.GetComponent<FunctionInstantiator>().Gradients();
    }
    
    public void SetPlayer()
    {
        Player = FRGameManager.Instance.LocalPlayer;
        if (Player == null)
        {
            Debug.LogError("XR Origin (XR Rig) not found in the scene.");
        }
        else
        {
            Debug.Log("XR Origin (XR Rig) found: " + Player.name);
            ThisMenu.SetActive(true);
        }
    }
    

}
