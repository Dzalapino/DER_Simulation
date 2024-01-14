using UnityEngine;

public class EnergyStructure : MonoBehaviour
{
    private GameObject _structureObject;
    public Vector3 Position { get; set; }

    public EnergyStructure(Vector3 position, string prefabName)
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
            Debug.LogError("Energy Structure prefab not found!");
        }
    }

    ~EnergyStructure()
    {
        // Destroy structure
        if (_structureObject != null)
        {
            // Destroy the instantiated prefab
            Destroy(_structureObject);
        }
    }

    public void MoveEnergyStructure(Vector3 offset)
    {
        if (_structureObject != null)
        {
            _structureObject.transform.position += offset;
        }
        else
        {
            Debug.LogError("Energy Structure prefab not found!");
        }
    }
}
