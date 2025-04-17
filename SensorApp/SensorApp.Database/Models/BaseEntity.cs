using System.ComponentModel.DataAnnotations;

namespace SensorApp.Database.Models;

/// <summary>
/// Abstract base class that other domain entities inherit from.
/// Provides a primary key Id for each entity.
/// </summary>
public abstract class BaseEntity
{
    [Key]
    public int Id { get; set; }
}