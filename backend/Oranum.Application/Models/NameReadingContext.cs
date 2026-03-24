using Oranum.Domain.ValueObjects;

namespace Oranum.Application.Models;

public sealed record NameReadingContext(
    string FullName,
    NumerologyProfile Numerology,
    IReadOnlyList<MysticKnowledgeNote> KnowledgeNotes);
