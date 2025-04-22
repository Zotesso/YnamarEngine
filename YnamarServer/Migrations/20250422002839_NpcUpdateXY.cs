using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YnamarServer.Migrations
{
    /// <inheritdoc />
    public partial class NpcUpdateXY : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "X",
                table: "Npcs");

            migrationBuilder.DropColumn(
                name: "Y",
                table: "Npcs");

            migrationBuilder.AddColumn<int>(
                name: "X",
                table: "MapNpc",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Y",
                table: "MapNpc",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "X",
                table: "MapNpc");

            migrationBuilder.DropColumn(
                name: "Y",
                table: "MapNpc");

            migrationBuilder.AddColumn<int>(
                name: "X",
                table: "Npcs",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Y",
                table: "Npcs",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
