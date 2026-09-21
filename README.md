# project-coffe-gtvt

> **Coffee Paradise** - Hệ thống Quản lý và Đặt món Quán Café trực tuyến phát triển trên nền tảng **ASP.NET Core MVC (.NET 10)** & **MySQL (Entity Framework Core Code First)**.

---

## 🌟 Tính Năng Chính
1. **Phía Khách Hàng:**
   - Xem thực đơn đa dạng (Cà phê pha phin, Espresso, Trà trái cây, Đá xay, Bánh ngọt).
   - Lọc món theo danh mục bằng AJAX mượt mà, hỗ trợ cuộn ngang trên điện thoại di động (Responsive).
   - Tìm kiếm và phân trang kết hợp sắp xếp giá.
   - Thêm vào giỏ hàng lưu bằng `Session`, hiển thị Toast notification hiện đại.
   - Đặt món trực tuyến: chọn hình thức Dùng tại quán (chọn bàn) hoặc Mang về, chọn phương thức thanh toán.
   - Đăng ký, đăng nhập tài khoản khách hàng (`Cookie Authentication`).

2. **Phía Quản Trị & Vận Hành (Admin & Staff):**
   - Dashboard thống kê doanh thu, tổng số đơn hàng, sơ đồ bàn và top món bán chạy.
   - Quản lý danh mục món (Categories), quản lý món ăn & thức uống (Products) kèm tải ảnh.
   - Quản lý sơ đồ bàn quán (CoffeeTables).
   - Tiếp nhận và xử lý hóa đơn, đơn đặt món (Orders & OrderDetails).
   - Mở rộng 15 bảng: Kho nguyên vật liệu (Ingredients), Nhà cung cấp (Suppliers), Công thức pha chế (Recipes), Đánh giá (Reviews), Tin tức (News), Mã giảm giá (Vouchers), Ca trực (Shifts) & Lịch phân công (EmployeeSchedules).

---

## 🚀 Hướng Dẫn Cài Đặt & Khởi Chạy

### 1. Yêu Cầu Hệ Thống
- .NET SDK 10.0+
- MySQL Server (XAMPP / MySQL Community Server)

### 2. Cấu Hình Cơ Sở Dữ Liệu
Cấu hình chuỗi kết nối trong `appsettings.json`:
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Port=3306;Database=coffeeshop_db;User=root;Password=;CharSet=utf8mb4;"
}
```
Khởi tạo cơ sở dữ liệu (đã có sẵn file script `coffeeshop_db.sql` hoặc chạy EF Core Migration):
```bash
dotnet ef database update
```

### 3. Chạy Ứng Dụng
```bash
dotnet run --project BaiTapLon.csproj
```
Truy cập trình duyệt: **http://localhost:5102**

### 4. Tài Khoản Demo Sẵn Có
- **Admin (Quản trị viên):** `admin` / `admin123`
- **Staff (Nhân viên thu ngân):** `staff` / `staff123`
- **Customer (Khách hàng):** `khachhang` / `123456`
