using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveColumnsInAcademicRequest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ACAD_AcReq_NewSlot",
                table: "ACAD_AcademicRequests");

            migrationBuilder.DropIndex(
                name: "IX_ACAD_AcademicRequests_NewSlotID",
                table: "ACAD_AcademicRequests");

            migrationBuilder.DropColumn(
                name: "NewMeetingDate",
                table: "ACAD_AcademicRequests");

            migrationBuilder.DropColumn(
                name: "NewSlotID",
                table: "ACAD_AcademicRequests");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateOnly>(
                name: "NewMeetingDate",
                table: "ACAD_AcademicRequests",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "NewSlotID",
                table: "ACAD_AcademicRequests",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ACAD_AcademicRequests_NewSlotID",
                table: "ACAD_AcademicRequests",
                column: "NewSlotID");

            migrationBuilder.AddForeignKey(
                name: "FK_ACAD_AcReq_NewSlot",
                table: "ACAD_AcademicRequests",
                column: "NewSlotID",
                principalTable: "CORE_LookUps",
                principalColumn: "LookUpID");
        }
    }
}
