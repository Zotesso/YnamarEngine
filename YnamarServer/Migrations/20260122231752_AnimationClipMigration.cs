using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace YnamarServer.Migrations
{
    /// <inheritdoc />
    public partial class AnimationClipMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AnimationClips",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Loop = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnimationClips", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AnimationFrames",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AnimationClipId = table.Column<int>(type: "integer", nullable: false),
                    TextureId = table.Column<int>(type: "integer", nullable: false),
                    SourceX = table.Column<int>(type: "integer", nullable: false),
                    SourceY = table.Column<int>(type: "integer", nullable: false),
                    SourceWidth = table.Column<int>(type: "integer", nullable: false),
                    SourceHeight = table.Column<int>(type: "integer", nullable: false),
                    Duration = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnimationFrames", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnimationFrames_AnimationClips_AnimationClipId",
                        column: x => x.AnimationClipId,
                        principalTable: "AnimationClips",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AnimationFrames_AnimationClipId",
                table: "AnimationFrames",
                column: "AnimationClipId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnimationFrames");

            migrationBuilder.DropTable(
                name: "AnimationClips");
        }
    }
}
