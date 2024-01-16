using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class City : EnergyStructureCluster
{
    public float MinEnergyConsumption { get; set; }
    public float MaxEnergyConsumption { get; set; }
    public City(Vector3 position, int numberOfHouses,
        float minEnergyConsumption, float maxEnergyConsumption) : base(position)
    {
        MinEnergyConsumption = minEnergyConsumption;
        MaxEnergyConsumption = maxEnergyConsumption;
        GenerateEnergyStructures(numberOfHouses);
    }

    protected override void GenerateEnergyStructures(int numberOfHouses)
    {
        // Generate positions for the houses in a filled circular pattern
        Vector3[] housePositions = DistributePointsInGrid(numberOfHouses);

        // Instantiate houses at the generated positions
        for (int i = 0; i < numberOfHouses; i++)
        {
            EnergyStructures.Add(ScriptableObject.CreateInstance<House>());
            (EnergyStructures.Last() as House).Initialize(
                housePositions[i], 
                Random.Range(MinEnergyConsumption, MaxEnergyConsumption)
            );
        }
    }
}