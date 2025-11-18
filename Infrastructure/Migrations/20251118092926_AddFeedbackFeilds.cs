using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddFeedbackFeilds : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Comment",
                table: "COM_Feedback",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "CommunicationSkills",
                table: "COM_Feedback",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ContentClarity",
                table: "COM_Feedback",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CourseRelevance",
                table: "COM_Feedback",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MaterialsQuality",
                table: "COM_Feedback",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TeacherSupportiveness",
                table: "COM_Feedback",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TeachingEffectiveness",
                table: "COM_Feedback",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CommunicationSkills",
                table: "COM_Feedback");

            migrationBuilder.DropColumn(
                name: "ContentClarity",
                table: "COM_Feedback");

            migrationBuilder.DropColumn(
                name: "CourseRelevance",
                table: "COM_Feedback");

            migrationBuilder.DropColumn(
                name: "MaterialsQuality",
                table: "COM_Feedback");

            migrationBuilder.DropColumn(
                name: "TeacherSupportiveness",
                table: "COM_Feedback");

            migrationBuilder.DropColumn(
                name: "TeachingEffectiveness",
                table: "COM_Feedback");

            migrationBuilder.AlterColumn<string>(
                name: "Comment",
                table: "COM_Feedback",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
