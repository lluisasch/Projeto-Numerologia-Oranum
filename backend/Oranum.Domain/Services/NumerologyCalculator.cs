using System.Globalization;
using System.Text;
using Oranum.Domain.Exceptions;
using Oranum.Domain.ValueObjects;

namespace Oranum.Domain.Services;

public sealed class NumerologyCalculator
{
    private static readonly Dictionary<char, int> LetterValues = new()
    {
        ['A'] = 1, ['J'] = 1, ['S'] = 1,
        ['B'] = 2, ['K'] = 2, ['T'] = 2,
        ['C'] = 3, ['L'] = 3, ['U'] = 3,
        ['D'] = 4, ['M'] = 4, ['V'] = 4,
        ['E'] = 5, ['N'] = 5, ['W'] = 5,
        ['F'] = 6, ['O'] = 6, ['X'] = 6,
        ['G'] = 7, ['P'] = 7, ['Y'] = 7,
        ['H'] = 8, ['Q'] = 8, ['Z'] = 8,
        ['I'] = 9, ['R'] = 9
    };

    private static readonly Dictionary<int, (string Meaning, string Archetype, string Energy, string[] Strengths, string[] Challenges)> NumberMap = new()
    {
        [1] = ("Iniciacao, coragem e impulso criador.", "Pioneiro Solar", "Expansiva e afirmativa.", ["lideranca magnetica", "clareza de direcao", "autonomia"], ["impaciencia", "rigidez", "isolamento emocional"]),
        [2] = ("Sensibilidade, uniao e intuicao refinada.", "Guardiao Lunar", "Receptiva e harmonizadora.", ["empatia", "escuta profunda", "diplomacia"], ["dependencia emocional", "indecisao", "excesso de cautela"]),
        [3] = ("Expressao, beleza e criatividade em fluxo.", "Artifice Estelar", "Leve, criativa e comunicativa.", ["carisma", "criatividade", "otimismo"], ["dispersao", "superficialidade", "necessidade de validacao"]),
        [4] = ("Estrutura, constancia e sabedoria pratica.", "Construtor de Portais", "Estavel e disciplinada.", ["consistencia", "lealdade", "organizacao"], ["controle excessivo", "teimosia", "resistencia ao novo"]),
        [5] = ("Movimento, liberdade e experiencia transformadora.", "Viajante Alquimico", "Mutavel e aventureira.", ["adaptabilidade", "versatilidade", "curiosidade"], ["inquietacao", "exageros", "dificuldade com rotina"]),
        [6] = ("Amor, cuidado e senso de beleza ritual.", "Curador de Venus", "Afetuosa e integradora.", ["acolhimento", "responsabilidade", "senso estetico"], ["cobranca", "culpa", "excesso de responsabilidade"]),
        [7] = ("Busca interior, simbolismo e contemplacao.", "Oraculo do Silencio", "Introspectiva e espiritual.", ["intuicao", "profundidade", "observacao"], ["distanciamento", "ceticismo emocional", "autocritica"]),
        [8] = ("Poder, realizacao e magnetismo material.", "Regente Arcano", "Intensa e realizadora.", ["presenca", "foco", "capacidade de manifestacao"], ["controle", "pressao interna", "dualidade entre poder e afeto"]),
        [9] = ("Compaixao universal e fechamento de ciclos.", "Anciao das Estrelas", "Ampla e humanitaria.", ["visao elevada", "generosidade", "sabedoria emocional"], ["idealizacao", "desgaste afetivo", "dificuldade de limites"]),
        [11] = ("Canal intuitivo, inspiracao e sensibilidade espiritual.", "Mensageiro do Limiar", "Visionaria e vibrante.", ["inspiracao", "mediacao", "alto magnetismo"], ["ansiedade", "sobrecarga energetica", "instabilidade"]),
        [22] = ("Construcao de legado e manifestacao consciente.", "Arquiteto dos Ceus", "Soberana e realizadora.", ["visao estrategica", "materializacao", "forca coletiva"], ["autoexigencia", "peso de responsabilidade", "medo de falhar"]),
        [33] = ("Servico compassivo e cura pela presenca.", "Mestre da Harmonia", "Elevada e amorosa.", ["cura", "sabedoria afetiva", "inspiracao"], ["sacrificio excessivo", "hipersensibilidade", "idealismo"])
    };

    public NumerologyProfile CalculateNameProfile(string fullName)
    {
        if (string.IsNullOrWhiteSpace(fullName))
        {
            throw new DomainValidationException("Informe um nome valido para a leitura energetica.");
        }

        var normalizedName = NormalizeLetters(fullName);
        if (string.IsNullOrWhiteSpace(normalizedName))
        {
            throw new DomainValidationException("Nao foi possivel interpretar o nome informado.");
        }

        var values = normalizedName
            .Select(letter => LetterValues.TryGetValue(letter, out var value) ? value : 0)
            .Where(value => value > 0)
            .ToArray();

        if (values.Length == 0)
        {
            throw new DomainValidationException("O nome precisa conter letras para gerar a leitura.");
        }

        var rawSum = values.Sum();
        var principalNumber = ReduceNumber(rawSum);
        var descriptor = NumberMap[principalNumber];

        return new NumerologyProfile(
            fullName.Trim(),
            normalizedName,
            values,
            rawSum,
            principalNumber,
            descriptor.Meaning,
            descriptor.Archetype,
            descriptor.Energy,
            descriptor.Strengths,
            descriptor.Challenges);
    }

    public int CalculateLifePath(DateOnly birthDate)
    {
        var digits = birthDate.ToString("yyyyMMdd", CultureInfo.InvariantCulture)
            .Where(char.IsDigit)
            .Select(character => character - '0')
            .Sum();

        return ReduceNumber(digits);
    }

    private static string NormalizeLetters(string input)
    {
        var normalized = input.Normalize(NormalizationForm.FormD);
        var builder = new StringBuilder();

        foreach (var character in normalized)
        {
            if (CharUnicodeInfo.GetUnicodeCategory(character) == UnicodeCategory.NonSpacingMark)
            {
                continue;
            }

            if (char.IsLetter(character))
            {
                builder.Append(char.ToUpperInvariant(character));
            }
        }

        return builder.ToString();
    }

    private static int ReduceNumber(int number)
    {
        while (number > 9 && number is not 11 and not 22 and not 33)
        {
            number = number.ToString(CultureInfo.InvariantCulture)
                .Where(char.IsDigit)
                .Select(character => character - '0')
                .Sum();
        }

        return number;
    }
}
