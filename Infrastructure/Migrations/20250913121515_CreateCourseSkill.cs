using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CreateCourseSkill : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ACAD_CourseSkills",
                columns: table => new
                {
                    CourseBenefitID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CourseID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SkillID = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ACAD_CourseSkills", x => x.CourseBenefitID);
                    table.ForeignKey(
                        name: "FK_ACAD_CourseSkills_ACAD_Courses_CourseID",
                        column: x => x.CourseID,
                        principalTable: "ACAD_Courses",
                        principalColumn: "CourseID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ACAD_CourseSkills_CORE_LookUps_SkillID",
                        column: x => x.SkillID,
                        principalTable: "CORE_LookUps",
                        principalColumn: "LookUpID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ACAD_CourseSkills_CourseID",
                table: "ACAD_CourseSkills",
                column: "CourseID");

            migrationBuilder.CreateIndex(
                name: "IX_ACAD_CourseSkills_SkillID",
                table: "ACAD_CourseSkills",
                column: "SkillID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ACAD_CourseSkills");
        }
    }
}
