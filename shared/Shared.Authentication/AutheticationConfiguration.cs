using System.Text;
using DotNetEnv;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Shared.Authentication.Models;

namespace Shared.Authentication;

public static class AutheticationConfiguration
{
    public static IServiceCollection ConfigureJwtAuthentication(this IServiceCollection services)
    {
        Env.TraversePath().Load();

        services.AddSingleton<JwtSettings>(_ => new JwtSettings()
        {
            Audience = "english-io",
            Issuer = "english-io",
            Secret = "AB56EC03-8577-43B6-AD6E-84DE08B1A07D",
            LifetimeInMinutes = 10800,
        });

        services.AddAuthentication(options =>
            {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.MapInboundClaims = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidAudience = "english-io",
                    ValidIssuer = "english-io",
                    ValidateAudience = true,
                    ValidateIssuer = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("AB56EC03-8577-43B6-AD6E-84DE08B1A07D")),
                    ClockSkew = TimeSpan.Zero
                };

                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"];
                        var path = context.HttpContext.Request.Path;
                        if (!string.IsNullOrWhiteSpace(accessToken) &&
                            path.StartsWithSegments("/hubs", StringComparison.InvariantCulture))
                        {
                            context.Token = accessToken;
                        }

                        return Task.CompletedTask;
                    }
                };
            });

        return services;
    }
}
