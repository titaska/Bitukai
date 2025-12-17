using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pos.Api.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "point_of_sale");

            migrationBuilder.CreateTable(
                name: "Businesses",
                schema: "point_of_sale",
                columns: table => new
                {
                    RegistrationNumber = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    VatCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Location = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Phone = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Email = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    CurrencyCode = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Businesses", x => x.RegistrationNumber);
                });

            migrationBuilder.CreateTable(
                name: "OrderLineOptions",
                schema: "point_of_sale",
                columns: table => new
                {
                    orderLineOptionId = table.Column<Guid>(type: "uuid", nullable: false),
                    orderLineId = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    value = table.Column<string>(type: "text", nullable: false),
                    priceDelta = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderLineOptions", x => x.orderLineOptionId);
                });

            migrationBuilder.CreateTable(
                name: "OrderLineTaxes",
                schema: "point_of_sale",
                columns: table => new
                {
                    orderLineTaxId = table.Column<Guid>(type: "uuid", nullable: false),
                    orderLineId = table.Column<Guid>(type: "uuid", nullable: false),
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
                    orderId = table.Column<Guid>(type: "uuid", nullable: false),
                    registrationNumber = table.Column<string>(type: "text", nullable: false),
                    customerId = table.Column<Guid>(type: "uuid", nullable: true),
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

            migrationBuilder.CreateTable(
                name: "Products",
                schema: "point_of_sale",
                columns: table => new
                {
                    productId = table.Column<Guid>(type: "uuid", nullable: false),
                    registrationNumber = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    type = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    basePrice = table.Column<decimal>(type: "numeric(12,2)", nullable: false),
                    taxCode = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    status = table.Column<bool>(type: "boolean", nullable: false),
                    durationMinutes = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.productId);
                });

            migrationBuilder.CreateTable(
                name: "reservations",
                schema: "point_of_sale",
                columns: table => new
                {
                    appointmentId = table.Column<string>(type: "text", nullable: false),
                    registrationNumber = table.Column<string>(type: "text", nullable: false),
                    customerId = table.Column<string>(type: "text", nullable: false),
                    serviceProductId = table.Column<string>(type: "text", nullable: false),
                    employeeId = table.Column<string>(type: "uuid", nullable: false),
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

            migrationBuilder.CreateTable(
                name: "taxes",
                schema: "point_of_sale",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    percentage = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_taxes", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Staff",
                schema: "point_of_sale",
                columns: table => new
                {
                    StaffId = table.Column<Guid>(type: "uuid", nullable: false),
                    RegistrationNumber = table.Column<string>(type: "character varying(50)", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false),
                    FirstName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    PhoneNumber = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Role = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    HireDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Password = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Staff", x => x.StaffId);
                    table.ForeignKey(
                        name: "FK_Staff_Businesses_RegistrationNumber",
                        column: x => x.RegistrationNumber,
                        principalSchema: "point_of_sale",
                        principalTable: "Businesses",
                        principalColumn: "RegistrationNumber",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderLines",
                schema: "point_of_sale",
                columns: table => new
                {
                    orderLineId = table.Column<Guid>(type: "uuid", nullable: false),
                    orderId = table.Column<Guid>(type: "uuid", nullable: false),
                    productId = table.Column<Guid>(type: "uuid", nullable: false),
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
                    table.ForeignKey(
                        name: "FK_OrderLines_Orders_orderId",
                        column: x => x.orderId,
                        principalSchema: "point_of_sale",
                        principalTable: "Orders",
                        principalColumn: "orderId",
                        onDelete: ReferentialAction.Cascade);
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
                    valideFrom = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    valideTo = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductStaff", x => x.productStaffId);
                    table.ForeignKey(
                        name: "FK_ProductStaff_Products_productId",
                        column: x => x.productId,
                        principalSchema: "point_of_sale",
                        principalTable: "Products",
                        principalColumn: "productId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductStaff_Staff_staffId",
                        column: x => x.staffId,
                        principalSchema: "point_of_sale",
                        principalTable: "Staff",
                        principalColumn: "StaffId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderLines_orderId",
                schema: "point_of_sale",
                table: "OrderLines",
                column: "orderId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductStaff_productId_staffId",
                schema: "point_of_sale",
                table: "ProductStaff",
                columns: new[] { "productId", "staffId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductStaff_staffId",
                schema: "point_of_sale",
                table: "ProductStaff",
                column: "staffId");

            migrationBuilder.CreateIndex(
                name: "IX_Staff_RegistrationNumber",
                schema: "point_of_sale",
                table: "Staff",
                column: "RegistrationNumber");

            migrationBuilder.CreateIndex(
                name: "IX_taxes_name",
                schema: "point_of_sale",
                table: "taxes",
                column: "name",
                unique: true);
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
                name: "ProductStaff",
                schema: "point_of_sale");

            migrationBuilder.DropTable(
                name: "reservations",
                schema: "point_of_sale");

            migrationBuilder.DropTable(
                name: "taxes",
                schema: "point_of_sale");

            migrationBuilder.DropTable(
                name: "Orders",
                schema: "point_of_sale");

            migrationBuilder.DropTable(
                name: "Products",
                schema: "point_of_sale");

            migrationBuilder.DropTable(
                name: "Staff",
                schema: "point_of_sale");

            migrationBuilder.DropTable(
                name: "Businesses",
                schema: "point_of_sale");
        }
    }
}
