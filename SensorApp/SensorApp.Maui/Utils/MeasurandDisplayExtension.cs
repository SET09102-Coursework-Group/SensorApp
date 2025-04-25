using SensorApp.Shared.Models;

namespace SensorApp.Maui.Utils;

public class MeasurandDisplayExtension
{
    public MeasurandModel Model { get; }
    public string DisplayName => string.IsNullOrWhiteSpace(Model.Unit) ? Model.Name : $"{Model.Name} ({Model.Unit})";

    public MeasurandDisplayExtension(MeasurandModel model)
    {
        Model = model;
    }
}

