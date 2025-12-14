using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pos.Api.Migrations
{
    /// <inheritdoc />
    public partial class FixServiceMoneyTypesAndRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StaffServiceAssignments_Services_ServiceId",
                schema: "point_of_sale",
                table: "StaffServiceAssignments");

            migrationBuilder.AlterColumn<decimal>(
                name: "Revenue",
                schema: "point_of_sale",
                table: "StaffServiceAssignments",
                type: "numeric(12,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                schema: "point_of_sale",
                table: "Services",
                type: "numeric(12,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.AddForeignKey(
                name: "FK_StaffServiceAssignments_Services_ServiceId",
                schema: "point_of_sale",
                table: "StaffServiceAssignments",
                column: "ServiceId",
                principalSchema: "point_of_sale",
                principalTable: "Services",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StaffServiceAssignments_Services_ServiceId",
                schema: "point_of_sale",
                table: "StaffServiceAssignments");

            migrationBuilder.AlterColumn<decimal>(
                name: "Revenue",
                schema: "point_of_sale",
                table: "StaffServiceAssignments",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(12,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                schema: "point_of_sale",
                table: "Services",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(12,2)");

            migrationBuilder.AddForeignKey(
                name: "FK_StaffServiceAssignments_Services_ServiceId",
                schema: "point_of_sale",
                table: "StaffServiceAssignments",
                column: "ServiceId",
                principalSchema: "point_of_sale",
                principalTable: "Services",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
