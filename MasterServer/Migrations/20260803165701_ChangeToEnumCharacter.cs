using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MasterServer.Migrations
{
    /// <inheritdoc />
    public partial class ChangeToEnumCharacter : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ModelId",
                table: "Characters");

            migrationBuilder.AddColumn<long>(
                name: "CharacterId",
                table: "Characters",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CharacterId",
                table: "Characters");

            migrationBuilder.AddColumn<int>(
                name: "ModelId",
                table: "Characters",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
