using Authentication.Features.Login.Models;
using Microsoft.AspNetCore.Components;

namespace Authentication.Features.Login.Components;

public partial class LoginForm : ComponentBase
{
    private readonly LoginRequest _loginRequest = new LoginRequest();
    private bool _isValid;

    private async Task Submit()
    {
        if (_isValid)
        {
            
        }
        else
        {
            
        }
    }
}