using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pos.Api.Migrations
{
    /// <inheritdoc />
    public partial class CreateReservationsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "reservations",
                schema: "point_of_sale",
                columns: table => new
                {
                    appointmentId = table.Column<string>(type: "text", nullable: false),
                    registrationNumber = table.Column<string>(type: "text", nullable: false),
                    customerId = table.Column<string>(type: "text", nullable: false),
                    serviceProductId = table.Column<string>(type: "text", nullable: false),
                    employeeId = table.Column<string>(type: "text", nullable: false),
                    startTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    durationMinutes = table.Column<int>(type: "integer", nullable: false),
                    status = table.Column<string>(type: "text", nullable: false),
                    orderId = table.Column<string>(type: "text", nullable: true),
                    notes = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_reservations", x => x.appointmentId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "reservations",
                schema: "point_of_sale");
        }
    }
}
