using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Pos.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddStaffServiceAssignments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "HireDate",
                schema: "point_of_sale",
                table: "Staff",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "startTime",
                schema: "point_of_sale",
                table: "reservations",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "createdAt",
                schema: "point_of_sale",
                table: "Orders",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "closedAt",
                schema: "point_of_sale",
                table: "Orders",
                type: "timestamp without time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "Services",
                schema: "point_of_sale",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Price = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Services", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StaffServiceAssignments",
                schema: "point_of_sale",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    StaffId = table.Column<int>(type: "integer", nullable: false),
                    ServiceId = table.Column<int>(type: "integer", nullable: false),
                    Revenue = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StaffServiceAssignments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StaffServiceAssignments_Services_ServiceId",
                        column: x => x.ServiceId,
                        principalSchema: "point_of_sale",
                        principalTable: "Services",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StaffServiceAssignments_Staff_StaffId",
                        column: x => x.StaffId,
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
                name: "IX_StaffServiceAssignments_ServiceId",
                schema: "point_of_sale",
                table: "StaffServiceAssignments",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_StaffServiceAssignments_StaffId_ServiceId",
                schema: "point_of_sale",
                table: "StaffServiceAssignments",
                columns: new[] { "StaffId", "ServiceId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderLines_Orders_orderId",
                schema: "point_of_sale",
                table: "OrderLines",
                column: "orderId",
                principalSchema: "point_of_sale",
                principalTable: "Orders",
                principalColumn: "orderId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderLines_Orders_orderId",
                schema: "point_of_sale",
                table: "OrderLines");

            migrationBuilder.DropTable(
                name: "StaffServiceAssignments",
                schema: "point_of_sale");

            migrationBuilder.DropTable(
                name: "Services",
                schema: "point_of_sale");

            migrationBuilder.DropIndex(
                name: "IX_OrderLines_orderId",
                schema: "point_of_sale",
                table: "OrderLines");

            migrationBuilder.AlterColumn<DateTime>(
                name: "HireDate",
                schema: "point_of_sale",
                table: "Staff",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "startTime",
                schema: "point_of_sale",
                table: "reservations",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "createdAt",
                schema: "point_of_sale",
                table: "Orders",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "closedAt",
                schema: "point_of_sale",
                table: "Orders",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldNullable: true);
        }
    }
}
