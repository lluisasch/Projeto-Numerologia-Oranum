namespace Oranum.Domain.ValueObjects;

public sealed record NumerologyProfile(
    string FullName,
    string NormalizedName,
    IReadOnlyList<int> LetterValues,
    int RawSum,
    int PrincipalNumber,
    string SymbolicMeaning,
    string PredominantArchetype,
    string EnergySignature,
    IReadOnlyList<string> StrengthHints,
    IReadOnlyList<string> ChallengeHints);
