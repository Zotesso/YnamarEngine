using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YnamarServer.Migrations
{
    /// <inheritdoc />
    public partial class NpcRespawnUpdateMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InRespawn",
                table: "MapNpc");

            migrationBuilder.AddColumn<int>(
                name: "RespawnTime",
                table: "Npcs",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RespawnWait",
                table: "MapNpc",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RespawnTime",
                table: "Npcs");

            migrationBuilder.DropColumn(
                name: "RespawnWait",
                table: "MapNpc");

            migrationBuilder.AddColumn<bool>(
                name: "InRespawn",
                table: "MapNpc",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
