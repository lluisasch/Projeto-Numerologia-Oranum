using Oranum.Domain.ValueObjects;

namespace Oranum.Application.Models;

public sealed record BirthDateReadingContext(
    string FullName,
    BirthProfile BirthProfile,
    IReadOnlyList<MysticKnowledgeNote> KnowledgeNotes);
