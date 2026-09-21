using Microsoft.EntityFrameworkCore;
using BaiTapLon.Models;
using BaiTapLon.Helpers;

namespace BaiTapLon.Data;

public class CoffeeShopDbContext : DbContext
{
    public CoffeeShopDbContext(DbContextOptions<CoffeeShopDbContext> options) : base(options)
    {
    }

    // 1. Phân hệ bán hàng & thực đơn
    public DbSet<Category> Categories { get; set; } = null!;
    public DbSet<Product> Products { get; set; } = null!;
    public DbSet<CoffeeTable> CoffeeTables { get; set; } = null!;
    public DbSet<Order> Orders { get; set; } = null!;
    public DbSet<OrderDetail> OrderDetails { get; set; } = null!;
    public DbSet<User> Users { get; set; } = null!;

    // 2. Phân hệ trải nghiệm khách hàng & khuyến mãi
    public DbSet<Wishlist> Wishlists { get; set; } = null!;
    public DbSet<Review> Reviews { get; set; } = null!;
    public DbSet<Voucher> Vouchers { get; set; } = null!;
    public DbSet<News> News { get; set; } = null!;

    // 3. Phân hệ quản lý kho nguyên liệu & công thức pha chế
    public DbSet<Supplier> Suppliers { get; set; } = null!;
    public DbSet<Ingredient> Ingredients { get; set; } = null!;
    public DbSet<Recipe> Recipes { get; set; } = null!;

    // 4. Phân hệ quản lý nhân sự & ca trực
    public DbSet<Shift> Shifts { get; set; } = null!;
    public DbSet<EmployeeSchedule> EmployeeSchedules { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Precision configuration
        modelBuilder.Entity<Product>()
            .Property(p => p.Price)
            .HasPrecision(18, 2);

        modelBuilder.Entity<Order>()
            .Property(o => o.TotalAmount)
            .HasPrecision(18, 2);

        modelBuilder.Entity<OrderDetail>()
            .Property(od => od.UnitPrice)
            .HasPrecision(18, 2);

        modelBuilder.Entity<Ingredient>()
            .Property(i => i.UnitPrice)
            .HasPrecision(18, 2);

        modelBuilder.Entity<Ingredient>()
            .Property(i => i.QuantityInStock)
            .HasPrecision(18, 2);

        modelBuilder.Entity<Ingredient>()
            .Property(i => i.MinimumStock)
            .HasPrecision(18, 2);

        modelBuilder.Entity<Recipe>()
            .Property(r => r.AmountNeeded)
            .HasPrecision(18, 2);

        modelBuilder.Entity<Shift>()
            .Property(s => s.HourlyWage)
            .HasPrecision(18, 2);

        modelBuilder.Entity<Voucher>()
            .Property(v => v.MaxDiscountAmount)
            .HasPrecision(18, 2);

        modelBuilder.Entity<Voucher>()
            .Property(v => v.MinOrderAmount)
            .HasPrecision(18, 2);

        // Unique username
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Username)
            .IsUnique();

        // Unique voucher code
        modelBuilder.Entity<Voucher>()
            .HasIndex(v => v.Code)
            .IsUnique();

        // Seed Categories
        modelBuilder.Entity<Category>().HasData(
            new Category { CategoryId = 1, Name = "Cà Phê Truyền Thống", Description = "Cà phê pha phin truyền thống Việt Nam đậm đà bản sắc", DisplayOrder = 1, Icon = "fa-coffee" },
            new Category { CategoryId = 2, Name = "Cà Phê Pha Máy (Espresso)", Description = "Espresso, Cappuccino, Latte phong cách Ý thượng hạng", DisplayOrder = 2, Icon = "fa-mug-hot" },
            new Category { CategoryId = 3, Name = "Trà & Trà Trái Cây", Description = "Trà đào cam sả, trà vải, trà sen thanh mát ngọt dịu", DisplayOrder = 3, Icon = "fa-leaf" },
            new Category { CategoryId = 4, Name = "Đá Xay & Sinh Tố (Ice Blended)", Description = "Matcha, Socola, Sinh tố xoài béo ngậy thơm ngon", DisplayOrder = 4, Icon = "fa-glass-water" },
            new Category { CategoryId = 5, Name = "Bánh Ngọt & Tráng Miệng", Description = "Tiramisu, Croissant, Bánh phô mai nướng thơm lừng", DisplayOrder = 5, Icon = "fa-cookie-bite" }
        );

