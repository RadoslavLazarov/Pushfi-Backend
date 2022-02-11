using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pushfi.Infrastructure.Migrations
{
    public partial class Fix_DegreeObtained_Column_Name : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DegreeObrained",
                table: "Customer",
                newName: "DegreeObtained");

            migrationBuilder.AddColumn<Guid>(
                name: "BrokerId",
                table: "Customer",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BrokerId",
                table: "Customer");

            migrationBuilder.RenameColumn(
                name: "DegreeObtained",
                table: "Customer",
                newName: "DegreeObrained");
        }
    }
}
