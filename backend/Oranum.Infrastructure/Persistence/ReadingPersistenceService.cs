using Oranum.Application.Abstractions;
using Oranum.Domain.Entities;
using Oranum.Infrastructure.Data;

namespace Oranum.Infrastructure.Persistence;

public sealed class ReadingPersistenceService : IReadingPersistenceService
{
    private readonly OranumDbContext _dbContext;

    public ReadingPersistenceService(OranumDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task SaveNameReadingAsync(NameReadingRecord record, CancellationToken cancellationToken)
    {
        _dbContext.NameReadings.Add(record);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task SaveBirthDateReadingAsync(BirthDateReadingRecord record, CancellationToken cancellationToken)
    {
        _dbContext.BirthDateReadings.Add(record);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task SaveCompatibilityReadingAsync(CompatibilityReadingRecord record, CancellationToken cancellationToken)
    {
        _dbContext.CompatibilityReadings.Add(record);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
