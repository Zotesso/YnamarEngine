using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YnamarServer.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedItemSpriteMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Sprite",
                table: "Items");

            migrationBuilder.AddColumn<int>(
                name: "Sprite",
                table: "Items",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Sprite",
                table: "Items",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");
        }
    }
}
