using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pos.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddColumnTypeToBusinessTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Type",
                schema: "point_of_sale",
                table: "Businesses",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                schema: "point_of_sale",
                table: "Businesses");
        }
    }
}
