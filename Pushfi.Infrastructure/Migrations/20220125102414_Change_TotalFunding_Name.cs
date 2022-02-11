using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pushfi.Infrastructure.Migrations
{
    public partial class Change_TotalFunding_Name : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TotalFundingAchieved",
                table: "CustomerEmailHistory",
                newName: "BackEndFee");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "BackEndFee",
                table: "CustomerEmailHistory",
                newName: "TotalFundingAchieved");
        }
    }
}
