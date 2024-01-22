using UnityEngine;

public class WindTurbine : EnergyStructure
{
    public void Initialize(Vector3 position)
    {
        base.Initialize("WindTurbine", position, CycleTarget.Wind);
    }
}