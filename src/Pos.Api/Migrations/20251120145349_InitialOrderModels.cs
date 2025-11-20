using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pos.Api.Migrations
{
    /// <inheritdoc />
    public partial class InitialOrderModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OrderLineOptions",
                schema: "point_of_sale",
                columns: table => new
                {
                    orderLineOptionId = table.Column<string>(type: "text", nullable: false),
                    orderLineId = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    value = table.Column<string>(type: "text", nullable: false),
                    priceDelta = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderLineOptions", x => x.orderLineOptionId);
                });

            migrationBuilder.CreateTable(
                name: "OrderLines",
                schema: "point_of_sale",
                columns: table => new
                {
                    orderLineId = table.Column<string>(type: "text", nullable: false),
                    orderId = table.Column<string>(type: "text", nullable: false),
                    productId = table.Column<string>(type: "text", nullable: false),
                    quantity = table.Column<int>(type: "integer", nullable: false),
                    assignedStaffId = table.Column<string>(type: "text", nullable: true),
                    appointmentId = table.Column<string>(type: "text", nullable: true),
                    notes = table.Column<string>(type: "text", nullable: true),
                    unitPrice = table.Column<decimal>(type: "numeric", nullable: false),
                    subTotal = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderLines", x => x.orderLineId);
                });

            migrationBuilder.CreateTable(
                name: "OrderLineTaxes",
                schema: "point_of_sale",
                columns: table => new
                {
                    orderLineTaxId = table.Column<string>(type: "text", nullable: false),
                    orderLineId = table.Column<string>(type: "text", nullable: false),
                    taxCode = table.Column<string>(type: "text", nullable: false),
                    taxPercentage = table.Column<decimal>(type: "numeric", nullable: false),
                    taxAmount = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderLineTaxes", x => x.orderLineTaxId);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                schema: "point_of_sale",
                columns: table => new
                {
                    orderId = table.Column<string>(type: "text", nullable: false),
                    registrationNumber = table.Column<string>(type: "text", nullable: false),
                    customerId = table.Column<string>(type: "text", nullable: true),
                    status = table.Column<string>(type: "text", nullable: false),
                    createdAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    closedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    serviceChargePct = table.Column<decimal>(type: "numeric", nullable: false),
                    tipAmount = table.Column<decimal>(type: "numeric", nullable: false),
                    subtotalAmount = table.Column<decimal>(type: "numeric", nullable: false),
                    taxAmount = table.Column<decimal>(type: "numeric", nullable: false),
                    discountAmount = table.Column<decimal>(type: "numeric", nullable: false),
                    serviceChargeAmount = table.Column<decimal>(type: "numeric", nullable: false),
                    totalDue = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.orderId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderLineOptions",
                schema: "point_of_sale");

            migrationBuilder.DropTable(
                name: "OrderLines",
                schema: "point_of_sale");

            migrationBuilder.DropTable(
                name: "OrderLineTaxes",
                schema: "point_of_sale");

            migrationBuilder.DropTable(
                name: "Orders",
                schema: "point_of_sale");
        }
    }
}
