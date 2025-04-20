using SensorApp.Shared.Models;

namespace SensorApp.Shared.Interfaces;

/// <summary>
/// Interface for creating <see cref="SensorPinInfo"/> objects for a given sensor.
/// This is used to generate the necessary information for displaying sensor pins on a map.
/// </summary>
public interface ISensorPinInfoFactory
{
    /// <summary>
    /// Creates a <see cref="SensorPinInfo"/> based on the provided sensor data.
    /// </summary>
    /// <param name="sensor">The sensor for which the pin information is created.</param>
    /// <returns>A <see cref="SensorPinInfo"/> object containing the pin's label, location, and other relevant info.</returns>
    SensorPinInfo CreatePinInfo(SensorModel sensor);
}