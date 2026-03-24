using Oranum.Domain.ValueObjects;

namespace Oranum.Domain.Services;

public sealed class CompatibilityCalculator
{
    public CompatibilityProfile Calculate(
        NumerologyProfile profile1,
        NumerologyProfile profile2,
        BirthProfile? birthProfile1,
        BirthProfile? birthProfile2)
    {
        var numerologyFactor = CalculateNumerologyFactor(profile1.PrincipalNumber, profile2.PrincipalNumber);
        var lifePathFactor = CalculateLifePathFactor(birthProfile1?.LifePathNumber, birthProfile2?.LifePathNumber);
        var elementalFactor = CalculateElementFactor(birthProfile1?.Element, birthProfile2?.Element);

        var weightedAverage = (numerologyFactor * 0.5m) + (lifePathFactor * 0.25m) + (elementalFactor * 0.25m);
        var score = (int)Math.Clamp(Math.Round(38m + (weightedAverage * 60m)), 38, 98);
        var level = ResolveLevel(score);

        return new CompatibilityProfile(
            score,
            level,
            ResolveEnergeticAffinity(score, profile1, profile2),
            ResolveEmotionalAffinity(score, birthProfile1, birthProfile2),
            ResolveSpiritualAffinity(score, profile1, birthProfile1, birthProfile2),
            ResolveStrengths(score, profile1, profile2, birthProfile1, birthProfile2),
            ResolveAttentionPoints(score, profile1, profile2, birthProfile1, birthProfile2),
            ResolveBalanceGuidance(profile1, profile2, birthProfile1, birthProfile2));
    }

    private static decimal CalculateNumerologyFactor(int number1, int number2)
    {
        var distance = Math.Abs(number1 - number2);
        var baseScore = 1m - (distance / 12m);

        if (number1 == number2)
        {
            baseScore += 0.18m;
        }

        if (number1 + number2 is 9 or 11 or 22)
        {
            baseScore += 0.08m;
        }

        return Math.Clamp(baseScore, 0.35m, 1m);
    }

    private static decimal CalculateLifePathFactor(int? lifePath1, int? lifePath2)
    {
        if (!lifePath1.HasValue || !lifePath2.HasValue)
        {
            return 0.64m;
        }

        return CalculateNumerologyFactor(lifePath1.Value, lifePath2.Value);
    }

    private static decimal CalculateElementFactor(string? element1, string? element2)
    {
        if (string.IsNullOrWhiteSpace(element1) || string.IsNullOrWhiteSpace(element2))
        {
            return 0.65m;
        }

        if (element1 == element2)
        {
            return 0.93m;
        }

        var pair = $"{element1}-{element2}";
        return pair switch
        {
            "Fogo-Ar" or "Ar-Fogo" => 0.88m,
            "Terra-Agua" or "Agua-Terra" => 0.87m,
            "Fogo-Terra" or "Terra-Fogo" => 0.72m,
            "Ar-Agua" or "Agua-Ar" => 0.69m,
            "Fogo-Agua" or "Agua-Fogo" => 0.54m,
            _ => 0.58m
        };
    }

    private static string ResolveLevel(int score) =>
        score switch
        {
            >= 88 => "Conexao rara",
            >= 75 => "Alta sintonia",
            >= 60 => "Compatibilidade promissora",
            >= 48 => "Atracao com aprendizados",
            _ => "Vinculo de contrastes transformadores"
        };

    private static string ResolveEnergeticAffinity(int score, NumerologyProfile profile1, NumerologyProfile profile2) =>
        score >= 75
            ? $"As vibracoes {profile1.PrincipalNumber} e {profile2.PrincipalNumber} criam um campo de ressonancia fluido, com troca energetica natural."
            : $"As vibracoes {profile1.PrincipalNumber} e {profile2.PrincipalNumber} revelam contraste fertil, capaz de gerar crescimento quando ha consciencia.";

