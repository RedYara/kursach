using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication3.Migrations
{
    public partial class test : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_App",
                table: "App");

            migrationBuilder.RenameTable(
                name: "App",
                newName: "Apps");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Apps",
                table: "Apps",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Apps_Id",
                table: "Apps",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Apps",
                table: "Apps");

            migrationBuilder.DropIndex(
                name: "IX_Apps_Id",
                table: "Apps");

            migrationBuilder.RenameTable(
                name: "Apps",
                newName: "App");

            migrationBuilder.AddPrimaryKey(
                name: "PK_App",
                table: "App",
                column: "Id");
        }
    }
}
