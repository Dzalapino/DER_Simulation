using System;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class EnergyStructure : ScriptableObject
{
    private GameObject _structureObject;
    public Vector3 Position { get; set; }

    private void OnDisable()
    {
        // Destroy structure
        if (_structureObject.IsUnityNull()) return;
        // Destroy the instantiated prefab
        Destroy(_structureObject);
    }
    
    protected void Initialize(Vector3 position, string prefabName)
    {
        Position = position;

        // Load the prefab from the resources folder
        GameObject prefab = Resources.Load<GameObject>($"Prefabs/{prefabName}");

        if (prefab != null)
        {
            // Instantiate the prefab at the specified position
            _structureObject = Instantiate(prefab, Position, Quaternion.identity);
        }
        else
        {
            Debug.LogError($"Energy Structure prefab not found with path: Prefabs/{prefabName}");
        }
    }

    public void MoveEnergyStructure(Vector3 offset)
    {
        Position += offset;
        if (_structureObject != null)
        {
            _structureObject.transform.position = Position;
        }
        else
        {
            Debug.LogError("Energy Structure prefab not found!");
        }
    }
}