    private static string ResolveEmotionalAffinity(int score, BirthProfile? birthProfile1, BirthProfile? birthProfile2)
    {
        if (birthProfile1 is null || birthProfile2 is null)
        {
            return "Sem as duas datas, a afinidade emocional se apresenta como intuicao inicial, observada mais pelo encontro das presencas do que por mapas completos.";
        }

        return score >= 70
            ? $"Os elementos {birthProfile1.Element.ToLowerInvariant()} e {birthProfile2.Element.ToLowerInvariant()} tendem a acolher as diferencas sem perder calor emocional."
            : $"Os elementos {birthProfile1.Element.ToLowerInvariant()} e {birthProfile2.Element.ToLowerInvariant()} pedem traducao afetiva para evitar ruidos e silencios acumulados.";
    }

    private static string ResolveSpiritualAffinity(int score, NumerologyProfile profile1, BirthProfile? birthProfile1, BirthProfile? birthProfile2)
    {
        var baseText = $"O numero {profile1.PrincipalNumber} traz um chamado de {profile1.SymbolicMeaning.ToLowerInvariant()}";

        if (birthProfile1 is null || birthProfile2 is null)
        {
            return $"{baseText}, e o vinculo sugere uma conexao espiritual percebida mais por simbolos e sincronicidades.";
        }

        return score >= 70
            ? $"{baseText}, enquanto os mapas indicam afinidade de proposito e fortalecimento mutuo."
            : $"{baseText}, e o encontro espiritual pode florescer quando ambos acolhem tempos e ritmos distintos.";
    }

    private static IReadOnlyList<string> ResolveStrengths(
        int score,
        NumerologyProfile profile1,
        NumerologyProfile profile2,
        BirthProfile? birthProfile1,
        BirthProfile? birthProfile2)
    {
        var strengths = new List<string>
        {
            $"o arquetipo {profile1.PredominantArchetype.ToLowerInvariant()} encontra eco no campo de {profile2.PredominantArchetype.ToLowerInvariant()}",
            score >= 70 ? "ha potencial de suporte mutuo em momentos de decisao" : "a relacao tende a despertar maturidade e autoconhecimento"
        };

        if (birthProfile1 is not null && birthProfile2 is not null)
        {
            strengths.Add($"a combinacao entre {birthProfile1.ZodiacSign.ToLowerInvariant()} e {birthProfile2.ZodiacSign.ToLowerInvariant()} amplia a leitura simbolica do encontro");
        }

        return strengths;
    }

    private static IReadOnlyList<string> ResolveAttentionPoints(
        int score,
        NumerologyProfile profile1,
        NumerologyProfile profile2,
        BirthProfile? birthProfile1,
        BirthProfile? birthProfile2)
    {
        var points = new List<string>
        {
            $"equilibrar os desafios de {profile1.ChallengeHints.First()} com {profile2.ChallengeHints.First()}",
            score >= 70 ? "evitar acomodacao em zonas de conforto energetico" : "traduzir expectativas antes que virem projecoes"
        };

        if (birthProfile1 is not null && birthProfile2 is not null && birthProfile1.Element != birthProfile2.Element)
        {
            points.Add("respeitar diferencas de ritmo emocional e de processamento interno");
        }

        return points;
    }

    private static string ResolveBalanceGuidance(
        NumerologyProfile profile1,
        NumerologyProfile profile2,
        BirthProfile? birthProfile1,
        BirthProfile? birthProfile2)
    {
        var elementalText = birthProfile1 is not null && birthProfile2 is not null
            ? $"Honrem a linguagem dos elementos {birthProfile1.Element.ToLowerInvariant()} e {birthProfile2.Element.ToLowerInvariant()}."
            : "Cultivem escuta e curiosidade para descobrir o ritmo real do vinculo.";

        return $"{elementalText} A chave de equilibrio esta em unir a iniciativa de {profile1.PredominantArchetype} com a sabedoria de {profile2.PredominantArchetype}.";
    }
}
