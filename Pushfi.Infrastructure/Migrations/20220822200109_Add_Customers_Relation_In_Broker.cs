using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pushfi.Infrastructure.Migrations
{
    public partial class Add_Customers_Relation_In_Broker : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Customer_BrokerId",
                table: "Customer",
                column: "BrokerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Customer_Broker_BrokerId",
                table: "Customer",
                column: "BrokerId",
                principalTable: "Broker",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customer_Broker_BrokerId",
                table: "Customer");

            migrationBuilder.DropIndex(
                name: "IX_Customer_BrokerId",
                table: "Customer");
        }
    }
}
