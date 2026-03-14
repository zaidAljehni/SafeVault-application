using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace SafeVault.Data;

public class AppDbContext : IdentityDbContext
{
    private IConfiguration _configuration;

    public AppDbContext(DbContextOptions<IdentityDbContext> options, IConfiguration configuration) : base(options)
    {
        this._configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        string server = this._configuration.GetSection("Db:Server").Value ??
                        throw new Exception("Db:Server not configured");
        string db = this._configuration.GetSection("Db:Database").Value ??
                    throw new Exception("Db:Database not configured");
        string user = this._configuration.GetSection("Db:User").Value ?? throw new Exception("Db:User not configured");
        string password = this._configuration.GetSection("Db:Password").Value ??
                          throw new Exception("Db:Password not configured");
        var connectionString = $"Server={server};Database={db};User={user};Password={password};";
        optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
    }
}