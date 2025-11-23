using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddRoomStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "RoomStatusId",
                table: "FAC_Rooms",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_FAC_Rooms_RoomStatusId",
                table: "FAC_Rooms",
                column: "RoomStatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_FAC_Rooms_RoomStatus",
                table: "FAC_Rooms",
                column: "RoomStatusId",
                principalTable: "CORE_LookUps",
                principalColumn: "LookUpID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FAC_Rooms_RoomStatus",
                table: "FAC_Rooms");

            migrationBuilder.DropIndex(
                name: "IX_FAC_Rooms_RoomStatusId",
                table: "FAC_Rooms");

            migrationBuilder.DropColumn(
                name: "RoomStatusId",
                table: "FAC_Rooms");
        }
    }
}
