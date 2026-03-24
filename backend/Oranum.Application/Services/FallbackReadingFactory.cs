using Oranum.Application.DTOs.Responses;
using Oranum.Application.Models;

namespace Oranum.Application.Services;

public static class FallbackReadingFactory
{
    public static NameReadingResponse CreateNameReading(NameReadingContext context) =>
        new(
            context.FullName,
            context.Numerology.PrincipalNumber,
            $"Vibração {context.Numerology.PrincipalNumber}: {context.Numerology.PredominantArchetype}",
            context.Numerology.EnergySignature,
            context.Numerology.PredominantArchetype,
            context.Numerology.SymbolicMeaning,
            context.Numerology.StrengthHints,
            context.Numerology.ChallengeHints,
            $"Na leitura simbólica do Oranum, o nome {context.FullName} se move por uma cadência {context.Numerology.NameCadence} e por uma lente de {context.Numerology.SymbolicLens}, sugerindo presença, sensibilidade e escolhas com significado.",
            $"Honre o número {context.Numerology.PrincipalNumber} com pequenos rituais de presença, escuta e intenção consciente.",
            $"Seu nome revela uma vibração ligada a {context.Numerology.SymbolicMeaning.ToLowerInvariant()} e encontra no arquétipo {context.Numerology.PredominantArchetype} uma forma clara de expressar essa força no cotidiano.");

    public static BirthDateReadingResponse CreateBirthReading(BirthDateReadingContext context) =>
        new(
            context.BirthProfile.BirthDate.ToString("yyyy-MM-dd"),
            context.BirthProfile.ZodiacSign,
            context.BirthProfile.Element,
            context.BirthProfile.LifePathNumber,
            context.BirthProfile.CentralEnergy,
            context.BirthProfile.SymbolicProfile,
            context.BirthProfile.Mission,
            context.BirthProfile.ChallengeHints,
            context.BirthProfile.PotentialHints,
            $"Permita que o signo {context.BirthProfile.ZodiacSign} e o caminho {context.BirthProfile.LifePathNumber} dialoguem com escolhas mais conscientes no cotidiano.");

    public static CompatibilityReadingResponse CreateCompatibilityReading(CompatibilityReadingContext context) =>
        new(
            context.Person1Name,
            context.Person2Name,
            context.CompatibilityProfile.CompatibilityScore,
            context.CompatibilityProfile.CompatibilityLevel,
            context.CompatibilityProfile.EnergeticAffinity,
            context.CompatibilityProfile.EmotionalAffinity,
            context.CompatibilityProfile.SpiritualAffinity,
            context.CompatibilityProfile.StrengthHints,
            context.CompatibilityProfile.AttentionHints,
            context.CompatibilityProfile.BalanceGuidance,
            $"{context.Person1Name} e {context.Person2Name} formam um vínculo de {context.CompatibilityProfile.RelationshipAxis.ToLowerInvariant()}, marcado por um encontro em que {context.CompatibilityProfile.EncounterTone.ToLowerInvariant()}.");
}