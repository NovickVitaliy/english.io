using System.ComponentModel.DataAnnotations;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Shared.Authentication.Models;

public class JwtSettings
{
    public const string IssuerKey = "JWT_ISSUER";
    public const string AudienceKey = "JWT_AUDIENCE";
    public const string LifetimeInMinutesKey = "JWT_LIFETIME_IN_MINUTES";
    public const string SecretKey = "JWT_SECRET";
    
    [Required]
    public string Audience { get; init; } = null!;
    
    [Required]
    public string Issuer { get; init; } = null!;

    [Required]
    public string Secret { get; init; } = null!;
    
    
    [Required]
    public int LifetimeInMinutes { get; init; }

    public SecurityKey GetSigninKey()
    {
        return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Secret));
    }
}