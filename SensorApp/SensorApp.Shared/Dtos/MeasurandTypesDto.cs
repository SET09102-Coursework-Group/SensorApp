using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SensorApp.Shared.Dtos;

public class MeasurandTypesDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Unit { get; set; } = null!;
}