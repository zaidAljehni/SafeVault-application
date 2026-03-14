using System.ComponentModel.DataAnnotations;

namespace SafeVault.Models;

public class RegisterDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [EmailAddress]
    [Compare("Email")]
    public string EmailConfirm { get; set; }

    [Required]
    public string Password { get; set; }
    
    [Required]
    public string role { get; set; }
}