using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addPlacementTest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "PlacementTestGrade",
                table: "IDN_Students",
                type: "decimal(5,2)",
                precision: 5,
                scale: 2,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ACAD_PlacementQuestions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SkillTypeID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QuestionType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    QuestionUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Difficulty = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2(0)", precision: 0, nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2(0)", precision: 0, nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ACAD_PlacementQuestions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ACAD_PlacementQuestions_CORE_LookUps_SkillTypeID",
                        column: x => x.SkillTypeID,
                        principalTable: "CORE_LookUps",
                        principalColumn: "LookUpID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ACAD_PlacementQuestions_IDN_Accounts_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "IDN_Accounts",
                        principalColumn: "AccountID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ACAD_PlacementQuestions_IDN_Accounts_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "IDN_Accounts",
                        principalColumn: "AccountID");
                });

            migrationBuilder.CreateTable(
                name: "ACAD_PlacementTests",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DurationMinutes = table.Column<int>(type: "int", nullable: false),
                    StoreUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2(0)", precision: 0, nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2(0)", precision: 0, nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ACAD_PlacementTests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ACAD_PlacementTests_IDN_Accounts_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "IDN_Accounts",
                        principalColumn: "AccountID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ACAD_PlacementTests_IDN_Accounts_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "IDN_Accounts",
                        principalColumn: "AccountID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ACAD_PlacementQuestions_CreatedBy",
                table: "ACAD_PlacementQuestions",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ACAD_PlacementQuestions_SkillTypeID",
                table: "ACAD_PlacementQuestions",
                column: "SkillTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_ACAD_PlacementQuestions_UpdatedBy",
                table: "ACAD_PlacementQuestions",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ACAD_PlacementTests_CreatedBy",
                table: "ACAD_PlacementTests",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ACAD_PlacementTests_UpdatedBy",
                table: "ACAD_PlacementTests",
                column: "UpdatedBy");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ACAD_PlacementQuestions");

            migrationBuilder.DropTable(
                name: "ACAD_PlacementTests");

            migrationBuilder.DropColumn(
                name: "PlacementTestGrade",
                table: "IDN_Students");
        }
    }
}
