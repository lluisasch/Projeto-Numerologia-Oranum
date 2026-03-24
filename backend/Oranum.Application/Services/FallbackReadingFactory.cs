using Oranum.Application.DTOs.Responses;
using Oranum.Application.Models;

namespace Oranum.Application.Services;

public static class FallbackReadingFactory
{
    public static NameReadingResponse CreateNameReading(NameReadingContext context) =>
        new(
            context.FullName,
            context.Numerology.PrincipalNumber,
            $"Vibracao {context.Numerology.PrincipalNumber}: {context.Numerology.PredominantArchetype}",
            context.Numerology.EnergySignature,
            context.Numerology.PredominantArchetype,
            context.Numerology.SymbolicMeaning,
            context.Numerology.StrengthHints,
            context.Numerology.ChallengeHints,
            $"Na leitura simbolica do Oranum, o nome {context.FullName} ecoa como um campo de {context.Numerology.PredominantArchetype.ToLowerInvariant()}, sugerindo sensibilidade para sinais, ciclos e escolhas com significado.",
            $"Honre o numero {context.Numerology.PrincipalNumber} com pequenos rituais de presenca, escuta e intencao consciente.",
            $"Seu nome revela uma vibracao de {context.Numerology.SymbolicMeaning.ToLowerInvariant()} e pede que voce traduza essa energia em atitudes alinhadas com a propria essencia.");

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
            $"O encontro entre {context.Person1Name} e {context.Person2Name} traz uma alquimia simbolica de {context.CompatibilityProfile.CompatibilityLevel.ToLowerInvariant()}, com espaco tanto para encanto quanto para amadurecimento.");
}
