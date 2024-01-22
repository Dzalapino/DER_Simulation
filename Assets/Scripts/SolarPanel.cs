using UnityEngine;

public class SolarPanel : EnergyStructure
{
    public void Initialize(Vector3 position)
    {
        base.Initialize("SolarPanel", position, CycleTarget.Solar);
    }
}