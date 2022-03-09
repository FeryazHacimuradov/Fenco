using Microsoft.EntityFrameworkCore.Migrations;

namespace Fenco.Migrations
{
    public partial class DeleteIsRegister : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsRegistered",
                table: "AspNetUsers");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsRegistered",
                table: "AspNetUsers",
                type: "bit",
                nullable: true);
        }
    }
}
