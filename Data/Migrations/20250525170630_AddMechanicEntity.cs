using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GaragePRO.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddMechanicEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "createdAt",
                table: "Mechanics");

            migrationBuilder.RenameColumn(
                name: "phone",
                table: "Mechanics",
                newName: "Phone");

            migrationBuilder.RenameColumn(
                name: "fullName",
                table: "Mechanics",
                newName: "FullName");

            migrationBuilder.AddColumn<string>(
                name: "AssignedVehicleBrand",
                table: "Mechanics",
                type: "TEXT",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "EmploymentStartYear",
                table: "Mechanics",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Seniority",
                table: "Mechanics",
                type: "TEXT",
                maxLength: 30,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Mechanics",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AssignedVehicleBrand",
                table: "Mechanics");

            migrationBuilder.DropColumn(
                name: "EmploymentStartYear",
                table: "Mechanics");

            migrationBuilder.DropColumn(
                name: "Seniority",
                table: "Mechanics");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Mechanics");

            migrationBuilder.RenameColumn(
                name: "Phone",
                table: "Mechanics",
                newName: "phone");

            migrationBuilder.RenameColumn(
                name: "FullName",
                table: "Mechanics",
                newName: "fullName");

            migrationBuilder.AddColumn<DateTime>(
                name: "createdAt",
                table: "Mechanics",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
