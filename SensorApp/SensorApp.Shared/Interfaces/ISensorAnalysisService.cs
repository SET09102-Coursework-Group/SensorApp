using SensorApp.Shared.Models;

namespace SensorApp.Shared.Interfaces;

public interface ISensorAnalysisService
{
    Dictionary<int, MeasurementModel> GetLatestMeasurementsByType(SensorModel sensor);
    bool IsThresholdBreached(SensorModel sensor);
}