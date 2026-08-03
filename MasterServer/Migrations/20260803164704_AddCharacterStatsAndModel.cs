using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MasterServer.Migrations
{
    /// <inheritdoc />
    public partial class AddCharacterStatsAndModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CurrentHp",
                table: "Characters",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CurrentMp",
                table: "Characters",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Lvl",
                table: "Characters",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ModelId",
                table: "Characters",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrentHp",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "CurrentMp",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Lvl",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "ModelId",
                table: "Characters");
        }
    }
}
