using System;
using System.Collections.Generic;
using System.Linq;

public enum CycleTarget
{
    House = 0,
    Solar = 1,
    Wind = 2
}

public class DailyEnergyCycle
{
    private CycleTarget Target { get; }
    private Dictionary<int, Tuple<float, float>> DailyPercentMinMax { get; } = new ();
    
    public DailyEnergyCycle(CycleTarget target)
    {
        Target = target;
        switch (target)
        {
            case CycleTarget.House:
                InitHouseDailyCycleMinMax();
                break;
            case CycleTarget.Solar:
                InitSolarDailyCycleMinMax();
                break;
            case CycleTarget.Wind:
                InitWindDailyCycleMinMax();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(target), target, null);
        }
    }
    
    private void InitHouseDailyCycleMinMax()
    {
        DailyPercentMinMax.Add(0, new Tuple<float, float>(1.875f, 3.125f));
        DailyPercentMinMax.Add(1, new Tuple<float, float>(1.875f, 3.125f));
        DailyPercentMinMax.Add(2, new Tuple<float, float>(1.875f, 3.125f));
        DailyPercentMinMax.Add(3, new Tuple<float, float>(0.9375f, 1.5625f));
        DailyPercentMinMax.Add(4, new Tuple<float, float>(0.9375f, 1.5625f));
        DailyPercentMinMax.Add(5, new Tuple<float, float>(0.9375f, 1.5625f));
        DailyPercentMinMax.Add(6, new Tuple<float, float>(2.8125f, 4.6875f));
        DailyPercentMinMax.Add(7, new Tuple<float, float>(2.8125f, 4.6875f));
        DailyPercentMinMax.Add(8, new Tuple<float, float>(2.8125f, 4.6875f));
        DailyPercentMinMax.Add(9, new Tuple<float, float>(2.8125f, 4.6875f));
        DailyPercentMinMax.Add(10, new Tuple<float, float>(3.75f, 6.25f));
        DailyPercentMinMax.Add(11, new Tuple<float, float>(3.75f, 6.25f));
        DailyPercentMinMax.Add(12, new Tuple<float, float>(3.75f, 6.25f));
        DailyPercentMinMax.Add(13, new Tuple<float, float>(3.75f, 6.25f));
        DailyPercentMinMax.Add(14, new Tuple<float, float>(4.6875f, 7.8125f));
        DailyPercentMinMax.Add(15, new Tuple<float, float>(4.6875f, 7.8125f));
        DailyPercentMinMax.Add(16, new Tuple<float, float>(4.6875f, 7.8125f));
        DailyPercentMinMax.Add(17, new Tuple<float, float>(4.6875f, 7.8125f));
        DailyPercentMinMax.Add(18, new Tuple<float, float>(3.75f, 6.25f));
        DailyPercentMinMax.Add(19, new Tuple<float, float>(3.75f, 6.25f));
        DailyPercentMinMax.Add(20, new Tuple<float, float>(3.75f, 6.25f));
        DailyPercentMinMax.Add(21, new Tuple<float, float>(3.75f, 6.25f));
        DailyPercentMinMax.Add(22, new Tuple<float, float>(1.875f, 3.125f));
        DailyPercentMinMax.Add(23, new Tuple<float, float>(1.875f, 3.125f));
    }
    
    private void InitSolarDailyCycleMinMax()
    {
        DailyPercentMinMax.Add(0, new Tuple<float, float>(0f, 0f));
        DailyPercentMinMax.Add(1, new Tuple<float, float>(0f, 0f));
        DailyPercentMinMax.Add(2, new Tuple<float, float>(0f, 0f));
        DailyPercentMinMax.Add(3, new Tuple<float, float>(0f, 0f));
        DailyPercentMinMax.Add(4, new Tuple<float, float>(0f, 0f));
        DailyPercentMinMax.Add(5, new Tuple<float, float>(0f, 0f));
        DailyPercentMinMax.Add(6, new Tuple<float, float>(0f, 2f));
        DailyPercentMinMax.Add(7, new Tuple<float, float>(0f, 4f));
        DailyPercentMinMax.Add(8, new Tuple<float, float>(0f, 6f));
        DailyPercentMinMax.Add(9, new Tuple<float, float>(0f, 8f));
        DailyPercentMinMax.Add(10, new Tuple<float, float>(0f, 10f));
        DailyPercentMinMax.Add(11, new Tuple<float, float>(0f, 13f));
        DailyPercentMinMax.Add(12, new Tuple<float, float>(0f, 14f));
        DailyPercentMinMax.Add(13, new Tuple<float, float>(0f, 13f));
        DailyPercentMinMax.Add(14, new Tuple<float, float>(0f, 10f));
        DailyPercentMinMax.Add(15, new Tuple<float, float>(0f, 8f));
        DailyPercentMinMax.Add(16, new Tuple<float, float>(0f, 6f));
        DailyPercentMinMax.Add(17, new Tuple<float, float>(0f, 4f));
        DailyPercentMinMax.Add(18, new Tuple<float, float>(0f, 2f));
        DailyPercentMinMax.Add(19, new Tuple<float, float>(0f, 0f));
        DailyPercentMinMax.Add(20, new Tuple<float, float>(0f, 0f));
        DailyPercentMinMax.Add(21, new Tuple<float, float>(0f, 0f));
        DailyPercentMinMax.Add(22, new Tuple<float, float>(0f, 0f));
        DailyPercentMinMax.Add(23, new Tuple<float, float>(0f, 0f));
    }
    
    private void InitWindDailyCycleMinMax()
    {
        for (int i = 0; i <= 23; i++)
        {
            DailyPercentMinMax.Add(i, new Tuple<float, float>(0f, 4.16f));
        }
    }

    public Dictionary<int, float> GetRealDailyValuePerHour(float desiredDailyValueCombined)
    {
        Dictionary<int, float> realDailyValuePerHour = new ();
        foreach (var minMax in DailyPercentMinMax)
        {
            realDailyValuePerHour.Add(
                minMax.Key,
                desiredDailyValueCombined * UnityEngine.Random.Range(minMax.Value.Item1, minMax.Value.Item2) / 100f
                );
        }
        return realDailyValuePerHour;
    }
}