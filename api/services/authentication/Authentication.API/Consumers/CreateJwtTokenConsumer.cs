using Authentication.API.Models;
using Authentication.API.Services.TokenGenerator;
using MassTransit;
using Microsoft.AspNetCore.Identity;
using Shared.ErrorHandling;
using Shared.MessageBus.Requests.CreateJwtToken;

namespace Authentication.API.Consumers;

public class CreateJwtTokenConsumer : IConsumer<CreateJwtTokenRequest>
{
    private readonly ITokenGenerator _tokenGenerator;
    private readonly UserManager<User> _userManager;

    public CreateJwtTokenConsumer(ITokenGenerator tokenGenerator, UserManager<User> userManager)
    {
        _tokenGenerator = tokenGenerator;
        _userManager = userManager;
    }

    public async Task Consume(ConsumeContext<CreateJwtTokenRequest> context)
    {
        var user = await _userManager.FindByEmailAsync(context.Message.UserEmail);
        if (user is null)
        {
            throw new InvalidOperationException();
        }

        var jwtTokenResult = await _tokenGenerator.GenerateJwtToken(user);

        await context.RespondAsync<CreateJwtTokenResponse>(new CreateJwtTokenResponse(jwtTokenResult.Data));
    }
}
