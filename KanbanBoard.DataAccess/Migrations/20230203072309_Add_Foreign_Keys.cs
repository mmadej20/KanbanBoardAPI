using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddForeignKeys : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ToDos_Boards_BoardId",
                table: "ToDos");

            migrationBuilder.AlterColumn<int>(
                name: "BoardId",
                table: "ToDos",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AssignedMemberId",
                table: "ToDos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ToDos_AssignedMemberId",
                table: "ToDos",
                column: "AssignedMemberId");

            migrationBuilder.AddForeignKey(
                name: "FK_ToDos_Boards_BoardId",
                table: "ToDos",
                column: "BoardId",
                principalTable: "Boards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ToDos_Members_AssignedMemberId",
                table: "ToDos",
                column: "AssignedMemberId",
                principalTable: "Members",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ToDos_Boards_BoardId",
                table: "ToDos");

            migrationBuilder.DropForeignKey(
                name: "FK_ToDos_Members_AssignedMemberId",
                table: "ToDos");

            migrationBuilder.DropIndex(
                name: "IX_ToDos_AssignedMemberId",
                table: "ToDos");

            migrationBuilder.DropColumn(
                name: "AssignedMemberId",
                table: "ToDos");

            migrationBuilder.AlterColumn<int>(
                name: "BoardId",
                table: "ToDos",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_ToDos_Boards_BoardId",
                table: "ToDos",
                column: "BoardId",
                principalTable: "Boards",
                principalColumn: "Id");
        }
    }
}
