using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class City : EnergyStructureCluster
{
    public float TargetEnergyConsumption { get; private set; }
    
    public float TargetEnergyConsumptionPerHouse => TargetEnergyConsumption / EnergyStructures.Count;
    public City(Vector3 position, int numberOfHouses, float targetEnergyConsumption) : base(position)
    {
        TargetEnergyConsumption = targetEnergyConsumption;
        
        // Generate positions for the houses in a filled circular pattern
        Vector3[] housePositions = DistributePointsInGrid(numberOfHouses);

        // Instantiate houses at the generated positions
        for (int i = 0; i < numberOfHouses; i++)
        {
            EnergyStructures.Add(new House(housePositions[i]));
        }
    }
}