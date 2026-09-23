# DANH SÁCH CHỨC NĂNG ĐÃ TRIỂN KHAI
## ĐỀ TÀI: WEBSITE QUẢN LÝ QUÁN CÀ PHÊ BREW & GO
**Nền tảng công nghệ:** ASP.NET Core MVC (.NET 10), MySQL (`coffeeshop_db`), Entity Framework Core Code First, Bootstrap 5, jQuery AJAX  
**Thư mục dự án:** `BaiTapLon/`  
**Ngày kiểm tra:** 23/09/2026  

> Lưu ý: giao diện và dữ liệu mẫu trong mã nguồn hiện vẫn sử dụng tên **Coffee Paradise**. Tài liệu này phân biệt chức năng đã hoạt động với phần mới chỉ có mô hình dữ liệu.

## 0. TỔNG QUAN MỨC ĐỘ HOÀN THIỆN

| Nhóm chức năng | Trạng thái | Phạm vi hiện có |
| --- | :---: | --- |
| Trang chủ và thực đơn | Đã hoạt động | Hiển thị, lọc, tìm kiếm, sắp xếp, phân trang và xem chi tiết món |
| Giỏ hàng và đặt món | Đã hoạt động | Giỏ hàng Session, đặt tại quán/mang đi, lưu đơn và chi tiết đơn |
| Tài khoản và phân quyền | Đã hoạt động | Đăng ký, đăng nhập Cookie, đăng xuất, phân quyền Admin/Staff |
| Quản trị bán hàng | Đã hoạt động | Dashboard, danh mục, sản phẩm, bàn và xử lý đơn |
| REST API | Đã hoạt động | API đọc sản phẩm/bàn, tìm kiếm sản phẩm và đổi trạng thái bàn |
| Kho và công thức | Đã hoạt động | Quản lý nguyên liệu/nhà cung cấp/công thức, nhập xuất, cảnh báo tồn và tự động trừ–hoàn kho |
| Nhân sự và ca làm | Đã hoạt động | Phân ca, check-in/check-out, tính giờ làm và tiền công tạm tính |
| Voucher | Đã hoạt động | Quản lý mã, kiểm tra điều kiện, giới hạn lượt và giảm tiền trên đơn |
| Tin tức, đánh giá, yêu thích | Đã hoạt động | Trang tin, gửi/duyệt đánh giá và danh sách món yêu thích theo tài khoản |
| Quản lý tài khoản | Đã hoạt động | Admin tạo tài khoản, phân vai trò, đặt lại mật khẩu và xóa có kiểm tra ràng buộc |
| Thanh toán điện tử | Chưa tích hợp thật | Có lựa chọn và QR minh họa; chưa có merchant/API callback VietQR hoặc MoMo |

---

## 1. PHÂN HỆ KHÁCH HÀNG (CLIENT / CUSTOMER)

### 1.1. Trang Chủ (`/` - `HomeController`)
* **Banner Hero & Giới thiệu:** Trình bày không gian quán, thông điệp thương hiệu và nút kêu gọi hành động (CTA) điều hướng nhanh đến thực đơn.
* **Danh mục nổi bật:** Hiển thị các nhóm đồ uống chủ đạo (Cà phê, Trà hoa quả, Đá xay, Bánh ngọt...).
* **Món nổi bật (Featured Products):** Tự động truy vấn các sản phẩm được đánh dấu `IsFeatured = true` để giới thiệu trên trang chủ. Đây chưa phải thống kê bán chạy theo số lượng đơn.

### 1.2. Thực Đơn & Tra Cứu Món (`/Product` - `ProductController`)
* **Lọc theo danh mục qua AJAX:**
  * Người dùng chọn danh mục (Pills menu), hệ thống gọi action `GetProductsPartial` và nạp PartialView `_ProductListPartial`.
  * Dữ liệu và hình ảnh cập nhật tức thời, **không giật/reload toàn bộ trang web**.
