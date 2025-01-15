using System.Security.Claims;
using Authentication.API.Models;
using MassTransit;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.JsonWebTokens;
using Shared;
using Shared.MessageBus.Events;

namespace Authentication.API.Consumers;

public class UserCreatedPreferencesConsumer : IConsumer<UserCreatedPreferences>
{
    private readonly UserManager<User> _userManager;
    
    public UserCreatedPreferencesConsumer(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public async Task Consume(ConsumeContext<UserCreatedPreferences> context)
    {
        var user = await _userManager.FindByEmailAsync(context.Message.UserEmail);
        if (user is null)
        {
            throw new ArgumentNullException("User was not found");
        }

        var claim = (await _userManager.GetClaimsAsync(user)).Single(x => x.Type == GlobalConstants.ApplicationClaimTypes.PreferencesConfigured);
        await _userManager.RemoveClaimAsync(user, claim);
        claim = new Claim(GlobalConstants.ApplicationClaimTypes.PreferencesConfigured, "true");
        await _userManager.AddClaimAsync(user, claim);
        Console.WriteLine("Claim added");
    }
}