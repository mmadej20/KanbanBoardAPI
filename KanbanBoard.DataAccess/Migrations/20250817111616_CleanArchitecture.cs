using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class CleanArchitecture : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ToDos_Members_AssignedMemberId",
                table: "ToDos");

            migrationBuilder.DropTable(
                name: "BoardMember");

            migrationBuilder.CreateTable(
                name: "BoardMemberEntity",
                columns: table => new
                {
                    BoardId = table.Column<int>(type: "int", nullable: false),
                    MemberId = table.Column<int>(type: "int", nullable: false),
                    JoinedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsOwner = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BoardMemberEntity", x => new { x.BoardId, x.MemberId });
                    table.ForeignKey(
                        name: "FK_BoardMemberEntity_Boards_BoardId",
                        column: x => x.BoardId,
                        principalTable: "Boards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BoardMemberEntity_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BoardMemberEntity_MemberId",
                table: "BoardMemberEntity",
                column: "MemberId");

            migrationBuilder.AddForeignKey(
                name: "FK_ToDos_Members_AssignedMemberId",
                table: "ToDos",
                column: "AssignedMemberId",
                principalTable: "Members",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ToDos_Members_AssignedMemberId",
                table: "ToDos");

            migrationBuilder.DropTable(
                name: "BoardMemberEntity");

            migrationBuilder.CreateTable(
                name: "BoardMember",
                columns: table => new
                {
                    BoardsId = table.Column<int>(type: "int", nullable: false),
                    MembersId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BoardMember", x => new { x.BoardsId, x.MembersId });
                    table.ForeignKey(
                        name: "FK_BoardMember_Boards_BoardsId",
                        column: x => x.BoardsId,
                        principalTable: "Boards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BoardMember_Members_MembersId",
                        column: x => x.MembersId,
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BoardMember_MembersId",
                table: "BoardMember",
                column: "MembersId");

            migrationBuilder.AddForeignKey(
                name: "FK_ToDos_Members_AssignedMemberId",
                table: "ToDos",
                column: "AssignedMemberId",
                principalTable: "Members",
                principalColumn: "Id");
        }
    }
}
