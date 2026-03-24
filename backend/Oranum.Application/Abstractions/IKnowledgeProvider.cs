using Oranum.Application.Models;

namespace Oranum.Application.Abstractions;

public interface IKnowledgeProvider
{
    Task<IReadOnlyList<MysticKnowledgeNote>> GetNotesAsync(string topic, CancellationToken cancellationToken);
}
