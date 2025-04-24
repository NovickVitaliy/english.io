using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace English.IO.Bot.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddUserEmailProperty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserEmail",
                table: "Users",
                type: "varchar(50)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserEmail",
                table: "Users");
        }
    }
}
