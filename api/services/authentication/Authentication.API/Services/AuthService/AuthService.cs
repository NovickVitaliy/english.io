using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Authentication.API.DTOs.Auth.Requests;
using Authentication.API.DTOs.Auth.Responses;
using Authentication.API.DTOs.Other;
using Authentication.API.Models;
using Authentication.API.Options;
using Authentication.API.Services.TokenGenerator;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using Shared;
using Shared.ErrorHandling;
using Shared.Services;

namespace Authentication.API.Services.AuthService;

public class AuthService : IAuthService
{
    private readonly ITokenGenerator _tokenGenerator;
    private readonly UserManager<User> _userManager;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ForgotPasswordOptions _forgotPasswordOptions;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly EmailVerificationOptions _emailVerificationOptions;

    public AuthService(ITokenGenerator tokenGenerator, UserManager<User> userManager, IHttpClientFactory httpClientFactory,
        IOptions<ForgotPasswordOptions> forgotPasswordOptions, IHttpContextAccessor httpContextAccessor, IOptions<EmailVerificationOptions> emailVerificationOptions)
    {
        _tokenGenerator = tokenGenerator;
        _userManager = userManager;
        _httpClientFactory = httpClientFactory;
        _httpContextAccessor = httpContextAccessor;
        _forgotPasswordOptions = forgotPasswordOptions.Value;
        _emailVerificationOptions = emailVerificationOptions.Value;
    }

    public async Task<Result<AuthResponse>> RegisterUser(RegisterUserRequest request)
    {
        var validationResult = request.IsValid();
        if (!validationResult.IsValid)
        {
            return Result<AuthResponse>.BadRequest(validationResult.ErrorMessage);
        }

        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user is not null)
        {
            return Result<AuthResponse>.BadRequest("User with such an email already exists.");
        }

