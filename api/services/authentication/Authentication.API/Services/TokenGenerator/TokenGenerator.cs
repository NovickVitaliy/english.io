using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Authentication.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Shared.Authentication.Models;
using Shared.ErrorHandling;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace Authentication.API.Services.TokenGenerator;

public class TokenGenerator : ITokenGenerator
{
    private readonly JwtSettings _jwtSettings;
    private readonly UserManager<User> _userManager;
    private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler;
    public TokenGenerator(UserManager<User> userManager, JwtSettings jwtSettings)
    {
        _userManager = userManager;
        _jwtSettings = jwtSettings;
        _jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
    }

    public async Task<Result<string>> GenerateJwtToken(User user)
    {
        var claims = (await _userManager.GetClaimsAsync(user)).ToList();
        var roles = (await _userManager.GetRolesAsync(user)).ToList();

        claims.AddRange([
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Iss, _jwtSettings.Issuer),
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
