using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddDropoutFieldsToAcademicRequest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "CompletedExitSurvey",
                table: "ACAD_AcademicRequests",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExitSurveyUrl",
                table: "ACAD_AcademicRequests",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompletedExitSurvey",
                table: "ACAD_AcademicRequests");

            migrationBuilder.DropColumn(
                name: "ExitSurveyUrl",
                table: "ACAD_AcademicRequests");
        }
    }
}
