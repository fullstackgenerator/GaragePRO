using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GaragePRO.Migrations
{
    /// <inheritdoc />
    public partial class RemoveInvoiceFieldInvoiceNumber : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InvoiceNumber",
                table: "Invoices");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "InvoiceNumber",
                table: "Invoices",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }
    }
}