        // Seed Products
        modelBuilder.Entity<Product>().HasData(
            new Product { ProductId = 1, Name = "Cà Phê Đen Đá", Price = 25000, CategoryId = 1, Description = "Robusta nguyên chất Đắk Lắk pha phin truyền thống, vị đắng thanh đậm đà.", ImageUrl = "/images/products/ca-phe-den.jpg", IsAvailable = true, IsFeatured = true },
            new Product { ProductId = 2, Name = "Cà Phê Sữa Đá", Price = 29000, CategoryId = 1, Description = "Cà phê đậm đà hòa quyện cùng sữa đặc béo ngậy, thức uống quốc dân.", ImageUrl = "/images/products/ca-phe-sua.jpg", IsAvailable = true, IsFeatured = true },
            new Product { ProductId = 3, Name = "Bạc Xỉu Sài Gòn", Price = 32000, CategoryId = 1, Description = "Nhiều sữa đặc và sữa tươi kèm một chút cà phê thơm dịu dàng.", ImageUrl = "/images/products/bac-xiu.jpg", IsAvailable = true, IsFeatured = false },
            new Product { ProductId = 4, Name = "Espresso Đậm Vị", Price = 35000, CategoryId = 2, Description = "Chiết xuất áp suất cao từ hạt Arabica Cầu Đất nguyên chất.", ImageUrl = "/images/products/espresso.jpg", IsAvailable = true, IsFeatured = false },
            new Product { ProductId = 5, Name = "Cappuccino Ý", Price = 45000, CategoryId = 2, Description = "Espresso kết hợp sữa nóng và lớp bọt sữa dày mịn rắc bột cacao.", ImageUrl = "/images/products/cappuccino.jpg", IsAvailable = true, IsFeatured = true },
            new Product { ProductId = 6, Name = "Caramel Macchiato", Price = 49000, CategoryId = 2, Description = "Sốt caramel béo ngọt quyện cùng vị đắng nhẹ của cà phê pha máy.", ImageUrl = "/images/products/macchiato.jpg", IsAvailable = true, IsFeatured = true },
            new Product { ProductId = 7, Name = "Trà Đào Cam Sả", Price = 39000, CategoryId = 3, Description = "Trà đen hương đào thanh mát, sả tươi ngát hương cùng miếng đào giòn ngọt.", ImageUrl = "/images/products/tra-dao.jpg", IsAvailable = true, IsFeatured = true },
            new Product { ProductId = 8, Name = "Trà Vải Hoa Nhài", Price = 39000, CategoryId = 3, Description = "Trà lài thơm nức kết hợp quả vải ngâm ngọt mọng nước.", ImageUrl = "/images/products/tra-vai.jpg", IsAvailable = true, IsFeatured = false },
            new Product { ProductId = 9, Name = "Matcha Đá Xay Kem Béo", Price = 49000, CategoryId = 4, Description = "Bột trà xanh Nhật Bản xay nhuyễn với đá viên và lớp kem tươi whipping cream.", ImageUrl = "/images/products/matcha-ice.jpg", IsAvailable = true, IsFeatured = true },
            new Product { ProductId = 10, Name = "Socola Bạc Hà Đá Xay", Price = 49000, CategoryId = 4, Description = "Hương vị sô cô la đậm đà the mát cùng tinh chất bạc hà sảng khoái.", ImageUrl = "/images/products/choco-mint.jpg", IsAvailable = true, IsFeatured = false },
            new Product { ProductId = 11, Name = "Bánh Tiramisu Ý", Price = 38000, CategoryId = 5, Description = "Bánh kem phô mai mascarpone đượm vị cà phê rượu nhẹ rắc bột cacao.", ImageUrl = "/images/products/tiramisu.jpg", IsAvailable = true, IsFeatured = true },
            new Product { ProductId = 12, Name = "Bánh Croissant Bơ Pháp", Price = 32000, CategoryId = 5, Description = "Bánh sừng bò nướng giòn rụm với nhiều lớp bơ thơm ngát.", ImageUrl = "/images/products/croissant.jpg", IsAvailable = true, IsFeatured = false }
        );

