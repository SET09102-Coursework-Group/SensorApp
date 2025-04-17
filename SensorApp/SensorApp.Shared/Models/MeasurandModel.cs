using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SensorApp.Shared.Models;

namespace SensorApp.Maui.Models
{
    public class MeasurandModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public float? Min_safe_threshold { get; set; }
        public float? Max_safe_threshold { get; set; }

        public ICollection<MeasurementModel> Measurements { get; set; }
    }
}