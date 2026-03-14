using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SafeVault.Models;
using SafeVault.Services;
using SafeVault.Utils;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace SafeVault.Controllers;

public class AuthController : Controller
{
    private UserManager<IdentityUser> _userManager;
    private SignInManager<IdentityUser> _signInManager;
    private RoleManager<IdentityRole> _roleManager;
    private TokenService _tokenService;
    private List<char> _allowedSpecialCharacters;

    public AuthController(
        UserManager<IdentityUser> userManager,
        SignInManager<IdentityUser> signInManager,
        RoleManager<IdentityRole> roleManager,
        TokenService tokenService
    ) : base()
    {
        this._userManager = userManager;
        this._signInManager = signInManager;
        this._roleManager = roleManager;
        this._tokenService = tokenService;
        this._allowedSpecialCharacters = new List<char>()
        {
            '!', '@', '#', '$', '%', '^', '&', '*', '(', ')', '-', '_', '=', '+', '?', '\\', '/', '|'
        };
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register([FromBody] RegisterDto dto)
    {
        if (!ModelState.IsValid)
        {
            return UnprocessableEntity("Invalid register data");
        }

        if (!this._sanitizeInput(dto.Email, false))
        {
            return UnprocessableEntity("Invalid characters in email");
        }

        if (!this._sanitizeInput(dto.Password, true))
        {
            return UnprocessableEntity("Invalid characters in password");
        }

        IdentityUser user = new IdentityUser()
        {
            Email = dto.Email,
            UserName = dto.Email,
        };
        IdentityResult result = await this._userManager.CreateAsync(user, dto.Password);
        if (result.Succeeded)
        {
            return Created($"/auth/details/{user.Id}", user);
        }

        return UnprocessableEntity(result.Errors);
    }

    [HttpPost]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        if (!ModelState.IsValid)
        {
            return UnprocessableEntity();
        }

        SignInResult result = await this._signInManager.PasswordSignInAsync(dto.Email, dto.Password, true, false);
        if (result.Succeeded)
        {
            IdentityUser user = await this._userManager.FindByEmailAsync(dto.Email);
            return Ok(user);
        }

        return Unauthorized();
    }

    [HttpPost]
    public async Task<IActionResult> Loginjwt([FromBody] LoginDto dto)
    {
        if (!ModelState.IsValid)
        {
            return UnprocessableEntity();
        }

        SignInResult result = await this._signInManager.PasswordSignInAsync(dto.Email, dto.Password, true, false);
        if (result.Succeeded)
        {
            IdentityUser? currentUser = await this._userManager.FindByEmailAsync(dto.Email);
            var roles = await this._userManager.GetRolesAsync(currentUser);
            var token = this._tokenService.GenerateToken(dto.Email, roles.ElementAt(0));
            return Ok(new { Token = token });
        }

        return Unauthorized("Invalid credentials");
    }

    protected bool _sanitizeInput(string input, bool WithAllowedSpecialCharacters = false)
    {
        return ValidationHelpers.IsValidInput(
                   input,
                   WithAllowedSpecialCharacters ? this._allowedSpecialCharacters : []
               ) &&
               ValidationHelpers.IsValidXssInput(input) &&
               ValidationHelpers.IsValidSqlInjectionInput(input);
    }
}