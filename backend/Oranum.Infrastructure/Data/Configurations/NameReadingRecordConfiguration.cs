using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Oranum.Domain.Entities;

namespace Oranum.Infrastructure.Data.Configurations;

public sealed class NameReadingRecordConfiguration : IEntityTypeConfiguration<NameReadingRecord>
{
    public void Configure(EntityTypeBuilder<NameReadingRecord> builder)
    {
        builder.ToTable("name_readings");
        builder.HasKey(entity => entity.Id);
        builder.Property(entity => entity.FullName).HasMaxLength(180).IsRequired();
        builder.Property(entity => entity.Archetype).HasMaxLength(120).IsRequired();
        builder.Property(entity => entity.NumerologyNumber).IsRequired();
        builder.Property(entity => entity.ResponseJson).HasColumnType("jsonb").IsRequired();
        builder.Property(entity => entity.Model).HasMaxLength(120);
        builder.Property(entity => entity.CreatedAtUtc).IsRequired();
    }
}
