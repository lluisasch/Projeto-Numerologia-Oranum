namespace Oranum.Application.DTOs.Requests;

public sealed record BirthDateReadingRequest(string FullName, DateOnly BirthDate);
