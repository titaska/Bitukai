using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pos.Api.Migrations
{
    /// <inheritdoc />
    public partial class Products : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Products",
                schema: "point_of_sale",
                columns: table => new
                {
                    productId = table.Column<Guid>(type: "uuid", nullable: false),
                    registrationNumber = table.Column<Guid>(type: "uuid", nullable: false),
                    type = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    basePrice = table.Column<decimal>(type: "numeric", nullable: false),
                    taxCode = table.Column<Guid>(type: "uuid", nullable: false),
                    status = table.Column<bool>(type: "boolean", nullable: false),
                    durationMinutes = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.productId);
                });

            migrationBuilder.CreateTable(
                name: "ProductStaff",
                schema: "point_of_sale",
                columns: table => new
                {
                    productStaffId = table.Column<Guid>(type: "uuid", nullable: false),
                    productId = table.Column<Guid>(type: "uuid", nullable: false),
                    staffId = table.Column<Guid>(type: "uuid", nullable: false),
                    status = table.Column<bool>(type: "boolean", nullable: false),
                    valideFrom = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    valideTo = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductStaff", x => x.productStaffId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products",
                schema: "point_of_sale");

            migrationBuilder.DropTable(
                name: "ProductStaff",
                schema: "point_of_sale");
        }
    }
}
