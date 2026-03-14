using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SafeVault.Constants;

namespace SafeVault.Controllers;

public class DashboardController : ControllerBase
{
    public DashboardController() : base()
    {
    }

    [Authorize(policy: AuthConstants.AdminPolicy)]
    public IActionResult Admin()
    {
        return Ok("access granted");
    }

    [Authorize(policy: AuthConstants.ManagerPolicy)]
    public IActionResult Manager()
    {
        return Ok("access granted");
    }

    [Authorize]
    public IActionResult Employee()
    {
        return Ok("access granted");
    }

    public IActionResult Guest()
    {
        return Ok("access granted");
    }
}