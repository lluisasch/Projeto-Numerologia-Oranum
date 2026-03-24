using Oranum.Domain.ValueObjects;

namespace Oranum.Domain.Services;

public sealed class AstrologyCalculator
{
    private static readonly Dictionary<string, string> ElementBySign = new()
    {
        ["Aries"] = "Fogo",
        ["Touro"] = "Terra",
        ["Gemeos"] = "Ar",
        ["Cancer"] = "Agua",
        ["Leao"] = "Fogo",
        ["Virgem"] = "Terra",
        ["Libra"] = "Ar",
        ["Escorpiao"] = "Agua",
        ["Sagitario"] = "Fogo",
        ["Capricornio"] = "Terra",
        ["Aquario"] = "Ar",
        ["Peixes"] = "Agua"
    };

    private static readonly Dictionary<string, string> SignEnergyMap = new()
    {
        ["Aries"] = "Impulso de iniciacao, coragem e movimento.",
        ["Touro"] = "Presenca serena, sensualidade e estabilidade.",
        ["Gemeos"] = "Curiosidade, linguagem e conexoes vivas.",
        ["Cancer"] = "Memoria emocional, cuidado e intuicao.",
        ["Leao"] = "Expressao criativa, brilho e nobreza afetiva.",
        ["Virgem"] = "Refino, observacao e servico consciente.",
        ["Libra"] = "Harmonia, beleza e senso de reciprocidade.",
        ["Escorpiao"] = "Profundidade, magnetismo e renascimento.",
        ["Sagitario"] = "Sentido, expansao e busca espiritual.",
        ["Capricornio"] = "Estrutura, legado e maturidade.",
        ["Aquario"] = "Originalidade, visao de futuro e liberdade.",
        ["Peixes"] = "Sensibilidade, imaginacao e transcendencia."
    };

    public BirthProfile CalculateBirthProfile(DateOnly birthDate, int lifePathNumber)
    {
        var zodiacSign = ResolveSign(birthDate);
        var element = ElementBySign[zodiacSign];
        var centralEnergy = SignEnergyMap[zodiacSign];
        var symbolicProfile = $"{zodiacSign} com caminho {lifePathNumber} cria uma assinatura de alma marcada por {ResolveLifePathTheme(lifePathNumber).ToLowerInvariant()}";
        var mission = $"Sua missao simbolica pede {ResolveLifePathTheme(lifePathNumber).ToLowerInvariant()} com a sensibilidade do elemento {element.ToLowerInvariant()}.";

        return new BirthProfile(
            birthDate,
            zodiacSign,
            element,
            lifePathNumber,
            centralEnergy,
            symbolicProfile,
            mission,
            ResolveChallenges(zodiacSign, lifePathNumber),
            ResolvePotentials(zodiacSign, lifePathNumber));
    }

    public string ResolveSign(DateOnly birthDate)
    {
        var month = birthDate.Month;
        var day = birthDate.Day;

        return (month, day) switch
        {
            (3, >= 21) or (4, <= 19) => "Aries",
            (4, >= 20) or (5, <= 20) => "Touro",
            (5, >= 21) or (6, <= 20) => "Gemeos",
            (6, >= 21) or (7, <= 22) => "Cancer",
            (7, >= 23) or (8, <= 22) => "Leao",
            (8, >= 23) or (9, <= 22) => "Virgem",
            (9, >= 23) or (10, <= 22) => "Libra",
            (10, >= 23) or (11, <= 21) => "Escorpiao",
            (11, >= 22) or (12, <= 21) => "Sagitario",
            (12, >= 22) or (1, <= 19) => "Capricornio",
            (1, >= 20) or (2, <= 18) => "Aquario",
            _ => "Peixes"
        };
    }

