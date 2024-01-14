public interface IEnergyProducer
{
    float ProductionAmount { get; set; }
    float CurrentEnergyToDistribute { get; set; }
    
    bool ProduceEnergy();
}
