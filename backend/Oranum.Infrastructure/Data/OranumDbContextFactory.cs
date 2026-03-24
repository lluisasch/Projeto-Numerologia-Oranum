using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Oranum.Infrastructure.Data;

public sealed class OranumDbContextFactory : IDesignTimeDbContextFactory<OranumDbContext>
{
    public OranumDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<OranumDbContext>();
        var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection")
            ?? "Host=localhost;Port=5432;Database=oranum;Username=oranum;Password=oranum";

        optionsBuilder.UseNpgsql(connectionString, npgsqlOptions =>
        {
            npgsqlOptions.MigrationsAssembly(typeof(OranumDbContext).Assembly.FullName);
        });

        return new OranumDbContext(optionsBuilder.Options);
    }
}