* **Tìm kiếm từ khóa kết hợp phân trang (Paging & Searching):**
  * Hỗ trợ tìm kiếm theo tên hoặc mô tả đồ uống.
  * Phân trang tự động (6 món/trang), bảo lưu trạng thái từ khóa và danh mục đang lọc khi chuyển trang.
* **Sắp xếp thực đơn:**
  * Sắp xếp theo giá tăng dần (`price_asc`), giảm dần (`price_desc`), theo tên A-Z (`name_asc`), hoặc theo độ nổi bật.
* **Chi tiết đồ uống (`/Product/Details/{id}`):**
  * Xem hình ảnh độ phân giải cao, tên, đơn giá, mô tả chi tiết, trạng thái phục vụ.
  * Tự động truy vấn và gợi ý 4 món liên quan cùng danh mục (`RelatedProducts`).
  * Cho phép chọn số lượng món và thêm trực tiếp vào giỏ hàng.

### 1.3. Quản Lý Giỏ Hàng (`/Cart` - `CartController`)
* **Thêm vào giỏ tức thời (AJAX Add-To-Cart):**
  * Nút "Chọn món" gửi yêu cầu bất đồng bộ đến `AddToCartAjax`.
  * Hiển thị thông báo Toast thành công mà không làm gián đoạn trải nghiệm duyệt menu.
  * Cập nhật số lượng hiển thị trên badge giỏ hàng thanh điều hướng thông qua `CartWidgetViewComponent`.
* **Lưu trữ giỏ hàng trong Session:**
  * Dữ liệu giỏ hàng được tuần tự hóa (JSON serialization) và lưu trữ an toàn trong `HttpContext.Session`.
* **Thao tác trên giỏ hàng:**
  * Xem danh sách món đã chọn, đơn giá, số lượng và tổng tiền tạm tính.
  * Tăng / giảm số lượng từng món hoặc xóa món khỏi giỏ hàng.

### 1.4. Đặt Món & Thanh Toán (`/Cart/Checkout`)
* **Lựa chọn hình thức phục vụ:**
  * **Dùng tại quán (Dine In):** Chọn bàn từ danh sách sơ đồ bàn trong CSDL.
  * **Mang về (Take Away):** Không bắt buộc chọn bàn.
* **Chọn phương thức thanh toán:**
  * Tiền mặt tại quầy.
  * Chuyển khoản ngân hàng/QR.
  * Ví điện tử MoMo.
  * Hệ thống hiện chỉ ghi nhận lựa chọn vào đơn hàng, chưa kết nối cổng thanh toán hoặc xác minh giao dịch tự động.
* **Thông tin giao nhận:**
  * Nhập họ tên khách hàng, số điện thoại liên hệ và ghi chú yêu cầu riêng (ít ngọt, nhiều đá...).
  * Tự động liên kết `UserId` nếu khách hàng đã đăng nhập.
* **Xử lý giao dịch & Lưu đơn:**
  * Tạo bản ghi mới trong bảng `Orders` và các dòng chi tiết `OrderDetails`.
  * Tự động đổi trạng thái bàn đã chọn sang `Occupied` (Có khách).
  * Dọn sạch giỏ hàng trong `Session` sau khi đặt thành công.
* **Xác nhận đơn hàng (`/Cart/OrderSuccess/{id}`):**
  * Hiển thị hóa đơn điện tử: Mã đơn hàng, ngày giờ, số bàn, danh sách chi tiết các món, tổng thanh toán và ghi chú.

### 1.5. Xác Thực Tài Khoản (`/Account` - `AccountController`)
* **Đăng ký tài khoản (`/Account/Register`):**
  * Khách hàng đăng ký tài khoản mới với Username, FullName, Email, SĐT, Password.
  * Mật khẩu được băm SHA-256 qua `PasswordHelper`. Cách này phục vụ bài tập nhưng nên chuyển sang ASP.NET Core Identity với thuật toán băm có salt khi triển khai thực tế.
