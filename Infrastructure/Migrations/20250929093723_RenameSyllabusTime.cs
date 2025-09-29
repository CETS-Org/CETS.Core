using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RenameSyllabusTime : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_ACAD_SyllabusItems_Minutes",
                table: "ACAD_SyllabusItems");

            migrationBuilder.RenameColumn(
                name: "EstimatedMinutes",
                table: "ACAD_SyllabusItems",
                newName: "TotalSlots");

            migrationBuilder.AddCheckConstraint(
                name: "CK_ACAD_SyllabusItems_Slots",
                table: "ACAD_SyllabusItems",
                sql: "[TotalSlots] > 0");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_ACAD_SyllabusItems_Slots",
                table: "ACAD_SyllabusItems");

            migrationBuilder.RenameColumn(
                name: "TotalSlots",
                table: "ACAD_SyllabusItems",
                newName: "EstimatedMinutes");

            migrationBuilder.AddCheckConstraint(
                name: "CK_ACAD_SyllabusItems_Minutes",
                table: "ACAD_SyllabusItems",
                sql: "[EstimatedMinutes] > 0");
        }
    }
}
