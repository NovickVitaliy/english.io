using System.Security.Claims;
using Authentication.API.DTOs.Auth.Requests;
using Authentication.API.DTOs.Auth.Responses;
using Authentication.API.Models;
using Authentication.API.Services.TokenGenerator;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Shared;
using Shared.ErrorHandling;

namespace Authentication.API.Services.AuthService;

public class AuthService : IAuthService
{
    private readonly ITokenGenerator _tokenGenerator;
    private readonly UserManager<User> _userManager;

    public AuthService(ITokenGenerator tokenGenerator, UserManager<User> userManager)
    {
        _tokenGenerator = tokenGenerator;
        _userManager = userManager;
    }

    public async Task<Result<AuthResponse>> RegisterUser(RegisterUserRequest request)
    {
        //TODO: add validation

        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user is not null)
        {
            return Result<AuthResponse>.BadRequest("User with such an email already exists.");
        }

        user = new User
        {
            Email = request.Email,
            UserName = request.Email
        };

        var result = await _userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
        {
            return Result<AuthResponse>.BadRequest(result.Errors.First().Description);
        }
        await _userManager.AddToRoleAsync(user, AuthConstants.Roles.User);
        await _userManager.AddClaimsAsync(user, [
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(GlobalConstants.ApplicationClaimTypes.PreferencesConfigured, "false"),
        ]);
        var tokenResult = await _tokenGenerator.GenerateJwtToken(user);
        return Result<AuthResponse>.Ok(new AuthResponse(user.UserName, user.Email, [AuthConstants.Roles.User], tokenResult.Data));
    }

    public async Task<Result<AuthResponse>> LoginUser(LoginUserRequest request)
    {
        //TODO: add validation

        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user is null)
        {
            return Result<AuthResponse>.BadRequest("User with such an email does not exist.");
        }

        var passwordMatch = await _userManager.CheckPasswordAsync(user, request.Password);
        if (!passwordMatch)
        {
            return Result<AuthResponse>.BadRequest("Incorrect password");
        }

        var tokenResult = await _tokenGenerator.GenerateJwtToken(user);
        var roles = await _userManager.GetRolesAsync(user);
        return Result<AuthResponse>.Ok(new AuthResponse(user.UserName!, user.Email!, roles.ToArray(), tokenResult.Data));
    }
    public async Task<Result<User>> ForgotPasswordAsync(ForgotPasswordRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user is null)
        {
            return Result<User>.NotFound(request.Email);
        }

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        var path = QueryHelpers.AddQueryString(request.ResetPasswordUrl, new Dictionary<string, string?>()
        {
            ["token"] = token,
            ["email"] = request.Email
        });

        //TODO: send to the notification api

        return Result<User>.Ok(user);
    }
}
