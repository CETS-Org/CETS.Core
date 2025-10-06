using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class EditTeachDateType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Slot",
                table: "HR_TeacherAvailabilities");

            migrationBuilder.DropColumn(
                name: "TeachDate",
                table: "HR_TeacherAvailabilities");

            migrationBuilder.AddColumn<string>(
                name: "TeachDay",
                table: "HR_TeacherAvailabilities",
                type: "nvarchar(20)",
                maxLength: 20,
                precision: 0,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "TimeSlotID",
                table: "HR_TeacherAvailabilities",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<string>(
                name: "DayOfWeek",
                table: "ACAD_CourseSchedules",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_HR_TeacherAvailabilities_TimeSlotID",
                table: "HR_TeacherAvailabilities",
                column: "TimeSlotID");

            migrationBuilder.AddForeignKey(
                name: "FK_HR_TeacherAvailabilities_CORE_LookUps_TimeSlotID",
                table: "HR_TeacherAvailabilities",
                column: "TimeSlotID",
                principalTable: "CORE_LookUps",
                principalColumn: "LookUpID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HR_TeacherAvailabilities_CORE_LookUps_TimeSlotID",
                table: "HR_TeacherAvailabilities");

            migrationBuilder.DropIndex(
                name: "IX_HR_TeacherAvailabilities_TimeSlotID",
                table: "HR_TeacherAvailabilities");

            migrationBuilder.DropColumn(
                name: "TeachDay",
                table: "HR_TeacherAvailabilities");

            migrationBuilder.DropColumn(
                name: "TimeSlotID",
                table: "HR_TeacherAvailabilities");

            migrationBuilder.AddColumn<int>(
                name: "Slot",
                table: "HR_TeacherAvailabilities",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "TeachDate",
                table: "HR_TeacherAvailabilities",
                type: "datetime2(0)",
                precision: 0,
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<string>(
                name: "DayOfWeek",
                table: "ACAD_CourseSchedules",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);
        }
    }
}
