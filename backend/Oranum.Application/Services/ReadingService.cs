using System.Text.Json;
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
            _logger.LogWarning(exception, "Falha ao gerar leitura com IA. Sera utilizado fallback deterministico.");
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
            NomeAnalisado = ReadOrFallback(payload.NomeAnalisado, fallback.NomeAnalisado),
            NumeroPrincipal = payload.NumeroPrincipal == 0 ? fallback.NumeroPrincipal : payload.NumeroPrincipal,
            TituloLeitura = ReadOrFallback(payload.TituloLeitura, fallback.TituloLeitura),
            EnergiaGeral = ReadOrFallback(payload.EnergiaGeral, fallback.EnergiaGeral),
            ArquetipoPredominante = ReadOrFallback(payload.ArquetipoPredominante, fallback.ArquetipoPredominante),
            SignificadoDoNome = ReadOrFallback(payload.SignificadoDoNome, fallback.SignificadoDoNome),
            Forcas = ReadListOrFallback(payload.Forcas, fallback.Forcas),
            Desafios = ReadListOrFallback(payload.Desafios, fallback.Desafios),
            LeituraXamanica = ReadOrFallback(payload.LeituraXamanica, fallback.LeituraXamanica),
            ConselhoEspiritual = ReadOrFallback(payload.ConselhoEspiritual, fallback.ConselhoEspiritual),
            ResumoFinal = ReadOrFallback(payload.ResumoFinal, fallback.ResumoFinal)
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
            SignoSolar = ReadOrFallback(payload.SignoSolar, fallback.SignoSolar),
            Elemento = ReadOrFallback(payload.Elemento, fallback.Elemento),
            CaminhoDeVida = payload.CaminhoDeVida == 0 ? fallback.CaminhoDeVida : payload.CaminhoDeVida,
            EnergiaCentral = ReadOrFallback(payload.EnergiaCentral, fallback.EnergiaCentral),
            TendenciasEmocionais = ReadOrFallback(payload.TendenciasEmocionais, fallback.TendenciasEmocionais),
            MissaoDeVida = ReadOrFallback(payload.MissaoDeVida, fallback.MissaoDeVida),
            Desafios = ReadListOrFallback(payload.Desafios, fallback.Desafios),
            Potenciais = ReadListOrFallback(payload.Potenciais, fallback.Potenciais),
            ConselhoFinal = ReadOrFallback(payload.ConselhoFinal, fallback.ConselhoFinal)
        };
    }

    private static CompatibilityReadingResponse NormalizeCompatibilityResponse(CompatibilityReadingResponse? payload, CompatibilityReadingContext context)
    {
        var fallback = FallbackReadingFactory.CreateCompatibilityReading(context);
        if (payload is null)
        {
            return fallback;
        }

        return payload with
        {
            Pessoa1 = ReadOrFallback(payload.Pessoa1, fallback.Pessoa1),
            Pessoa2 = ReadOrFallback(payload.Pessoa2, fallback.Pessoa2),
            CompatibilidadePercentual = payload.CompatibilidadePercentual == 0 ? fallback.CompatibilidadePercentual : payload.CompatibilidadePercentual,
            NivelCompatibilidade = ReadOrFallback(payload.NivelCompatibilidade, fallback.NivelCompatibilidade),
            AfinidadeEnergetica = ReadOrFallback(payload.AfinidadeEnergetica, fallback.AfinidadeEnergetica),
            AfinidadeEmocional = ReadOrFallback(payload.AfinidadeEmocional, fallback.AfinidadeEmocional),
            AfinidadeEspiritual = ReadOrFallback(payload.AfinidadeEspiritual, fallback.AfinidadeEspiritual),
            PontosFortes = ReadListOrFallback(payload.PontosFortes, fallback.PontosFortes),
            PontosDeAtencao = ReadListOrFallback(payload.PontosDeAtencao, fallback.PontosDeAtencao),
            ConselhoRelacional = ReadOrFallback(payload.ConselhoRelacional, fallback.ConselhoRelacional),
            ResumoVinculo = ReadOrFallback(payload.ResumoVinculo, fallback.ResumoVinculo)
        };
    }

    private static string ReadOrFallback(string? value, string fallback) =>
        string.IsNullOrWhiteSpace(value) ? fallback : value.Trim();

    private static IReadOnlyList<string> ReadListOrFallback(IReadOnlyList<string>? value, IReadOnlyList<string> fallback) =>
        value is { Count: > 0 } ? value : fallback;
}
