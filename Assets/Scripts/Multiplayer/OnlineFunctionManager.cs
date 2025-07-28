using Photon.Pun;
using UnityEngine;

public class OnlineFunctionManager : MonoBehaviourPun
{

    private GameObject FunctionGenerator;

    [SerializeField] private GameObject InputPanel;
    [SerializeField] private GameObject MenuPanel;
    [SerializeField] private GameObject TransportationPanel;

    private string Globalfunction;
    void Start()
    {
        FunctionGenerator = GameObject.Find("FunctionGenerator");
    }

    [PunRPC]
    public void CallFunctionGenerator(string function)
    {
        Debug.Log("La función ingresada es: " + function);
        FunctionGenerator.GetComponent<FunctionGenerator>().SetFunctionToFreeFunction(function);
        MenuPanel.SetActive(false);
        TransportationPanel.SetActive(true);
        InputPanel.SetActive(false);
    }

    public void OnlineCaller()
    {
        string func = InputPanel.GetComponent<CaptureFunction>().input;
        Debug.Log("El valor de func es: " + func);
        photonView.RPC("CallFunctionGenerator", RpcTarget.AllBuffered, func );
    }
}