        // Seed Coffee Tables
        modelBuilder.Entity<CoffeeTable>().HasData(
            new CoffeeTable { TableId = 1, TableName = "Bàn 01", Capacity = 2, Area = "Tầng 1", Status = "Available" },
            new CoffeeTable { TableId = 2, TableName = "Bàn 02", Capacity = 4, Area = "Tầng 1", Status = "Available" },
            new CoffeeTable { TableId = 3, TableName = "Bàn 03", Capacity = 4, Area = "Tầng 1", Status = "Occupied" },
            new CoffeeTable { TableId = 4, TableName = "Bàn 04", Capacity = 6, Area = "Tầng 1", Status = "Available" },
            new CoffeeTable { TableId = 5, TableName = "Bàn 05", Capacity = 2, Area = "Tầng 2", Status = "Available" },
            new CoffeeTable { TableId = 6, TableName = "Bàn 06", Capacity = 4, Area = "Tầng 2", Status = "Reserved" },
            new CoffeeTable { TableId = 7, TableName = "Bàn 07", Capacity = 6, Area = "Tầng 2", Status = "Available" },
            new CoffeeTable { TableId = 8, TableName = "Bàn Sân Vườn 01", Capacity = 4, Area = "Sân Vườn", Status = "Available" },
            new CoffeeTable { TableId = 9, TableName = "Bàn Sân Vườn 02", Capacity = 8, Area = "Sân Vườn", Status = "Available" }
        );

        // Seed Users
        modelBuilder.Entity<User>().HasData(
            new User
            {
                UserId = 1,
                Username = "admin",
                PasswordHash = PasswordHelper.HashPassword("admin123"),
                FullName = "Quản Trị Viên",
                Email = "admin@cafe.vn",
                PhoneNumber = "0901234567",
                Role = "Admin",
                CreatedAt = new DateTime(2026, 1, 1)
            },
            new User
            {
                UserId = 2,
                Username = "staff",
                PasswordHash = PasswordHelper.HashPassword("staff123"),
                FullName = "Nhân Viên Thu Ngân",
                Email = "staff@cafe.vn",
                PhoneNumber = "0908888999",
                Role = "Staff",
                CreatedAt = new DateTime(2026, 1, 1)
            },
            new User
            {
                UserId = 3,
                Username = "khachhang",
                PasswordHash = PasswordHelper.HashPassword("123456"),
                FullName = "Nguyễn Văn Khách",
                Email = "khach@gmail.com",
                PhoneNumber = "0912345678",
                Role = "Customer",
                CreatedAt = new DateTime(2026, 1, 1)
            }
        );

        // Seed Suppliers
        modelBuilder.Entity<Supplier>().HasData(
            new Supplier { SupplierId = 1, Name = "Nông Trại Cà Phê Cầu Đất Farm", ContactPerson = "Nguyễn Hữu Đạt", PhoneNumber = "0987111222", Email = "dat@caudatfarm.vn", Address = "Xã Xuân Trường, TP. Đà Lạt, Lâm Đồng" },
            new Supplier { SupplierId = 2, Name = "Công Ty Sữa Vinamilk Chi Nhánh Đà Lạt", ContactPerson = "Trần Thị Mai", PhoneNumber = "0988222333", Email = "mai.tt@vinamilk.com.vn", Address = "Khu công nghiệp Lộc Phát, Lâm Đồng" },
            new Supplier { SupplierId = 3, Name = "Công Ty Nguyên Liệu Pha Chế Tân Nhất Hương", ContactPerson = "Lê Quang Minh", PhoneNumber = "0909333444", Email = "minh@tannhathuong.com", Address = "Quận Tân Phú, TP. Hồ Chí Minh" },
            new Supplier { SupplierId = 4, Name = "Công Ty Bao Bì & Cốc Giấy Thân Thiện Eco Cup", ContactPerson = "Phạm Gia Huy", PhoneNumber = "0918444555", Email = "contact@ecocup.vn", Address = "Gia Lâm, Hà Nội" }
        );

