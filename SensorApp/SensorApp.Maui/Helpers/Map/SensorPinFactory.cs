using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;
using SensorApp.Shared.Models;
using SensorApp.Maui.Interfaces;
using SensorApp.Shared.Services;
using SensorApp.Shared.Interfaces;

namespace SensorApp.Maui.Helpers.Map;

/// <summary>
/// Factory class responsible for creating map pins for sensors.
/// Implements the <see cref="ISensorPinInfoFactory"/> interface.
/// This factory is specifically responsible for creating the UI pin elements on an interactive map.
/// </summary>
public class SensorPinFactory : ISensorPinFactory
{
    private readonly ISensorPinInfoFactory _pinInfoFactory;

    /// <summary>
    /// Initializes a new instance of the SensorPinFactory.
    /// </summary>
    /// <param name="pinInfoFactory">Takes an <see cref="ISensorPinInfoFactory"/> for creating the information needed for the pin.</param>
    public SensorPinFactory(ISensorPinInfoFactory pinInfoFactory)
    {
        _pinInfoFactory = pinInfoFactory;
    }

    /// <summary>
    /// Creates a map pin for the specified sensor.
    /// </summary>
    /// <param name="sensor">The sensor model that the pin represents.</param>
    /// <returns>A new Pin object representing the sensor on the map.</returns>
    public Pin CreatePin(SensorModel sensor)
    {
        var info = _pinInfoFactory.CreatePinInfo(sensor);
        return new Pin
        {
            Label = info.Label,
            Address = info.Address,
            Type = PinType.Place,
            Location = new Location(info.Latitude, info.Longitude)
        };
    }
}