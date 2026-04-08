using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace YnamarServer.Migrations
{
    /// <inheritdoc />
    public partial class MapMetadataMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                @"DO $$
                BEGIN
                    IF EXISTS (
                        SELECT 1 FROM information_schema.table_constraints
                        WHERE constraint_name = 'FK_MapLayer_Maps_MapId'
                    ) THEN
                        ALTER TABLE ""MapLayer"" DROP CONSTRAINT ""FK_MapLayer_Maps_MapId"";
                    END IF;
                END $$;");
            migrationBuilder.DropPrimaryKey(
                name: "PK_Maps",
                table: "Maps");

            migrationBuilder.RenameTable(
                name: "Maps",
                newName: "Map");

            migrationBuilder.AddColumn<int>(
                name: "MapMetadataId",
                table: "MapLayer",
                type: "integer",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Map",
                table: "Map",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "MapsMetadata",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    MaxMapX = table.Column<int>(type: "integer", nullable: false),
                    MaxMapY = table.Column<int>(type: "integer", nullable: false),
                    LastUpdate = table.Column<byte[]>(type: "bytea", rowVersion: true, nullable: false),
                    FilePath = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MapsMetadata", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MapLayer_MapMetadataId",
                table: "MapLayer",
                column: "MapMetadataId");

            migrationBuilder.AddForeignKey(
                name: "FK_MapLayer_Map_MapId",
                table: "MapLayer",
                column: "MapId",
                principalTable: "Map",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MapLayer_MapsMetadata_MapMetadataId",
                table: "MapLayer",
                column: "MapMetadataId",
                principalTable: "MapsMetadata",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MapLayer_Map_MapId",
                table: "MapLayer");

            migrationBuilder.DropForeignKey(
                name: "FK_MapLayer_MapsMetadata_MapMetadataId",
                table: "MapLayer");

            migrationBuilder.DropTable(
                name: "MapsMetadata");

            migrationBuilder.DropIndex(
                name: "IX_MapLayer_MapMetadataId",
                table: "MapLayer");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Map",
                table: "Map");

            migrationBuilder.DropColumn(
                name: "MapMetadataId",
                table: "MapLayer");

            migrationBuilder.RenameTable(
                name: "Map",
                newName: "Maps");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Maps",
                table: "Maps",
                column: "Id");

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
