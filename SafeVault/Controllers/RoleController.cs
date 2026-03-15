using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SafeVault.Models;

namespace SafeVault.Controllers;

public class RoleController : ControllerBase
{
    private RoleManager<IdentityRole> _roleManager;

    public RoleController(RoleManager<IdentityRole> roleManager) : base()
    {
        this._roleManager = roleManager;
    }

    [HttpGet]
    public IActionResult Index(int limit = 3, int offset = 0)
    {
        List<IdentityRole> roles = this._roleManager.Roles.Take(limit).Skip(offset).ToList();
        return Ok(roles);
    }

    [HttpGet]
    public IActionResult Details([FromQuery] string name)
    {
        if (name == null)
        {
            return BadRequest("Role is missing");
        }

        IdentityRole? role = this._roleManager.Roles
            .ToList()
            .Find(role => String.Equals(role.Name.ToLower(), name.ToLower()));
        if (role == null)
        {
            return NotFound("Role not found");
        }

        return Ok(role);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] RoleDto dto)
    {
        if (!ModelState.IsValid)
        {
            return UnprocessableEntity();
        }

        IdentityRole role = new IdentityRole() { Name = dto.name.ToLower() };

        var result = await this._roleManager.CreateAsync(role);
        if (result.Succeeded)
        {
            return Created();
        }

        return UnprocessableEntity();
    }
}