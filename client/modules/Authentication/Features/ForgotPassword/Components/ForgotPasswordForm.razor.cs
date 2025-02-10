using Authentication.Features.ForgotPassword.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using MudBlazor;
using Shared.Options;

namespace Authentication.Features.ForgotPassword.Components;

public partial class ForgotPasswordForm : ComponentBase
{
    private ForgotPasswordRequest _forgotPasswordRequest = null!;
    private MudForm _form = null!;

    [Inject] private IOptions<ClientOptions> ClientOptions { get; init; } = null!;

    protected override void OnParametersSet()
    {
        _forgotPasswordRequest = new ForgotPasswordRequest()
        {
            ResetPasswordUrl = ClientOptions.Value.GetClientBaseUrl()
        };
    }

    private static Task Submit()
    {
        return Task.CompletedTask;
    }
}

