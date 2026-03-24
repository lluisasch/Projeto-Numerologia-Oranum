using Microsoft.EntityFrameworkCore;
using Oranum.Domain.Entities;

namespace Oranum.Infrastructure.Data;

public sealed class OranumDbContext : DbContext
{
    public OranumDbContext(DbContextOptions<OranumDbContext> options)
        : base(options)
    {
    }

    public DbSet<NameReadingRecord> NameReadings => Set<NameReadingRecord>();
    public DbSet<BirthDateReadingRecord> BirthDateReadings => Set<BirthDateReadingRecord>();
    public DbSet<CompatibilityReadingRecord> CompatibilityReadings => Set<CompatibilityReadingRecord>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(OranumDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
