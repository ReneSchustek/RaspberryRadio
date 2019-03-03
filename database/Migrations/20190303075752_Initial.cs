using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Database.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppConfiguration",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    CreatedById = table.Column<int>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    UpdatedById = table.Column<int>(nullable: true),
                    Lang = table.Column<string>(nullable: true),
                    Client = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppConfiguration", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppInfo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    CreatedById = table.Column<int>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    UpdatedById = table.Column<int>(nullable: true),
                    Info = table.Column<string>(maxLength: 255, nullable: true),
                    Software = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppInfo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppToken",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    CreatedById = table.Column<int>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    UpdatedById = table.Column<int>(nullable: true),
                    DirbleToken = table.Column<string>(maxLength: 255, nullable: true),
                    OpenWeatherToken = table.Column<string>(maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppToken", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Calendars",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    CreatedById = table.Column<int>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    UpdatedById = table.Column<int>(nullable: true),
                    Name = table.Column<string>(maxLength: 255, nullable: true),
                    Username = table.Column<string>(maxLength: 255, nullable: true),
                    Password = table.Column<string>(maxLength: 255, nullable: true),
                    Url = table.Column<string>(maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Calendars", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DailyScripture",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    CreatedById = table.Column<int>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    UpdatedById = table.Column<int>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    Text = table.Column<string>(nullable: true),
                    Comment = table.Column<string>(nullable: true),
                    Language = table.Column<string>(nullable: true),
                    Publication = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DailyScripture", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DailyScriptureLanguage",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    CreatedById = table.Column<int>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    UpdatedById = table.Column<int>(nullable: true),
                    Language = table.Column<string>(nullable: false),
                    Url = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DailyScriptureLanguage", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OpenWeatherCity",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    CreatedById = table.Column<int>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    UpdatedById = table.Column<int>(nullable: true),
                    CityId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Country = table.Column<string>(nullable: false),
                    Lon = table.Column<decimal>(nullable: false),
                    Lat = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OpenWeatherCity", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OpenWeatherSavedCities",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    CreatedById = table.Column<int>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    UpdatedById = table.Column<int>(nullable: true),
                    WeatherCitiesId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OpenWeatherSavedCities", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppConfiguration");

            migrationBuilder.DropTable(
                name: "AppInfo");

            migrationBuilder.DropTable(
                name: "AppToken");

            migrationBuilder.DropTable(
                name: "Calendars");

            migrationBuilder.DropTable(
                name: "DailyScripture");

            migrationBuilder.DropTable(
                name: "DailyScriptureLanguage");

            migrationBuilder.DropTable(
                name: "OpenWeatherCity");

            migrationBuilder.DropTable(
                name: "OpenWeatherSavedCities");
        }
    }
}
