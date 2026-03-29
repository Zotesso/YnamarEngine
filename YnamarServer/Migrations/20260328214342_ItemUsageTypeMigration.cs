using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace YnamarServer.Migrations
{
    /// <inheritdoc />
    public partial class ItemUsageTypeMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_InventorySlots",
                table: "InventorySlots");

            migrationBuilder.DropIndex(
                name: "IX_InventorySlots_InventoryId",
                table: "InventorySlots");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "InventorySlots");

            migrationBuilder.AddColumn<int>(
                name: "UsageType",
                table: "Items",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_InventorySlots",
                table: "InventorySlots",
                columns: new[] { "InventoryId", "SlotId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_InventorySlots",
                table: "InventorySlots");

            migrationBuilder.DropColumn(
                name: "UsageType",
                table: "Items");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "InventorySlots",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_InventorySlots",
                table: "InventorySlots",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_InventorySlots_InventoryId",
                table: "InventorySlots",
                column: "InventoryId");
        }
    }
}
