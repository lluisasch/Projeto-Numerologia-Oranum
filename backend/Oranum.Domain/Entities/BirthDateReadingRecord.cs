namespace Oranum.Domain.Entities;

public sealed class BirthDateReadingRecord : BaseEntity
{
    public required string FullName { get; set; }
    public required DateOnly BirthDate { get; set; }
    public required int LifePathNumber { get; set; }
    public required string ZodiacSign { get; set; }
    public required string ResponseJson { get; set; }
    public string? Model { get; set; }
}
