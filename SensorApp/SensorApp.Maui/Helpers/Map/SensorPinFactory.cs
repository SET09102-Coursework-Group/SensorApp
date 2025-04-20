using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;
using SensorApp.Shared.Models;
using SensorApp.Maui.Interfaces;
using SensorApp.Shared.Services;
using SensorApp.Shared.Interfaces;

namespace SensorApp.Maui.Helpers.Map;

public class SensorPinFactory : ISensorPinFactory
{
    private readonly ISensorPinInfoFactory _pinInfoFactory;

    public SensorPinFactory(ISensorPinInfoFactory pinInfoFactory)
    {
        _pinInfoFactory = pinInfoFactory;
    }

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