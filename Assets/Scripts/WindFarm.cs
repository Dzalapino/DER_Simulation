using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WindFarm : EnergyStructureCluster
{
    public float TargetEnergyProduction { get; set; }
    public WindFarm(Vector3 position, int numberOfTurbines, float targetEnergyProduction) : base(position)
    {
        TargetEnergyProduction = targetEnergyProduction;
        GenerateEnergyStructures(numberOfTurbines);
    }

    protected override void GenerateEnergyStructures(int numberOfPanels)
    {
        // Generate positions for the houses in a filled circular pattern
        Vector3[] turbinePositions = DistributePointsInGrid(numberOfPanels);

        // Instantiate houses at the generated positions
        for (int i = 0; i < numberOfPanels; i++)
        {
            EnergyStructures.Add(ScriptableObject.CreateInstance<WindTurbine>());
            (EnergyStructures.Last() as WindTurbine).Initialize(turbinePositions[i]);
        }
    }
}