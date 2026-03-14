using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using SafeVault.Constants;
using SafeVault.Data;
using SafeVault.Services;
using SafeVault.Tests;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppDbContext>();
builder.Services.AddControllers();
builder.Services.AddSingleton<TestInputValidation>();
builder.Services.AddSingleton<TokenService>();

var jwtSettings = builder.Configuration.GetSection("JwtSettings") ?? throw new Exception("JwtSettings not configured");
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer("JwtBearer", options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Secret"]))
        };
    });

// Add Identity
builder.Services
    .AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>();

// Use the bcrypt hasher whenever Identity hashes passwords for IdentityUser
builder.Services
    .AddScoped<IPasswordHasher<IdentityUser>, BCryptPasswordHasher<IdentityUser>>();

builder.Services.AddAuthorizationBuilder()
    .AddPolicy(
        AuthConstants.AdminPolicy,
        policy => policy.RequireRole("admin")
    )
    .AddPolicy(
        AuthConstants.ManagerPolicy,
        policy => policy.RequireRole("manager")
    );

var app = builder.Build();

app.UseRouting();
app.UseAuthorization();
app.MapControllers();

app.MapGet("/", () => "Hello World!");

app.Run();