using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GaragePRO.Migrations
{
    /// <inheritdoc />
    public partial class UpdateInvoiceModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AmountReturned",
                table: "Invoices");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "AmountReturned",
                table: "Invoices",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
