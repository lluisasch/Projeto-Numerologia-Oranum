using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Oranum.Domain.Entities;

namespace Oranum.Infrastructure.Data.Configurations;

public sealed class CompatibilityReadingRecordConfiguration : IEntityTypeConfiguration<CompatibilityReadingRecord>
{
    public void Configure(EntityTypeBuilder<CompatibilityReadingRecord> builder)
    {
        builder.ToTable("compatibility_readings");
        builder.HasKey(entity => entity.Id);
        builder.Property(entity => entity.Person1Name).HasMaxLength(180).IsRequired();
        builder.Property(entity => entity.Person2Name).HasMaxLength(180).IsRequired();
        builder.Property(entity => entity.Person1BirthDate).HasColumnType("date");
        builder.Property(entity => entity.Person2BirthDate).HasColumnType("date");
        builder.Property(entity => entity.CompatibilityScore).IsRequired();
        builder.Property(entity => entity.ResponseJson).HasColumnType("jsonb").IsRequired();
        builder.Property(entity => entity.Model).HasMaxLength(120);
        builder.Property(entity => entity.CreatedAtUtc).IsRequired();
    }
}
