using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GaragePRO.Data.Migrations
{
    /// <inheritdoc />
    public partial class RenameCustomerIdInVehicle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vehicles_Customers_customerId",
                table: "Vehicles");

            migrationBuilder.RenameColumn(
                name: "year",
                table: "Vehicles",
                newName: "Year");

            migrationBuilder.RenameColumn(
                name: "model",
                table: "Vehicles",
                newName: "Model");

            migrationBuilder.RenameColumn(
                name: "mileage",
                table: "Vehicles",
                newName: "Mileage");

            migrationBuilder.RenameColumn(
                name: "make",
                table: "Vehicles",
                newName: "Make");

            migrationBuilder.RenameColumn(
                name: "customerId",
                table: "Vehicles",
                newName: "CustomerId");

            migrationBuilder.RenameIndex(
                name: "IX_Vehicles_customerId",
                table: "Vehicles",
                newName: "IX_Vehicles_CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Vehicles_Customers_CustomerId",
                table: "Vehicles",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vehicles_Customers_CustomerId",
                table: "Vehicles");

            migrationBuilder.RenameColumn(
                name: "Year",
                table: "Vehicles",
                newName: "year");

            migrationBuilder.RenameColumn(
                name: "Model",
                table: "Vehicles",
                newName: "model");

            migrationBuilder.RenameColumn(
                name: "Mileage",
                table: "Vehicles",
                newName: "mileage");

            migrationBuilder.RenameColumn(
                name: "Make",
                table: "Vehicles",
                newName: "make");

            migrationBuilder.RenameColumn(
                name: "CustomerId",
                table: "Vehicles",
                newName: "customerId");

            migrationBuilder.RenameIndex(
                name: "IX_Vehicles_CustomerId",
                table: "Vehicles",
                newName: "IX_Vehicles_customerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Vehicles_Customers_customerId",
                table: "Vehicles",
                column: "customerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
