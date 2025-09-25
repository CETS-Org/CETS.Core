using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateLearningMaterial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ACAD_LearningMaterials_Created",
                table: "ACAD_LearningMaterials");

            migrationBuilder.DropForeignKey(
                name: "FK_ACAD_LearningMaterials_Uploader",
                table: "ACAD_LearningMaterials");

            migrationBuilder.DropIndex(
                name: "IX_ACAD_LearningMaterials_UploaderID",
                table: "ACAD_LearningMaterials");

            migrationBuilder.DropColumn(
                name: "UploaderID",
                table: "ACAD_LearningMaterials");

            migrationBuilder.AddForeignKey(
                name: "FK_ACAD_LearningMaterials_Created",
                table: "ACAD_LearningMaterials",
                column: "CreatedBy",
                principalTable: "IDN_Accounts",
                principalColumn: "AccountID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ACAD_LearningMaterials_Created",
                table: "ACAD_LearningMaterials");

            migrationBuilder.AddColumn<Guid>(
                name: "UploaderID",
                table: "ACAD_LearningMaterials",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_ACAD_LearningMaterials_UploaderID",
                table: "ACAD_LearningMaterials",
                column: "UploaderID");

            migrationBuilder.AddForeignKey(
                name: "FK_ACAD_LearningMaterials_Created",
                table: "ACAD_LearningMaterials",
                column: "CreatedBy",
                principalTable: "IDN_Accounts",
                principalColumn: "AccountID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ACAD_LearningMaterials_Uploader",
                table: "ACAD_LearningMaterials",
                column: "UploaderID",
                principalTable: "IDN_Accounts",
                principalColumn: "AccountID");
        }
    }
}
