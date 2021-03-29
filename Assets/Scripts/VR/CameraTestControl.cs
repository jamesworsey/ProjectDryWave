using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTestControl : MonoBehaviour
{
    public float mouseSensitivity = 1.0f;
    public float cameraSmoothFactor = 0.1f;
    float targetX = 0, targetY = 0;
    float cameraX = 0, cameraY = 0;

    [SerializeField] GameObject playerCamera = null;

    private void Start()
    {
        playerCamera = gameObject;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        targetX += -Input.GetAxis("Mouse Y");
        targetY += Input.GetAxis("Mouse X");

        targetX = Mathf.Clamp(targetX, -45, 45);

        cameraX = Mathf.Lerp(cameraX, targetX, cameraSmoothFactor);
        cameraY = Mathf.Lerp(cameraY, targetY, cameraSmoothFactor);

        Vector3 cameraMovement = new Vector3(cameraX, cameraY, 0) * mouseSensitivity;

        playerCamera.transform.eulerAngles = cameraMovement;
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, playerCamera.transform.eulerAngles.y, 0);
    }
}
