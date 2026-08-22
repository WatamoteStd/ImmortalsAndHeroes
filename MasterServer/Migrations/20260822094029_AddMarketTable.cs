using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MasterServer.Migrations
{
    /// <inheritdoc />
    public partial class AddMarketTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GlobalMarket",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    LocationId = table.Column<long>(type: "bigint", nullable: false),
                    SellerCharacterId = table.Column<long>(type: "bigint", nullable: false),
                    SellerName = table.Column<string>(type: "text", nullable: false),
                    ItemType = table.Column<long>(type: "bigint", nullable: false),
                    Quality = table.Column<byte>(type: "smallint", nullable: false),
                    Count = table.Column<long>(type: "bigint", nullable: false),
                    PricePerUnit = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsCityOrder = table.Column<bool>(type: "boolean", nullable: false),
                    IsSold = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GlobalMarket", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GlobalMarket_LocationId_IsSold_ItemType",
                table: "GlobalMarket",
                columns: new[] { "LocationId", "IsSold", "ItemType" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GlobalMarket");
        }
    }
}
