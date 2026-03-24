using System.Globalization;
using System.Text;
using Oranum.Domain.Exceptions;
using Oranum.Domain.ValueObjects;

namespace Oranum.Domain.Services;

public sealed class NumerologyCalculator
{
    private static readonly HashSet<char> Vowels = ['A', 'E', 'I', 'O', 'U'];

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

    private static readonly Dictionary<int, NumerologyDescriptor> NumberMap = new()
    {
        [1] = new("Início, coragem e impulso criador.", "Presença afirmativa, luminosa e direta.", ["liderança natural", "clareza de direção", "autonomia em momentos decisivos"], ["impaciência", "rigidez", "dificuldade de pedir apoio"], "Herói", "Governante"),
        [2] = new("Sensibilidade, vínculo e diplomacia refinada.", "Energia suave, receptiva e harmonizadora.", ["escuta profunda", "empatia", "capacidade de conciliar"], ["indecisão", "excesso de cautela", "dependência emocional"], "Cuidador", "Inocente"),
        [3] = new("Expressão, beleza e criatividade em fluxo.", "Vibração leve, inspiradora e comunicativa.", ["carisma", "imaginação viva", "facilidade de encantar"], ["dispersão", "necessidade de validação", "dificuldade de sustentar foco"], "Criador", "Amante"),
        [4] = new("Estrutura, constância e sabedoria prática.", "Campo estável, confiável e disciplinado.", ["organização", "consistência", "lealdade"], ["controle excessivo", "teimosia", "resistência ao novo"], "Governante", "Sábio"),
        [5] = new("Movimento, liberdade e travessias transformadoras.", "Vibração inquieta, versátil e cheia de descoberta.", ["adaptabilidade", "curiosidade", "facilidade para renovar caminhos"], ["impulsividade", "exageros", "dificuldade com rotina"], "Explorador", "Rebelde"),
        [6] = new("Afeto, cuidado e senso de beleza relacional.", "Energia acolhedora, envolvente e integradora.", ["presença afetiva", "responsabilidade", "harmonia nos vínculos"], ["culpa", "cobrança", "excesso de responsabilidade"], "Amante", "Cuidador"),
        [7] = new("Busca interior, contemplação e profundidade simbólica.", "Campo introspectivo, intuitivo e observador.", ["discernimento", "intuição", "leitura sensível de padrões"], ["isolamento emocional", "autocrítica", "distanciamento"], "Sábio", "Mago"),
        [8] = new("Realização, magnetismo e poder de manifestação.", "Energia intensa, estratégica e realizadora.", ["foco", "presença", "capacidade de materializar"], ["pressão interna", "controle", "rigidez afetiva"], "Governante", "Herói"),
        [9] = new("Compaixão, encerramento de ciclos e visão ampla.", "Vibração generosa, emocionalmente profunda e inspiradora.", ["generosidade", "sensibilidade humana", "visão de conjunto"], ["idealização", "desgaste afetivo", "dificuldade de impor limites"], "Inocente", "Cuidador"),
        [11] = new("Inspiração intuitiva, percepção elevada e visão simbólica.", "Presença visionária, vibrante e magnética.", ["inspiração", "sensibilidade espiritual", "capacidade de iluminar caminhos"], ["ansiedade", "sobrecarga emocional", "instabilidade"], "Mago", "Sábio"),
        [22] = new("Construção de legado e concretização consciente.", "Energia ampla, firme e orientada a resultados duradouros.", ["visão estratégica", "força coletiva", "capacidade de estruturar grandes projetos"], ["autoexigência", "peso de responsabilidade", "medo de falhar"], "Governante", "Criador"),
        [33] = new("Serviço compassivo, presença curadora e inspiração amorosa.", "Vibração elevada, afetuosa e profundamente humana.", ["acolhimento", "sabedoria afetiva", "capacidade de inspirar pelo exemplo"], ["sacrifício excessivo", "hipersensibilidade", "dificuldade de se priorizar"], "Cuidador", "Inocente")
    };

    private static readonly Dictionary<string, string> ArchetypeDescriptions = new(StringComparer.OrdinalIgnoreCase)
    {
        ["Herói"] = "Representa coragem, iniciativa e disposição para abrir caminho quando a vida pede atitude.",
        ["Cuidador"] = "Representa acolhimento, proteção e a vontade de sustentar vínculos com delicadeza e presença.",
        ["Criador"] = "Representa imaginação, expressão e o impulso de transformar sensibilidade em forma e beleza.",
        ["Governante"] = "Representa estrutura, responsabilidade e capacidade de organizar a própria vida com firmeza.",
        ["Explorador"] = "Representa liberdade, curiosidade e desejo de atravessar novas experiências com autenticidade.",
        ["Amante"] = "Representa afeto, conexão, beleza e intensidade emocional vivida com o coração desperto.",
        ["Sábio"] = "Representa reflexão, discernimento e busca por sentido antes de qualquer passo importante.",
        ["Mago"] = "Representa transformação, visão e poder de perceber possibilidades escondidas no invisível.",
        ["Inocente"] = "Representa leveza, esperança e uma forma luminosa de enxergar o mundo e os vínculos.",
        ["Rebelde"] = "Representa ruptura, autenticidade e coragem para romper padrões que já não fazem sentido."
    };

