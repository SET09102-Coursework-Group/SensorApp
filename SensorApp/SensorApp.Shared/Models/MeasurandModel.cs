namespace SensorApp.Shared.Models
{
    public class MeasurandModel : BaseEntity
    {
        public string Name { get; set; }
        public string Unit { get; set; }
        public float? Min_safe_threshold { get; set; }
        public float? Max_safe_threshold { get; set; }

        public ICollection<MeasurementModel> Measurements { get; set; }
    }
}