using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YnamarServer.Migrations
{
    /// <inheritdoc />
    public partial class MapMetadataVersionMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastUpdate",
                table: "MapsMetadata");

            migrationBuilder.AddColumn<int>(
                name: "Version",
                table: "MapsMetadata",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Version",
                table: "MapsMetadata");

            migrationBuilder.AddColumn<byte[]>(
                name: "LastUpdate",
                table: "MapsMetadata",
                type: "bytea",
                rowVersion: true,
                nullable: false,
                defaultValue: new byte[0]);
        }
    }
}