    private static string ResolveLifePathTheme(int lifePathNumber) =>
        lifePathNumber switch
        {
            1 => "liderar o proprio destino",
            2 => "cultivar cooperacao e escuta",
            3 => "expressar a propria verdade com beleza",
            4 => "construir bases consistentes",
            5 => "abrir caminhos e atravessar mudancas",
            6 => "nutrir e harmonizar relacoes",
            7 => "buscar sabedoria interior",
            8 => "manifestar poder com consciencia",
            9 => "encerrar ciclos com compaixao",
            11 => "inspirar por intuicao",
            22 => "materializar visoes amplas",
            33 => "servir e curar pela presenca",
            _ => "equilibrar experiencias com discernimento"
        };

    private static IReadOnlyList<string> ResolveChallenges(string zodiacSign, int lifePathNumber)
    {
        var challenges = new List<string>
        {
            zodiacSign switch
            {
                "Aries" => "moderar impulsos sem apagar sua coragem",
                "Touro" => "flexibilizar quando a vida pedir mudanca",
                "Gemeos" => "aprofundar sem dispersar sua energia",
                "Cancer" => "proteger a sensibilidade sem se fechar",
                "Leao" => "brilhar sem depender de aprovacao constante",
                "Virgem" => "trocar perfeccionismo por discernimento amoroso",
                "Libra" => "escolher com firmeza sem se perder no outro",
                "Escorpiao" => "soltar o controle diante do invisivel",
                "Sagitario" => "ancorar visoes em constancia",
                "Capricornio" => "permitir vulnerabilidade junto com ambicao",
                "Aquario" => "integrar liberdade e intimidade",
                _ => "transformar sensibilidade em limites saudaveis"
            }
        };

        challenges.Add(lifePathNumber switch
        {
            1 => "evitar excesso de individualismo",
            2 => "nao silenciar a propria vontade",
            3 => "canalizar criatividade com foco",
            4 => "nao endurecer diante do imprevisto",
            5 => "sustentar compromissos sem perder liberdade",
            6 => "nao assumir responsabilidades que nao sao suas",
            7 => "evitar isolamento emocional",
            8 => "humanizar a relacao com poder e resultado",
            9 => "estabelecer limites energeticos",
            11 => "organizar a intuicao para nao sobrecarregar o corpo",
            22 => "dividir o peso da visao com processos concretos",
            33 => "cuidar de si antes de sustentar todos ao redor",
            _ => "filtrar excessos emocionais"
        });

        return challenges;
    }

    private static IReadOnlyList<string> ResolvePotentials(string zodiacSign, int lifePathNumber)
    {
        var potentials = new List<string>
        {
            zodiacSign switch
            {
                "Aries" => "capacidade de abrir caminhos com coragem",
                "Touro" => "poder de materializar e sustentar",
                "Gemeos" => "dom de traduzir ideias em conexao",
                "Cancer" => "forca intuitiva e acolhedora",
                "Leao" => "presenca inspiradora e generosa",
                "Virgem" => "talento para refinar, curar e organizar",
                "Libra" => "habilidade de harmonizar ambientes e vinculos",
                "Escorpiao" => "potencial de transformacao profunda",
                "Sagitario" => "visao expansiva e filosofica",
                "Capricornio" => "lideranca serena e construcao de legado",
                "Aquario" => "originalidade e pensamento de futuro",
                _ => "imaginacao espiritual e empatia elevada"
            }
        };

        potentials.Add(lifePathNumber switch
        {
            1 => "liderar com autenticidade",
            2 => "criar pontes e aliancas",
            3 => "encantar pela expressao",
            4 => "erguer estruturas confiaveis",
            5 => "renovar cenarios com versatilidade",
            6 => "cuidar com beleza e presenca",
            7 => "interpretar o invisivel com profundidade",
            8 => "manifestar resultados com magnetismo",
            9 => "inspirar pela compaixao",
            11 => "iluminar caminhos por intuicao",
            22 => "concretizar visoes coletivas",
            33 => "curar e orientar com amor",
            _ => "trazer equilibrio aos ciclos"
        });

        return potentials;
    }
}