* **Đăng nhập bằng Cookie Authentication (`/Account/Login`):**
  * Xác thực qua Cookie với `ClaimsIdentity` (lưu ID, Username, FullName, Role).
  * Tùy chọn **Ghi nhớ đăng nhập (Remember Me)** duy trì phiên làm việc trong vòng 7 ngày.
* **Điều hướng phân quyền tự động:**
  * Tài khoản có vai trò `Admin` hoặc `Staff` tự động chuyển hướng vào Phân hệ quản trị `/Admin/Dashboard`.
  * Khách hàng thông thường chuyển hướng về Trang chủ hoặc trang yêu cầu trước đó (`ReturnUrl`).
* **Đăng xuất an toàn (`/Account/Logout`):** Xóa bỏ Cookie xác thực phiên làm việc.
* **Kiểm soát truy cập (`/Account/AccessDenied`):** Chặn các yêu cầu trái quyền và hiển thị thông báo lỗi thân thiện.

### 1.6. Tiện Ích Khách Hàng
* **Voucher:** Nhập mã tại giỏ hàng; hệ thống kiểm tra thời hạn, giá trị đơn tối thiểu, giới hạn lượt và mức giảm tối đa.
* **Tin tức (`/News`):** Xem danh sách bài viết, nội dung chi tiết và lượt xem.
* **Đánh giá món:** Xem điểm trung bình, gửi nhận xét 1–5 sao; đánh giá mới chờ nhân viên duyệt.
* **Món yêu thích (`/Wishlist`):** Người dùng đăng nhập có thể thêm/bỏ và xem danh sách món đã lưu.

---

## 2. PHÂN HỆ QUẢN TRỊ & NHÂN VIÊN (ADMIN & STAFF AREA - `/Admin`)

### 2.1. Bảng Điều Khiển Thống Kê (`/Admin/Dashboard` - `DashboardController`)
* **Chỉ số KPI tổng quan:**
  * **Tổng doanh thu:** Tính toán trên các đơn hàng đã thanh toán (`IsPaid = true`) hoặc hoàn thành (`Completed`).
  * **Tổng số đơn hàng:** Thống kê toàn bộ số đơn phát sinh.
  * **Tổng số món:** Số lượng sản phẩm đồ uống đang có trên thực đơn.
  * **Tổng số khách hàng:** Số lượng tài khoản người dùng đăng ký.
* **Giám sát trạng thái bàn theo thời gian thực:**
  * Thống kê số bàn đang có khách (`Occupied`) vs số bàn còn trống (`Available`).
* **Danh sách đơn hàng mới nhất:** Hiển thị 7 đơn gần nhất kèm trạng thái, số bàn và thời gian đặt món.
* **Top sản phẩm nổi bật:** Danh sách các món đặc sắc của quán.

### 2.2. Quản Lý Sơ Đồ Bàn (`/Admin/Table` - `TableController`)
* **Sơ đồ bàn trực quan:**
  * Hiển thị danh sách bàn phân nhóm theo khu vực (Khu A - Máy lạnh, Khu B - Sân vườn, Tầng 2...).
  * Trực quan hóa trạng thái bằng màu sắc: Xanh lá (`Available` - Trống), Đỏ (`Occupied` - Đang có khách), Vàng cam (`Reserved` - Đã đặt trước).
* **Cập nhật trạng thái bàn nhanh:** Nhân viên và Quản trị viên có thể đổi trạng thái bàn nhanh chóng.
* **Thêm mới và Xóa bàn *(Quyền Admin)*:**
  * Thêm bàn mới với tên bàn, sức chứa (số ghế), khu vực.
  * Xóa bàn không còn sử dụng.

### 2.3. Quản Lý & Xử Lý Đơn Hàng (`/Admin/Order` - `OrderController`)
* **Lọc và phân loại đơn:** Lọc danh sách theo trạng thái: Chờ xử lý (`Pending`), Đang pha chế (`Processing`), Hoàn thành (`Completed`), Đã hủy (`Cancelled`).
* **Chi tiết đơn hàng (`/Admin/Order/Details/{id}`):**
  * Thông tin khách hàng, số điện thoại, ngày giờ đặt, bàn phục vụ, phương thức thanh toán.
  * Bảng chi tiết từng món đồ uống, số lượng, đơn giá, thành tiền.
