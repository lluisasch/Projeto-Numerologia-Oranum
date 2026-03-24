namespace Oranum.Application.DTOs.Responses;

public sealed record CompatibilityReadingResponse(
    string Pessoa1,
    string Pessoa2,
    int CompatibilidadePercentual,
    string NivelCompatibilidade,
    string AfinidadeEnergetica,
    string AfinidadeEmocional,
    string AfinidadeEspiritual,
    IReadOnlyList<string> PontosFortes,
    IReadOnlyList<string> PontosDeAtencao,
    string ConselhoRelacional,
    string ResumoVinculo);
