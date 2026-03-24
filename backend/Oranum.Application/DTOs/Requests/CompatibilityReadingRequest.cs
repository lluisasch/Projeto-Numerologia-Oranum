namespace Oranum.Application.DTOs.Requests;

public sealed record CompatibilityReadingRequest(
    string Person1Name,
    DateOnly? Person1BirthDate,
    string Person2Name,
    DateOnly? Person2BirthDate);
