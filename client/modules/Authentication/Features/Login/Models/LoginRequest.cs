using System.ComponentModel.DataAnnotations;

namespace Authentication.Features.Login.Models;

public class LoginRequest
{
    [EmailAddress(ErrorMessage = "Email should be in valid format")]
    [Required(ErrorMessage = "Email is required")]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "Password is required")]
    public string Password { get; set; } = null!;
}