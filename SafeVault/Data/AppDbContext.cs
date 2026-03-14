using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace SafeVault.Data;

public class AppDbContext : IdentityDbContext
{
    public AppDbContext(DbContextOptions<IdentityDbContext> options) : base(options)
    {
    }
}