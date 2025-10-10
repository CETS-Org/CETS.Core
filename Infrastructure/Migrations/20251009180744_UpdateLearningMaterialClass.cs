using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateLearningMaterialClass : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ACAD_LearningMaterials_Class",
                table: "ACAD_LearningMaterials");

            migrationBuilder.RenameColumn(
                name: "ClassID",
                table: "ACAD_LearningMaterials",
                newName: "ClassMeetingID");

            migrationBuilder.RenameIndex(
                name: "IX_ACAD_LearningMaterials_ClassID",
                table: "ACAD_LearningMaterials",
                newName: "IX_ACAD_LearningMaterials_ClassMeetingID");

            migrationBuilder.AddForeignKey(
                name: "FK_ACAD_LearningMaterials_ClassMeeting",
                table: "ACAD_LearningMaterials",
                column: "ClassMeetingID",
                principalTable: "ACAD_ClassMeetings",
                principalColumn: "MeetingID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ACAD_LearningMaterials_ClassMeeting",
                table: "ACAD_LearningMaterials");

            migrationBuilder.RenameColumn(
                name: "ClassMeetingID",
                table: "ACAD_LearningMaterials",
                newName: "ClassID");

            migrationBuilder.RenameIndex(
                name: "IX_ACAD_LearningMaterials_ClassMeetingID",
                table: "ACAD_LearningMaterials",
                newName: "IX_ACAD_LearningMaterials_ClassID");

            migrationBuilder.AddForeignKey(
                name: "FK_ACAD_LearningMaterials_Class",
                table: "ACAD_LearningMaterials",
                column: "ClassID",
                principalTable: "ACAD_Classes",
                principalColumn: "ClassID");
        }
    }
}
