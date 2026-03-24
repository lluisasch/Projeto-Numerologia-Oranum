namespace Oranum.Domain.Entities;

public sealed class CompatibilityReadingRecord : BaseEntity
{
    public required string Person1Name { get; set; }
    public DateOnly? Person1BirthDate { get; set; }
    public required string Person2Name { get; set; }
    public DateOnly? Person2BirthDate { get; set; }
    public required int CompatibilityScore { get; set; }
    public required string ResponseJson { get; set; }
    public string? Model { get; set; }
}
