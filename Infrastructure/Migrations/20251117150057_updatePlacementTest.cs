using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updatePlacementTest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ACAD_PlacementQuestions_CORE_LookUps_SkillTypeID",
                table: "ACAD_PlacementQuestions");

            migrationBuilder.DropForeignKey(
                name: "FK_ACAD_PlacementQuestions_IDN_Accounts_CreatedBy",
                table: "ACAD_PlacementQuestions");

            migrationBuilder.DropForeignKey(
                name: "FK_ACAD_PlacementQuestions_IDN_Accounts_UpdatedBy",
                table: "ACAD_PlacementQuestions");

            migrationBuilder.DropForeignKey(
                name: "FK_ACAD_PlacementTests_IDN_Accounts_CreatedBy",
                table: "ACAD_PlacementTests");

            migrationBuilder.DropForeignKey(
                name: "FK_ACAD_PlacementTests_IDN_Accounts_UpdatedBy",
                table: "ACAD_PlacementTests");

            migrationBuilder.DropColumn(
                name: "QuestionType",
                table: "ACAD_PlacementQuestions");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "ACAD_PlacementTests",
                newName: "PlacementTestID");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "ACAD_PlacementQuestions",
                newName: "PlacementQuestionID");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "ACAD_PlacementTests",
                type: "datetime2(0)",
                precision: 0,
                nullable: false,
                defaultValueSql: "(sysutcdatetime())",
                oldClrType: typeof(DateTime),
                oldType: "datetime2(0)",
                oldPrecision: 0);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "ACAD_PlacementQuestions",
                type: "datetime2(0)",
                precision: 0,
                nullable: false,
                defaultValueSql: "(sysutcdatetime())",
                oldClrType: typeof(DateTime),
                oldType: "datetime2(0)",
                oldPrecision: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "QuestionTypeID",
                table: "ACAD_PlacementQuestions",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_ACAD_PlacementQuestions_QuestionTypeID",
                table: "ACAD_PlacementQuestions",
                column: "QuestionTypeID");

            migrationBuilder.AddForeignKey(
                name: "FK_ACAD_PlacementQuestions_Created",
                table: "ACAD_PlacementQuestions",
                column: "CreatedBy",
                principalTable: "IDN_Accounts",
                principalColumn: "AccountID");

            migrationBuilder.AddForeignKey(
                name: "FK_ACAD_PlacementQuestions_QuestionTypeID",
                table: "ACAD_PlacementQuestions",
                column: "QuestionTypeID",
                principalTable: "CORE_LookUps",
                principalColumn: "LookUpID");

            migrationBuilder.AddForeignKey(
                name: "FK_ACAD_PlacementQuestions_SkillType",
                table: "ACAD_PlacementQuestions",
                column: "SkillTypeID",
                principalTable: "CORE_LookUps",
                principalColumn: "LookUpID");

            migrationBuilder.AddForeignKey(
                name: "FK_ACAD_PlacementQuestions_Updated",
                table: "ACAD_PlacementQuestions",
                column: "UpdatedBy",
                principalTable: "IDN_Accounts",
                principalColumn: "AccountID");

            migrationBuilder.AddForeignKey(
                name: "FK_ACAD_PlacementTests_Created",
                table: "ACAD_PlacementTests",
                column: "CreatedBy",
                principalTable: "IDN_Accounts",
                principalColumn: "AccountID");

            migrationBuilder.AddForeignKey(
                name: "FK_ACAD_PlacementTests_Updated",
                table: "ACAD_PlacementTests",
                column: "UpdatedBy",
                principalTable: "IDN_Accounts",
                principalColumn: "AccountID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ACAD_PlacementQuestions_Created",
                table: "ACAD_PlacementQuestions");

            migrationBuilder.DropForeignKey(
                name: "FK_ACAD_PlacementQuestions_QuestionTypeID",
                table: "ACAD_PlacementQuestions");

            migrationBuilder.DropForeignKey(
                name: "FK_ACAD_PlacementQuestions_SkillType",
                table: "ACAD_PlacementQuestions");

            migrationBuilder.DropForeignKey(
                name: "FK_ACAD_PlacementQuestions_Updated",
                table: "ACAD_PlacementQuestions");

            migrationBuilder.DropForeignKey(
                name: "FK_ACAD_PlacementTests_Created",
                table: "ACAD_PlacementTests");

            migrationBuilder.DropForeignKey(
                name: "FK_ACAD_PlacementTests_Updated",
                table: "ACAD_PlacementTests");

            migrationBuilder.DropIndex(
                name: "IX_ACAD_PlacementQuestions_QuestionTypeID",
                table: "ACAD_PlacementQuestions");

            migrationBuilder.DropColumn(
                name: "QuestionTypeID",
                table: "ACAD_PlacementQuestions");

            migrationBuilder.RenameColumn(
                name: "PlacementTestID",
                table: "ACAD_PlacementTests",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "PlacementQuestionID",
                table: "ACAD_PlacementQuestions",
                newName: "Id");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "ACAD_PlacementTests",
                type: "datetime2(0)",
                precision: 0,
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2(0)",
                oldPrecision: 0,
                oldDefaultValueSql: "(sysutcdatetime())");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "ACAD_PlacementQuestions",
                type: "datetime2(0)",
                precision: 0,
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2(0)",
                oldPrecision: 0,
                oldDefaultValueSql: "(sysutcdatetime())");

            migrationBuilder.AddColumn<string>(
                name: "QuestionType",
                table: "ACAD_PlacementQuestions",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_ACAD_PlacementQuestions_CORE_LookUps_SkillTypeID",
                table: "ACAD_PlacementQuestions",
                column: "SkillTypeID",
                principalTable: "CORE_LookUps",
                principalColumn: "LookUpID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ACAD_PlacementQuestions_IDN_Accounts_CreatedBy",
                table: "ACAD_PlacementQuestions",
                column: "CreatedBy",
                principalTable: "IDN_Accounts",
                principalColumn: "AccountID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ACAD_PlacementQuestions_IDN_Accounts_UpdatedBy",
                table: "ACAD_PlacementQuestions",
                column: "UpdatedBy",
                principalTable: "IDN_Accounts",
                principalColumn: "AccountID");

            migrationBuilder.AddForeignKey(
                name: "FK_ACAD_PlacementTests_IDN_Accounts_CreatedBy",
                table: "ACAD_PlacementTests",
                column: "CreatedBy",
                principalTable: "IDN_Accounts",
                principalColumn: "AccountID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ACAD_PlacementTests_IDN_Accounts_UpdatedBy",
                table: "ACAD_PlacementTests",
                column: "UpdatedBy",
                principalTable: "IDN_Accounts",
                principalColumn: "AccountID");
        }
    }
}
