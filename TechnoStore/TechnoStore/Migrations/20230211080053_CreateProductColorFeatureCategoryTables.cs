using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TechnoStore.Migrations
{
    public partial class CreateProductColorFeatureCategoryTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Colors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Colors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Categoryİd = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SellPrice = table.Column<double>(type: "float", nullable: false),
                    DiscountPrice = table.Column<double>(type: "float", nullable: false),
                    CostPrice = table.Column<double>(type: "float", nullable: false),
                    IsAvailablity = table.Column<bool>(type: "bit", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Features",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    ColorId = table.Column<int>(type: "int", nullable: true),
                    Ekran = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DaxiliYaddaş = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    OperativYaddaş = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    EsasKamera = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    OnKamera = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    NüveSayı = table.Column<int>(type: "int", nullable: true),
                    ProsessorunAdı = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ProsessorunTezliyi = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    EmeliyyatSistemi = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    EmeliyyatSistemiVersiyası = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    İstehsalİli = table.Column<int>(type: "int", nullable: true),
                    Çeki = table.Column<double>(type: "float", nullable: true),
                    İstehsalçı = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    EkranNovu = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    EkranIcazesi = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Tezlik = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SesSistemi = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IşığınNövü = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Cheki = table.Column<double>(type: "float", maxLength: 50, nullable: true),
                    Olchu = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    İstehsalcı = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Features", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Features_Colors_ColorId",
                        column: x => x.ColorId,
                        principalTable: "Colors",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Features_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Features_ColorId",
                table: "Features",
                column: "ColorId");

            migrationBuilder.CreateIndex(
                name: "IX_Features_ProductId",
                table: "Features",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryId",
                table: "Products",
                column: "CategoryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Features");

            migrationBuilder.DropTable(
                name: "Colors");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
