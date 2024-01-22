using System.Collections.Generic;
using UnityEngine;

public abstract class EnergyStructureCluster
{
    public Vector3 Position { get; set; }
    public List<EnergyStructure> EnergyStructures { get; }
    
    public EnergyStructureCluster(Vector3 position)
    {
        Position = position;
        EnergyStructures = new List<EnergyStructure>();
    }
    
    public void DestroyCluster()
    {
        EnergyStructures.ForEach(energyStructure => energyStructure.DestroyEnergyStructure());
    }
    
    protected Vector3[] DistributePointsInGrid(int n)
    {
        int rows = Mathf.CeilToInt(Mathf.Sqrt(n));
        int columns = Mathf.CeilToInt(n / (float)rows);

        Vector3[] points = new Vector3[n];

        for (int i = 0; i < n; i++)
        {
            int row = i % rows;
            int col = i / rows;

            float x = col * Constants.HouseSeparation - 0.5f * (columns - 1) * Constants.HouseSeparation;
            float y = Constants.InitialHousePosition.y;
            float z = row * Constants.HouseSeparation - 0.5f * (rows - 1) * Constants.HouseSeparation;

            points[i] = new Vector3(x, y, z) + Position;
        }

        return points;
    }
    
    public void MoveCluster(Vector3 offset)
    {
        Position += offset;
        foreach (var energyStructure in EnergyStructures)
        {
            energyStructure.MoveEnergyStructure(offset);
        }
    }
}
