using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CreateWishlist : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ACAD_CourseWishlist",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StudentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CourseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2(0)", precision: 0, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ACAD_CourseWishlist", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ACAD_CourseWishlist_ACAD_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "ACAD_Courses",
                        principalColumn: "CourseID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ACAD_CourseWishlist_IDN_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "IDN_Students",
                        principalColumn: "AccountID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ACAD_CourseWishlist_CourseId",
                table: "ACAD_CourseWishlist",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_ACAD_CourseWishlist_StudentId",
                table: "ACAD_CourseWishlist",
                column: "StudentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ACAD_CourseWishlist");
        }
    }
}
