using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WindFarm : EnergyStructureCluster
{
    public float TargetEnergyProduction { get; set; }
    public WindFarm(Vector3 position, int numberOfTurbines, float targetEnergyProduction) : base(position)
    {
        TargetEnergyProduction = targetEnergyProduction;
        // Generate positions for the turbines in a filled circular pattern
        Vector3[] turbinePositions = DistributePointsInGrid(numberOfTurbines);

        // Instantiate turbines at the generated positions
        for (int i = 0; i < numberOfTurbines; i++)
        {
            EnergyStructures.Add(new WindTurbine(turbinePositions[i]));
        }
    }
}