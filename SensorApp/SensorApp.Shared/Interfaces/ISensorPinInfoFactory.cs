using SensorApp.Shared.Models;

namespace SensorApp.Shared.Interfaces;

public interface ISensorPinInfoFactory
{
    SensorPinInfo CreatePinInfo(SensorModel sensor);
}