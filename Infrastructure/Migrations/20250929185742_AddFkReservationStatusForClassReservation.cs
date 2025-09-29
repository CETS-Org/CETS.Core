using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddFkReservationStatusForClassReservation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ReservationStatusID",
                table: "ACAD_ClassReservations",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ACAD_ClassReservations_ReservationStatusID",
                table: "ACAD_ClassReservations",
                column: "ReservationStatusID");

            migrationBuilder.AddForeignKey(
                name: "FK_ACAD_ACAD_ClassReservations_ReservationStatus",
                table: "ACAD_ClassReservations",
                column: "ReservationStatusID",
                principalTable: "CORE_LookUps",
                principalColumn: "LookUpID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ACAD_ACAD_ClassReservations_ReservationStatus",
                table: "ACAD_ClassReservations");

            migrationBuilder.DropIndex(
                name: "IX_ACAD_ClassReservations_ReservationStatusID",
                table: "ACAD_ClassReservations");

            migrationBuilder.DropColumn(
                name: "ReservationStatusID",
                table: "ACAD_ClassReservations");
        }
    }
}
