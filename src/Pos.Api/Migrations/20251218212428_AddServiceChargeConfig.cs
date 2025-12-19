using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pos.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddServiceChargeConfig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "serviceChargeConfig",
                schema: "point_of_sale",
                columns: table => new
                {
                    serviceChargeConfigId = table.Column<Guid>(type: "uuid", nullable: false),
                    registrationNumber = table.Column<string>(type: "text", nullable: false),
                    percentage = table.Column<decimal>(type: "numeric", nullable: false),
                    validFrom = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    validTo = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_serviceChargeConfig", x => x.serviceChargeConfigId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "serviceChargeConfig",
                schema: "point_of_sale");
        }
    }
}
