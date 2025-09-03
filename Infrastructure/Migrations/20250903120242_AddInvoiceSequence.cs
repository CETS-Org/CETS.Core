using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddInvoiceSequence : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_FIN_Invoices_SeriesSeq_Filtered",
                table: "FIN_Invoices");

            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.RenameColumn(
                name: "Sequence",
                table: "FIN_Invoices",
                newName: "PaymentSequence");

            migrationBuilder.CreateSequence<int>(
                name: "InvoiceSequence",
                schema: "dbo",
                startValue: 1000000L);

            migrationBuilder.AddColumn<int>(
                name: "InvoiceSequence",
                table: "FIN_Invoices",
                type: "int",
                nullable: false,
                defaultValueSql: "NEXT VALUE FOR dbo.InvoiceSequence");

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
                oldMaxLength: 50);

            migrationBuilder.CreateIndex(
                name: "IX_FIN_Invoices_SeriesSeq_Filtered",
                table: "FIN_Invoices",
                columns: new[] { "SeriesID", "PaymentSequence" },
                unique: true,
                filter: "([SeriesID] IS NOT NULL AND [PaymentSequence] IS NOT NULL)");

            migrationBuilder.AddCheckConstraint(
                name: "CK_FIN_Invoices_Sequence",
                table: "FIN_Invoices",
                sql: "[PaymentSequence] IS NULL OR [PaymentSequence] >= 1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_FIN_Invoices_SeriesSeq_Filtered",
                table: "FIN_Invoices");

            migrationBuilder.DropCheckConstraint(
                name: "CK_FIN_Invoices_Sequence",
                table: "FIN_Invoices");

            migrationBuilder.DropColumn(
                name: "InvoiceSequence",
                table: "FIN_Invoices");

            migrationBuilder.DropSequence(
                name: "InvoiceSequence",
                schema: "dbo");

            migrationBuilder.RenameColumn(
                name: "PaymentSequence",
                table: "FIN_Invoices",
                newName: "Sequence");

            migrationBuilder.AlterColumn<string>(
                name: "InvoiceNumber",
                table: "FIN_Invoices",
                type: "varchar(50)",
                unicode: false,
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldUnicode: false,
                oldMaxLength: 50,
                oldComputedColumnSql: "'INV-' + CONVERT(VARCHAR(10), [InvoiceSequence]) + '-' + RIGHT('000' + CONVERT(VARCHAR(3), [PaymentSequence]), 3)");

            migrationBuilder.CreateIndex(
                name: "IX_FIN_Invoices_SeriesSeq_Filtered",
                table: "FIN_Invoices",
                columns: new[] { "SeriesID", "Sequence" },
                unique: true,
                filter: "([SeriesID] IS NOT NULL AND [Sequence] IS NOT NULL)");
        }
    }
}
