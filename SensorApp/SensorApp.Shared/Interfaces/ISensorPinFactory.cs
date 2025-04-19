using Microsoft.Maui.Controls.Maps;
using SensorApp.Shared.Models;

namespace SensorApp.Shared.Interfaces;

public interface ISensorPinFactory
{
    Pin CreatePin(SensorModel sensor);
}