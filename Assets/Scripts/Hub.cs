using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hub
{
    private Vector3 Position { get; set; } = Vector3.zero;
    public City City { get; } = null;
    public SolarFarm SolarFarm { get; } = null;
    public WindFarm WindFarm { get; } = null;

    public Hub(int numberOfHouses, float targetCityConsumption, float targetSolarProduction, float targetWindProduction)
    {
        City = new City(
            Constants.InitialHousePosition,
            numberOfHouses,
            targetCityConsumption
            );
        SolarFarm = new SolarFarm(
            Constants.InitialHousePosition + Vector3.left * numberOfHouses * Constants.HouseSeparation / 2f,
            5,
            targetSolarProduction
            );
        WindFarm = new WindFarm(
            Constants.InitialWindTurbinePosition + Vector3.right * numberOfHouses * Constants.HouseSeparation / 2f,
            5,
            targetWindProduction
            );
    }
    
    public void DestroyHub()
    {
        City.DestroyCluster();
        SolarFarm.DestroyCluster();
        WindFarm.DestroyCluster();
    }
    
    public void MoveHub(Vector3 offset)
    {
        Position += offset;
        City.MoveCluster(offset);
        SolarFarm.MoveCluster(offset);
        WindFarm.MoveCluster(offset);
    }
}
