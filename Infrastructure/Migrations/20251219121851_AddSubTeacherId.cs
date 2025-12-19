using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSubTeacherId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "SubTeacherAssignmentID",
                table: "ACAD_ClassMeetings",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ACAD_ClassMeetings_SubTeacherAssignmentID",
                table: "ACAD_ClassMeetings",
                column: "SubTeacherAssignmentID");

            migrationBuilder.AddForeignKey(
                name: "FK_ACAD_ClassMeetings_ACAD_CourseTeacherAssignments_SubTeacherAssignmentID",
                table: "ACAD_ClassMeetings",
                column: "SubTeacherAssignmentID",
                principalTable: "ACAD_CourseTeacherAssignments",
                principalColumn: "AssignmentID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ACAD_ClassMeetings_ACAD_CourseTeacherAssignments_SubTeacherAssignmentID",
                table: "ACAD_ClassMeetings");

            migrationBuilder.DropIndex(
                name: "IX_ACAD_ClassMeetings_SubTeacherAssignmentID",
                table: "ACAD_ClassMeetings");

            migrationBuilder.DropColumn(
                name: "SubTeacherAssignmentID",
                table: "ACAD_ClassMeetings");
        }
    }
}
