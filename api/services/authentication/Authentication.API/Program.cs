using System.Text;
using Authentication.API.Data;
using Authentication.API.Models;
using Authentication.API.Services.AuthService;
using Authentication.API.Services.TokenGenerator;
using Authentication.API.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOptions<JwtSettings>()
    .BindConfiguration(JwtSettings.ConfigurationKey)
    .ValidateDataAnnotations()
    .ValidateOnStart();

builder.Services.AddControllers();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var jwtSettings = builder.Configuration.GetSection(JwtSettings.ConfigurationKey).Get<JwtSettings>() ?? throw new InvalidOperationException("JWT settings are not configured.");
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidAudience = jwtSettings.Audience,
            ValidIssuer = jwtSettings.Issuer,
            ValidateAudience = true,
            ValidateIssuer = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret)),
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddIdentity<User, Role>()
    .AddEntityFrameworkStores<AuthDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddDbContext<AuthDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString(AuthDbContext.DefaultConnectionStringPosition));
});

builder.Services.AddScoped<ITokenGenerator, TokenGenerator>();
builder.Services.AddScoped<IAuthService, AuthService>();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();