        // Seed Ingredients
        modelBuilder.Entity<Ingredient>().HasData(
            new Ingredient { IngredientId = 1, Name = "Hạt Cà Phê Arabica Cầu Đất", Unit = "kg", QuantityInStock = 50, MinimumStock = 10, UnitPrice = 280000, SupplierId = 1 },
            new Ingredient { IngredientId = 2, Name = "Hạt Cà Phê Robusta Buôn Ma Thuột", Unit = "kg", QuantityInStock = 80, MinimumStock = 15, UnitPrice = 160000, SupplierId = 1 },
            new Ingredient { IngredientId = 3, Name = "Sữa Đặc Có Đường Ông Thọ", Unit = "hộp", QuantityInStock = 120, MinimumStock = 20, UnitPrice = 24000, SupplierId = 2 },
            new Ingredient { IngredientId = 4, Name = "Sữa Tươi Thanh Trùng 100%", Unit = "lít", QuantityInStock = 60, MinimumStock = 15, UnitPrice = 35000, SupplierId = 2 },
            new Ingredient { IngredientId = 5, Name = "Trà Đen Hương Đào Cao Cấp", Unit = "kg", QuantityInStock = 25, MinimumStock = 5, UnitPrice = 220000, SupplierId = 3 },
            new Ingredient { IngredientId = 6, Name = "Đào Miếng Ngâm Nước Đường", Unit = "hộp", QuantityInStock = 45, MinimumStock = 10, UnitPrice = 42000, SupplierId = 3 },
            new Ingredient { IngredientId = 7, Name = "Bột Trà Xanh Matcha Uji Nhật Bản", Unit = "kg", QuantityInStock = 15, MinimumStock = 3, UnitPrice = 650000, SupplierId = 3 },
            new Ingredient { IngredientId = 8, Name = "Sốt Caramel Torani Nhập Khẩu", Unit = "chai", QuantityInStock = 20, MinimumStock = 5, UnitPrice = 185000, SupplierId = 3 },
            new Ingredient { IngredientId = 9, Name = "Bột Cacao Nguyên Chất", Unit = "kg", QuantityInStock = 18, MinimumStock = 5, UnitPrice = 210000, SupplierId = 3 },
            new Ingredient { IngredientId = 10, Name = "Cốc Giấy Take-away & Nắp Sinh Học", Unit = "cái", QuantityInStock = 2000, MinimumStock = 300, UnitPrice = 1500, SupplierId = 4 }
        );

        // Seed Recipes (Định lượng cho từng ly đồ uống)
        modelBuilder.Entity<Recipe>().HasData(
            new Recipe { RecipeId = 1, ProductId = 1, IngredientId = 2, AmountNeeded = 25, Unit = "g" }, // Cà phê đen: 25g Robusta
            new Recipe { RecipeId = 2, ProductId = 2, IngredientId = 2, AmountNeeded = 25, Unit = "g" }, // Cà phê sữa: 25g Robusta
            new Recipe { RecipeId = 3, ProductId = 2, IngredientId = 3, AmountNeeded = 30, Unit = "g" }, // Cà phê sữa: 30g Sữa đặc
            new Recipe { RecipeId = 4, ProductId = 4, IngredientId = 1, AmountNeeded = 18, Unit = "g" }, // Espresso: 18g Arabica
            new Recipe { RecipeId = 5, ProductId = 5, IngredientId = 1, AmountNeeded = 18, Unit = "g" }, // Cappuccino: 18g Arabica
            new Recipe { RecipeId = 6, ProductId = 5, IngredientId = 4, AmountNeeded = 150, Unit = "ml" }, // Cappuccino: 150ml Sữa tươi
            new Recipe { RecipeId = 7, ProductId = 7, IngredientId = 5, AmountNeeded = 15, Unit = "g" }, // Trà đào: 15g Trà đen
            new Recipe { RecipeId = 8, ProductId = 7, IngredientId = 6, AmountNeeded = 50, Unit = "g" }, // Trà đào: 50g Đào ngâm
            new Recipe { RecipeId = 9, ProductId = 9, IngredientId = 7, AmountNeeded = 10, Unit = "g" }, // Matcha đá xay: 10g Matcha
            new Recipe { RecipeId = 10, ProductId = 9, IngredientId = 4, AmountNeeded = 100, Unit = "ml" } // Matcha đá xay: 100ml Sữa tươi
        );

