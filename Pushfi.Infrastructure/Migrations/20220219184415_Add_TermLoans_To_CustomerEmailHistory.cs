using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pushfi.Infrastructure.Migrations
{
    public partial class Add_TermLoans_To_CustomerEmailHistory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "HighTermLoan",
                table: "CustomerEmailHistory",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LowTermLoan",
                table: "CustomerEmailHistory",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HighTermLoan",
                table: "CustomerEmailHistory");

            migrationBuilder.DropColumn(
                name: "LowTermLoan",
                table: "CustomerEmailHistory");
        }
    }
}
