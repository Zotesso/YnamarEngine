using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace YnamarServer.Migrations
{
    /// <inheritdoc />
    public partial class MapLayerMapIdNotMappedMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MapLayer_Map_MapId",
                table: "MapLayer");

            migrationBuilder.DropTable(
                name: "Map");

            migrationBuilder.Sql(
                @"DROP INDEX IF EXISTS ""IX_MapLayer_MapId"";");

            migrationBuilder.DropColumn(
                name: "MapId",
                table: "MapLayer");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MapId",
                table: "MapLayer",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Map",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    LastUpdate = table.Column<byte[]>(type: "bytea", rowVersion: true, nullable: false),
                    MaxMapX = table.Column<int>(type: "integer", nullable: false),
                    MaxMapY = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Map", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MapLayer_MapId",
                table: "MapLayer",
                column: "MapId");

            migrationBuilder.AddForeignKey(
                name: "FK_MapLayer_Map_MapId",
                table: "MapLayer",
                column: "MapId",
                principalTable: "Map",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
