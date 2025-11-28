using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSuspensionFieldsToAcademicRequest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateOnly>(
                name: "ExpectedReturnDate",
                table: "ACAD_AcademicRequests",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReasonCategory",
                table: "ACAD_AcademicRequests",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateOnly>(
                name: "SuspensionEndDate",
                table: "ACAD_AcademicRequests",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<DateOnly>(
                name: "SuspensionStartDate",
                table: "ACAD_AcademicRequests",
                type: "date",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExpectedReturnDate",
                table: "ACAD_AcademicRequests");

            migrationBuilder.DropColumn(
                name: "ReasonCategory",
                table: "ACAD_AcademicRequests");

            migrationBuilder.DropColumn(
                name: "SuspensionEndDate",
                table: "ACAD_AcademicRequests");

            migrationBuilder.DropColumn(
                name: "SuspensionStartDate",
                table: "ACAD_AcademicRequests");
        }
    }
}
