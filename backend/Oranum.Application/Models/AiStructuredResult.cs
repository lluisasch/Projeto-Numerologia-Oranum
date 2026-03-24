namespace Oranum.Application.Models;

public sealed record AiStructuredResult<T>(T? Payload, string? Model, string? RawContent);
