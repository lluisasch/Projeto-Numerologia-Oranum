namespace Oranum.Domain.ValueObjects;

public sealed record BirthProfile(
    DateOnly BirthDate,
    string ZodiacSign,
    string Element,
    int LifePathNumber,
    string CentralEnergy,
    string SymbolicProfile,
    string Mission,
    IReadOnlyList<string> ChallengeHints,
    IReadOnlyList<string> PotentialHints);
