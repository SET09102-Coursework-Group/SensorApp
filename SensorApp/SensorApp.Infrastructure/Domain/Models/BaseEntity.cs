using System.ComponentModel.DataAnnotations;

namespace SensorApp.Infrastructure.Domain.Models;

public abstract class BaseEntity
{
    [Key]
    public int Id { get; set; }
}