using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CreateCourseSchedule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ACAD_CourseSchedules",
                columns: table => new
                {
                    CourseScheduleID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CourseID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LookUpID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DayOfWeek = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2(0)", precision: 0, nullable: false, defaultValueSql: "(sysutcdatetime())"),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2(0)", precision: 0, nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ACAD_CourseSchedules", x => x.CourseScheduleID);
                    table.ForeignKey(
                        name: "FK_ACAD_CourseSchedules_Course",
                        column: x => x.CourseID,
                        principalTable: "ACAD_Courses",
                        principalColumn: "CourseID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ACAD_CourseSchedules_TimeSlot",
                        column: x => x.LookUpID,
                        principalTable: "CORE_LookUps",
                        principalColumn: "LookUpID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ACAD_CourseSchedules_CourseID",
                table: "ACAD_CourseSchedules",
                column: "CourseID");

            migrationBuilder.CreateIndex(
                name: "IX_ACAD_CourseSchedules_LookUpID",
                table: "ACAD_CourseSchedules",
                column: "LookUpID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ACAD_CourseSchedules");
        }
    }
}
