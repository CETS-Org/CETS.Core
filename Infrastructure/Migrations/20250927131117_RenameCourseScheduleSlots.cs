using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RenameCourseScheduleSlots : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LookUpID",
                table: "ACAD_CourseSchedules",
                newName: "TimeSlotID");

            migrationBuilder.RenameIndex(
                name: "IX_ACAD_CourseSchedules_LookUpID",
                table: "ACAD_CourseSchedules",
                newName: "IX_ACAD_CourseSchedules_TimeSlotID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TimeSlotID",
                table: "ACAD_CourseSchedules",
                newName: "LookUpID");

            migrationBuilder.RenameIndex(
                name: "IX_ACAD_CourseSchedules_TimeSlotID",
                table: "ACAD_CourseSchedules",
                newName: "IX_ACAD_CourseSchedules_LookUpID");
        }
    }
}
