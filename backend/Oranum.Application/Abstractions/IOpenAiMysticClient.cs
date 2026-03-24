using Oranum.Application.Models;

namespace Oranum.Application.Abstractions;

public interface IOpenAiMysticClient
{
    Task<AiStructuredResult<T>> GenerateJsonAsync<T>(string systemPrompt, string userPrompt, CancellationToken cancellationToken);
}
