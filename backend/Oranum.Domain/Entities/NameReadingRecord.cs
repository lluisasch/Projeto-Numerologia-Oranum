namespace Oranum.Domain.Entities;

public sealed class NameReadingRecord : BaseEntity
{
    public required string FullName { get; set; }
    public required int NumerologyNumber { get; set; }
    public required string Archetype { get; set; }
    public required string ResponseJson { get; set; }
    public string? Model { get; set; }
}
