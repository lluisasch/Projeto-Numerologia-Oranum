using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Oranum.Domain.Entities;

namespace Oranum.Infrastructure.Data.Configurations;

public sealed class BirthDateReadingRecordConfiguration : IEntityTypeConfiguration<BirthDateReadingRecord>
{
    public void Configure(EntityTypeBuilder<BirthDateReadingRecord> builder)
    {
        builder.ToTable("birthdate_readings");
        builder.HasKey(entity => entity.Id);
        builder.Property(entity => entity.FullName).HasMaxLength(180).IsRequired();
        builder.Property(entity => entity.BirthDate).HasColumnType("date").IsRequired();
        builder.Property(entity => entity.LifePathNumber).IsRequired();
        builder.Property(entity => entity.ZodiacSign).HasMaxLength(50).IsRequired();
        builder.Property(entity => entity.ResponseJson).HasColumnType("jsonb").IsRequired();
        builder.Property(entity => entity.Model).HasMaxLength(120);
        builder.Property(entity => entity.CreatedAtUtc).IsRequired();
    }
}
