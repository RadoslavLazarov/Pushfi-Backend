using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pushfi.Infrastructure.Migrations
{
    public partial class Add_ScoreFactors_And_CreditScores_To_CustomerEmailHistory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreditScore",
                table: "CustomerEmailHistory",
                newName: "CreditScoreTUC");

            migrationBuilder.AddColumn<int>(
                name: "CreditScoreEQF",
                table: "CustomerEmailHistory",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CreditScoreEXP",
                table: "CustomerEmailHistory",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ScoreFactors",
                table: "CustomerEmailHistory",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreditScoreEQF",
                table: "CustomerEmailHistory");

            migrationBuilder.DropColumn(
                name: "CreditScoreEXP",
                table: "CustomerEmailHistory");

            migrationBuilder.DropColumn(
                name: "ScoreFactors",
                table: "CustomerEmailHistory");

            migrationBuilder.RenameColumn(
                name: "CreditScoreTUC",
                table: "CustomerEmailHistory",
                newName: "CreditScore");
        }
    }
}
