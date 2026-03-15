using System.ComponentModel.DataAnnotations;

namespace SafeVault.Models;

public class RoleDto
{
    [Required]
    public string name { get; set; }
}