using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Learning.Infrastructure.Database.Statistics.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserStatisticsSnapshots",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserEmail = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    WordsPracticed = table.Column<int>(type: "integer", nullable: false),
                    ExercisesCompleted = table.Column<int>(type: "integer", nullable: false),
                    FillInTheGapsAccuracy = table.Column<float>(type: "real", nullable: false),
                    TranslateWordsAccuracy = table.Column<float>(type: "real", nullable: false),
                    ContrastTaskAccuracy = table.Column<float>(type: "real", nullable: false),
                    ReadingComprehensionAccuracy = table.Column<float>(type: "real", nullable: false),
                    UnknownWordsMarked = table.Column<int>(type: "integer", nullable: false),
                    WordsSavedToDeck = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserStatisticsSnapshots", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserStreaks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserEmail = table.Column<string>(type: "character varying(256)", unicode: false, maxLength: 256, nullable: false),
                    CurrentStreak = table.Column<int>(type: "integer", nullable: false),
                    LongestStreak = table.Column<int>(type: "integer", nullable: false),
                    LastPracticeDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserStreaks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WordProgressHistories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserEmail = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    SenseId = table.Column<Guid>(type: "uuid", nullable: false),
                    DeckId = table.Column<Guid>(type: "uuid", nullable: false),
                    Task = table.Column<string>(type: "text", nullable: false),
                    WasCorrect = table.Column<bool>(type: "boolean", nullable: false),
                    PracticedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WordProgressHistories", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserStatisticsSnapshots_UserEmail_Date",
                table: "UserStatisticsSnapshots",
                columns: new[] { "UserEmail", "Date" });

            migrationBuilder.CreateIndex(
                name: "IX_UserStreaks_UserEmail",
                table: "UserStreaks",
                column: "UserEmail",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WordProgressHistories_UserEmail_PracticedAt",
                table: "WordProgressHistories",
                columns: new[] { "UserEmail", "PracticedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_WordProgressHistories_UserEmail_SenseId",
                table: "WordProgressHistories",
                columns: new[] { "UserEmail", "SenseId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserStatisticsSnapshots");

            migrationBuilder.DropTable(
                name: "UserStreaks");

            migrationBuilder.DropTable(
                name: "WordProgressHistories");
        }
    }
}
