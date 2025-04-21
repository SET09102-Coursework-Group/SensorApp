using SensorApp.Shared.Models;

namespace SensorApp.Tests.TestHelpersForMap;

public class MeasurandFactory()
{
    public static MeasurandModel CreateMeasurand(int measurandId, string name, string unit, float min_safe_threshold, float max_safe_threshold)
    {
        return new MeasurandModel
            {
                Id = measurandId,
                Name = name,
                Unit = unit,
                Min_safe_threshold = min_safe_threshold,
                Max_safe_threshold = max_safe_threshold
        };
    }
}
