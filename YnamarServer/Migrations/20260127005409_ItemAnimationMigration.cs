using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YnamarServer.Migrations
{
    /// <inheritdoc />
    public partial class ItemAnimationMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AnimationClipId",
                table: "Items",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Items_AnimationClipId",
                table: "Items",
                column: "AnimationClipId");

            migrationBuilder.AddForeignKey(
                name: "FK_Items_AnimationClips_AnimationClipId",
                table: "Items",
                column: "AnimationClipId",
                principalTable: "AnimationClips",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_AnimationClips_AnimationClipId",
                table: "Items");

            migrationBuilder.DropIndex(
                name: "IX_Items_AnimationClipId",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "AnimationClipId",
                table: "Items");
        }
    }
}
