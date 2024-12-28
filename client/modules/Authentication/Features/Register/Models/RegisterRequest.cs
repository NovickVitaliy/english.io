using System.ComponentModel.DataAnnotations;

namespace Authentication.Features.Register.Models;

public class RegisterRequest
{
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Email address should be in valid format")] 
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "Password is required")]
    public string Password { get; set; } = null!;

    [Required]
    [Compare(nameof(Password), ErrorMessage = "Confirm password should match password")]
    public string ConfirmPassword { get; set; } = null!;
}