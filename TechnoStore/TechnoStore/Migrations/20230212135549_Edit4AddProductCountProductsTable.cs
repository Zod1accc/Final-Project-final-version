using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TechnoStore.Migrations
{
    public partial class Edit4AddProductCountProductsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProdutCount",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProdutCount",
                table: "Products");
        }
    }
}
