using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateFkPackageCourseForClassReservation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ACAD_ClassReservations_ACAD_CoursePackages_CoursePackageID",
                table: "ACAD_ClassReservations");

            migrationBuilder.DropIndex(
                name: "IX_ACAD_ClassReservations_CoursePackageID",
                table: "ACAD_ClassReservations");

            migrationBuilder.RenameColumn(
                name: "ReservationID",
                table: "ACAD_ClassReservations",
                newName: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_ACAD_ClassReservations_CoursePackageID",
                table: "ACAD_ClassReservations",
                column: "CoursePackageID");

            migrationBuilder.AddForeignKey(
                name: "FK_ACAD_ClassReservations_Package",
                table: "ACAD_ClassReservations",
                column: "CoursePackageID",
                principalTable: "ACAD_CoursePackages",
                principalColumn: "PackageID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ACAD_ClassReservations_Package",
                table: "ACAD_ClassReservations");

            migrationBuilder.DropIndex(
                name: "IX_ACAD_ClassReservations_CoursePackageID",
                table: "ACAD_ClassReservations");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "ACAD_ClassReservations",
                newName: "ReservationID");

            migrationBuilder.CreateIndex(
                name: "IX_ACAD_ClassReservations_CoursePackageID",
                table: "ACAD_ClassReservations",
                column: "CoursePackageID",
                unique: true,
                filter: "[CoursePackageID] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_ACAD_ClassReservations_ACAD_CoursePackages_CoursePackageID",
                table: "ACAD_ClassReservations",
                column: "CoursePackageID",
                principalTable: "ACAD_CoursePackages",
                principalColumn: "PackageID");
        }
    }
}
