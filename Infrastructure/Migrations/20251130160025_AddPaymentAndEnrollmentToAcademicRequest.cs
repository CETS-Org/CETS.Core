using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPaymentAndEnrollmentToAcademicRequest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "EnrollmentID",
                table: "ACAD_AcademicRequests",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "PaymentID",
                table: "ACAD_AcademicRequests",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ACAD_AcademicRequests_EnrollmentID",
                table: "ACAD_AcademicRequests",
                column: "EnrollmentID");

            migrationBuilder.CreateIndex(
                name: "IX_ACAD_AcademicRequests_PaymentID",
                table: "ACAD_AcademicRequests",
                column: "PaymentID");

            migrationBuilder.AddForeignKey(
                name: "FK_ACAD_AcReq_Enrollment",
                table: "ACAD_AcademicRequests",
                column: "EnrollmentID",
                principalTable: "ACAD_Enrollments",
                principalColumn: "EnrollmentID");

            migrationBuilder.AddForeignKey(
                name: "FK_ACAD_AcReq_Payment",
                table: "ACAD_AcademicRequests",
                column: "PaymentID",
                principalTable: "FIN_Payments",
                principalColumn: "PaymentID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ACAD_AcReq_Enrollment",
                table: "ACAD_AcademicRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_ACAD_AcReq_Payment",
                table: "ACAD_AcademicRequests");

            migrationBuilder.DropIndex(
                name: "IX_ACAD_AcademicRequests_EnrollmentID",
                table: "ACAD_AcademicRequests");

            migrationBuilder.DropIndex(
                name: "IX_ACAD_AcademicRequests_PaymentID",
                table: "ACAD_AcademicRequests");

            migrationBuilder.DropColumn(
                name: "EnrollmentID",
                table: "ACAD_AcademicRequests");

            migrationBuilder.DropColumn(
                name: "PaymentID",
                table: "ACAD_AcademicRequests");
        }
    }
}
