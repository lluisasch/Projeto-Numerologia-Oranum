using Oranum.Domain.ValueObjects;

namespace Oranum.Application.Models;

public sealed record CompatibilityReadingContext(
    string Person1Name,
    string Person2Name,
    NumerologyProfile Profile1,
    NumerologyProfile Profile2,
    BirthProfile? BirthProfile1,
    BirthProfile? BirthProfile2,
    CompatibilityProfile CompatibilityProfile,
    IReadOnlyList<MysticKnowledgeNote> KnowledgeNotes);
