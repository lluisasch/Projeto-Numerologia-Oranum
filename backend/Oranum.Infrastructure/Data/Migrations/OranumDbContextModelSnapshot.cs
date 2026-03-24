using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Oranum.Infrastructure.Data;

#nullable disable

namespace Oranum.Infrastructure.Data.Migrations;

[DbContext(typeof(OranumDbContext))]
partial class OranumDbContextModelSnapshot : ModelSnapshot
{
    protected override void BuildModel(ModelBuilder modelBuilder)
    {
#pragma warning disable 612, 618
        modelBuilder
            .HasAnnotation("ProductVersion", "9.0.8")
            .HasAnnotation("Relational:MaxIdentifierLength", 63);

        modelBuilder.Entity("Oranum.Domain.Entities.BirthDateReadingRecord", builder =>
        {
            builder.Property<Guid>("Id")
                .ValueGeneratedOnAdd()
                .HasColumnType("uuid");

            builder.Property<DateOnly>("BirthDate")
                .HasColumnType("date");

            builder.Property<DateTime>("CreatedAtUtc")
                .HasColumnType("timestamp with time zone");

            builder.Property<string>("FullName")
                .IsRequired()
                .HasMaxLength(180)
                .HasColumnType("character varying(180)");

            builder.Property<int>("LifePathNumber")
                .HasColumnType("integer");

            builder.Property<string>("Model")
                .HasMaxLength(120)
                .HasColumnType("character varying(120)");

            builder.Property<string>("ResponseJson")
                .IsRequired()
                .HasColumnType("jsonb");

            builder.Property<string>("ZodiacSign")
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnType("character varying(50)");

            builder.HasKey("Id");
            builder.ToTable("birthdate_readings");
        });

        modelBuilder.Entity("Oranum.Domain.Entities.CompatibilityReadingRecord", builder =>
        {
            builder.Property<Guid>("Id")
                .ValueGeneratedOnAdd()
                .HasColumnType("uuid");

            builder.Property<int>("CompatibilityScore")
                .HasColumnType("integer");

            builder.Property<DateTime>("CreatedAtUtc")
                .HasColumnType("timestamp with time zone");

            builder.Property<string>("Model")
                .HasMaxLength(120)
                .HasColumnType("character varying(120)");

            builder.Property<string>("Person1Name")
                .IsRequired()
                .HasMaxLength(180)
                .HasColumnType("character varying(180)");

            builder.Property<DateOnly?>("Person1BirthDate")
                .HasColumnType("date");

            builder.Property<string>("Person2Name")
                .IsRequired()
                .HasMaxLength(180)
                .HasColumnType("character varying(180)");

            builder.Property<DateOnly?>("Person2BirthDate")
                .HasColumnType("date");

            builder.Property<string>("ResponseJson")
                .IsRequired()
                .HasColumnType("jsonb");

            builder.HasKey("Id");
            builder.ToTable("compatibility_readings");
        });

        modelBuilder.Entity("Oranum.Domain.Entities.NameReadingRecord", builder =>
        {
            builder.Property<Guid>("Id")
                .ValueGeneratedOnAdd()
                .HasColumnType("uuid");

            builder.Property<string>("Archetype")
                .IsRequired()
                .HasMaxLength(120)
                .HasColumnType("character varying(120)");

            builder.Property<DateTime>("CreatedAtUtc")
                .HasColumnType("timestamp with time zone");

            builder.Property<string>("FullName")
                .IsRequired()
                .HasMaxLength(180)
                .HasColumnType("character varying(180)");

            builder.Property<string>("Model")
                .HasMaxLength(120)
                .HasColumnType("character varying(120)");

            builder.Property<int>("NumerologyNumber")
                .HasColumnType("integer");

            builder.Property<string>("ResponseJson")
                .IsRequired()
                .HasColumnType("jsonb");

            builder.HasKey("Id");
            builder.ToTable("name_readings");
        });
#pragma warning restore 612, 618
    }
}
