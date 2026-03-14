using System.ComponentModel.DataAnnotations;

namespace SafeVault.Models;

public class LoginDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    [Required]
    public string Password { get; set; }
    [Required]
    public bool RememberMe { get; set; }
}