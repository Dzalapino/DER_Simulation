using System.Collections;
using System.Collections.Generic;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;

public class Hub
{
    public Vector3 Position { get; private set; } = Vector3.zero;
    public City City { get; } = null;
    public SolarFarm SolarFarm { get; } = null;
    public WindFarm WindFarm { get; } = null;

    public float EnergyStorageSize { get; set; } = 0f;
    public float CurrentEnergyStorage { get; set; } = 0f;
    
    public float CurrentEnergyStoragePercentage => CurrentEnergyStorage / EnergyStorageSize * 100f;

    public Hub(int numberOfHouses, float targetCityConsumption, float targetSolarProduction, 
        float targetWindProduction, float energyStorageSize)
    {
        EnergyStorageSize = energyStorageSize;
        CurrentEnergyStorage = energyStorageSize / 2f;
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
    
    public string GenerateDailyInfo()
    {
        StringBuilder info = new ();
        info.AppendLine("General info about daily consumption:");
        
        // Get the daily details for the solar farm
        if (!SolarFarm.IsUnityNull())
        {
            info.AppendLine("Solar farm production per hour:");
            var dailyValuePerHour = SolarFarm.DailyEnergyCycle.GetRealDailyValuePerHour(SolarFarm.TargetEnergyProduction);
            foreach (var keyValuePair in dailyValuePerHour)
            {
                info.AppendLine($"Hour {keyValuePair.Key}: {keyValuePair.Value} MWh");
                CurrentEnergyStorage += keyValuePair.Value;
            }
        }
        
        // Get the daily details for the wind farm
        if (!WindFarm.IsUnityNull())
        {
            info.AppendLine("Wind farm production per hour:");
            var dailyValuePerHour = WindFarm.DailyEnergyCycle.GetRealDailyValuePerHour(WindFarm.TargetEnergyProduction);
            foreach (var keyValuePair in dailyValuePerHour)
            {
                info.AppendLine($"Hour {keyValuePair.Key}: {keyValuePair.Value} MWh");
                CurrentEnergyStorage += keyValuePair.Value;
            }
        }
        
        int i = 0;
        float realCollectiveDailyConsumption = 0f;
        // Get the daily details for each house
        foreach (var energyStructure in City.EnergyStructures)
        {
            var house = energyStructure as House;
            var dailyValuePerHour = house.DailyEnergyCycle.GetRealDailyValuePerHour(City.TargetEnergyConsumptionPerHouse);
            float houseDailyConsumption = 0f;
            foreach (var keyValuePair in dailyValuePerHour)
            {
                houseDailyConsumption += keyValuePair.Value;
            }
            info.AppendLine($"House {i++} on position {house.Position}:\nTotal daily consumption: {houseDailyConsumption}");
            realCollectiveDailyConsumption += houseDailyConsumption;
            info.AppendLine();
        }
        
        CurrentEnergyStorage -= realCollectiveDailyConsumption;
        
        info.AppendLine(
            $"Total daily city consumption: {realCollectiveDailyConsumption}\n" + 
            $"Energy in storage after production/consumption cycle: {CurrentEnergyStorage} MWh\n" +
            $"Which is {CurrentEnergyStoragePercentage}% of the total storage size"
            );
        
        if (CurrentEnergyStorage > EnergyStorageSize * 3 / 4)
        {
            info.AppendLine(
                "Whole city produced more energy than the storage could handle\n" +
                "It was necessary to sell energy to the grid\n" +
                $"Total amount of energy sold: {CurrentEnergyStorage - EnergyStorageSize + EnergyStorageSize * 1 / 4} MWh");
            CurrentEnergyStorage = EnergyStorageSize * 3 / 4;
        }
        else if (CurrentEnergyStorage < 0f)
        {
            info.AppendLine(
                "Whole city used more energy than the storage could provide\n" +
                "It was necessary to buy the energy from grid\n" +
                $"Total cost of the energy from grid: {-CurrentEnergyStorage * Constants.EnergyPricePerMWh} PLN"
                );
        }
        else
        {
            info.AppendLine("Whole city used free energy from the storage!");
        }
        
        return info.ToString();
    }
}
