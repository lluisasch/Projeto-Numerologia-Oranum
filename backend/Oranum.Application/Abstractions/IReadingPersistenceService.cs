using Oranum.Domain.Entities;

namespace Oranum.Application.Abstractions;

public interface IReadingPersistenceService
{
    Task SaveNameReadingAsync(NameReadingRecord record, CancellationToken cancellationToken);
    Task SaveBirthDateReadingAsync(BirthDateReadingRecord record, CancellationToken cancellationToken);
    Task SaveCompatibilityReadingAsync(CompatibilityReadingRecord record, CancellationToken cancellationToken);
}
