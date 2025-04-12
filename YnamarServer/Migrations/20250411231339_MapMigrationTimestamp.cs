using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YnamarServer.Migrations
{
    /// <inheritdoc />
    public partial class MapMigrationTimestamp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "LastUpdate",
                table: "Maps",
                type: "bytea",
                rowVersion: true,
                nullable: false,
                defaultValue: new byte[0]);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastUpdate",
                table: "Maps");
        }
    }
}
