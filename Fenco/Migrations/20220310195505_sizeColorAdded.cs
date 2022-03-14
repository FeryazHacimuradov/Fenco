using Microsoft.EntityFrameworkCore.Migrations;

namespace Fenco.Migrations
{
    public partial class sizeColorAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ParentSizeId",
                table: "Sizes",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ParentColorId",
                table: "Colors",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Sizes_ParentSizeId",
                table: "Sizes",
                column: "ParentSizeId");

            migrationBuilder.CreateIndex(
                name: "IX_Colors_ParentColorId",
                table: "Colors",
                column: "ParentColorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Colors_Colors_ParentColorId",
                table: "Colors",
                column: "ParentColorId",
                principalTable: "Colors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Sizes_Sizes_ParentSizeId",
                table: "Sizes",
                column: "ParentSizeId",
                principalTable: "Sizes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Colors_Colors_ParentColorId",
                table: "Colors");

            migrationBuilder.DropForeignKey(
                name: "FK_Sizes_Sizes_ParentSizeId",
                table: "Sizes");

            migrationBuilder.DropIndex(
                name: "IX_Sizes_ParentSizeId",
                table: "Sizes");

            migrationBuilder.DropIndex(
                name: "IX_Colors_ParentColorId",
                table: "Colors");

            migrationBuilder.DropColumn(
                name: "ParentSizeId",
                table: "Sizes");

            migrationBuilder.DropColumn(
                name: "ParentColorId",
                table: "Colors");
        }
    }
}
