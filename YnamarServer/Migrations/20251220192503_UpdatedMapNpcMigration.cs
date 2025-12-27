using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YnamarServer.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedMapNpcMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MapNpc_MapLayer_LayerId",
                table: "MapNpc");

            migrationBuilder.DropForeignKey(
                name: "FK_MapNpc_Npcs_NpcId",
                table: "MapNpc");

            migrationBuilder.AlterColumn<int>(
                name: "LayerId",
                table: "MapNpc",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_MapNpc_MapLayer_LayerId",
                table: "MapNpc",
                column: "LayerId",
                principalTable: "MapLayer",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MapNpc_Npcs_NpcId",
                table: "MapNpc",
                column: "NpcId",
                principalTable: "Npcs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MapNpc_MapLayer_LayerId",
                table: "MapNpc");

            migrationBuilder.DropForeignKey(
                name: "FK_MapNpc_Npcs_NpcId",
                table: "MapNpc");

            migrationBuilder.AlterColumn<int>(
                name: "LayerId",
                table: "MapNpc",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_MapNpc_MapLayer_LayerId",
                table: "MapNpc",
                column: "LayerId",
                principalTable: "MapLayer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MapNpc_Npcs_NpcId",
                table: "MapNpc",
                column: "NpcId",
                principalTable: "Npcs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
