using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateEnrollModule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ACAD_ClassReservations_Class",
                table: "ACAD_ClassReservations");

            migrationBuilder.DropForeignKey(
                name: "FK_ACAD_ClassReservations_Invoice",
                table: "ACAD_ClassReservations");

            migrationBuilder.DropForeignKey(
                name: "FK_FIN_Invoices_PlanType",
                table: "FIN_Invoices");

            migrationBuilder.DropIndex(
                name: "IX_FIN_Invoices_PlanTypeID",
                table: "FIN_Invoices");

            migrationBuilder.DropIndex(
                name: "IX_FIN_Invoices_SeriesSeq_Filtered",
                table: "FIN_Invoices");

            migrationBuilder.DropIndex(
                name: "UQ_FIN_Invoices_Number",
                table: "FIN_Invoices");

            migrationBuilder.DropCheckConstraint(
                name: "CK_FIN_Invoices_Sequence",
                table: "FIN_Invoices");

            migrationBuilder.DropIndex(
                name: "IX_ACAD_ClassReservations_InvoiceID",
                table: "ACAD_ClassReservations");

            migrationBuilder.DropIndex(
                name: "IX_ACAD_ClassReservations_StudentID",
                table: "ACAD_ClassReservations");

            migrationBuilder.DropIndex(
                name: "UQ_ACAD_ClassReservations",
                table: "ACAD_ClassReservations");

           

            migrationBuilder.DropColumn(
                name: "PlanTypeID",
                table: "FIN_Invoices");

            migrationBuilder.DropColumn(
                name: "SeriesID",
                table: "FIN_Invoices");

            migrationBuilder.DropColumn(
                name: "ClassID",
                table: "ACAD_ClassReservations");

            migrationBuilder.RenameColumn(
                name: "InvoiceID",
                table: "ACAD_ClassReservations",
                newName: "CoursePackageID");

            migrationBuilder.AddColumn<DateOnly>(
                name: "DueDate",
                table: "FIN_InvoiceItems",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "InvoiceID",
                table: "ACAD_Enrollments",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "ReservationID",
                table: "ACAD_ClassReservations",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldDefaultValueSql: "(newid())");

            migrationBuilder.AlterColumn<string>(
                name: "InvoiceNumber",
                table: "FIN_Invoices",
                type: "varchar(50)",
                unicode: false,
                maxLength: 50,
                nullable: false,
                computedColumnSql: "'INV-' + CONVERT(VARCHAR(4), YEAR(GETDATE())) + RIGHT('0000000' + CONVERT(VARCHAR(7), [InvoiceSequence]), 7)",
                stored: false,
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldUnicode: false,
                oldMaxLength: 50,
                oldComputedColumnSql: "'INV-' + CONVERT(VARCHAR(10), [InvoiceSequence]) + '-' + RIGHT('000' + CONVERT(VARCHAR(3), [PaymentSequence]), 3)",
                oldStored: true);

            migrationBuilder.DropColumn(
               name: "PaymentSequence",
               table: "FIN_Invoices");

            migrationBuilder.RestartSequence(
                name: "InvoiceSequence",
                schema: "dbo",
                startValue: 1L);

            migrationBuilder.CreateTable(
                name: "FIN_ReservationItems",
                columns: table => new
                {
                    ReservationItemID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InvoiceID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CourseID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PaymentSequence = table.Column<int>(type: "int", nullable: true),
                    PlanTypeID = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FIN_ReservationItems", x => x.ReservationItemID);
                    table.ForeignKey(
                        name: "FK_FIN_ReservationItems_Course",
                        column: x => x.CourseID,
                        principalTable: "ACAD_Courses",
                        principalColumn: "CourseID");
                    table.ForeignKey(
                        name: "FK_FIN_ReservationItems_Invoice",
                        column: x => x.InvoiceID,
                        principalTable: "FIN_Invoices",
                        principalColumn: "InvoiceID");
                    table.ForeignKey(
                        name: "FK_FIN_ReservationItems_PlanType",
                        column: x => x.PlanTypeID,
                        principalTable: "CORE_LookUps",
                        principalColumn: "LookUpID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ACAD_Enrollments_InvoiceID",
                table: "ACAD_Enrollments",
                column: "InvoiceID",
                unique: true,
                filter: "[InvoiceID] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ACAD_ClassReservations_CoursePackageID",
                table: "ACAD_ClassReservations",
                column: "CoursePackageID",
                unique: true,
                filter: "[CoursePackageID] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "UQ_ACAD_ClassReservations_Student",
                table: "ACAD_ClassReservations",
                column: "StudentID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FIN_ReservationItems_CourseID",
                table: "FIN_ReservationItems",
                column: "CourseID");

            migrationBuilder.CreateIndex(
                name: "IX_FIN_ReservationItems_InvoiceID",
                table: "FIN_ReservationItems",
                column: "InvoiceID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FIN_ReservationItems_PlanTypeID",
                table: "FIN_ReservationItems",
                column: "PlanTypeID");

            migrationBuilder.AddForeignKey(
                name: "FK_ACAD_ClassReservations_ACAD_CoursePackages_CoursePackageID",
                table: "ACAD_ClassReservations",
                column: "CoursePackageID",
                principalTable: "ACAD_CoursePackages",
                principalColumn: "PackageID");

            migrationBuilder.AddForeignKey(
                name: "FK_ACAD_Enrollments_Invoice",
                table: "ACAD_Enrollments",
                column: "InvoiceID",
                principalTable: "FIN_Invoices",
                principalColumn: "InvoiceID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ACAD_ClassReservations_ACAD_CoursePackages_CoursePackageID",
                table: "ACAD_ClassReservations");

            migrationBuilder.DropForeignKey(
                name: "FK_ACAD_Enrollments_Invoice",
                table: "ACAD_Enrollments");

            migrationBuilder.DropTable(
                name: "FIN_ReservationItems");

            migrationBuilder.DropIndex(
                name: "IX_ACAD_Enrollments_InvoiceID",
                table: "ACAD_Enrollments");

            migrationBuilder.DropIndex(
                name: "IX_ACAD_ClassReservations_CoursePackageID",
                table: "ACAD_ClassReservations");

            migrationBuilder.DropIndex(
                name: "UQ_ACAD_ClassReservations_Student",
                table: "ACAD_ClassReservations");

            migrationBuilder.DropColumn(
                name: "DueDate",
                table: "FIN_InvoiceItems");

            migrationBuilder.DropColumn(
                name: "InvoiceID",
                table: "ACAD_Enrollments");

            migrationBuilder.RenameColumn(
                name: "CoursePackageID",
                table: "ACAD_ClassReservations",
                newName: "InvoiceID");

            migrationBuilder.AddColumn<int>(
                name: "PaymentSequence",
                table: "FIN_Invoices",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "PlanTypeID",
                table: "FIN_Invoices",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SeriesID",
                table: "FIN_Invoices",
                type: "varchar(50)",
                unicode: false,
                maxLength: 50,
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "ReservationID",
                table: "ACAD_ClassReservations",
                type: "uniqueidentifier",
                nullable: false,
                defaultValueSql: "(newid())",
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<Guid>(
                name: "ClassID",
                table: "ACAD_ClassReservations",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<string>(
                name: "InvoiceNumber",
                table: "FIN_Invoices",
                type: "varchar(50)",
                unicode: false,
                maxLength: 50,
                nullable: false,
                computedColumnSql: "'INV-' + CONVERT(VARCHAR(10), [InvoiceSequence]) + '-' + RIGHT('000' + CONVERT(VARCHAR(3), [PaymentSequence]), 3)",
                stored: true,
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldUnicode: false,
                oldMaxLength: 50,
                oldComputedColumnSql: "'INV-' + CONVERT(VARCHAR(4), YEAR(GETDATE())) + RIGHT('0000000' + CONVERT(VARCHAR(7), [InvoiceSequence]), 7)",
                oldStored: false);

            migrationBuilder.RestartSequence(
                name: "InvoiceSequence",
                schema: "dbo",
                startValue: 1000000L);

            migrationBuilder.CreateIndex(
                name: "IX_FIN_Invoices_PlanTypeID",
                table: "FIN_Invoices",
                column: "PlanTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_FIN_Invoices_SeriesSeq_Filtered",
                table: "FIN_Invoices",
                columns: new[] { "SeriesID", "PaymentSequence" },
                unique: true,
                filter: "([SeriesID] IS NOT NULL AND [PaymentSequence] IS NOT NULL)");

            migrationBuilder.CreateIndex(
                name: "UQ_FIN_Invoices_Number",
                table: "FIN_Invoices",
                column: "InvoiceNumber",
                unique: true);

            migrationBuilder.AddCheckConstraint(
                name: "CK_FIN_Invoices_Sequence",
                table: "FIN_Invoices",
                sql: "[PaymentSequence] IS NULL OR [PaymentSequence] >= 1");

            migrationBuilder.CreateIndex(
                name: "IX_ACAD_ClassReservations_InvoiceID",
                table: "ACAD_ClassReservations",
                column: "InvoiceID");

            migrationBuilder.CreateIndex(
                name: "IX_ACAD_ClassReservations_StudentID",
                table: "ACAD_ClassReservations",
                column: "StudentID");

            migrationBuilder.CreateIndex(
                name: "UQ_ACAD_ClassReservations",
                table: "ACAD_ClassReservations",
                columns: new[] { "ClassID", "StudentID" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ACAD_ClassReservations_Class",
                table: "ACAD_ClassReservations",
                column: "ClassID",
                principalTable: "ACAD_Classes",
                principalColumn: "ClassID");

            migrationBuilder.AddForeignKey(
                name: "FK_ACAD_ClassReservations_Invoice",
                table: "ACAD_ClassReservations",
                column: "InvoiceID",
                principalTable: "FIN_Invoices",
                principalColumn: "InvoiceID");

            migrationBuilder.AddForeignKey(
                name: "FK_FIN_Invoices_PlanType",
                table: "FIN_Invoices",
                column: "PlanTypeID",
                principalTable: "CORE_LookUps",
                principalColumn: "LookUpID");
        }
    }
}
