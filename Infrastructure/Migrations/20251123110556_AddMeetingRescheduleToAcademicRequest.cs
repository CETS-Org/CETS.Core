using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddMeetingRescheduleToAcademicRequest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ClassMeetingID",
                table: "ACAD_AcademicRequests",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateOnly>(
                name: "NewMeetingDate",
                table: "ACAD_AcademicRequests",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "NewRoomID",
                table: "ACAD_AcademicRequests",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "NewSlotID",
                table: "ACAD_AcademicRequests",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ACAD_AcademicRequests_ClassMeetingID",
                table: "ACAD_AcademicRequests",
                column: "ClassMeetingID");

            migrationBuilder.CreateIndex(
                name: "IX_ACAD_AcademicRequests_NewRoomID",
                table: "ACAD_AcademicRequests",
                column: "NewRoomID");

            migrationBuilder.CreateIndex(
                name: "IX_ACAD_AcademicRequests_NewSlotID",
                table: "ACAD_AcademicRequests",
                column: "NewSlotID");

            migrationBuilder.AddForeignKey(
                name: "FK_ACAD_AcademicRequests_ACAD_ClassMeetings_ClassMeetingID",
                table: "ACAD_AcademicRequests",
                column: "ClassMeetingID",
                principalTable: "ACAD_ClassMeetings",
                principalColumn: "MeetingID");

            migrationBuilder.AddForeignKey(
                name: "FK_ACAD_AcademicRequests_CORE_LookUps_NewSlotID",
                table: "ACAD_AcademicRequests",
                column: "NewSlotID",
                principalTable: "CORE_LookUps",
                principalColumn: "LookUpID");

            migrationBuilder.AddForeignKey(
                name: "FK_ACAD_AcademicRequests_FAC_Rooms_NewRoomID",
                table: "ACAD_AcademicRequests",
                column: "NewRoomID",
                principalTable: "FAC_Rooms",
                principalColumn: "RoomID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ACAD_AcademicRequests_ACAD_ClassMeetings_ClassMeetingID",
                table: "ACAD_AcademicRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_ACAD_AcademicRequests_CORE_LookUps_NewSlotID",
                table: "ACAD_AcademicRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_ACAD_AcademicRequests_FAC_Rooms_NewRoomID",
                table: "ACAD_AcademicRequests");

            migrationBuilder.DropIndex(
                name: "IX_ACAD_AcademicRequests_ClassMeetingID",
                table: "ACAD_AcademicRequests");

            migrationBuilder.DropIndex(
                name: "IX_ACAD_AcademicRequests_NewRoomID",
                table: "ACAD_AcademicRequests");

            migrationBuilder.DropIndex(
                name: "IX_ACAD_AcademicRequests_NewSlotID",
                table: "ACAD_AcademicRequests");

            migrationBuilder.DropColumn(
                name: "ClassMeetingID",
                table: "ACAD_AcademicRequests");

            migrationBuilder.DropColumn(
                name: "NewMeetingDate",
                table: "ACAD_AcademicRequests");

            migrationBuilder.DropColumn(
                name: "NewRoomID",
                table: "ACAD_AcademicRequests");

            migrationBuilder.DropColumn(
                name: "NewSlotID",
                table: "ACAD_AcademicRequests");
        }
    }
}
