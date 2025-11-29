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
            migrationBuilder.RenameColumn(
                name: "ExitSurveyUrl",
                table: "ACAD_AcademicRequests",
                newName: "ExitSurveyId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ExitSurveyId",
                table: "ACAD_AcademicRequests",
                newName: "ExitSurveyUrl");
        }
    }
}
