using Oranum.Application.DTOs.Requests;
using Oranum.Application.DTOs.Responses;

namespace Oranum.Application.Abstractions;

public interface IReadingService
{
    Task<NameReadingResponse> GenerateNameReadingAsync(NameReadingRequest request, CancellationToken cancellationToken);
    Task<BirthDateReadingResponse> GenerateBirthDateReadingAsync(BirthDateReadingRequest request, CancellationToken cancellationToken);
    Task<CompatibilityReadingResponse> GenerateCompatibilityReadingAsync(CompatibilityReadingRequest request, CancellationToken cancellationToken);
}
