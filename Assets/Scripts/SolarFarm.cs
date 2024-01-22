using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SolarFarm : EnergyStructureCluster
{
    public float TargetEnergyProduction { get; set; }
    public SolarFarm(Vector3 position, int numberOfPanels, float targetEnergyProduction) : base(position)
    {
        TargetEnergyProduction = targetEnergyProduction;
        GenerateEnergyStructures(numberOfPanels);
    }

    protected override void GenerateEnergyStructures(int numberOfPanels)
    {
        // Generate positions for the houses in a filled circular pattern
        Vector3[] turbinePositions = DistributePointsInGrid(numberOfPanels);

        // Instantiate houses at the generated positions
        for (int i = 0; i < numberOfPanels; i++)
        {
            EnergyStructures.Add(ScriptableObject.CreateInstance<SolarPanel>());
            (EnergyStructures.Last() as SolarPanel).Initialize(turbinePositions[i]);
        }
    }
}