using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YnamarServer.Migrations
{
    /// <inheritdoc />
    public partial class AnimationFramePolygonsMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SourceHeight",
                table: "AnimationFrames");

            migrationBuilder.DropColumn(
                name: "SourceWidth",
                table: "AnimationFrames");

            migrationBuilder.DropColumn(
                name: "SourceX",
                table: "AnimationFrames");

            migrationBuilder.DropColumn(
                name: "SourceY",
                table: "AnimationFrames");

            migrationBuilder.AddColumn<string>(
                name: "Polygons",
                table: "AnimationFrames",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Polygons",
                table: "AnimationFrames");

            migrationBuilder.AddColumn<int>(
                name: "SourceHeight",
                table: "AnimationFrames",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SourceWidth",
                table: "AnimationFrames",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SourceX",
                table: "AnimationFrames",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SourceY",
                table: "AnimationFrames",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
