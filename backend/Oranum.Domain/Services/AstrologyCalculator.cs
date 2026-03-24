using Oranum.Domain.ValueObjects;

namespace Oranum.Domain.Services;

public sealed class AstrologyCalculator
{
    private static readonly Dictionary<string, string> ElementBySign = new()
    {
        ["Áries"] = "Fogo",
        ["Touro"] = "Terra",
        ["Gêmeos"] = "Ar",
        ["Câncer"] = "Água",
        ["Leão"] = "Fogo",
        ["Virgem"] = "Terra",
        ["Libra"] = "Ar",
        ["Escorpião"] = "Água",
        ["Sagitário"] = "Fogo",
        ["Capricórnio"] = "Terra",
        ["Aquário"] = "Ar",
        ["Peixes"] = "Água"
    };

    private static readonly Dictionary<string, string> SignEnergyMap = new()
    {
        ["Áries"] = "Impulso de início, coragem e movimento.",
        ["Touro"] = "Presença serena, sensualidade e estabilidade.",
        ["Gêmeos"] = "Curiosidade, linguagem e conexões vivas.",
        ["Câncer"] = "Memória emocional, cuidado e intuição.",
        ["Leão"] = "Expressão criativa, brilho e nobreza afetiva.",
        ["Virgem"] = "Refino, observação e serviço consciente.",
        ["Libra"] = "Harmonia, beleza e senso de reciprocidade.",
        ["Escorpião"] = "Profundidade, magnetismo e renascimento.",
        ["Sagitário"] = "Sentido, expansão e busca espiritual.",
        ["Capricórnio"] = "Estrutura, legado e maturidade.",
        ["Aquário"] = "Originalidade, visão de futuro e liberdade.",
        ["Peixes"] = "Sensibilidade, imaginação e transcendência."
    };

    public BirthProfile CalculateBirthProfile(DateOnly birthDate, int lifePathNumber)
    {
        var zodiacSign = ResolveSign(birthDate);
        var element = ElementBySign[zodiacSign];
        var centralEnergy = SignEnergyMap[zodiacSign];
        var symbolicProfile = $"{zodiacSign} com caminho {lifePathNumber} forma uma assinatura marcada por {ResolveLifePathTheme(lifePathNumber).ToLowerInvariant()}";
        var mission = $"Sua missão simbólica pede {ResolveLifePathTheme(lifePathNumber).ToLowerInvariant()} com a sensibilidade do elemento {element.ToLowerInvariant()}.";

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
            (3, >= 21) or (4, <= 19) => "Áries",
            (4, >= 20) or (5, <= 20) => "Touro",
            (5, >= 21) or (6, <= 20) => "Gêmeos",
            (6, >= 21) or (7, <= 22) => "Câncer",
            (7, >= 23) or (8, <= 22) => "Leão",
            (8, >= 23) or (9, <= 22) => "Virgem",
            (9, >= 23) or (10, <= 22) => "Libra",
            (10, >= 23) or (11, <= 21) => "Escorpião",
            (11, >= 22) or (12, <= 21) => "Sagitário",
            (12, >= 22) or (1, <= 19) => "Capricórnio",
            (1, >= 20) or (2, <= 18) => "Aquário",
            _ => "Peixes"
        };
    }

    private static string ResolveLifePathTheme(int lifePathNumber) =>
        lifePathNumber switch
        {
            1 => "liderar o próprio destino",
            2 => "cultivar cooperação e escuta",
            3 => "expressar a própria verdade com beleza",
            4 => "construir bases consistentes",
            5 => "abrir caminhos e atravessar mudanças",
            6 => "nutrir e harmonizar relações",
            7 => "buscar sabedoria interior",
            8 => "manifestar poder com consciência",
            9 => "encerrar ciclos com compaixão",
            11 => "inspirar por intuição",
            22 => "materializar visões amplas",
            33 => "servir e curar pela presença",
            _ => "equilibrar experiências com discernimento"
        };

    private static IReadOnlyList<string> ResolveChallenges(string zodiacSign, int lifePathNumber)
    {
        var challenges = new List<string>
        {
            zodiacSign switch
            {
                "Áries" => "Moderar impulsos sem apagar a própria coragem.",
                "Touro" => "Flexibilizar quando a vida pedir mudança.",
                "Gêmeos" => "Aprofundar sem dispersar a energia.",
                "Câncer" => "Proteger a sensibilidade sem se fechar.",
                "Leão" => "Brilhar sem depender de aprovação constante.",
                "Virgem" => "Trocar perfeccionismo por discernimento amoroso.",
                "Libra" => "Escolher com firmeza sem se perder no outro.",
                "Escorpião" => "Soltar o controle diante do invisível.",
                "Sagitário" => "Ancorar visões em constância.",
                "Capricórnio" => "Permitir vulnerabilidade junto com ambição.",
                "Aquário" => "Integrar liberdade e intimidade.",
                _ => "Transformar sensibilidade em limites saudáveis."
            }
        };

        challenges.Add(lifePathNumber switch
        {
            1 => "Evitar excesso de individualismo.",
            2 => "Não silenciar a própria vontade.",
            3 => "Canalizar criatividade com foco.",
            4 => "Não endurecer diante do imprevisto.",
            5 => "Sustentar compromissos sem perder liberdade.",
            6 => "Não assumir responsabilidades que não são suas.",
            7 => "Evitar isolamento emocional.",
            8 => "Humanizar a relação com poder e resultado.",
            9 => "Estabelecer limites energéticos.",
            11 => "Organizar a intuição para não sobrecarregar o corpo.",
            22 => "Dividir o peso da visão com processos concretos.",
            33 => "Cuidar de si antes de sustentar todos ao redor.",
            _ => "Filtrar excessos emocionais."
        });

        return challenges;
    }

    private static IReadOnlyList<string> ResolvePotentials(string zodiacSign, int lifePathNumber)
    {
        var potentials = new List<string>
        {
            zodiacSign switch
            {
                "Áries" => "Capacidade de abrir caminhos com coragem.",
                "Touro" => "Poder de materializar e sustentar.",
                "Gêmeos" => "Dom de traduzir ideias em conexão.",
                "Câncer" => "Força intuitiva e acolhedora.",
                "Leão" => "Presença inspiradora e generosa.",
                "Virgem" => "Talento para refinar, curar e organizar.",
                "Libra" => "Habilidade de harmonizar ambientes e vínculos.",
                "Escorpião" => "Potencial de transformação profunda.",
                "Sagitário" => "Visão expansiva e filosófica.",
                "Capricórnio" => "Liderança serena e construção de legado.",
                "Aquário" => "Originalidade e pensamento de futuro.",
                _ => "Imaginação espiritual e empatia elevada."
            }
        };

        potentials.Add(lifePathNumber switch
        {
            1 => "Liderar com autenticidade.",
            2 => "Criar pontes e alianças.",
            3 => "Encantar pela expressão.",
            4 => "Erguer estruturas confiáveis.",
            5 => "Renovar cenários com versatilidade.",
            6 => "Cuidar com beleza e presença.",
            7 => "Interpretar o invisível com profundidade.",
            8 => "Manifestar resultados com magnetismo.",
            9 => "Inspirar pela compaixão.",
            11 => "Iluminar caminhos por intuição.",
            22 => "Concretizar visões coletivas.",
            33 => "Curar e orientar com amor.",
            _ => "Trazer equilíbrio aos ciclos."
        });

        return potentials;
    }
}