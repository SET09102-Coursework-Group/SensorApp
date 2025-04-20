using SensorApp.Shared.Models;
using Microsoft.Maui.Controls.Maps;

namespace SensorApp.Maui.Interfaces;

public interface ISensorPinFactory
{
    Pin CreatePin(SensorModel sensor);
}