using Authentication.API.DTOs.Auth.Requests;
using Authentication.API.DTOs.Auth.Responses;
using Authentication.API.Models;
using Authentication.API.Services.TokenGenerator;
using Microsoft.AspNetCore.Identity;
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
}