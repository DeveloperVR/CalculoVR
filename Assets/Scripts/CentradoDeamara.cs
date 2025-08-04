using UnityEngine;
using UnityEngine.XR;

public class XROriginRecentering : MonoBehaviour
{
    public GameObject Player; // El objeto XR Origin o XR Rig

    public void RecenterToHeadset()
    {
        // Obtener la posición y rotación de la cámara (HMD)
        Transform cameraTransform = Camera.main.transform;

        // Calcula el desplazamiento entre el XR Origin y la cabeza
        Vector3 headsetPosition = cameraTransform.position;
        Vector3 offset = Player.transform.position - headsetPosition;
        offset.y = 0f; // Opcional: mantener la altura actual

        // Aplicar corrección al XR Origin
        Player.transform.position += offset;

        // Corregir inclinación si es necesario (alinear rotación horizontal)
        Vector3 forward = cameraTransform.forward;
        forward.y = 0;
        if (forward.sqrMagnitude > 0.01f)
        {
            Player.transform.forward = forward;
        }
    }

    public void SetPlayer()
    {
        Player = RCGameManager.Instance.LocalPlayer;
        if (Player == null)
        {
            Debug.LogError("XR Origin (XR Rig) not found in the scene.");
        }
    }
    
    public void SetOfflinePlayer()
    {
        Player = GameObject.Find("XR Player");
        Debug.Log("XR Player found: " + Player.name);
    }
}
