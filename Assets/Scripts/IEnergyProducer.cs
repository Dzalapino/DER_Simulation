public interface IEnergyProducer
{
    float ProductionAmount { get; set; }
    float CurrentEnergyToDistribute { get; set; }

    void ProduceEnergy();
    float GetEnergy(float amountEnergyTaken);
}
