using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pos.Api.Migrations
{
    /// <inheritdoc />
    public partial class FixedOneVariable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                @"ALTER TABLE point_of_sale.""ProductStaff"" 
          ALTER COLUMN ""staffId"" TYPE integer USING ""staffId""::integer;"
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "staffId",
                schema: "point_of_sale",
                table: "ProductStaff",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");
        }
    }
}
