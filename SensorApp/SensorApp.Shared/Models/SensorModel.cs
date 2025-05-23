﻿namespace SensorApp.Shared.Models;

/// <summary>
/// Represents a sensor object with its details, including type, location, status, and associated measurements.
/// </summary>
public class SensorModel : BaseEntity
{
    public string Type { get; set; }
    public float Longitude { get; set; }
    public float Latitude { get; set; }
    public string? Site_zone { get; set; }
    public string Status { get; set; }
    public ICollection<MeasurementModel> Measurements { get; set; } = new List<MeasurementModel>();
}