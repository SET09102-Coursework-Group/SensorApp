using Microsoft.Maui.Controls.Maps;
using SensorApp.Shared.Models;

namespace SensorApp.Maui.Interfaces;

public interface ISensorPinFactory
{
    Pin CreatePin(SensorModel sensor);
}