using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Unicode;
using Microsoft.IdentityModel.Tokens;

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
    public string EncryptingSecret { get; init; } = null!;
    
    [Required]
    public int LifetimeInMinutes { get; init; }

    public SecurityKey GetSigninKey()
    {
        return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Secret));
    }
    
    public SecurityKey GetEcryptingKey()
    {
        return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(EncryptingSecret));
    }
}