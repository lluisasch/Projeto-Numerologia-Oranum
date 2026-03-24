using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Oranum.Infrastructure.Data.Migrations;

public partial class InitialCreate : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "birthdate_readings",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                FullName = table.Column<string>(type: "character varying(180)", maxLength: 180, nullable: false),
                BirthDate = table.Column<DateOnly>(type: "date", nullable: false),
                LifePathNumber = table.Column<int>(type: "integer", nullable: false),
                ZodiacSign = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                ResponseJson = table.Column<string>(type: "jsonb", nullable: false),
                Model = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: true),
                CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_birthdate_readings", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "compatibility_readings",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                Person1Name = table.Column<string>(type: "character varying(180)", maxLength: 180, nullable: false),
                Person1BirthDate = table.Column<DateOnly>(type: "date", nullable: true),
                Person2Name = table.Column<string>(type: "character varying(180)", maxLength: 180, nullable: false),
                Person2BirthDate = table.Column<DateOnly>(type: "date", nullable: true),
                CompatibilityScore = table.Column<int>(type: "integer", nullable: false),
                ResponseJson = table.Column<string>(type: "jsonb", nullable: false),
                Model = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: true),
                CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_compatibility_readings", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "name_readings",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                FullName = table.Column<string>(type: "character varying(180)", maxLength: 180, nullable: false),
                NumerologyNumber = table.Column<int>(type: "integer", nullable: false),
                Archetype = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                ResponseJson = table.Column<string>(type: "jsonb", nullable: false),
                Model = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: true),
                CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_name_readings", x => x.Id);
            });
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(name: "birthdate_readings");
        migrationBuilder.DropTable(name: "compatibility_readings");
        migrationBuilder.DropTable(name: "name_readings");
    }
}
