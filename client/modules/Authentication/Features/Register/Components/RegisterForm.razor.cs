using System.Text.Json;
using Authentication.Features.Register.Models;
using Microsoft.AspNetCore.Components;

namespace Authentication.Features.Register.Components;

public partial class RegisterForm : ComponentBase
{
    private readonly RegisterRequest _registerRequest = new RegisterRequest();
    private bool _isValid = true;

    private async Task Submit()
    {
        Console.WriteLine(JsonSerializer.Serialize(_registerRequest));
    }
}