using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float panSpeed = 5f;
    public float panBorderThickness = 10f;
    public float zoomSpeed = 15f;

    private Camera _mainCamera;

    private void Start()
    {
        // Set the initial camera position
        transform.position = new Vector3(0f, 20f, 0f);
        // Make the camera point down
        transform.rotation = Quaternion.Euler(90f, 0f, 0f);
        
        // Get the Camera component attached to the same GameObject as this script
        _mainCamera = GetComponent<Camera>();

        if (_mainCamera.IsUnityNull())
        {
            Debug.LogError("Camera component not found on the GameObject CameraController is attached to");
        }
    }

    void Update()
    {
        // Update camera
        PanCamera();
        ZoomCamera();
    }

    void PanCamera()
    {
        Vector3 position = transform.position;

        // Check if the cursor is near the screen borders
        if (Input.mousePosition.y >= Screen.height - panBorderThickness)
        {
            position.z += panSpeed * Time.deltaTime;
        }

        if (Input.mousePosition.y <= panBorderThickness)
        {
            position.z -= panSpeed * Time.deltaTime;
        }

        if (Input.mousePosition.x >= Screen.width - panBorderThickness)
        {
            position.x += panSpeed * Time.deltaTime;
        }

        if (Input.mousePosition.x <= panBorderThickness)
        {
            position.x -= panSpeed * Time.deltaTime;
        }

        // Apply the new position to the camera
        transform.position = position;
    }

    void ZoomCamera()
    {
        if (_mainCamera.IsUnityNull())
        {
            Debug.Log("Camera component is null. Ensure the script is attached to a GameObject with a Camera component.");
            return;
        }

        float scroll = Input.GetAxis("Mouse ScrollWheel");

        // Adjust the camera's position based on scroll input
        var transform1 = _mainCamera.transform;
        transform1.position += transform1.forward * (scroll * zoomSpeed);
    }
}
