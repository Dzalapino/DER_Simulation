using UnityEngine;

public class House : EnergyStructure
{
    public House(Vector3 position) : base("House", position, CycleTarget.House) { }
}
