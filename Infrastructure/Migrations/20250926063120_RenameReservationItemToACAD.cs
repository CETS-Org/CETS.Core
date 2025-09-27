using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RenameReservationItemToACAD : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FIN_ReservationItems_Course",
                table: "FIN_ReservationItems");

            migrationBuilder.DropForeignKey(
                name: "FK_FIN_ReservationItems_Invoice",
                table: "FIN_ReservationItems");

            migrationBuilder.DropForeignKey(
                name: "FK_FIN_ReservationItems_PlanType",
                table: "FIN_ReservationItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FIN_ReservationItems",
                table: "FIN_ReservationItems");

            migrationBuilder.RenameTable(
                name: "FIN_ReservationItems",
                newName: "ACAD_ReservationItems");

            migrationBuilder.RenameIndex(
                name: "IX_FIN_ReservationItems_PlanTypeID",
                table: "ACAD_ReservationItems",
                newName: "IX_ACAD_ReservationItems_PlanTypeID");

            migrationBuilder.RenameIndex(
                name: "IX_FIN_ReservationItems_InvoiceID",
                table: "ACAD_ReservationItems",
                newName: "IX_ACAD_ReservationItems_InvoiceID");

            migrationBuilder.RenameIndex(
                name: "IX_FIN_ReservationItems_CourseID",
                table: "ACAD_ReservationItems",
                newName: "IX_ACAD_ReservationItems_CourseID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ACAD_ReservationItems",
                table: "ACAD_ReservationItems",
                column: "ReservationItemID");

            migrationBuilder.AddForeignKey(
                name: "FK_ACAD_ReservationItems_Course",
                table: "ACAD_ReservationItems",
                column: "CourseID",
                principalTable: "ACAD_Courses",
                principalColumn: "CourseID");

            migrationBuilder.AddForeignKey(
                name: "FK_ACAD_ReservationItems_Invoice",
                table: "ACAD_ReservationItems",
                column: "InvoiceID",
                principalTable: "FIN_Invoices",
                principalColumn: "InvoiceID");

            migrationBuilder.AddForeignKey(
                name: "FK_ACAD_ReservationItems_PlanType",
                table: "ACAD_ReservationItems",
                column: "PlanTypeID",
                principalTable: "CORE_LookUps",
                principalColumn: "LookUpID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ACAD_ReservationItems_Course",
                table: "ACAD_ReservationItems");

            migrationBuilder.DropForeignKey(
                name: "FK_ACAD_ReservationItems_Invoice",
                table: "ACAD_ReservationItems");

            migrationBuilder.DropForeignKey(
                name: "FK_ACAD_ReservationItems_PlanType",
                table: "ACAD_ReservationItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ACAD_ReservationItems",
                table: "ACAD_ReservationItems");

            migrationBuilder.RenameTable(
                name: "ACAD_ReservationItems",
                newName: "FIN_ReservationItems");

            migrationBuilder.RenameIndex(
                name: "IX_ACAD_ReservationItems_PlanTypeID",
                table: "FIN_ReservationItems",
                newName: "IX_FIN_ReservationItems_PlanTypeID");

            migrationBuilder.RenameIndex(
                name: "IX_ACAD_ReservationItems_InvoiceID",
                table: "FIN_ReservationItems",
                newName: "IX_FIN_ReservationItems_InvoiceID");

            migrationBuilder.RenameIndex(
                name: "IX_ACAD_ReservationItems_CourseID",
                table: "FIN_ReservationItems",
                newName: "IX_FIN_ReservationItems_CourseID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FIN_ReservationItems",
                table: "FIN_ReservationItems",
                column: "ReservationItemID");

            migrationBuilder.AddForeignKey(
                name: "FK_FIN_ReservationItems_Course",
                table: "FIN_ReservationItems",
                column: "CourseID",
                principalTable: "ACAD_Courses",
                principalColumn: "CourseID");

            migrationBuilder.AddForeignKey(
                name: "FK_FIN_ReservationItems_Invoice",
                table: "FIN_ReservationItems",
                column: "InvoiceID",
                principalTable: "FIN_Invoices",
                principalColumn: "InvoiceID");

            migrationBuilder.AddForeignKey(
                name: "FK_FIN_ReservationItems_PlanType",
                table: "FIN_ReservationItems",
                column: "PlanTypeID",
                principalTable: "CORE_LookUps",
                principalColumn: "LookUpID");
        }
    }
}
