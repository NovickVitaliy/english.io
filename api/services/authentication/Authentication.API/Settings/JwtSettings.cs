using System.ComponentModel.DataAnnotations;

namespace Authentication.API.Settings;

public class JwtSettings
{
    public const string ConfigurationKey = "JwtSettings";
    
    [Required]
    public string Audience { get; init; } = null!;
    
    [Required]
    public string Issuer { get; init; } = null!;

    [Required]
    public string Secret { get; init; } = null!;
    
    [Required]
    public int LifetimeInMinutes { get; init; }
}