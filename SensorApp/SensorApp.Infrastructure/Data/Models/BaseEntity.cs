using System.ComponentModel.DataAnnotations;

namespace SensorApp.Infrastructure.Data.Models;

public abstract class BaseEntity
{
    [Key]
    public int Id { get; set; }
}