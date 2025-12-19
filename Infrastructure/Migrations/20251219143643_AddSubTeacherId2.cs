using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSubTeacherId2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "SubTeacherAssignmentID",
                table: "ACAD_Classes",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ACAD_Classes_SubTeacherAssignmentID",
                table: "ACAD_Classes",
                column: "SubTeacherAssignmentID");

            migrationBuilder.AddForeignKey(
                name: "FK_ACAD_Classes_ACAD_CourseTeacherAssignments_SubTeacherAssignmentID",
                table: "ACAD_Classes",
                column: "SubTeacherAssignmentID",
                principalTable: "ACAD_CourseTeacherAssignments",
                principalColumn: "AssignmentID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ACAD_Classes_ACAD_CourseTeacherAssignments_SubTeacherAssignmentID",
                table: "ACAD_Classes");

            migrationBuilder.DropIndex(
                name: "IX_ACAD_Classes_SubTeacherAssignmentID",
                table: "ACAD_Classes");

            migrationBuilder.DropColumn(
                name: "SubTeacherAssignmentID",
                table: "ACAD_Classes");
        }
    }
}
