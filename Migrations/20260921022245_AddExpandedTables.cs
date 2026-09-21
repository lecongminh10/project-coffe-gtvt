using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BaiTapLon.Migrations
{
    /// <inheritdoc />
    public partial class AddExpandedTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "News",
                columns: table => new
                {
                    NewsId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Slug = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Summary = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Content = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ImageUrl = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AuthorId = table.Column<int>(type: "int", nullable: true),
                    PublishedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ViewCount = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_News", x => x.NewsId);
                    table.ForeignKey(
                        name: "FK_News_Users_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Reviews",
                columns: table => new
                {
                    ReviewId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    CustomerName = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Rating = table.Column<int>(type: "int", nullable: false),
                    Comment = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    IsApproved = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reviews", x => x.ReviewId);
                    table.ForeignKey(
                        name: "FK_Reviews_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reviews_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Shifts",
                columns: table => new
                {
                    ShiftId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ShiftName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    StartTime = table.Column<TimeSpan>(type: "time(6)", nullable: false),
                    EndTime = table.Column<TimeSpan>(type: "time(6)", nullable: false),
                    HourlyWage = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shifts", x => x.ShiftId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Suppliers",
                columns: table => new
                {
                    SupplierId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ContactPerson = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PhoneNumber = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Address = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Suppliers", x => x.SupplierId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Vouchers",
                columns: table => new
                {
                    VoucherId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DiscountPercent = table.Column<int>(type: "int", nullable: false),
                    MaxDiscountAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    MinOrderAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UsageLimit = table.Column<int>(type: "int", nullable: false),
                    UsedCount = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vouchers", x => x.VoucherId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Wishlists",
                columns: table => new
                {
                    WishlistId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wishlists", x => x.WishlistId);
                    table.ForeignKey(
                        name: "FK_Wishlists_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Wishlists_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "EmployeeSchedules",
                columns: table => new
                {
                    ScheduleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ShiftId = table.Column<int>(type: "int", nullable: false),
                    WorkDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Status = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Note = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeSchedules", x => x.ScheduleId);
                    table.ForeignKey(
                        name: "FK_EmployeeSchedules_Shifts_ShiftId",
                        column: x => x.ShiftId,
                        principalTable: "Shifts",
                        principalColumn: "ShiftId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmployeeSchedules_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Ingredients",
                columns: table => new
                {
                    IngredientId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Unit = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    QuantityInStock = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    MinimumStock = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    SupplierId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ingredients", x => x.IngredientId);
                    table.ForeignKey(
                        name: "FK_Ingredients_Suppliers_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "Suppliers",
                        principalColumn: "SupplierId");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Recipes",
                columns: table => new
                {
                    RecipeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    IngredientId = table.Column<int>(type: "int", nullable: false),
                    AmountNeeded = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Unit = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recipes", x => x.RecipeId);
                    table.ForeignKey(
                        name: "FK_Recipes_Ingredients_IngredientId",
                        column: x => x.IngredientId,
                        principalTable: "Ingredients",
                        principalColumn: "IngredientId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Recipes_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "News",
                columns: new[] { "NewsId", "AuthorId", "Content", "ImageUrl", "IsActive", "PublishedAt", "Slug", "Summary", "Title", "ViewCount" },
                values: new object[,]
                {
                    { 1, 1, "<p>Cà phê phin từ lâu đã trở thành biểu tượng của sự tĩnh lặng và kiên nhẫn. Những giọt Robusta sánh đậm hòa cùng sữa đặc béo ngọt tạo nên bản hòa ca vị giác khó quên...</p>", "/images/products/ca-phe-den.jpg", true, new DateTime(2026, 9, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "nghe-thuat-thuong-thuc-ca-phe-pha-phin", "Từng giọt cà phê nhỏ chậm rãi qua chiếc phin nhôm không chỉ là cách chiết xuất hương vị, mà là cả một nét văn hóa sống chậm thanh tao của người Việt.", "Nghệ Thuật Thưởng Thức Cà Phê Pha Phin Truyền Thống Việt Nam", 385 },
                    { 2, 1, "<p>Những trái cà phê chín đỏ được người nông dân hái thủ công, sơ chế ướt và rang mộc ở mức vừa phải để lưu giữ trọn vẹn hương hoa cỏ thanh nhã...</p>", "/images/products/espresso.jpg", true, new DateTime(2026, 9, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "hanh-trinh-hat-arabica-cau-dat", "Nằm ở độ cao trên 1500m so với mực nước biển, Cầu Đất (Đà Lạt) được thiên nhiên ưu ái khí hậu ôn đới lý tưởng để ươm mầm những hạt Arabica thơm ngọt dịu dàng.", "Hành Trình Hạt Arabica Từ Vùng Đất Cầu Đất Đến Tách Cà Phê Của Bạn", 290 },
                    { 3, 1, "<p>Với mong muốn mang lại nguồn cảm hứng bất tận khi làm việc và trò chuyện, khu vực sân vườn được trang bị hệ thống phun sương mát mẻ và ổ cắm điện tiện lợi...</p>", "/images/products/tra-dao.jpg", true, new DateTime(2026, 9, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "khai-truong-khong-gian-rooftop-san-vuon", "Coffee Paradise chính thức ra mắt khu vực sân vườn ngoài trời ngập tràn cây xanh và góc rooftop ngắm hoàng hôn cực chill dành cho các bạn trẻ.", "Khai Trương Không Gian Rooftop & Sân Vườn Xanh Mát", 512 }
                });

            migrationBuilder.InsertData(
                table: "Reviews",
                columns: new[] { "ReviewId", "Comment", "CreatedAt", "CustomerName", "IsApproved", "ProductId", "Rating", "UserId" },
                values: new object[,]
                {
                    { 1, "Cà phê sữa đá rất đậm đà, chuẩn vị Tây Nguyên, không bị ngọt gắt. Rất hài lòng!", new DateTime(2026, 9, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Nguyễn Văn Khách", true, 2, 5, 3 },
                    { 2, "Trà đào cam sả thơm lừng vị sả tươi, miếng đào giòn ngọt và thanh mát.", new DateTime(2026, 9, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), "Trần Hoàng Anh", true, 7, 5, 3 },
                    { 3, "Cappuccino bọt sữa vẽ hình rất đẹp, ấm nóng và béo mịn. Sẽ quay lại thử thêm món khác!", new DateTime(2026, 9, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), "Lê Thu Thảo", true, 5, 4, null },
                    { 4, "Matcha đá xay thơm chuẩn matcha Nhật, kem whipping cream béo ngậy ăn cực mê!", new DateTime(2026, 9, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "Phạm Minh Đức", true, 9, 5, null },
                    { 5, "Bánh Tiramisu mềm mịn, đậm đà vị cà phê rượu và không bị ngấy xíu nào.", new DateTime(2026, 9, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "Vũ Mai Linh", true, 11, 5, null }
                });

            migrationBuilder.InsertData(
                table: "Shifts",
                columns: new[] { "ShiftId", "EndTime", "HourlyWage", "ShiftName", "StartTime" },
                values: new object[,]
                {
                    { 1, new TimeSpan(0, 12, 0, 0, 0), 26000m, "Ca Sáng (Mở Cửa & Chuẩn Bị)", new TimeSpan(0, 7, 0, 0, 0) },
                    { 2, new TimeSpan(0, 17, 30, 0, 0), 26000m, "Ca Chiều (Phục Vụ Cao Điểm)", new TimeSpan(0, 12, 0, 0, 0) },
                    { 3, new TimeSpan(0, 22, 30, 0, 0), 29000m, "Ca Tối (Chill & Dọn Dẹp)", new TimeSpan(0, 17, 30, 0, 0) }
                });

            migrationBuilder.InsertData(
                table: "Suppliers",
                columns: new[] { "SupplierId", "Address", "ContactPerson", "Email", "Name", "PhoneNumber" },
                values: new object[,]
                {
                    { 1, "Xã Xuân Trường, TP. Đà Lạt, Lâm Đồng", "Nguyễn Hữu Đạt", "dat@caudatfarm.vn", "Nông Trại Cà Phê Cầu Đất Farm", "0987111222" },
                    { 2, "Khu công nghiệp Lộc Phát, Lâm Đồng", "Trần Thị Mai", "mai.tt@vinamilk.com.vn", "Công Ty Sữa Vinamilk Chi Nhánh Đà Lạt", "0988222333" },
                    { 3, "Quận Tân Phú, TP. Hồ Chí Minh", "Lê Quang Minh", "minh@tannhathuong.com", "Công Ty Nguyên Liệu Pha Chế Tân Nhất Hương", "0909333444" },
                    { 4, "Gia Lâm, Hà Nội", "Phạm Gia Huy", "contact@ecocup.vn", "Công Ty Bao Bì & Cốc Giấy Thân Thiện Eco Cup", "0918444555" }
                });

            migrationBuilder.InsertData(
                table: "Vouchers",
                columns: new[] { "VoucherId", "Code", "DiscountPercent", "EndDate", "IsActive", "MaxDiscountAmount", "MinOrderAmount", "StartDate", "UsageLimit", "UsedCount" },
                values: new object[,]
                {
                    { 1, "CHAOBAN", 10, new DateTime(2026, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), true, 30000m, 50000m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 500, 42 },
                    { 2, "COFFEE20", 20, new DateTime(2026, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), true, 50000m, 80000m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 200, 88 },
                    { 3, "FREESHIP", 15, new DateTime(2026, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), true, 25000m, 60000m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1000, 156 },
                    { 4, "VIPSTUDENT", 15, new DateTime(2026, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), true, 40000m, 40000m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 300, 65 }
                });

            migrationBuilder.InsertData(
                table: "Wishlists",
                columns: new[] { "WishlistId", "CreatedAt", "ProductId", "UserId" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 9, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 3 },
                    { 2, new DateTime(2026, 9, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), 7, 3 },
                    { 3, new DateTime(2026, 9, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 5, 3 }
                });

            migrationBuilder.InsertData(
                table: "EmployeeSchedules",
                columns: new[] { "ScheduleId", "Note", "ShiftId", "Status", "UserId", "WorkDate" },
                values: new object[,]
                {
                    { 1, "Trực quầy thu ngân và kiểm tra quầy bánh", 1, "Completed", 2, new DateTime(2026, 9, 21, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, "Phụ trách pha chế máy Espresso", 2, "Scheduled", 2, new DateTime(2026, 9, 22, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3, "Trực bàn và kiểm kê kho cuối ca", 3, "Scheduled", 2, new DateTime(2026, 9, 23, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "Ingredients",
                columns: new[] { "IngredientId", "MinimumStock", "Name", "QuantityInStock", "SupplierId", "Unit", "UnitPrice" },
                values: new object[,]
                {
                    { 1, 10m, "Hạt Cà Phê Arabica Cầu Đất", 50m, 1, "kg", 280000m },
                    { 2, 15m, "Hạt Cà Phê Robusta Buôn Ma Thuột", 80m, 1, "kg", 160000m },
                    { 3, 20m, "Sữa Đặc Có Đường Ông Thọ", 120m, 2, "hộp", 24000m },
                    { 4, 15m, "Sữa Tươi Thanh Trùng 100%", 60m, 2, "lít", 35000m },
                    { 5, 5m, "Trà Đen Hương Đào Cao Cấp", 25m, 3, "kg", 220000m },
                    { 6, 10m, "Đào Miếng Ngâm Nước Đường", 45m, 3, "hộp", 42000m },
                    { 7, 3m, "Bột Trà Xanh Matcha Uji Nhật Bản", 15m, 3, "kg", 650000m },
                    { 8, 5m, "Sốt Caramel Torani Nhập Khẩu", 20m, 3, "chai", 185000m },
                    { 9, 5m, "Bột Cacao Nguyên Chất", 18m, 3, "kg", 210000m },
                    { 10, 300m, "Cốc Giấy Take-away & Nắp Sinh Học", 2000m, 4, "cái", 1500m }
                });

            migrationBuilder.InsertData(
                table: "Recipes",
                columns: new[] { "RecipeId", "AmountNeeded", "IngredientId", "ProductId", "Unit" },
                values: new object[,]
                {
                    { 1, 25m, 2, 1, "g" },
                    { 2, 25m, 2, 2, "g" },
                    { 3, 30m, 3, 2, "g" },
                    { 4, 18m, 1, 4, "g" },
                    { 5, 18m, 1, 5, "g" },
                    { 6, 150m, 4, 5, "ml" },
                    { 7, 15m, 5, 7, "g" },
                    { 8, 50m, 6, 7, "g" },
                    { 9, 10m, 7, 9, "g" },
                    { 10, 100m, 4, 9, "ml" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeSchedules_ShiftId",
                table: "EmployeeSchedules",
                column: "ShiftId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeSchedules_UserId",
                table: "EmployeeSchedules",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Ingredients_SupplierId",
                table: "Ingredients",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_News_AuthorId",
                table: "News",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_Recipes_IngredientId",
                table: "Recipes",
                column: "IngredientId");

            migrationBuilder.CreateIndex(
                name: "IX_Recipes_ProductId",
                table: "Recipes",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_ProductId",
                table: "Reviews",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_UserId",
                table: "Reviews",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Vouchers_Code",
                table: "Vouchers",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Wishlists_ProductId",
                table: "Wishlists",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Wishlists_UserId",
                table: "Wishlists",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmployeeSchedules");

            migrationBuilder.DropTable(
                name: "News");

            migrationBuilder.DropTable(
                name: "Recipes");

            migrationBuilder.DropTable(
                name: "Reviews");

            migrationBuilder.DropTable(
                name: "Vouchers");

            migrationBuilder.DropTable(
                name: "Wishlists");

            migrationBuilder.DropTable(
                name: "Shifts");

            migrationBuilder.DropTable(
                name: "Ingredients");

            migrationBuilder.DropTable(
                name: "Suppliers");
        }
    }
}
