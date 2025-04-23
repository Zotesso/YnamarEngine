using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YnamarServer.Migrations
{
    /// <inheritdoc />
    public partial class NpcUpdateDir : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte>(
                name: "Dir",
                table: "MapNpc",
                type: "smallint",
                nullable: false,
                defaultValue: (byte)0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Dir",
                table: "MapNpc");
        }
    }
}
