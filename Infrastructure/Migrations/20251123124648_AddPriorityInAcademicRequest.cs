using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPriorityInAcademicRequest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "PriorityID",
                table: "ACAD_AcademicRequests",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ACAD_AcademicRequests_PriorityID",
                table: "ACAD_AcademicRequests",
                column: "PriorityID");

            migrationBuilder.AddForeignKey(
                name: "FK_ACAD_AcReq_Priority",
                table: "ACAD_AcademicRequests",
                column: "PriorityID",
                principalTable: "CORE_LookUps",
                principalColumn: "LookUpID",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ACAD_AcReq_Priority",
                table: "ACAD_AcademicRequests");

            migrationBuilder.DropIndex(
                name: "IX_ACAD_AcademicRequests_PriorityID",
                table: "ACAD_AcademicRequests");

            migrationBuilder.DropColumn(
                name: "PriorityID",
                table: "ACAD_AcademicRequests");
        }
    }
}
