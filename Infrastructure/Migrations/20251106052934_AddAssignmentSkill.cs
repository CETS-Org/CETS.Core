using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddAssignmentSkill : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "SkillID",
                table: "ACAD_Assignments",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ACAD_Assignments_SkillID",
                table: "ACAD_Assignments",
                column: "SkillID");

            migrationBuilder.AddForeignKey(
                name: "FK_ACAD_Assignment_Skills",
                table: "ACAD_Assignments",
                column: "SkillID",
                principalTable: "CORE_LookUps",
                principalColumn: "LookUpID",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ACAD_Assignment_Skills",
                table: "ACAD_Assignments");

            migrationBuilder.DropIndex(
                name: "IX_ACAD_Assignments_SkillID",
                table: "ACAD_Assignments");

            migrationBuilder.DropColumn(
                name: "SkillID",
                table: "ACAD_Assignments");
        }
    }
}
