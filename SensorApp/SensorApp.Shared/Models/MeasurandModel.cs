namespace SensorApp.Shared.Models
{
    /// <summary>
    /// Represents an object of type measurand, which is a physical quantity or property measured by sensors.
    /// </summary>
    public class MeasurandModel : BaseEntity
    {
        public string Name { get; set; }
        public string Unit { get; set; }
        public float? Min_safe_threshold { get; set; }
        public float? Max_safe_threshold { get; set; }

        public string DisplayName => string.IsNullOrWhiteSpace(Unit) ? Name : $"{Name} ({Unit})";
    }
}