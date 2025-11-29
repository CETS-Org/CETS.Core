using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RenameExitSurveyUrlToExitSurveyId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Rename column from ExitSurveyUrl to ExitSurveyId
            migrationBuilder.RenameColumn(
                name: "ExitSurveyUrl",
                table: "ACAD_AcademicRequests",
                newName: "ExitSurveyId");

            // Alter column type to nvarchar(100) with max length
            migrationBuilder.AlterColumn<string>(
                name: "ExitSurveyId",
                table: "ACAD_AcademicRequests",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Revert column type back to nvarchar(max)
            migrationBuilder.AlterColumn<string>(
                name: "ExitSurveyId",
                table: "ACAD_AcademicRequests",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            // Rename column back from ExitSurveyId to ExitSurveyUrl
            migrationBuilder.RenameColumn(
                name: "ExitSurveyId",
                table: "ACAD_AcademicRequests",
                newName: "ExitSurveyUrl");
        }
    }
}

