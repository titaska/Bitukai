using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pos.Api.Migrations
{
    public partial class UpdatedOrderModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Orders table
            migrationBuilder.Sql(
                @"ALTER TABLE point_of_sale.""Orders"" 
                  ALTER COLUMN ""customerId"" TYPE uuid USING ""customerId""::uuid;"
            );

            migrationBuilder.Sql(
                @"ALTER TABLE point_of_sale.""Orders"" 
                  ALTER COLUMN ""orderId"" TYPE uuid USING ""orderId""::uuid;"
            );

            // OrderLineTaxes table
            migrationBuilder.Sql(
                @"ALTER TABLE point_of_sale.""OrderLineTaxes"" 
                  ALTER COLUMN ""orderLineId"" TYPE uuid USING ""orderLineId""::uuid;"
            );

            migrationBuilder.Sql(
                @"ALTER TABLE point_of_sale.""OrderLineTaxes"" 
                  ALTER COLUMN ""orderLineTaxId"" TYPE uuid USING ""orderLineTaxId""::uuid;"
            );

            // OrderLines table
            migrationBuilder.Sql(
                @"ALTER TABLE point_of_sale.""OrderLines"" 
                  ALTER COLUMN ""productId"" TYPE uuid USING ""productId""::uuid;"
            );

            migrationBuilder.Sql(
                @"ALTER TABLE point_of_sale.""OrderLines"" 
                  ALTER COLUMN ""orderId"" TYPE uuid USING ""orderId""::uuid;"
            );

            migrationBuilder.Sql(
                @"ALTER TABLE point_of_sale.""OrderLines"" 
                  ALTER COLUMN ""assignedStaffId"" TYPE uuid USING ""assignedStaffId""::uuid;"
            );

            migrationBuilder.Sql(
                @"ALTER TABLE point_of_sale.""OrderLines"" 
                  ALTER COLUMN ""appointmentId"" TYPE uuid USING ""appointmentId""::uuid;"
            );

            migrationBuilder.Sql(
                @"ALTER TABLE point_of_sale.""OrderLines"" 
                  ALTER COLUMN ""orderLineId"" TYPE uuid USING ""orderLineId""::uuid;"
            );

            // OrderLineOptions table
            migrationBuilder.Sql(
                @"ALTER TABLE point_of_sale.""OrderLineOptions"" 
                  ALTER COLUMN ""orderLineId"" TYPE uuid USING ""orderLineId""::uuid;"
            );

            migrationBuilder.Sql(
                @"ALTER TABLE point_of_sale.""OrderLineOptions"" 
                  ALTER COLUMN ""orderLineOptionId"" TYPE uuid USING ""orderLineOptionId""::uuid;"
            );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Revert columns back to text
            migrationBuilder.AlterColumn<string>(
                name: "customerId",
                schema: "point_of_sale",
                table: "Orders",
                type: "text",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "orderId",
                schema: "point_of_sale",
                table: "Orders",
                type: "text",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<string>(
                name: "orderLineId",
                schema: "point_of_sale",
                table: "OrderLineTaxes",
                type: "text",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<string>(
                name: "orderLineTaxId",
                schema: "point_of_sale",
                table: "OrderLineTaxes",
                type: "text",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<string>(
                name: "productId",
                schema: "point_of_sale",
                table: "OrderLines",
                type: "text",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<string>(
                name: "orderId",
                schema: "point_of_sale",
                table: "OrderLines",
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

            migrationBuilder.AlterColumn<string>(
                name: "orderLineId",
                schema: "point_of_sale",
                table: "OrderLines",
                type: "text",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<string>(
                name: "orderLineId",
                schema: "point_of_sale",
                table: "OrderLineOptions",
                type: "text",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<string>(
                name: "orderLineOptionId",
                schema: "point_of_sale",
                table: "OrderLineOptions",
                type: "text",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid");
        }
    }
}

