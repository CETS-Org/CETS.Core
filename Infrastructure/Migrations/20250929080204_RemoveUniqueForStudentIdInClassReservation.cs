using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveUniqueForStudentIdInClassReservation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "UQ_ACAD_ClassReservations_Student",
                table: "ACAD_ClassReservations");

            migrationBuilder.CreateIndex(
                name: "IX_ACAD_ClassReservations_StudentID",
                table: "ACAD_ClassReservations",
                column: "StudentID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ACAD_ClassReservations_StudentID",
                table: "ACAD_ClassReservations");

            migrationBuilder.CreateIndex(
                name: "UQ_ACAD_ClassReservations_Student",
                table: "ACAD_ClassReservations",
                column: "StudentID",
                unique: true);
        }
    }
}
