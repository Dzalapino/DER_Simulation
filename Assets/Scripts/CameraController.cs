using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float panSpeed = 5f;
    public float panBorderThickness = 10f;

    private void Start()
    {
        // Set the initial camera position
        transform.position = new Vector3(0f, 20f, 0f);
        // Make the camera point down
        transform.rotation = Quaternion.Euler(90f, 0f, 0f);
    }

    void Update()
    {
        // Update camera
        PanCamera();
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
}
