using UnityEngine;

public class House : EnergyStructure
{
    public DailyEnergyCycle DailyEnergyCycle { get; set; }

    public House(Vector3 position) : base("House", position)
    {
        DailyEnergyCycle = new DailyEnergyCycle(CycleTarget.House);
    }
}
