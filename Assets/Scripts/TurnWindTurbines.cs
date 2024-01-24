using System;
using UnityEngine;

public class TurnWindTurbines : MonoBehaviour
{
    public float rotationSpeed = 100f;
    private GameObject[] _propellers = Array.Empty<GameObject>();

    // Update is called once per frame
    void Update()
    {
        foreach (var propeller in _propellers)
        {
            // Rotate the propeller around its local Z-axis
            propeller.transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
        }
    }

    public void RefreshPropellers()
    {
        _propellers = GameObject.FindGameObjectsWithTag("Propeller");
    }
    
    public void ClearWindTurbines()
    {
        _propellers = Array.Empty<GameObject>();
    }
}
