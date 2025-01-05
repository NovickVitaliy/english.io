using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Authentication.API.Models;
using Authentication.API.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Shared.ErrorHandling;
using JwtConstants = Microsoft.IdentityModel.JsonWebTokens.JwtConstants;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace Authentication.API.Services.TokenGenerator;

public class TokenGenerator : ITokenGenerator
{
    private readonly JwtSettings _jwtSettings;
    private readonly UserManager<User> _userManager;
    private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler;
    public TokenGenerator(UserManager<User> userManager, IOptions<JwtSettings> jwtSettingsOptions)
    {
        _userManager = userManager;
        _jwtSettings = jwtSettingsOptions.Value;
        _jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
    }

    public async Task<Result<string>> GenerateJwtToken(User user)
    {
        var claims = (await _userManager.GetClaimsAsync(user)).ToList();
        var roles = (await _userManager.GetRolesAsync(user)).ToList();
        
        claims.AddRange([
            new Claim(JwtRegisteredClaimNames.Email, user.Email!),
            new Claim(JwtRegisteredClaimNames.Name, user.UserName!),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Iss, _jwtSettings.Issuer),
            ..claims,
            ..roles.Select(r => new Claim(ClaimTypes.Role, r))
        ]);

        var signingCredentials = new SigningCredentials(
            _jwtSettings.GetSigninKey(),
            SecurityAlgorithms.HmacSha256);

        var dateIssued = DateTime.UtcNow;
        
        var token = _jwtSecurityTokenHandler.CreateJwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            subject: new ClaimsIdentity(claims),
            notBefore: dateIssued,
            expires: dateIssued.AddMinutes(_jwtSettings.LifetimeInMinutes),
            issuedAt: dateIssued,
            signingCredentials: signingCredentials);

        return Result<string>.Ok(_jwtSecurityTokenHandler.WriteToken(token));
    }
}