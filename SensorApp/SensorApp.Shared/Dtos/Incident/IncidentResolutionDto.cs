using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SensorApp.Shared.Dtos.Incident;

/// <summary>
/// Data Transfer Object (DTO) used for providing the resolution details of an incident.
/// This DTO contains the necessary information to mark an incident as resolved, including comments and resolution date.
/// </summary>
public class IncidentResolutionDto
{
    public required string ResolutionComments { get; set; }
    public required DateTime ResolutionDate { get; set; }
}
