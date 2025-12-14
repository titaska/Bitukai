using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pos.Api.Migrations
{
    /// <inheritdoc />
    public partial class VariableConsistency : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "staffId",
                schema: "point_of_sale",
                table: "ProductStaff",
                type: "text",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<string>(
                name: "taxCode",
                schema: "point_of_sale",
                table: "Products",
                type: "text",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<string>(
                name: "registrationNumber",
                schema: "point_of_sale",
                table: "Products",
                type: "text",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<string>(
                name: "assignedStaffId",
                schema: "point_of_sale",
                table: "OrderLines",
                type: "text",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "appointmentId",
                schema: "point_of_sale",
                table: "OrderLines",
                type: "text",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "staffId",
                schema: "point_of_sale",
                table: "ProductStaff",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<Guid>(
                name: "taxCode",
                schema: "point_of_sale",
                table: "Products",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<Guid>(
                name: "registrationNumber",
                schema: "point_of_sale",
                table: "Products",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<Guid>(
                name: "assignedStaffId",
                schema: "point_of_sale",
                table: "OrderLines",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "appointmentId",
                schema: "point_of_sale",
                table: "OrderLines",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);
        }
    }
}
