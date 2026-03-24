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
        var score = (int)Math.Clamp(Math.Round(38m + (weightedAverage * 60m)), 38m, 98m);
        var level = ResolveLevel(score);
        var relationshipAxis = ResolveRelationshipAxis(score, profile1, profile2, birthProfile1, birthProfile2);
        var archetypeDynamic = ResolveArchetypeDynamic(profile1, profile2);
        var numberDynamic = ResolveNumberDynamic(profile1, profile2);
        var elementalDynamic = ResolveElementalDynamic(birthProfile1, birthProfile2);
        var encounterTone = ResolveEncounterTone(score, profile1, profile2, birthProfile1, birthProfile2);
        var conflictPattern = ResolveConflictPattern(profile1, profile2, birthProfile1, birthProfile2);

        return new CompatibilityProfile(
            score,
            level,
            ResolveEnergeticAffinity(score, profile1, profile2, numberDynamic, encounterTone),
            ResolveEmotionalAffinity(score, birthProfile1, birthProfile2, elementalDynamic, conflictPattern),
            ResolveSpiritualAffinity(profile1, profile2, birthProfile1, birthProfile2, relationshipAxis, archetypeDynamic),
            ResolveStrengths(score, profile1, profile2, birthProfile1, birthProfile2, archetypeDynamic, numberDynamic, elementalDynamic),
            ResolveAttentionPoints(score, profile1, profile2, birthProfile1, birthProfile2, conflictPattern),
            ResolveBalanceGuidance(profile1, profile2, birthProfile1, birthProfile2, relationshipAxis, conflictPattern),
            relationshipAxis,
            archetypeDynamic,
            numberDynamic,
            elementalDynamic,
            encounterTone,
            conflictPattern);
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

        var pair = BuildPairKey(element1, element2);
        return pair switch
        {
            "Ar-Fogo" => 0.88m,
            "Terra-Água" => 0.87m,
            "Fogo-Terra" => 0.72m,
            "Ar-Água" => 0.69m,
            "Fogo-Água" => 0.54m,
            _ => 0.58m
        };
    }

    private static string ResolveLevel(int score) =>
        score switch
        {
            >= 88 => "Conexão rara",
            >= 75 => "Alta sintonia",
            >= 60 => "Compatibilidade promissora",
            >= 48 => "Atração com aprendizados",
            _ => "Encontro de contrastes transformadores"
        };

    private static string ResolveRelationshipAxis(int score, NumerologyProfile profile1, NumerologyProfile profile2, BirthProfile? birthProfile1, BirthProfile? birthProfile2)
    {
        if (profile1.PredominantArchetype == profile2.PredominantArchetype || profile1.PrincipalNumber == profile2.PrincipalNumber)
        {
            return "Espelho e reconhecimento";
        }

        if (score >= 82)
        {
            return "Complemento que amplia";
        }

        if (birthProfile1 is not null && birthProfile2 is not null && birthProfile1.Element != birthProfile2.Element)
        {
            return "Diferença que faz crescer";
        }

        return score >= 62
            ? "Troca viva e aprendizado"
            : "Aprendizado pelo contraste";
    }

    private static string ResolveArchetypeDynamic(NumerologyProfile profile1, NumerologyProfile profile2)
    {
        if (profile1.PredominantArchetype == profile2.PredominantArchetype)
        {
            return profile1.PredominantArchetype switch
            {
                "Herói" => "Os dois têm força de iniciativa e podem disputar o volante da relação.",
                "Cuidador" => "Os dois cuidam com facilidade, mas podem carregar peso demais sem perceber.",
                "Criador" => "Os dois alimentam imaginação, encanto e vontade de sair do óbvio.",
                "Governante" => "Os dois gostam de direção clara, o que fortalece a construção, mas pode gerar disputa por comando.",
                "Explorador" => "Os dois precisam de espaço, novidade e movimento para se sentirem vivos no vínculo.",
                "Amante" => "Os dois valorizam afeto, intensidade e presença emocional.",
                "Sábio" => "Os dois observam muito antes de se abrir, o que torna a conexão profunda, porém mais lenta.",
                "Mago" => "Os dois captam sutilezas e costumam sentir que a relação mexe com algo invisível.",
                "Inocente" => "Os dois procuram leveza, paz e uma sensação de abrigo no encontro.",
                _ => "Os dois desafiam padrões e podem provocar mudanças fortes um no outro."
            };
        }

        var pair = BuildPairKey(profile1.PredominantArchetype, profile2.PredominantArchetype);
        return pair switch
        {
            "Amante-Cuidador" => "Há afeto, acolhimento e vontade de construir intimidade com delicadeza.",
            "Criador-Mago" => "Imaginação e transformação se encontram com sensação de encantamento.",
            "Governante-Herói" => "Direção e coragem formam um par de ação, presença e conquista.",
            "Explorador-Rebelde" => "Liberdade e ruptura criam um vínculo inquieto, intenso e pouco previsível.",
            "Inocente-Cuidador" => "Leveza e proteção criam uma atmosfera de abrigo emocional.",
            "Mago-Sábio" => "Sensibilidade simbólica e profundidade mental criam um encontro mais silencioso e profundo.",
            "Criador-Governante" => "Imaginação e forma pedem equilíbrio entre inspiração e realidade.",
            "Amante-Herói" => "Calor afetivo e impulso de conquista deixam o encontro magnético e direto.",
            "Cuidador-Sábio" => "Escuta e discernimento constroem uma conexão calma, inteligente e gradual.",
            "Amante-Explorador" => "Desejo de vínculo e gosto pelo novo trazem fascínio, movimento e descoberta.",
            _ => $"O arquétipo {profile1.PredominantArchetype} encontra o arquétipo {profile2.PredominantArchetype} em uma diferença que pode se complementar."
        };
    }

    private static string ResolveNumberDynamic(NumerologyProfile profile1, NumerologyProfile profile2)
    {
        var distance = Math.Abs(profile1.PrincipalNumber - profile2.PrincipalNumber);

        if (profile1.PrincipalNumber == profile2.PrincipalNumber)
        {
            return $"Os dois nomes vibram no número {profile1.PrincipalNumber}, então é fácil reconhecer no outro reações e impulsos parecidos.";
        }

        if (profile1.PrincipalNumber + profile2.PrincipalNumber is 9 or 11 or 22)
        {
            return $"Os números {profile1.PrincipalNumber} e {profile2.PrincipalNumber} criam uma sensação de encontro marcante, como se um despertasse algo importante no outro.";
        }

        if (distance <= 2)
        {
            return $"Os números {profile1.PrincipalNumber} e {profile2.PrincipalNumber} caminham em ritmos próximos, favorecendo entendimento rápido e sintonia de ritmo.";
        }

        if (distance >= 5)
        {
            return $"Os números {profile1.PrincipalNumber} e {profile2.PrincipalNumber} vivem em ritmos bem diferentes, o que pode gerar fascínio, mas também exigir mais conversa.";
        }

        return $"Os números {profile1.PrincipalNumber} e {profile2.PrincipalNumber} mostram diferenças moderadas, com espaço para troca desde que cada um respeite o jeito do outro.";
    }

    private static string ResolveElementalDynamic(BirthProfile? birthProfile1, BirthProfile? birthProfile2)
    {
        if (birthProfile1 is null || birthProfile2 is null)
        {
            return "Sem as duas datas, essa parte aparece mais no jeito como o encontro é sentido do que em uma leitura mais completa.";
        }

        if (birthProfile1.Element == birthProfile2.Element)
        {
            return $"Os dois compartilham o elemento {birthProfile1.Element.ToLowerInvariant()}, o que facilita reconhecer o jeito do outro de sentir e reagir.";
        }

        var pair = BuildPairKey(birthProfile1.Element, birthProfile2.Element);
        return pair switch
        {
            "Ar-Fogo" => "Ar e fogo criam uma mistura viva, estimulante e espontânea, com impulso e resposta rápida.",
            "Terra-Água" => "Terra e água favorecem cuidado, aprofundamento e uma construção emocional mais constante.",
            "Fogo-Terra" => "Fogo e terra juntam iniciativa com estabilidade, mas precisam ajustar velocidade e expectativa.",
            "Ar-Água" => "Ar e água pedem mais clareza emocional para que razão e sensibilidade não falem línguas diferentes.",
            "Fogo-Água" => "Fogo e água costumam viver atração intensa com oscilações emocionais, o que pede mais maturidade no vínculo.",
            _ => $"{birthProfile1.Element} e {birthProfile2.Element} criam uma combinação menos óbvia, que cresce quando há curiosidade real sobre o ritmo do outro."
        };
    }

    private static string ResolveEncounterTone(int score, NumerologyProfile profile1, NumerologyProfile profile2, BirthProfile? birthProfile1, BirthProfile? birthProfile2)
    {
        var pair = BuildPairKey(profile1.PredominantArchetype, profile2.PredominantArchetype);

        if (pair is "Explorador-Rebelde" or "Amante-Herói")
        {
            return "O encontro tende a ser rápido, magnético e cheio de impulso.";
        }

        if (pair is "Amante-Cuidador" or "Inocente-Cuidador")
        {
            return "O encontro tende a ser envolvente, afetivo e de aproximação delicada.";
        }

        if (pair is "Mago-Sábio" or "Cuidador-Sábio")
        {
            return "O encontro tende a ser mais silencioso, observador e profundo.";
        }

        if (birthProfile1?.Element == "Fogo" || birthProfile2?.Element == "Fogo")
        {
            return score >= 70
                ? "O encontro costuma chegar com calor, presença e faísca inicial."
                : "O encontro costuma ser intenso, mas pode responder rápido demais em momentos delicados.";
        }

        return score >= 70
            ? "O encontro costuma nascer com naturalidade e abrir espaço para confiança."
            : "O encontro costuma misturar curiosidade, provocação e necessidade de ajuste.";
    }

    private static string ResolveConflictPattern(NumerologyProfile profile1, NumerologyProfile profile2, BirthProfile? birthProfile1, BirthProfile? birthProfile2)
    {
        var pair = BuildPairKey(profile1.PredominantArchetype, profile2.PredominantArchetype);

        if (pair == "Governante-Rebelde")
        {
            return "o choque entre controle e autonomia";
        }

        if (pair == "Amante-Sábio")
        {
            return "a diferença entre quem precisa de calor imediato e quem precisa de tempo para se abrir";
        }

        if (pair == "Cuidador-Explorador")
        {
            return "a diferença entre proteger o vínculo e manter espaço para respirar";
        }

        if (pair == "Herói-Cuidador")
        {
            return "o risco de confundir iniciativa com cobrança ou excesso de cuidado";
        }

        if (birthProfile1 is not null && birthProfile2 is not null && BuildPairKey(birthProfile1.Element, birthProfile2.Element) == "Fogo-Água")
        {
            return "a distância entre intensidade imediata e sensibilidade profunda";
        }

        if (profile1.PredominantArchetype == profile2.PredominantArchetype)
        {
            return "a tendência de repetir a mesma sombra dos dois lados";
        }

        return "a diferença de ritmo emocional e de expectativa";
    }

    private static string ResolveEnergeticAffinity(int score, NumerologyProfile profile1, NumerologyProfile profile2, string numberDynamic, string encounterTone)
    {
        var prefix = score >= 75
            ? $"A química entre {profile1.FullName} e {profile2.FullName} tende a nascer com facilidade."
            : $"A química entre {profile1.FullName} e {profile2.FullName} existe, mas não é linear.";

        return $"{prefix} {encounterTone} {numberDynamic}";
    }

    private static string ResolveEmotionalAffinity(int score, BirthProfile? birthProfile1, BirthProfile? birthProfile2, string elementalDynamic, string conflictPattern)
    {
        if (birthProfile1 is null || birthProfile2 is null)
        {
            return $"Sem as duas datas, a afinidade emocional aparece mais na sensação do encontro do que em uma leitura mais completa. Ainda assim, vale atenção para {conflictPattern}.";
        }

        var prefix = score >= 70
            ? "Emocionalmente, há boa chance de acolhimento e leitura mútua."
            : "Emocionalmente, o vínculo pede mais clareza do que suposição.";

        return $"{prefix} {elementalDynamic} Aqui, o ponto mais sensível costuma ser {conflictPattern}.";
    }

    private static string ResolveSpiritualAffinity(NumerologyProfile profile1, NumerologyProfile profile2, BirthProfile? birthProfile1, BirthProfile? birthProfile2, string relationshipAxis, string archetypeDynamic)
    {
        var purposeText = birthProfile1 is not null && birthProfile2 is not null
            ? $"Os caminhos de vida {birthProfile1.LifePathNumber} e {birthProfile2.LifePathNumber} reforçam a ideia de que esse encontro toca aprendizados importantes."
            : "Mesmo sem as duas datas, o vínculo sugere uma lição que aparece nas repetições, nos sinais e no efeito que um desperta no outro.";

        return $"No fundo, este vínculo fala de {relationshipAxis.ToLowerInvariant()}. {archetypeDynamic} {purposeText}";
    }

    private static IReadOnlyList<string> ResolveStrengths(int score, NumerologyProfile profile1, NumerologyProfile profile2, BirthProfile? birthProfile1, BirthProfile? birthProfile2, string archetypeDynamic, string numberDynamic, string elementalDynamic)
    {
        var strengths = new List<string>
        {
            Capitalize(archetypeDynamic),
            score >= 70 ? "Há potencial de incentivo mútuo em decisões importantes." : "O vínculo pode gerar autoconhecimento e ampliar o repertório emocional.",
            Capitalize(numberDynamic)
        };

        if (birthProfile1 is not null && birthProfile2 is not null)
        {
            strengths[2] = Capitalize(elementalDynamic);
        }

        return strengths;
    }

    private static IReadOnlyList<string> ResolveAttentionPoints(int score, NumerologyProfile profile1, NumerologyProfile profile2, BirthProfile? birthProfile1, BirthProfile? birthProfile2, string conflictPattern)
    {
        var points = new List<string>
        {
            $"Vale atenção para {conflictPattern}.",
            $"Equilibrar os desafios de {profile1.ChallengeHints.First().ToLowerInvariant()} com {profile2.ChallengeHints.First().ToLowerInvariant()}.",
            score >= 70 ? "Evitar acomodação em zonas de conforto afetivo." : "Traduzir expectativas antes que elas virem projeções."
        };

        if (birthProfile1 is not null && birthProfile2 is not null && birthProfile1.Element != birthProfile2.Element)
        {
            points[2] = "Respeitar diferenças de ritmo emocional e do jeito de cada um sentir as coisas.";
        }

        return points;
    }

    private static string ResolveBalanceGuidance(NumerologyProfile profile1, NumerologyProfile profile2, BirthProfile? birthProfile1, BirthProfile? birthProfile2, string relationshipAxis, string conflictPattern)
    {
        var elementalText = birthProfile1 is not null && birthProfile2 is not null
            ? $"Honrem a linguagem dos elementos {birthProfile1.Element.ToLowerInvariant()} e {birthProfile2.Element.ToLowerInvariant()}."
            : "Cultivem escuta e curiosidade para descobrir o ritmo real do vínculo.";

        return $"{elementalText} Como este par vive uma história de {relationshipAxis.ToLowerInvariant()}, o equilíbrio aparece quando vocês não deixam que {conflictPattern} fale mais alto do que o melhor da troca entre {profile1.PredominantArchetype} e {profile2.PredominantArchetype}.";
    }

    private static string BuildPairKey(string first, string second) =>
        string.Compare(first, second, StringComparison.OrdinalIgnoreCase) <= 0
            ? $"{first}-{second}"
            : $"{second}-{first}";

    private static string Capitalize(string text) => string.IsNullOrWhiteSpace(text)
        ? text
        : char.ToUpperInvariant(text[0]) + text[1..];
}