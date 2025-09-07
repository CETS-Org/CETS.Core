using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreateAddName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateSequence<int>(
                name: "SeqClass");

            migrationBuilder.AddColumn<int>(
                name: "ClassNum",
                table: "ACAD_Classes",
                type: "int",
                nullable: false,
                defaultValueSql: "NEXT VALUE FOR [SeqClass]");

            migrationBuilder.AddColumn<string>(
                name: "ClassName",
                table: "ACAD_Classes",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                computedColumnSql: "('CLS' + RIGHT('0000' + CONVERT(varchar(4), [ClassNum]), 4))",
                stored: true);

            migrationBuilder.CreateIndex(
                name: "UX_ACAD_Classes_ClassName",
                table: "ACAD_Classes",
                column: "ClassName",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "UX_ACAD_Classes_ClassName",
                table: "ACAD_Classes");

            migrationBuilder.DropColumn(
                name: "ClassName",
                table: "ACAD_Classes");

            migrationBuilder.DropColumn(
                name: "ClassNum",
                table: "ACAD_Classes");

            migrationBuilder.DropSequence(
                name: "SeqClass");
        }
    }
}
