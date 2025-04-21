using SensorApp.Shared.Models;
using Microsoft.Maui.Controls.Maps;

namespace SensorApp.Maui.Interfaces;

/// <summary>
/// Interface defining the contract for a factory that creates map pins for sensors.
/// </summary>
public interface ISensorPinFactory
{
    /// <summary>
    /// Creates a map pin for the specified sensor.
    /// </summary>
    /// <param name="sensor">The sensor model that the pin represents.</param>
    /// <returns>A Pin object representing the sensor on the map.</returns>
    Pin CreatePin(SensorModel sensor);
}