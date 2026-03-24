namespace Oranum.Application.DTOs.Responses;

public sealed record NameReadingResponse(
    string NomeAnalisado,
    int NumeroPrincipal,
    string TituloLeitura,
    string EnergiaGeral,
    string ArquetipoPredominante,
    string SignificadoDoNome,
    IReadOnlyList<string> Forcas,
    IReadOnlyList<string> Desafios,
    string LeituraXamanica,
    string ConselhoEspiritual,
    string ResumoFinal);
