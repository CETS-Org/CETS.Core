using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUniquenessForTeacherAvailability : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_HR_TeacherAvailabilities_TeacherID",
                table: "HR_TeacherAvailabilities");

            migrationBuilder.CreateIndex(
                name: "UQ_HR_TeacherAvailabilities_Teacher_Day_Slot",
                table: "HR_TeacherAvailabilities",
                columns: new[] { "TeacherID", "TeachDay", "TimeSlotID" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "UQ_HR_TeacherAvailabilities_Teacher_Day_Slot",
                table: "HR_TeacherAvailabilities");

            migrationBuilder.CreateIndex(
                name: "IX_HR_TeacherAvailabilities_TeacherID",
                table: "HR_TeacherAvailabilities",
                column: "TeacherID");
        }
    }
}
