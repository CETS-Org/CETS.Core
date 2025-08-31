using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddStudentNumber : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateSequence<int>(
                name: "SeqStudent");

            migrationBuilder.AddColumn<int>(
                name: "StudentNumber",
                table: "IDN_Students",
                type: "int",
                nullable: false,
                defaultValueSql: "NEXT VALUE FOR [SeqStudent]");

            migrationBuilder.AlterColumn<string>(
                name: "StudentCode",
                table: "IDN_Students",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                computedColumnSql: "('STU'+RIGHT('000000'+CONVERT(varchar(6), [StudentNumber]), 6))",
                stored: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.CreateIndex(
                name: "UX_IDN_Students_StudentCode",
                table: "IDN_Students",
                column: "StudentCode",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "UX_IDN_Students_StudentCode",
                table: "IDN_Students");

            migrationBuilder.DropColumn(
                name: "StudentNumber",
                table: "IDN_Students");

            migrationBuilder.DropSequence(
                name: "SeqStudent");

            migrationBuilder.AlterColumn<string>(
                name: "StudentCode",
                table: "IDN_Students",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20,
                oldComputedColumnSql: "('STU'+RIGHT('000000'+CONVERT(varchar(6), [StudentNumber]), 6))");
        }
    }
}
