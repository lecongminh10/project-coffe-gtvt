using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BaiTapLon.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Icon = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.CategoryId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "CoffeeTables",
                columns: table => new
                {
                    TableId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TableName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Capacity = table.Column<int>(type: "int", nullable: false),
                    Area = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Status = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoffeeTables", x => x.TableId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Username = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PasswordHash = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FullName = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PhoneNumber = table.Column<string>(type: "varchar(15)", maxLength: 15, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Role = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    ProductId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Price = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Description = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ImageUrl = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsAvailable = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IsFeatured = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.ProductId);
                    table.ForeignKey(
                        name: "FK_Products_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "CategoryId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    OrderId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    OrderDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CustomerName = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CustomerPhone = table.Column<string>(type: "varchar(15)", maxLength: 15, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TableId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Status = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PaymentMethod = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsPaid = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Notes = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.OrderId);
                    table.ForeignKey(
                        name: "FK_Orders_CoffeeTables_TableId",
                        column: x => x.TableId,
                        principalTable: "CoffeeTables",
                        principalColumn: "TableId");
                    table.ForeignKey(
                        name: "FK_Orders_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "OrderDetails",
                columns: table => new
                {
                    OrderDetailId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderDetails", x => x.OrderDetailId);
                    table.ForeignKey(
                        name: "FK_OrderDetails_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "OrderId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderDetails_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "CategoryId", "Description", "DisplayOrder", "Icon", "Name" },
                values: new object[,]
                {
                    { 1, "Cà phê pha phin truyền thống Việt Nam đậm đà bản sắc", 1, "fa-coffee", "Cà Phê Truyền Thống" },
                    { 2, "Espresso, Cappuccino, Latte phong cách Ý thượng hạng", 2, "fa-mug-hot", "Cà Phê Pha Máy (Espresso)" },
                    { 3, "Trà đào cam sả, trà vải, trà sen thanh mát ngọt dịu", 3, "fa-leaf", "Trà & Trà Trái Cây" },
                    { 4, "Matcha, Socola, Sinh tố xoài béo ngậy thơm ngon", 4, "fa-glass-water", "Đá Xay & Sinh Tố (Ice Blended)" },
                    { 5, "Tiramisu, Croissant, Bánh phô mai nướng thơm lừng", 5, "fa-cookie-bite", "Bánh Ngọt & Tráng Miệng" }
                });

            migrationBuilder.InsertData(
                table: "CoffeeTables",
                columns: new[] { "TableId", "Area", "Capacity", "Status", "TableName" },
                values: new object[,]
                {
                    { 1, "Tầng 1", 2, "Available", "Bàn 01" },
                    { 2, "Tầng 1", 4, "Available", "Bàn 02" },
                    { 3, "Tầng 1", 4, "Occupied", "Bàn 03" },
                    { 4, "Tầng 1", 6, "Available", "Bàn 04" },
                    { 5, "Tầng 2", 2, "Available", "Bàn 05" },
                    { 6, "Tầng 2", 4, "Reserved", "Bàn 06" },
                    { 7, "Tầng 2", 6, "Available", "Bàn 07" },
                    { 8, "Sân Vườn", 4, "Available", "Bàn Sân Vườn 01" },
                    { 9, "Sân Vườn", 8, "Available", "Bàn Sân Vườn 02" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "CreatedAt", "Email", "FullName", "PasswordHash", "PhoneNumber", "Role", "Username" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "admin@cafe.vn", "Quản Trị Viên", "JAvlGPq9JyTdtvBO6x2llnRI1+gxwIyPqCKAn3THIKk=", "0901234567", "Admin", "admin" },
                    { 2, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "staff@cafe.vn", "Nhân Viên Thu Ngân", "EBdue3sk0xes/PjSBkz9LyThVPe1qWYDB31e+BPWprY=", "0908888999", "Staff", "staff" },
                    { 3, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "khach@gmail.com", "Nguyễn Văn Khách", "jZae727K08KaOmKSgOaGzww/XVqGr/PKEgIMkjrcbJI=", "0912345678", "Customer", "khachhang" }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "ProductId", "CategoryId", "Description", "ImageUrl", "IsAvailable", "IsFeatured", "Name", "Price" },
                values: new object[,]
                {
                    { 1, 1, "Robusta nguyên chất Đắk Lắk pha phin truyền thống, vị đắng thanh đậm đà.", "/images/products/ca-phe-den.jpg", true, true, "Cà Phê Đen Đá", 25000m },
                    { 2, 1, "Cà phê đậm đà hòa quyện cùng sữa đặc béo ngậy, thức uống quốc dân.", "/images/products/ca-phe-sua.jpg", true, true, "Cà Phê Sữa Đá", 29000m },
                    { 3, 1, "Nhiều sữa đặc và sữa tươi kèm một chút cà phê thơm dịu dàng.", "/images/products/bac-xiu.jpg", true, false, "Bạc Xỉu Sài Gòn", 32000m },
                    { 4, 2, "Chiết xuất áp suất cao từ hạt Arabica Cầu Đất nguyên chất.", "/images/products/espresso.jpg", true, false, "Espresso Đậm Vị", 35000m },
                    { 5, 2, "Espresso kết hợp sữa nóng và lớp bọt sữa dày mịn rắc bột cacao.", "/images/products/cappuccino.jpg", true, true, "Cappuccino Ý", 45000m },
                    { 6, 2, "Sốt caramel béo ngọt quyện cùng vị đắng nhẹ của cà phê pha máy.", "/images/products/macchiato.jpg", true, true, "Caramel Macchiato", 49000m },
                    { 7, 3, "Trà đen hương đào thanh mát, sả tươi ngát hương cùng miếng đào giòn ngọt.", "/images/products/tra-dao.jpg", true, true, "Trà Đào Cam Sả", 39000m },
                    { 8, 3, "Trà lài thơm nức kết hợp quả vải ngâm ngọt mọng nước.", "/images/products/tra-vai.jpg", true, false, "Trà Vải Hoa Nhài", 39000m },
                    { 9, 4, "Bột trà xanh Nhật Bản xay nhuyễn với đá viên và lớp kem tươi whipping cream.", "/images/products/matcha-ice.jpg", true, true, "Matcha Đá Xay Kem Béo", 49000m },
                    { 10, 4, "Hương vị sô cô la đậm đà the mát cùng tinh chất bạc hà sảng khoái.", "/images/products/choco-mint.jpg", true, false, "Socola Bạc Hà Đá Xay", 49000m },
                    { 11, 5, "Bánh kem phô mai mascarpone đượm vị cà phê rượu nhẹ rắc bột cacao.", "/images/products/tiramisu.jpg", true, true, "Bánh Tiramisu Ý", 38000m },
                    { 12, 5, "Bánh sừng bò nướng giòn rụm với nhiều lớp bơ thơm ngát.", "/images/products/croissant.jpg", true, false, "Bánh Croissant Bơ Pháp", 32000m }
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_OrderId",
                table: "OrderDetails",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_ProductId",
                table: "OrderDetails",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_TableId",
                table: "Orders",
                column: "TableId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_UserId",
                table: "Orders",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryId",
                table: "Products",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Username",
                table: "Users",
                column: "Username",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderDetails");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "CoffeeTables");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
