using System.Collections.Generic;

public interface IEnergyConsumer
{
    float ConsumptionAmount { get; set; }
    List<IEnergyProducer> EnergySources { get; set; }
    
    void ConsumeEnergy();
}
