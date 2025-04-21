namespace SensorApp.Shared.Models;

/// <summary>
/// Represents the information associated with a pin used for displaying sensors on a map.
/// </summary>
public class SensorPinInfo
{
    public string Label { get; set; }
    public string Address { get; set; }
    public float Latitude { get; set; }
    public float Longitude { get; set; }
}
