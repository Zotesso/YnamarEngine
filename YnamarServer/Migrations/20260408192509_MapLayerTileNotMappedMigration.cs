using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace YnamarServer.Migrations
{
    /// <inheritdoc />
    public partial class MapLayerTileNotMappedMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tile");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tile",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MapLayerId = table.Column<int>(type: "integer", nullable: false),
                    Data1 = table.Column<int>(type: "integer", nullable: false),
                    Data2 = table.Column<int>(type: "integer", nullable: false),
                    Data3 = table.Column<int>(type: "integer", nullable: false),
                    Moral = table.Column<byte>(type: "smallint", nullable: false),
                    TileX = table.Column<int>(type: "integer", nullable: false),
                    TileY = table.Column<int>(type: "integer", nullable: false),
                    TilesetNumber = table.Column<int>(type: "integer", nullable: false),
                    Type = table.Column<byte>(type: "smallint", nullable: false),
                    X = table.Column<int>(type: "integer", nullable: false),
                    Y = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tile_MapLayer_MapLayerId",
                        column: x => x.MapLayerId,
                        principalTable: "MapLayer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tile_MapLayerId",
                table: "Tile",
                column: "MapLayerId");
        }
    }
}
