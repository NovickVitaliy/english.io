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

    public string Audience { get; init; } = "english-io";

    public string Issuer { get; init; } = "english-io";

    public string Secret { get; init; } = "AB56EC03-8577-43B6-AD6E-84DE08B1A07D";

    public int LifetimeInMinutes { get; init; } = 10800;

    public SecurityKey GetSigninKey()
    {
        return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Secret));
    }
}
