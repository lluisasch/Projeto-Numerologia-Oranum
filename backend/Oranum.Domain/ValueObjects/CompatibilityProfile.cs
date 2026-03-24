namespace Oranum.Domain.ValueObjects;

public sealed record CompatibilityProfile(
    int CompatibilityScore,
    string CompatibilityLevel,
    string EnergeticAffinity,
    string EmotionalAffinity,
    string SpiritualAffinity,
    IReadOnlyList<string> StrengthHints,
    IReadOnlyList<string> AttentionHints,
    string BalanceGuidance,
    string RelationshipAxis,
    string ArchetypeDynamic,
    string NumberDynamic,
    string ElementalDynamic,
    string EncounterTone,
    string ConflictPattern);