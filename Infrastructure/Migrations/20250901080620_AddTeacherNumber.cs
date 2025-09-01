using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTeacherNumber : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateSequence<int>(
                name: "SeqTeacher");

            migrationBuilder.AddColumn<int>(
                name: "TeacherNumber",
                table: "IDN_Teachers",
                type: "int",
                nullable: false,
                defaultValueSql: "NEXT VALUE FOR [SeqTeacher]");

            migrationBuilder.AlterColumn<string>(
                name: "TeacherCode",
                table: "IDN_Teachers",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                computedColumnSql: "('TCH'+RIGHT('000000'+CONVERT(varchar(6), [TeacherNumber]), 6))",
                stored: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.CreateIndex(
                name: "UX_IDN_Teachers_TeacherCode",
                table: "IDN_Teachers",
                column: "TeacherCode",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "UX_IDN_Teachers_TeacherCode",
                table: "IDN_Teachers");

            migrationBuilder.DropColumn(
                name: "TeacherNumber",
                table: "IDN_Teachers");

            migrationBuilder.DropSequence(
                name: "SeqTeacher");

            migrationBuilder.AlterColumn<string>(
                name: "TeacherCode",
                table: "IDN_Teachers",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20,
                oldComputedColumnSql: "('TCH'+RIGHT('000000'+CONVERT(varchar(6), [TeacherNumber]), 6))");
        }
    }
}
