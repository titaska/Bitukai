using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pos.Api.Migrations
{ 
    public partial class AddClientInfoToReservations : Migration
    {
    /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "customerId",
                schema: "point_of_sale",
                table: "reservations",
                newName: "ClientSurname");

            migrationBuilder.AddColumn<string>(
                name: "ClientName",
                schema: "point_of_sale",
                table: "reservations",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ClientPhone",
                schema: "point_of_sale",
                table: "reservations",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

    /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClientName",
                schema: "point_of_sale",
                table: "reservations");

            migrationBuilder.DropColumn(
                name: "ClientPhone",
                schema: "point_of_sale",
                table: "reservations");

            migrationBuilder.RenameColumn(
                name: "ClientSurname",
                schema: "point_of_sale",
                table: "reservations",
                newName: "customerId");
        }
    }
       
}
