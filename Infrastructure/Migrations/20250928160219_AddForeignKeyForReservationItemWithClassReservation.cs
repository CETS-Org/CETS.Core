using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddForeignKeyForReservationItemWithClassReservation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ClassReservationID",
                table: "ACAD_ReservationItems",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_ACAD_ReservationItems_ClassReservationID",
                table: "ACAD_ReservationItems",
                column: "ClassReservationID");

            migrationBuilder.AddForeignKey(
                name: "FK_ACAD_ReservationItems_ClassReservation",
                table: "ACAD_ReservationItems",
                column: "ClassReservationID",
                principalTable: "ACAD_ClassReservations",
                principalColumn: "ReservationID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ACAD_ReservationItems_ClassReservation",
                table: "ACAD_ReservationItems");

            migrationBuilder.DropIndex(
                name: "IX_ACAD_ReservationItems_ClassReservationID",
                table: "ACAD_ReservationItems");

            migrationBuilder.DropColumn(
                name: "ClassReservationID",
                table: "ACAD_ReservationItems");
        }
    }
}
