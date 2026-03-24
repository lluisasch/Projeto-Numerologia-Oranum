using System.Globalization;
using System.Text.Json;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using Oranum.Application.Abstractions;
using Oranum.Application.DTOs.Requests;
using Oranum.Application.DTOs.Responses;
using Oranum.Application.Models;
using Oranum.Application.Prompts;
using Oranum.Application.Validators;
using Oranum.Domain.Entities;
using Oranum.Domain.Services;

namespace Oranum.Application.Services;

public sealed class ReadingService : IReadingService
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web)
    {
        WriteIndented = false
    };

    private static readonly HashSet<string> AllowedArchetypes = new(StringComparer.OrdinalIgnoreCase)
    {
        "Herói",
        "Cuidador",
        "Criador",
        "Governante",
        "Explorador",
        "Amante",
        "Sábio",
        "Mago",
        "Inocente",
        "Rebelde"
    };

    private static readonly string[] InternalTerms =
    [
        "backend",
        "frontend",
        "json",
        "prompt",
        "algorit",
        "modelo",
        "sistema",
        "interface",
        "layout",
        "site",
        "api",
        "inteligência artificial",
        " ia "
    ];

    private static readonly (string Source, string Target)[] WordingReplacements =
    [
        ("timing", "ritmo"),
        ("processamento interno", "jeito de sentir"),
        ("tradução afetiva", "clareza emocional"),
        ("chamada simbólica", "sensação de destino"),
        ("espelhamento", "efeito de espelho"),
        ("contraste fértil", "diferença que pode fazer bem"),
        ("camada elemental", "leitura dos elementos"),
        ("mapa completo", "leitura mais completa"),
        ("eixo deste vínculo é", "No fundo, este vínculo fala de"),
        ("eixo do vínculo", "tom principal do vínculo")
    ];

    private readonly ReadingRequestValidator _validator;
    private readonly NumerologyCalculator _numerologyCalculator;
    private readonly AstrologyCalculator _astrologyCalculator;
    private readonly CompatibilityCalculator _compatibilityCalculator;
    private readonly IEnumerable<IKnowledgeProvider> _knowledgeProviders;
    private readonly IOpenAiMysticClient _openAiMysticClient;
    private readonly IReadingPersistenceService _persistenceService;
    private readonly ILogger<ReadingService> _logger;

    public ReadingService(
        ReadingRequestValidator validator,
        NumerologyCalculator numerologyCalculator,
        AstrologyCalculator astrologyCalculator,
        CompatibilityCalculator compatibilityCalculator,
        IEnumerable<IKnowledgeProvider> knowledgeProviders,
        IOpenAiMysticClient openAiMysticClient,
        IReadingPersistenceService persistenceService,
        ILogger<ReadingService> logger)
    {
        _validator = validator;
        _numerologyCalculator = numerologyCalculator;
        _astrologyCalculator = astrologyCalculator;
        _compatibilityCalculator = compatibilityCalculator;
        _knowledgeProviders = knowledgeProviders;
        _openAiMysticClient = openAiMysticClient;
        _persistenceService = persistenceService;
        _logger = logger;
    }

    public async Task<NameReadingResponse> GenerateNameReadingAsync(NameReadingRequest request, CancellationToken cancellationToken)
    {
        _validator.Validate(request);

        var numerology = _numerologyCalculator.CalculateNameProfile(request.FullName);
        var context = new NameReadingContext(request.FullName.Trim(), numerology, await GetKnowledgeNotesAsync("name", cancellationToken));
        var prompt = MysticPromptFactory.CreateNamePrompt(context);

        var aiResult = await TryGenerateAsync(() => _openAiMysticClient.GenerateJsonAsync<NameReadingResponse>(prompt.SystemPrompt, prompt.UserPrompt, cancellationToken));
        var response = NormalizeNameResponse(aiResult.Payload, context);

        await _persistenceService.SaveNameReadingAsync(new NameReadingRecord
        {
            FullName = context.FullName,
            NumerologyNumber = numerology.PrincipalNumber,
            Archetype = numerology.PredominantArchetype,
            ResponseJson = JsonSerializer.Serialize(response, JsonOptions),
            Model = aiResult.Model ?? "fallback"
        }, cancellationToken);

        return response;
    }

    public async Task<BirthDateReadingResponse> GenerateBirthDateReadingAsync(BirthDateReadingRequest request, CancellationToken cancellationToken)
    {
        _validator.Validate(request);

        var lifePath = _numerologyCalculator.CalculateLifePath(request.BirthDate);
        var birthProfile = _astrologyCalculator.CalculateBirthProfile(request.BirthDate, lifePath);
        var context = new BirthDateReadingContext(request.FullName.Trim(), birthProfile, await GetKnowledgeNotesAsync("birthdate", cancellationToken));
        var prompt = MysticPromptFactory.CreateBirthPrompt(context);

        var aiResult = await TryGenerateAsync(() => _openAiMysticClient.GenerateJsonAsync<BirthDateReadingResponse>(prompt.SystemPrompt, prompt.UserPrompt, cancellationToken));
        var response = NormalizeBirthResponse(aiResult.Payload, context);

        await _persistenceService.SaveBirthDateReadingAsync(new BirthDateReadingRecord
        {
            FullName = context.FullName,
            BirthDate = request.BirthDate,
            LifePathNumber = birthProfile.LifePathNumber,
            ZodiacSign = birthProfile.ZodiacSign,
            ResponseJson = JsonSerializer.Serialize(response, JsonOptions),
            Model = aiResult.Model ?? "fallback"
        }, cancellationToken);

        return response;
    }

    public async Task<CompatibilityReadingResponse> GenerateCompatibilityReadingAsync(CompatibilityReadingRequest request, CancellationToken cancellationToken)
    {
        _validator.Validate(request);

        var profile1 = _numerologyCalculator.CalculateNameProfile(request.Person1Name);
        var profile2 = _numerologyCalculator.CalculateNameProfile(request.Person2Name);
        var birthProfile1 = request.Person1BirthDate.HasValue
            ? _astrologyCalculator.CalculateBirthProfile(request.Person1BirthDate.Value, _numerologyCalculator.CalculateLifePath(request.Person1BirthDate.Value))
            : null;
        var birthProfile2 = request.Person2BirthDate.HasValue
            ? _astrologyCalculator.CalculateBirthProfile(request.Person2BirthDate.Value, _numerologyCalculator.CalculateLifePath(request.Person2BirthDate.Value))
            : null;

        var compatibilityProfile = _compatibilityCalculator.Calculate(profile1, profile2, birthProfile1, birthProfile2);
        var context = new CompatibilityReadingContext(
            request.Person1Name.Trim(),
            request.Person2Name.Trim(),
            profile1,
            profile2,
            birthProfile1,
            birthProfile2,
            compatibilityProfile,
            await GetKnowledgeNotesAsync("compatibility", cancellationToken));

        var prompt = MysticPromptFactory.CreateCompatibilityPrompt(context);
        var aiResult = await TryGenerateAsync(() => _openAiMysticClient.GenerateJsonAsync<CompatibilityReadingResponse>(prompt.SystemPrompt, prompt.UserPrompt, cancellationToken));
        var response = NormalizeCompatibilityResponse(aiResult.Payload, context);

        await _persistenceService.SaveCompatibilityReadingAsync(new CompatibilityReadingRecord
        {
            Person1Name = context.Person1Name,
            Person1BirthDate = request.Person1BirthDate,
            Person2Name = context.Person2Name,
            Person2BirthDate = request.Person2BirthDate,
            CompatibilityScore = compatibilityProfile.CompatibilityScore,
            ResponseJson = JsonSerializer.Serialize(response, JsonOptions),
            Model = aiResult.Model ?? "fallback"
        }, cancellationToken);

        return response;
    }

    private async Task<AiStructuredResult<T>> TryGenerateAsync<T>(Func<Task<AiStructuredResult<T>>> action)
    {
        try
        {
            return await action();
        }
        catch (Exception exception)
        {
            _logger.LogWarning(exception, "Falha ao gerar leitura com IA. Será utilizado fallback determinístico.");
            return new AiStructuredResult<T>(default, null, null);
        }
    }

    private async Task<IReadOnlyList<MysticKnowledgeNote>> GetKnowledgeNotesAsync(string topic, CancellationToken cancellationToken)
    {
        var allNotes = new List<MysticKnowledgeNote>();

        foreach (var provider in _knowledgeProviders)
        {
            var notes = await provider.GetNotesAsync(topic, cancellationToken);
            allNotes.AddRange(notes);
        }

        return allNotes
            .DistinctBy(note => $"{note.Source}:{note.Content}")
            .ToArray();
    }

    private static NameReadingResponse NormalizeNameResponse(NameReadingResponse? payload, NameReadingContext context)
    {
        var fallback = FallbackReadingFactory.CreateNameReading(context);
        if (payload is null)
        {
            return fallback;
        }

        return payload with
        {
            NomeAnalisado = NormalizeDisplayName(ReadOrFallback(payload.NomeAnalisado, fallback.NomeAnalisado)),
            NumeroPrincipal = NormalizeNumber(payload.NumeroPrincipal, fallback.NumeroPrincipal),
            TituloLeitura = NormalizeTitle(ReadOrFallback(payload.TituloLeitura, fallback.TituloLeitura)),
            EnergiaGeral = NormalizeParagraph(ReadOrFallback(payload.EnergiaGeral, fallback.EnergiaGeral)),
            ArquetipoPredominante = NormalizeArchetype(payload.ArquetipoPredominante, fallback.ArquetipoPredominante),
            SignificadoDoNome = NormalizeParagraph(ReadOrFallback(payload.SignificadoDoNome, fallback.SignificadoDoNome)),
            Forcas = NormalizeList(payload.Forcas, fallback.Forcas),
            Desafios = NormalizeList(payload.Desafios, fallback.Desafios),
            LeituraXamanica = NormalizeParagraph(ReadOrFallback(payload.LeituraXamanica, fallback.LeituraXamanica)),
            ConselhoEspiritual = NormalizeParagraph(ReadOrFallback(payload.ConselhoEspiritual, fallback.ConselhoEspiritual)),
            ResumoFinal = NormalizeParagraph(ReadOrFallback(payload.ResumoFinal, fallback.ResumoFinal))
        };
    }

    private static BirthDateReadingResponse NormalizeBirthResponse(BirthDateReadingResponse? payload, BirthDateReadingContext context)
    {
        var fallback = FallbackReadingFactory.CreateBirthReading(context);
        if (payload is null)
        {
            return fallback;
        }

        return payload with
        {
            DataNascimento = ReadOrFallback(payload.DataNascimento, fallback.DataNascimento),
            SignoSolar = NormalizeTitle(ReadOrFallback(payload.SignoSolar, fallback.SignoSolar)),
            Elemento = NormalizeTitle(ReadOrFallback(payload.Elemento, fallback.Elemento)),
            CaminhoDeVida = NormalizeNumber(payload.CaminhoDeVida, fallback.CaminhoDeVida),
            EnergiaCentral = NormalizeParagraph(ReadOrFallback(payload.EnergiaCentral, fallback.EnergiaCentral)),
            TendenciasEmocionais = NormalizeParagraph(ReadOrFallback(payload.TendenciasEmocionais, fallback.TendenciasEmocionais)),
            MissaoDeVida = NormalizeParagraph(ReadOrFallback(payload.MissaoDeVida, fallback.MissaoDeVida)),
            Desafios = NormalizeList(payload.Desafios, fallback.Desafios),
            Potenciais = NormalizeList(payload.Potenciais, fallback.Potenciais),
            ConselhoFinal = NormalizeParagraph(ReadOrFallback(payload.ConselhoFinal, fallback.ConselhoFinal))
        };
    }

    private static CompatibilityReadingResponse NormalizeCompatibilityResponse(CompatibilityReadingResponse? payload, CompatibilityReadingContext context)
    {
        var fallback = FallbackReadingFactory.CreateCompatibilityReading(context);
        if (payload is null)
        {
            return fallback;
        }

        var percentual = payload.CompatibilidadePercentual is >= 1 and <= 100
            ? payload.CompatibilidadePercentual
            : fallback.CompatibilidadePercentual;

        return payload with
        {
            Pessoa1 = NormalizeDisplayName(ReadOrFallback(payload.Pessoa1, fallback.Pessoa1)),
            Pessoa2 = NormalizeDisplayName(ReadOrFallback(payload.Pessoa2, fallback.Pessoa2)),
            CompatibilidadePercentual = percentual,
            NivelCompatibilidade = NormalizeTitle(ReadOrFallback(payload.NivelCompatibilidade, fallback.NivelCompatibilidade)),
            AfinidadeEnergetica = NormalizeParagraph(ReadOrFallback(payload.AfinidadeEnergetica, fallback.AfinidadeEnergetica)),
            AfinidadeEmocional = NormalizeParagraph(ReadOrFallback(payload.AfinidadeEmocional, fallback.AfinidadeEmocional)),
            AfinidadeEspiritual = NormalizeParagraph(ReadOrFallback(payload.AfinidadeEspiritual, fallback.AfinidadeEspiritual)),
            PontosFortes = NormalizeList(payload.PontosFortes, fallback.PontosFortes),
            PontosDeAtencao = NormalizeList(payload.PontosDeAtencao, fallback.PontosDeAtencao),
            ConselhoRelacional = NormalizeParagraph(ReadOrFallback(payload.ConselhoRelacional, fallback.ConselhoRelacional)),
            ResumoVinculo = NormalizeParagraph(ReadOrFallback(payload.ResumoVinculo, fallback.ResumoVinculo))
        };
    }

    private static int NormalizeNumber(int value, int fallback) => value == 0 ? fallback : value;

    private static string ReadOrFallback(string? value, string fallback)
    {
        var candidate = string.IsNullOrWhiteSpace(value) ? fallback : value.Trim();
        candidate = NaturalizeWording(candidate);
        return ContainsInternalTerms(candidate) ? fallback : candidate;
    }

    private static IReadOnlyList<string> NormalizeList(IReadOnlyList<string>? value, IReadOnlyList<string> fallback)
    {
        var source = value is { Count: > 0 } ? value : fallback;

        var normalized = source
            .Where(item => !string.IsNullOrWhiteSpace(item))
            .Select(item => NormalizeParagraph(item))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToArray();

        return normalized.Length > 0 ? normalized : fallback;
    }

    private static string NormalizeArchetype(string? value, string fallback)
    {
        var candidate = NormalizeTitle(ReadOrFallback(value, fallback));
        return AllowedArchetypes.Contains(candidate) ? candidate : fallback;
    }

    private static string NormalizeTitle(string value)
    {
        var normalized = CollapseWhitespace(NaturalizeWording(value));
        normalized = normalized.Trim().TrimEnd('.', '!', '?', ';', ':');
        return CapitalizeFirstLetter(normalized);
    }

    private static string NormalizeParagraph(string value)
    {
        var normalized = CollapseWhitespace(NaturalizeWording(value));
        normalized = normalized.Replace(" ,", ",").Replace(" .", ".").Replace(" ;", ";").Replace(" :", ":");
        normalized = CapitalizeFirstLetter(normalized.Trim());

        if (string.IsNullOrWhiteSpace(normalized))
        {
            return normalized;
        }

        return normalized.EndsWith('.') || normalized.EndsWith('!') || normalized.EndsWith('?')
            ? normalized
            : $"{normalized}.";
    }

    private static string CollapseWhitespace(string value)
    {
        var parts = value
            .Replace("\r", " ")
            .Replace("\n", " ")
            .Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        return string.Join(' ', parts);
    }

    private static string NaturalizeWording(string value)
    {
        var normalized = value;
        foreach (var (source, target) in WordingReplacements)
        {
            normalized = Regex.Replace(normalized, Regex.Escape(source), target, RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
        }

        return normalized;
    }

    private static string CapitalizeFirstLetter(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return value;
        }

        var chars = value.ToCharArray();
        for (var index = 0; index < chars.Length; index++)
        {
            if (!char.IsLetter(chars[index]))
            {
                continue;
            }

            chars[index] = char.ToUpper(chars[index], CultureInfo.GetCultureInfo("pt-BR"));
            return new string(chars);
        }

        return value;
    }

    private static string NormalizeDisplayName(string value)
    {
        var culture = CultureInfo.GetCultureInfo("pt-BR");
        var particles = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "da", "de", "do", "das", "dos", "e" };
        var words = CollapseWhitespace(value)
            .Split(' ', StringSplitOptions.RemoveEmptyEntries)
            .Select((word, index) =>
            {
                var lowered = word.ToLower(culture);
                if (index > 0 && particles.Contains(lowered))
                {
                    return lowered;
                }

                return char.ToUpper(lowered[0], culture) + lowered[1..];
            });

        return string.Join(' ', words);
    }

    private static bool ContainsInternalTerms(string value)
    {
        var lowered = $" {value.ToLowerInvariant()} ";
        return InternalTerms.Any(term => lowered.Contains(term));
    }
}