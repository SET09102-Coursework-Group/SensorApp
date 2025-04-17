using System.ComponentModel.DataAnnotations;

namespace SensorApp.Shared.Models;

/// <summary>
/// Holds the configuration settings for JWT token generation.
/// </summary>
public class JwtSettings
{
    [Required]
    public string Key { get; set; } = string.Empty;

    [Required]
    public string Issuer { get; set; } = string.Empty;

    [Required]
    public string Audience { get; set; } = string.Empty;

    [Required]
    public int DurationInMinutes { get; set; }
}