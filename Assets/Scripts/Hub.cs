using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hub
{
    public Vector3 Position { get; set; } = Vector3.zero;
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
}
