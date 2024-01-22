using UnityEngine;

public class House : EnergyStructure
{
    public void Initialize(Vector3 position)
    {
        base.Initialize("House", position, CycleTarget.House);
    }
}
