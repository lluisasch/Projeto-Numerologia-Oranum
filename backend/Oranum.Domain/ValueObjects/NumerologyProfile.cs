namespace Oranum.Domain.ValueObjects;

public sealed record NumerologyProfile(
    string FullName,
    string NormalizedName,
    IReadOnlyList<int> LetterValues,
    int RawSum,
    int PrincipalNumber,
    int VowelNumber,
    int ConsonantNumber,
    int DominantNumber,
    int LetterCount,
    string InitialLetter,
    string NameCadence,
    string SymbolicLens,
    string SymbolicMeaning,
    string PredominantArchetype,
    string ArchetypeDescription,
    string EnergySignature,
    IReadOnlyList<string> StrengthHints,
    IReadOnlyList<string> ChallengeHints);