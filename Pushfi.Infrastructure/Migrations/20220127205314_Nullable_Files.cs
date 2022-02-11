using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pushfi.Infrastructure.Migrations
{
    public partial class Nullable_Files : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BrokerEntity_AspNetUsers_UserId",
                table: "BrokerEntity");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BrokerEntity",
                table: "BrokerEntity");

            migrationBuilder.RenameTable(
                name: "BrokerEntity",
                newName: "Broker");

            migrationBuilder.RenameIndex(
                name: "IX_BrokerEntity_UserId",
                table: "Broker",
                newName: "IX_Broker_UserId");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "Broker",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Broker",
                table: "Broker",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Broker_AspNetUsers_UserId",
                table: "Broker",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Broker_AspNetUsers_UserId",
                table: "Broker");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Broker",
                table: "Broker");

            migrationBuilder.RenameTable(
                name: "Broker",
                newName: "BrokerEntity");

            migrationBuilder.RenameIndex(
                name: "IX_Broker_UserId",
                table: "BrokerEntity",
                newName: "IX_BrokerEntity_UserId");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "BrokerEntity",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BrokerEntity",
                table: "BrokerEntity",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BrokerEntity_AspNetUsers_UserId",
                table: "BrokerEntity",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
