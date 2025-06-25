using UnityEngine;
using UnityEngine.XR;

public class XROriginRecentering : MonoBehaviour
{
    public GameObject xrOrigin; // El objeto XR Origin o XR Rig

    public void RecenterToHeadset()
    {
        // Obtener la posición y rotación de la cámara (HMD)
        Transform cameraTransform = Camera.main.transform;

        // Calcula el desplazamiento entre el XR Origin y la cabeza
        Vector3 headsetPosition = cameraTransform.position;
        Vector3 offset = xrOrigin.transform.position - headsetPosition;
        offset.y = 0f; // Opcional: mantener la altura actual

        // Aplicar corrección al XR Origin
        xrOrigin.transform.position += offset;

        // Corregir inclinación si es necesario (alinear rotación horizontal)
        Vector3 forward = cameraTransform.forward;
        forward.y = 0;
        if (forward.sqrMagnitude > 0.01f)
        {
            xrOrigin.transform.forward = forward;
        }
    }
}
