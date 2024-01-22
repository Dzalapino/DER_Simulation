using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class City : EnergyStructureCluster
{
    public float TargetEnergyConsumption { get; set; }
    public City(Vector3 position, int numberOfHouses, float targetEnergyConsumption) : base(position)
    {
        TargetEnergyConsumption = targetEnergyConsumption;
        GenerateEnergyStructures(numberOfHouses);
    }

    protected override void GenerateEnergyStructures(int numberOfPanels)
    {
        // Generate positions for the houses in a filled circular pattern
        Vector3[] housePositions = DistributePointsInGrid(numberOfPanels);

        // Instantiate houses at the generated positions
        for (int i = 0; i < numberOfPanels; i++)
        {
            EnergyStructures.Add(ScriptableObject.CreateInstance<House>());
            (EnergyStructures.Last() as House).Initialize(housePositions[i]);
        }
    }
}