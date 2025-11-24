using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddClassTransferMeetingDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<DateOnly>(
                name: "FromMeetingDate",
                table: "ACAD_AcademicRequests",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "FromSlotID",
                table: "ACAD_AcademicRequests",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateOnly>(
                name: "ToMeetingDate",
                table: "ACAD_AcademicRequests",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ToSlotID",
                table: "ACAD_AcademicRequests",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ACAD_AcademicRequests_FromSlotID",
                table: "ACAD_AcademicRequests",
                column: "FromSlotID");

            migrationBuilder.CreateIndex(
                name: "IX_ACAD_AcademicRequests_ToSlotID",
                table: "ACAD_AcademicRequests",
                column: "ToSlotID");

            migrationBuilder.AddForeignKey(
                name: "FK_ACAD_AcReq_ClassMeeting",
                table: "ACAD_AcademicRequests",
                column: "ClassMeetingID",
                principalTable: "ACAD_ClassMeetings",
                principalColumn: "MeetingID",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_ACAD_AcReq_FromSlot",
                table: "ACAD_AcademicRequests",
                column: "FromSlotID",
                principalTable: "CORE_LookUps",
                principalColumn: "LookUpID");

            migrationBuilder.AddForeignKey(
                name: "FK_ACAD_AcReq_NewRoom",
                table: "ACAD_AcademicRequests",
                column: "NewRoomID",
                principalTable: "FAC_Rooms",
                principalColumn: "RoomID",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_ACAD_AcReq_NewSlot",
                table: "ACAD_AcademicRequests",
                column: "NewSlotID",
                principalTable: "CORE_LookUps",
                principalColumn: "LookUpID");

            migrationBuilder.AddForeignKey(
                name: "FK_ACAD_AcReq_ToSlot",
                table: "ACAD_AcademicRequests",
                column: "ToSlotID",
                principalTable: "CORE_LookUps",
                principalColumn: "LookUpID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ACAD_AcReq_ClassMeeting",
                table: "ACAD_AcademicRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_ACAD_AcReq_FromSlot",
                table: "ACAD_AcademicRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_ACAD_AcReq_NewRoom",
                table: "ACAD_AcademicRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_ACAD_AcReq_NewSlot",
                table: "ACAD_AcademicRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_ACAD_AcReq_ToSlot",
                table: "ACAD_AcademicRequests");

            migrationBuilder.DropIndex(
                name: "IX_ACAD_AcademicRequests_FromSlotID",
                table: "ACAD_AcademicRequests");

            migrationBuilder.DropIndex(
                name: "IX_ACAD_AcademicRequests_ToSlotID",
                table: "ACAD_AcademicRequests");

            migrationBuilder.DropColumn(
                name: "FromMeetingDate",
                table: "ACAD_AcademicRequests");

            migrationBuilder.DropColumn(
                name: "FromSlotID",
                table: "ACAD_AcademicRequests");

            migrationBuilder.DropColumn(
                name: "ToMeetingDate",
                table: "ACAD_AcademicRequests");

            migrationBuilder.DropColumn(
                name: "ToSlotID",
                table: "ACAD_AcademicRequests");

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
    }
}
