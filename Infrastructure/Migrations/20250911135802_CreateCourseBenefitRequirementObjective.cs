using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CreateCourseBenefitRequirementObjective : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CourseObjective",
                table: "ACAD_Courses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ACAD_CourseBenefits",
                columns: table => new
                {
                    CourseBenefitID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CourseID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BenefitID = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ACAD_CourseBenefits", x => x.CourseBenefitID);
                    table.ForeignKey(
                        name: "FK_ACAD_CourseBenefits_ACAD_Courses_CourseID",
                        column: x => x.CourseID,
                        principalTable: "ACAD_Courses",
                        principalColumn: "CourseID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ACAD_CourseBenefits_CORE_LookUps_BenefitID",
                        column: x => x.BenefitID,
                        principalTable: "CORE_LookUps",
                        principalColumn: "LookUpID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ACAD_CourseRequirements",
                columns: table => new
                {
                    CourseRequirementID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CourseID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RequirementID = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ACAD_CourseRequirements", x => x.CourseRequirementID);
                    table.ForeignKey(
                        name: "FK_ACAD_CourseRequirements_ACAD_Courses_CourseID",
                        column: x => x.CourseID,
                        principalTable: "ACAD_Courses",
                        principalColumn: "CourseID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ACAD_CourseRequirements_CORE_LookUps_RequirementID",
                        column: x => x.RequirementID,
                        principalTable: "CORE_LookUps",
                        principalColumn: "LookUpID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ACAD_CourseBenefits_BenefitID",
                table: "ACAD_CourseBenefits",
                column: "BenefitID");

            migrationBuilder.CreateIndex(
                name: "IX_ACAD_CourseBenefits_CourseID",
                table: "ACAD_CourseBenefits",
                column: "CourseID");

            migrationBuilder.CreateIndex(
                name: "IX_ACAD_CourseRequirements_CourseID",
                table: "ACAD_CourseRequirements",
                column: "CourseID");

            migrationBuilder.CreateIndex(
                name: "IX_ACAD_CourseRequirements_RequirementID",
                table: "ACAD_CourseRequirements",
                column: "RequirementID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ACAD_CourseBenefits");

            migrationBuilder.DropTable(
                name: "ACAD_CourseRequirements");

            migrationBuilder.DropColumn(
                name: "CourseObjective",
                table: "ACAD_Courses");
        }
    }
}