* **Cập nhật tiến độ đơn & Thanh toán:**
  * Thay đổi trạng thái xử lý đơn hàng và đánh dấu đã thanh toán (`IsPaid`).
  * **Tự động đồng bộ trạng thái bàn thông minh:**
    * Chuyển đơn sang `Processing` $\rightarrow$ Tự động đổi trạng thái bàn tương ứng sang `Occupied`.
    * Chuyển đơn sang `Completed` hoặc `Cancelled` $\rightarrow$ Tự động giải phóng bàn về `Available`.
* **In hóa đơn bán lẻ (Print Receipt):** Hỗ trợ định dạng in hóa đơn chuyên nghiệp trực tiếp từ trình duyệt cho thu ngân.
* **Xóa đơn hàng *(Quyền Admin)*:** Xóa các đơn rác hoặc đơn thử nghiệm.

### 2.4. Quản Lý Danh Mục Món (`/Admin/Category` - `CategoryController` - *Quyền Admin*)
* **Xem danh sách danh mục:** Thống kê tên danh mục, icon đại diện, thứ tự hiển thị và số lượng món hiện có trong danh mục.
* **Thêm danh mục mới:** Nhập tên nhóm đồ uống, mô tả, chọn class biểu tượng FontAwesome, cài đặt thứ tự hiển thị.
* **Chỉnh sửa danh mục:** Cập nhật thông tin và thứ tự sắp xếp.
* **Xóa danh mục an toàn:** Hệ thống tự động kiểm tra và ngăn chặn hành vi xóa nếu danh mục đó vẫn còn chứa sản phẩm, phòng tránh lỗi toàn vẹn dữ liệu.

### 2.5. Quản Lý Món Ăn / Đồ Uống (`/Admin/Product` - `ProductController` - *Quyền Admin*)
* **Danh sách thực đơn quản trị:** Tra cứu theo từ khóa, lọc theo nhóm danh mục.
* **Thêm món mới kèm Upload ảnh thực tế:**
  * Nhập tên món, chọn danh mục, đơn giá, mô tả.
  * Đánh dấu món đặc sắc (`IsFeatured`), kích hoạt phục vụ (`IsAvailable`).
  * **Xử lý tải tệp ảnh:** Nhận file từ `IFormFile`, tạo mã GUID tránh trùng tên, lưu trữ thực tế vào thư mục `wwwroot/uploads/` và lưu đường dẫn vào CSDL.
* **Cập nhật món:** Chỉnh sửa thông tin, giá bán và thay thế ảnh đại diện mới.
* **Xóa món:** Gỡ bỏ món khỏi danh mục phục vụ.

### 2.6. Kho Nguyên Liệu (`/Admin/Inventory`)
* Quản lý nguyên liệu, nhà cung cấp, tồn tối thiểu, đơn giá và hệ số quy đổi kg/g, lít/ml hoặc hộp/g.
* Nhập, xuất, điều chỉnh tồn kho và lưu nhật ký `StockTransactions`.
* Cảnh báo trực quan khi số lượng tồn nhỏ hơn hoặc bằng định mức tối thiểu.

### 2.7. Công Thức Pha Chế (`/Admin/Recipe`)
* Khai báo định lượng nguyên liệu cho từng món.
* Khi đơn chuyển sang `Processing` hoặc `Completed`, hệ thống kiểm tra đủ nguyên liệu và trừ kho trong transaction.
* Cờ `InventoryDeducted` ngăn trừ hai lần; khi hủy đơn, nguyên liệu được hoàn kho và ghi lịch sử.

### 2.8. Nhân Sự & Chấm Công (`/Admin/Workforce`)
* Admin phân nhân viên theo ngày và ca làm; ngăn phân trùng ca.
* Staff chỉ xem lịch của mình và thực hiện check-in/check-out.
* Tính số giờ thực tế và tiền công tạm tính theo mức lương giờ của ca.