        user = new User
        {
            Email = request.Email, UserName = request.Email
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
            new Claim(JwtRegisteredClaimNames.EmailVerified, "false")
        ]);
        var tokenResult = await _tokenGenerator.GenerateJwtToken(user);
        return Result<AuthResponse>.Ok(new AuthResponse(user.UserName, user.Email, [AuthConstants.Roles.User], tokenResult.Data, false));
    }

    public async Task<Result<AuthResponse>> LoginUser(LoginUserRequest request)
    {
        var validationResult = request.IsValid();
        if (!validationResult.IsValid)
        {
            return Result<AuthResponse>.BadRequest(validationResult.ErrorMessage);
        }

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
        var isEmailVerified = bool.Parse((await _userManager.GetClaimsAsync(user)).SingleOrDefault(x => x.Type == JwtRegisteredClaimNames.EmailVerified)!.Value);
        return Result<AuthResponse>.Ok(new AuthResponse(user.UserName!, user.Email!, roles.ToArray(), tokenResult.Data, isEmailVerified));
    }
    public async Task<Result<User>> ForgotPasswordAsync(ForgotPasswordRequest request)
    {
        var validationResult = request.IsValid();
        if (!validationResult.IsValid)
        {
            return Result<User>.BadRequest(validationResult.ErrorMessage);
        }

        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user is null)
        {
            return Result<User>.NotFound(request.Email);
        }

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        var path = QueryHelpers.AddQueryString(request.ResetPasswordUrl, new Dictionary<string, string?>()
        {
            ["token"] = token, ["email"] = request.Email
        });

        var body = GenerateMessageBody(request.Email, path);
        var subject = "Password reset";

        var sendEmailMessageRequest = new SendEmailMessageRequest(request.Email, request.Email, subject, "Html", body);

        var client = _httpClientFactory.CreateClient(SharedServicesConstants.NotificationHttpClientName);
        var response = await client.SendAsync(new HttpRequestMessage()
        {
            Method = HttpMethod.Post,
            RequestUri = new Uri("api/notifications/send-message", UriKind.Relative),
            Content = new StringContent(JsonSerializer.Serialize(sendEmailMessageRequest), Encoding.UTF8, "application/json")
        });

        return response.IsSuccessStatusCode
            ? Result<User>.Ok(user)
            : Result<User>.BadRequest(response.ReasonPhrase ?? "Error occured while sending message. Try later");
    }
    public async Task<Result<User>> ResetPasswordAsync(ResetPasswordRequest request)
    {
        var validationResult = request.IsValid();
        if (!validationResult.IsValid)
        {
            return Result<User>.BadRequest(validationResult.ErrorMessage);
        }

        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user is null)
        {
            return Result<User>.NotFound(request.Email);
        }

        var result = await _userManager.ResetPasswordAsync(user, request.ResetToken, request.NewPassword);
        if (result.Succeeded)
        {
            return Result<User>.Ok(user);
        }

        return Result<User>.BadRequest(result.Errors.First().Description);
    }

    public async Task<Result<bool>> ChangePasswordAsync(ChangePasswordRequest request)
    {
        var validationResult = request.IsValid();
        if (!validationResult.IsValid)
        {
            return Result<bool>.BadRequest(validationResult.ErrorMessage);
        }

        var email = _httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Email)?.Value;
        if (string.IsNullOrWhiteSpace(email))
        {
            return Result<bool>.BadRequest("Invalid request");
        }

        var user = await _userManager.FindByEmailAsync(email);
        if (user is null)
        {
            return Result<bool>.NotFound("User with given email was not found");
        }

        var result = await _userManager.ChangePasswordAsync(user, request.OldPassword, request.NewPassword);
        if (result.Succeeded)
        {
            return Result<bool>.Ok(true);
        }
        return Result<bool>.BadRequest(result.Errors.FirstOrDefault()?.Description ?? "Error Occured");
    }

    public async Task<Result<User>> SendVerifyingEmailMessageAsync(SendVerifyingEmailMessageRequest request)
    {
        var email = _httpContextAccessor.HttpContext!.User.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Email)!.Value;
        if (string.IsNullOrWhiteSpace(email))
        {
            return Result<User>.BadRequest("Invalid email");
        }

        var user = await _userManager.FindByEmailAsync(email);
        if (user is null)
        {
            return Result<User>.NotFound(email);
        }

        var subject = "Email Verification";
        var verifyEmailToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        var path = QueryHelpers.AddQueryString(request.VerifyEmailUrl.ToString(), "token", verifyEmailToken);
        var body = GenerateVerificationEmailMessageBody(email, path);
        var sendEmailMessageRequest = new SendEmailMessageRequest(email, email, subject, "Html", body);

        var client = _httpClientFactory.CreateClient(SharedServicesConstants.NotificationHttpClientName);
        var response = await client.SendAsync(new HttpRequestMessage()
        {
            Method = HttpMethod.Post,
            RequestUri = new Uri("api/notifications/send-message", UriKind.Relative),
            Content = new StringContent(JsonSerializer.Serialize(sendEmailMessageRequest), Encoding.UTF8, "application/json")
        });

        return response.IsSuccessStatusCode
            ? Result<User>.Ok(user)
            : Result<User>.BadRequest(response.ReasonPhrase ?? "Error occured");
    }

    public async Task<Result<string>> VerifyEmailAsync(VerifyEmailRequest request)
    {
        var validationResult = request.IsValid();
        if (!validationResult.IsValid)
        {
            return Result<string>.BadRequest(validationResult.ErrorMessage);
        }

        var email = _httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Email)?.Value;
        if (string.IsNullOrWhiteSpace(email))
        {
            return Result<string>.BadRequest("Invalid email");
        }

        var user = await _userManager.FindByEmailAsync(email);
        if (user is null)
        {
            return Result<string>.BadRequest("User is not found");
        }

        var result = await _userManager.ConfirmEmailAsync(user, request.Token);
        if (result.Succeeded)
        {
            var accessTokenResult = await _tokenGenerator.GenerateJwtToken(user);
            return Result<string>.Ok(accessTokenResult.Data);
        }

        return Result<string>.BadRequest(result.Errors.FirstOrDefault()?.Description ?? "Error occured");
    }

    private string GenerateVerificationEmailMessageBody(string email, string path)
    {
        return string.Format(_emailVerificationOptions.MessageTemplate, email, path);
    }

    private string GenerateMessageBody(string requestEmail, string path)
    {
        return string.Format(_forgotPasswordOptions.MessageTemplate, requestEmail, path);
    }
}
