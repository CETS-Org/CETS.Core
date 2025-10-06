using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDayConversion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "TeachDay",
                table: "HR_TeacherAvailabilities",
                type: "int",
                precision: 0,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20,
                oldPrecision: 0);

            migrationBuilder.AlterColumn<int>(
                name: "DayOfWeek",
                table: "ACAD_CourseSchedules",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "TeachDay",
                table: "HR_TeacherAvailabilities",
                type: "nvarchar(20)",
                maxLength: 20,
                precision: 0,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldPrecision: 0);

            migrationBuilder.AlterColumn<string>(
                name: "DayOfWeek",
                table: "ACAD_CourseSchedules",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