### 2.9. Voucher (`/Admin/Voucher`)
* Quản lý phần trăm giảm, mức giảm tối đa, giá trị đơn tối thiểu, thời hạn, trạng thái và giới hạn lượt dùng.
* Ghi `VoucherCode`, `DiscountAmount` và tổng tiền sau giảm vào đơn hàng.

### 2.10. Nội Dung & Chăm Sóc Khách Hàng
* **Tin tức (`/Admin/News`):** Tạo, công khai và xóa bài viết; tự sinh slug thân thiện.
* **Đánh giá (`/Admin/Review`):** Duyệt, ẩn hoặc xóa nhận xét của khách hàng.
* **Yêu thích:** Dữ liệu tách theo tài khoản và chống lưu trùng một món.

### 2.11. Quản Lý Tài Khoản (`/Admin/User`)
* Admin tạo tài khoản Customer/Staff/Admin, cập nhật vai trò và đặt mật khẩu.
* Ngăn xóa tài khoản đang đăng nhập hoặc còn dữ liệu nghiệp vụ liên quan.

---

## 3. DỊCH VỤ RESTFUL WEB API (`/api/`)

Hệ thống cung cấp sẵn các endpoint RESTful API chuẩn JSON phục vụ tích hợp đa nền tảng (POS tại quầy, App Mobile, Màn hình bếp KDS):

| Phương thức | Endpoint URL | Chức Năng |
| :---: | :--- | :--- |
| `GET` | `/api/ProductsApi` | Lấy danh sách toàn bộ món đồ uống (hỗ trợ query string `?categoryId=`) |
| `GET` | `/api/ProductsApi/{id}` | Lấy thông tin chi tiết một món theo ID |
| `GET` | `/api/ProductsApi/search?q={keyword}` | Tìm kiếm món đồ uống theo từ khóa |
| `GET` | `/api/TablesApi` | Lấy danh sách tất cả các bàn và trạng thái phục vụ |
| `PUT` | `/api/TablesApi/{id}/status` | Cập nhật trạng thái bàn từ xa (gửi body JSON `{ status: "Occupied" }`) |

---

## 4. THIẾT KẾ CƠ SỞ DỮ LIỆU MỞ RỘNG (16 ENTITIES)

Cơ sở dữ liệu **MySQL** (`coffeeshop_db`) sử dụng Entity Framework Core Code First với 16 thực thể:

1. **`Categories`**: Nhóm danh mục đồ uống và bánh ngọt.
2. **`Products`**: Thông tin món ăn, đồ uống, giá cả, hình ảnh, trạng thái phục vụ.
3. **`CoffeeTables`**: Sơ đồ bàn, số lượng chỗ ngồi, khu vực và trạng thái.
4. **`Users`**: Tài khoản người dùng, phân quyền vai trò (`Admin`, `Staff`, `Customer`).
5. **`Orders`**: Đơn đặt món, khách hàng, phương thức thanh toán, tổng tiền, trạng thái đơn.
6. **`OrderDetails`**: Dòng chi tiết hóa đơn (món gọi, số lượng, đơn giá tại thời điểm đặt).
7. **`Reviews`**: Đánh giá xếp hạng sao (1-5 sao) và nhận xét của khách hàng về món.
8. **`Wishlists`**: Danh sách đồ uống yêu thích được lưu lại của từng khách hàng.
9. **`Vouchers`**: Quản lý mã giảm giá, mức chiết khấu, hạn mức và thời gian áp dụng.
10. **`News`**: Tin tức, bài viết truyền thông, chương trình khuyến mãi của quán.
11. **`Suppliers`**: Danh bạ các nhà cung ứng nguyên vật liệu pha chế.
12. **`Ingredients`**: Quản lý kho nguyên vật liệu, đơn vị tính, số lượng tồn kho và định mức tối thiểu.
13. **`Recipes`**: Công thức định mức nguyên vật liệu cấu thành nên mỗi món đồ uống.
14. **`Shifts`**: Danh mục các ca làm việc (Sáng, Chiều, Tối, Đêm) và mức lương theo giờ.
15. **`EmployeeSchedules`**: Bảng phân công lịch làm việc chi tiết cho nhân viên theo từng ca trực.
16. **`StockTransactions`**: Nhật ký nhập, xuất, điều chỉnh và hoàn kho theo đơn hàng.

