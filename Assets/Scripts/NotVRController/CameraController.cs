using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float sensX;
    [SerializeField] private float sensY;

    public Transform orientation;
    private float xRotation;
    private float yRotation;
    Ray cameraRay;
    LayerMask UILayerMask;

    [SerializeField]Camera cam;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        UILayerMask = LayerMask.GetMask("UI");
    }

    // Update is called once per frame
    void Update()
    {
        HandleCameraRotation();
        HandleRaycast();
    }

    public void HandleCameraRotation()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;

        yRotation += mouseX;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.localRotation = Quaternion.Euler(xRotation, yRotation, 0);
    }

    public void HandleRaycast()
    {
        

        if (Input.GetMouseButtonDown(0))
        { 
            cameraRay = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            if (Physics.Raycast(cameraRay, out RaycastHit hit, 50f, UILayerMask))
            {
                Debug.Log("Hit: " + hit.collider.name);

                UnityEngine.UI.Button button = hit.collider.GetComponent<UnityEngine.UI.Button>();
                if (button != null)
                {
                    button.onClick.Invoke();
                    Debug.Log("Button clicked: " + button.name);
                }
                // You can add logic here to interact with the object hit by the raycast
            }
        }
        
    }
        
}
