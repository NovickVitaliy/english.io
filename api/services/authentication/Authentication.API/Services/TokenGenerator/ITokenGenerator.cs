using Authentication.API.Models;
using Shared.ErrorHandling;

namespace Authentication.API.Services.TokenGenerator;

public interface ITokenGenerator
{
    Task<Result<string>> GenerateJwtToken(User user);
}