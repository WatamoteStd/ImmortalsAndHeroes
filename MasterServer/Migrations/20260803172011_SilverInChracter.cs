using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MasterServer.Migrations
{
    /// <inheritdoc />
    public partial class SilverInChracter : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "Silver",
                table: "Characters",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Silver",
                table: "Characters");
        }
    }
}
