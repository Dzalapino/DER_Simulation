using System;
using System.Collections.Generic;
using UnityEngine;

public class House : EnergyStructure, IEnergyConsumer
{
    public float ConsumptionAmount { get; set; }
    public List<IEnergyProducer> EnergySources { get; set; }
    
    public House(Vector3 position, float consumptionAmount) : base(position, "House")
    {
        ConsumptionAmount = consumptionAmount;
    }

    public void ConsumeEnergy()
    {
        float energyToConsume = ConsumptionAmount;
        // TODO: Okay so basically how the energy have to be taken ? XD Mostly from the closest source if it can and then from the next ones?
        foreach (var energySource in EnergySources)
        {
            energyToConsume = energySource.GetEnergy(energyToConsume);
            
            // Stop taking energy from sources if the amount needed was already consumed
            if (Math.Abs(energyToConsume) < float.Epsilon)
            {
                break;
            }
        }
        
        // Check if there still is some energy to be consumed
        if (Math.Abs(energyToConsume) > float.Epsilon)
        {
            // TODO: Blackout baby. But how to do it? Of course we will do it visually but how. And how it is being done irl?
            Blackout();
        }
    }

    public void Blackout()
    {
        Debug.Log($"House on position {Position} has a blackout :(");
    }
}