---

## 5. ĐIỂM NỔI BẬT VỀ MẶT KỸ THUẬT & KIẾN TRÚC

* **Tối ưu trải nghiệm người dùng (UX/UI):**
  * Thiết kế giao diện hiện đại phong cách F&B với tông màu ấm cúng, tương thích Responsive trên Desktop, Tablet và Mobile.
  * Tích hợp công nghệ **jQuery AJAX** cho giỏ hàng, bộ lọc danh mục và phân trang giúp thao tác không độ trễ, không reload trang.
* **Mô hình hóa thành phần (ViewComponents):**
  * Tách biệt logic điều hướng danh mục (`CategoryMenuViewComponent`) và biểu tượng giỏ hàng mini (`CartWidgetViewComponent`).
* **Bảo mật nhiều lớp:**
  * Các form nghiệp vụ chính như đăng nhập, đăng ký, checkout và quản trị có token chống CSRF `[ValidateAntiForgeryToken]`.
  * Phân quyền truy cập dựa trên vai trò `[Authorize(Roles = "Admin")]` và `[Authorize(Roles = "Admin,Staff")]`.
  * Mật khẩu đang được băm một chiều bằng SHA-256.

---

## 6. CHỨC NĂNG CHƯA ĐƯỢC TRIỂN KHAI HOÀN CHỈNH

Các phần còn cần thông tin hoặc hạ tầng bên ngoài để hoàn thiện ở mức sản phẩm thực tế:

1. Kết nối merchant VietQR/MoMo thật, ký request và xử lý callback xác nhận thanh toán.
2. Bảng lương chính thức gồm phụ cấp, tăng ca, khấu trừ và quy trình duyệt; hiện hệ thống mới tính tiền công tạm tính từ giờ check-in/out.
3. Phiếu mua hàng/nhập hàng có công nợ nhà cung cấp; hiện kho hỗ trợ nhập, xuất và điều chỉnh trực tiếp.
4. Nâng cấp mật khẩu SHA-256 sang ASP.NET Core Identity với salt và chính sách khóa tài khoản.

## 7. KẾT QUẢ KIỂM TRA TRIỂN KHAI

Ngày 23/09/2026, dự án được kiểm tra với các kết quả:

| Hạng mục | Kết quả |
| --- | --- |
| `dotnet build --no-restore` | Thành công, 0 lỗi, 0 cảnh báo |
| Kết nối MySQL `coffeeshop_db` | Thành công |
| `/`, `/Product`, `/Cart`, `/Account/Login` | HTTP 200 |
| `/Admin/Dashboard` khi chưa đăng nhập | HTTP 302, chuyển đến trang đăng nhập đúng phân quyền |
| `/api/ProductsApi`, `/api/TablesApi` | HTTP 200 |
| Các route `/Admin/Inventory`, `/Admin/Recipe`, `/Admin/Workforce`, `/Admin/Voucher`, `/Admin/Review`, `/Admin/News`, `/Admin/User` | HTTP 200 |
| Trừ kho theo công thức | 25 g: `80.0000` → `79.9750`; hủy đơn hoàn lại `80.0000` |
| Voucher `CHAOBAN` | Đơn 50.000đ giảm 5.000đ, tổng còn 45.000đ |
| Chấm công Staff | Check-in/out thành công, trạng thái ca chuyển `Completed` |

Khởi chạy dự án:

```bash
cd BaiTapLon
dotnet ef database update
dotnet run
```

Địa chỉ mặc định: `http://localhost:5102`.
