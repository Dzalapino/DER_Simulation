using Unity.VisualScripting;
using UnityEngine;

public class EnergyStructure
{
    private GameObject _structureObject;
    public DailyEnergyCycle DailyEnergyCycle { get; set; }
    public Vector3 Position { get; set; }
    
    public EnergyStructure(string prefabName, Vector3 position, CycleTarget cycleTarget)
    {
        Position = position;
        DailyEnergyCycle = new DailyEnergyCycle(cycleTarget);

        // Load the prefab from the resources folder
        GameObject prefab = Resources.Load<GameObject>($"Prefabs/{prefabName}");

        if (prefab != null)
        {
            // Instantiate the prefab at the specified position
            _structureObject = Object.Instantiate(prefab, Position, Quaternion.identity);
        }
        else
        {
            Debug.LogError($"Energy Structure prefab not found with path: Prefabs/{prefabName}");
        }
    }
    
    public void DestroyEnergyStructure()
    {
        if (_structureObject.IsUnityNull()) return;
        Object.Destroy(_structureObject);
        _structureObject = null;
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