        // Seed Shifts
        modelBuilder.Entity<Shift>().HasData(
            new Shift { ShiftId = 1, ShiftName = "Ca Sáng (Mở Cửa & Chuẩn Bị)", StartTime = new TimeSpan(7, 0, 0), EndTime = new TimeSpan(12, 0, 0), HourlyWage = 26000 },
            new Shift { ShiftId = 2, ShiftName = "Ca Chiều (Phục Vụ Cao Điểm)", StartTime = new TimeSpan(12, 0, 0), EndTime = new TimeSpan(17, 30, 0), HourlyWage = 26000 },
            new Shift { ShiftId = 3, ShiftName = "Ca Tối (Chill & Dọn Dẹp)", StartTime = new TimeSpan(17, 30, 0), EndTime = new TimeSpan(22, 30, 0), HourlyWage = 29000 }
        );

        // Seed EmployeeSchedules
        modelBuilder.Entity<EmployeeSchedule>().HasData(
            new EmployeeSchedule { ScheduleId = 1, UserId = 2, ShiftId = 1, WorkDate = new DateTime(2026, 9, 21), Status = "Completed", Note = "Trực quầy thu ngân và kiểm tra quầy bánh" },
            new EmployeeSchedule { ScheduleId = 2, UserId = 2, ShiftId = 2, WorkDate = new DateTime(2026, 9, 22), Status = "Scheduled", Note = "Phụ trách pha chế máy Espresso" },
            new EmployeeSchedule { ScheduleId = 3, UserId = 2, ShiftId = 3, WorkDate = new DateTime(2026, 9, 23), Status = "Scheduled", Note = "Trực bàn và kiểm kê kho cuối ca" }
        );

        // Seed Vouchers
        modelBuilder.Entity<Voucher>().HasData(
            new Voucher { VoucherId = 1, Code = "CHAOBAN", DiscountPercent = 10, MaxDiscountAmount = 30000, MinOrderAmount = 50000, StartDate = new DateTime(2026, 1, 1), EndDate = new DateTime(2026, 12, 31), UsageLimit = 500, UsedCount = 42, IsActive = true },
            new Voucher { VoucherId = 2, Code = "COFFEE20", DiscountPercent = 20, MaxDiscountAmount = 50000, MinOrderAmount = 80000, StartDate = new DateTime(2026, 1, 1), EndDate = new DateTime(2026, 12, 31), UsageLimit = 200, UsedCount = 88, IsActive = true },
            new Voucher { VoucherId = 3, Code = "FREESHIP", DiscountPercent = 15, MaxDiscountAmount = 25000, MinOrderAmount = 60000, StartDate = new DateTime(2026, 1, 1), EndDate = new DateTime(2026, 12, 31), UsageLimit = 1000, UsedCount = 156, IsActive = true },
            new Voucher { VoucherId = 4, Code = "VIPSTUDENT", DiscountPercent = 15, MaxDiscountAmount = 40000, MinOrderAmount = 40000, StartDate = new DateTime(2026, 1, 1), EndDate = new DateTime(2026, 12, 31), UsageLimit = 300, UsedCount = 65, IsActive = true }
        );

