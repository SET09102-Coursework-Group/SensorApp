namespace SensorApp.Shared.Models;

/// <summary>
/// Provides a base class for all entities with a unique integer identifier.
/// Intended to be inherited by other domain models in the application.
/// </summary>
public abstract class BaseEntity
{
    public int Id { get; set; }
}