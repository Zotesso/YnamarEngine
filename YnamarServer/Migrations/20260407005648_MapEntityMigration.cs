using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YnamarServer.Migrations
{
    /// <inheritdoc />
    public partial class MapEntityMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MapLayer_Maps_MapId",
                table: "MapLayer");

            migrationBuilder.DropIndex(
                name: "IX_MapLayer_MapId",
                table: "MapLayer");

            migrationBuilder.AddColumn<string>(
                name: "FilePath",
                table: "Maps",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FilePath",
                table: "Maps");

            migrationBuilder.CreateIndex(
                name: "IX_MapLayer_MapId",
                table: "MapLayer",
                column: "MapId");

            migrationBuilder.AddForeignKey(
                name: "FK_MapLayer_Maps_MapId",
                table: "MapLayer",
                column: "MapId",
                principalTable: "Maps",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