        // Seed News
        modelBuilder.Entity<News>().HasData(
            new News
            {
                NewsId = 1,
                Title = "Nghệ Thuật Thưởng Thức Cà Phê Pha Phin Truyền Thống Việt Nam",
                Slug = "nghe-thuat-thuong-thuc-ca-phe-pha-phin",
                Summary = "Từng giọt cà phê nhỏ chậm rãi qua chiếc phin nhôm không chỉ là cách chiết xuất hương vị, mà là cả một nét văn hóa sống chậm thanh tao của người Việt.",
                Content = "<p>Cà phê phin từ lâu đã trở thành biểu tượng của sự tĩnh lặng và kiên nhẫn. Những giọt Robusta sánh đậm hòa cùng sữa đặc béo ngọt tạo nên bản hòa ca vị giác khó quên...</p>",
                ImageUrl = "/images/products/ca-phe-den.jpg",
                AuthorId = 1,
                PublishedAt = new DateTime(2026, 9, 15),
                ViewCount = 385,
                IsActive = true
            },
            new News
            {
                NewsId = 2,
                Title = "Hành Trình Hạt Arabica Từ Vùng Đất Cầu Đất Đến Tách Cà Phê Của Bạn",
                Slug = "hanh-trinh-hat-arabica-cau-dat",
                Summary = "Nằm ở độ cao trên 1500m so với mực nước biển, Cầu Đất (Đà Lạt) được thiên nhiên ưu ái khí hậu ôn đới lý tưởng để ươm mầm những hạt Arabica thơm ngọt dịu dàng.",
                Content = "<p>Những trái cà phê chín đỏ được người nông dân hái thủ công, sơ chế ướt và rang mộc ở mức vừa phải để lưu giữ trọn vẹn hương hoa cỏ thanh nhã...</p>",
                ImageUrl = "/images/products/espresso.jpg",
                AuthorId = 1,
                PublishedAt = new DateTime(2026, 9, 18),
                ViewCount = 290,
                IsActive = true
            },
            new News
            {
                NewsId = 3,
                Title = "Khai Trương Không Gian Rooftop & Sân Vườn Xanh Mát",
                Slug = "khai-truong-khong-gian-rooftop-san-vuon",
                Summary = "Coffee Paradise chính thức ra mắt khu vực sân vườn ngoài trời ngập tràn cây xanh và góc rooftop ngắm hoàng hôn cực chill dành cho các bạn trẻ.",
                Content = "<p>Với mong muốn mang lại nguồn cảm hứng bất tận khi làm việc và trò chuyện, khu vực sân vườn được trang bị hệ thống phun sương mát mẻ và ổ cắm điện tiện lợi...</p>",
                ImageUrl = "/images/products/tra-dao.jpg",
                AuthorId = 1,
                PublishedAt = new DateTime(2026, 9, 20),
                ViewCount = 512,
                IsActive = true
            }
        );

        // Seed Reviews
        modelBuilder.Entity<Review>().HasData(
            new Review { ReviewId = 1, ProductId = 2, UserId = 3, CustomerName = "Nguyễn Văn Khách", Rating = 5, Comment = "Cà phê sữa đá rất đậm đà, chuẩn vị Tây Nguyên, không bị ngọt gắt. Rất hài lòng!", CreatedAt = new DateTime(2026, 9, 18), IsApproved = true },
            new Review { ReviewId = 2, ProductId = 7, UserId = 3, CustomerName = "Trần Hoàng Anh", Rating = 5, Comment = "Trà đào cam sả thơm lừng vị sả tươi, miếng đào giòn ngọt và thanh mát.", CreatedAt = new DateTime(2026, 9, 19), IsApproved = true },
            new Review { ReviewId = 3, ProductId = 5, UserId = null, CustomerName = "Lê Thu Thảo", Rating = 4, Comment = "Cappuccino bọt sữa vẽ hình rất đẹp, ấm nóng và béo mịn. Sẽ quay lại thử thêm món khác!", CreatedAt = new DateTime(2026, 9, 19), IsApproved = true },
            new Review { ReviewId = 4, ProductId = 9, UserId = null, CustomerName = "Phạm Minh Đức", Rating = 5, Comment = "Matcha đá xay thơm chuẩn matcha Nhật, kem whipping cream béo ngậy ăn cực mê!", CreatedAt = new DateTime(2026, 9, 20), IsApproved = true },
            new Review { ReviewId = 5, ProductId = 11, UserId = null, CustomerName = "Vũ Mai Linh", Rating = 5, Comment = "Bánh Tiramisu mềm mịn, đậm đà vị cà phê rượu và không bị ngấy xíu nào.", CreatedAt = new DateTime(2026, 9, 20), IsApproved = true }
        );

        // Seed Wishlists
        modelBuilder.Entity<Wishlist>().HasData(
            new Wishlist { WishlistId = 1, UserId = 3, ProductId = 2, CreatedAt = new DateTime(2026, 9, 18) },
            new Wishlist { WishlistId = 2, UserId = 3, ProductId = 7, CreatedAt = new DateTime(2026, 9, 19) },
            new Wishlist { WishlistId = 3, UserId = 3, ProductId = 5, CreatedAt = new DateTime(2026, 9, 20) }
        );
    }
}
