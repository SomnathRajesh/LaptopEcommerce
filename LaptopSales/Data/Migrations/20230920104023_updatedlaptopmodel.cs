using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LaptopSales.Data.Migrations
{
    public partial class updatedlaptopmodel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TagId",
                table: "Laptops",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Laptops_TagId",
                table: "Laptops",
                column: "TagId");

            migrationBuilder.AddForeignKey(
                name: "FK_Laptops_Tags_TagId",
                table: "Laptops",
                column: "TagId",
                principalTable: "Tags",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Laptops_Tags_TagId",
                table: "Laptops");

            migrationBuilder.DropIndex(
                name: "IX_Laptops_TagId",
                table: "Laptops");

            migrationBuilder.DropColumn(
                name: "TagId",
                table: "Laptops");
        }
    }
}
