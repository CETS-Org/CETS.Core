using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddACAD_WeeklyFeedback : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ACAD_WeeklyFeedback",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClassID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClassMeetingID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TeacherID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StudentID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WeekNumber = table.Column<int>(type: "int", nullable: false),
                    Participation = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    AssignmentQuality = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    SkillProgress = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    NextStep = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    CustomNote = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(sysutcdatetime())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ACAD_WeeklyFeedback", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WeeklyFeedback_Class",
                        column: x => x.ClassID,
                        principalTable: "ACAD_Classes",
                        principalColumn: "ClassID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WeeklyFeedback_ClassMeeting",
                        column: x => x.ClassMeetingID,
                        principalTable: "ACAD_ClassMeetings",
                        principalColumn: "MeetingID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WeeklyFeedback_Student",
                        column: x => x.StudentID,
                        principalTable: "IDN_Students",
                        principalColumn: "AccountID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WeeklyFeedback_Teacher",
                        column: x => x.TeacherID,
                        principalTable: "IDN_Teachers",
                        principalColumn: "AccountID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ACAD_WeeklyFeedback_ClassID_StudentID_WeekNumber",
                table: "ACAD_WeeklyFeedback",
                columns: new[] { "ClassID", "StudentID", "WeekNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ACAD_WeeklyFeedback_ClassID_WeekNumber",
                table: "ACAD_WeeklyFeedback",
                columns: new[] { "ClassID", "WeekNumber" });

            migrationBuilder.CreateIndex(
                name: "IX_ACAD_WeeklyFeedback_ClassMeetingID",
                table: "ACAD_WeeklyFeedback",
                column: "ClassMeetingID");

            migrationBuilder.CreateIndex(
                name: "IX_ACAD_WeeklyFeedback_StudentID",
                table: "ACAD_WeeklyFeedback",
                column: "StudentID");

            migrationBuilder.CreateIndex(
                name: "IX_ACAD_WeeklyFeedback_TeacherID",
                table: "ACAD_WeeklyFeedback",
                column: "TeacherID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ACAD_WeeklyFeedback");
        }
    }
}
