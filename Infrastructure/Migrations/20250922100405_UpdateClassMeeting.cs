using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateClassMeeting : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_ACAD_ClassMeetings_Times",
                table: "ACAD_ClassMeetings");

            migrationBuilder.DropColumn(
                name: "EndsAt",
                table: "ACAD_ClassMeetings");

            migrationBuilder.DropColumn(
                name: "StartsAt",
                table: "ACAD_ClassMeetings");

            migrationBuilder.AddColumn<DateOnly>(
                name: "Date",
                table: "ACAD_ClassMeetings",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<bool>(
                name: "IsStudy",
                table: "ACAD_ClassMeetings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "SlotID",
                table: "ACAD_ClassMeetings",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_ACAD_ClassMeetings_SlotID",
                table: "ACAD_ClassMeetings",
                column: "SlotID");

            migrationBuilder.AddForeignKey(
                name: "FK_ACAD_ClassMeetings_Slot",
                table: "ACAD_ClassMeetings",
                column: "SlotID",
                principalTable: "CORE_LookUps",
                principalColumn: "LookUpID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ACAD_ClassMeetings_Slot",
                table: "ACAD_ClassMeetings");

            migrationBuilder.DropIndex(
                name: "IX_ACAD_ClassMeetings_SlotID",
                table: "ACAD_ClassMeetings");

            migrationBuilder.DropColumn(
                name: "Date",
                table: "ACAD_ClassMeetings");

            migrationBuilder.DropColumn(
                name: "IsStudy",
                table: "ACAD_ClassMeetings");

            migrationBuilder.DropColumn(
                name: "SlotID",
                table: "ACAD_ClassMeetings");

            migrationBuilder.AddColumn<DateTime>(
                name: "EndsAt",
                table: "ACAD_ClassMeetings",
                type: "datetime2(0)",
                precision: 0,
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "StartsAt",
                table: "ACAD_ClassMeetings",
                type: "datetime2(0)",
                precision: 0,
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddCheckConstraint(
                name: "CK_ACAD_ClassMeetings_Times",
                table: "ACAD_ClassMeetings",
                sql: "[EndsAt] > [StartsAt]");
        }
    }
}
