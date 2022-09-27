using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pushfi.Infrastructure.Migrations
{
    public partial class Add_ExternalUrl_In_BrokerEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ExternalUrl",
                table: "Broker",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExternalUrl",
                table: "Broker");
        }
    }
}