    public NumerologyProfile CalculateNameProfile(string fullName)
    {
        if (string.IsNullOrWhiteSpace(fullName))
        {
            throw new DomainValidationException("Informe um nome válido para a leitura energética.");
        }

        var normalizedName = NormalizeLetters(fullName);
        if (string.IsNullOrWhiteSpace(normalizedName))
        {
            throw new DomainValidationException("Não foi possível interpretar o nome informado.");
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
        var letters = normalizedName.ToCharArray();
        var vowelValues = letters.Where(letter => Vowels.Contains(letter)).Select(letter => LetterValues[letter]).ToArray();
        var consonantValues = letters.Where(letter => !Vowels.Contains(letter)).Select(letter => LetterValues[letter]).ToArray();
        var vowelNumber = ReduceNumber(vowelValues.Sum());
        var consonantNumber = ReduceNumber(consonantValues.Sum());
        var dominantNumber = ResolveDominantNumber(values);
        var initialLetter = normalizedName[0].ToString();
        var letterCount = normalizedName.Length;
        var cadence = ResolveNameCadence(letterCount, vowelValues.Length, consonantValues.Length);
        var symbolicLens = ResolveSymbolicLens(initialLetter, dominantNumber);
        var archetype = ResolveArchetype(descriptor, vowelNumber, consonantNumber, dominantNumber, letterCount, initialLetter);
        var archetypeDescription = ArchetypeDescriptions[archetype];

        return new NumerologyProfile(
            fullName.Trim(),
            normalizedName,
            values,
            rawSum,
            principalNumber,
            vowelNumber,
            consonantNumber,
            dominantNumber,
            letterCount,
            initialLetter,
            cadence,
            symbolicLens,
            descriptor.Meaning,
            archetype,
            archetypeDescription,
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
        if (number <= 0)
        {
            return 0;
        }

        while (number > 9 && number is not 11 and not 22 and not 33)
        {
            number = number.ToString(CultureInfo.InvariantCulture)
                .Where(char.IsDigit)
                .Select(character => character - '0')
                .Sum();
        }

        return number;
    }

    private static int ResolveDominantNumber(IReadOnlyList<int> values) =>
        values.GroupBy(value => value)
            .OrderByDescending(group => group.Count())
            .ThenByDescending(group => group.Key)
            .First()
            .Key;

    private static string ResolveNameCadence(int letterCount, int vowelCount, int consonantCount)
    {
        var total = Math.Max(1, vowelCount + consonantCount);
        var vowelRatio = vowelCount / (decimal)total;

        if (letterCount <= 4)
        {
            return "curta e incisiva";
        }

        if (vowelRatio >= 0.55m)
        {
            return "fluida e melodiosa";
        }

        if (vowelRatio <= 0.35m)
        {
            return "firme e marcada";
        }

        if (letterCount >= 10)
        {
            return "longa e envolvente";
        }

        return "equilibrada e ritmada";
    }

    private static string ResolveSymbolicLens(string initialLetter, int dominantNumber)
    {
        var initialLens = initialLetter switch
        {
            "A" or "B" or "C" => "início e presença",
            "D" or "E" or "F" => "expressão e vínculo",
            "G" or "H" or "I" => "imaginação e sensibilidade",
            "J" or "K" or "L" => "clareza e organização",
            "M" or "N" or "O" => "acolhimento e memória",
            "P" or "Q" or "R" => "movimento e conquista",
            "S" or "T" or "U" => "intuição e magnetismo",
            _ => "transformação e visão"
        };

        var numericLens = dominantNumber switch
        {
            1 or 8 => "com brilho de liderança",
            2 or 6 => "com delicadeza relacional",
            3 or 5 => "com leveza criativa",
            4 or 7 => "com profundidade reflexiva",
            _ => "com amplitude emocional"
        };

        return $"{initialLens} {numericLens}";
    }

    private static string ResolveArchetype(NumerologyDescriptor descriptor, int vowelNumber, int consonantNumber, int dominantNumber, int letterCount, string initialLetter)
    {
        var selectionSeed = vowelNumber + consonantNumber + dominantNumber + letterCount + initialLetter[0];
        return selectionSeed % 2 == 0 ? descriptor.PrimaryArchetype : descriptor.SecondaryArchetype;
    }

    private sealed record NumerologyDescriptor(string Meaning, string Energy, string[] Strengths, string[] Challenges, string PrimaryArchetype, string SecondaryArchetype);
}