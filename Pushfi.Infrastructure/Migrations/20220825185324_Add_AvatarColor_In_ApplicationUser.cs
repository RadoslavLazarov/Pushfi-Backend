using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pushfi.Infrastructure.Migrations
{
    public partial class Add_AvatarColor_In_ApplicationUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AvatarColor",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AvatarColor",
                table: "AspNetUsers");
        }
    }
}
