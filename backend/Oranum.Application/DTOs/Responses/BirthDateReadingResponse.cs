namespace Oranum.Application.DTOs.Responses;

public sealed record BirthDateReadingResponse(
    string DataNascimento,
    string SignoSolar,
    string Elemento,
    int CaminhoDeVida,
    string EnergiaCentral,
    string TendenciasEmocionais,
    string MissaoDeVida,
    IReadOnlyList<string> Desafios,
    IReadOnlyList<string> Potenciais,
    string ConselhoFinal);
