# Hướng Dẫn Setup Dự Án E-Commerce ASP.NET Core MVC

![ASP.NET Core](https://img.shields.io/badge/ASP.NET%20Core-512BD4?style=flat-square&logo=dotnet&logoColor=white&logoWidth=20)
![Entity Framework Core](https://img.shields.io/badge/EF%20Core-512BD4?style=flat-square&logo=entityframework&logoColor=white&logoWidth=20)
![SQL Server](https://img.shields.io/badge/SQL%20Server-CC2927?style=flat-square&logo=sql-server&logoColor=white&logoWidth=20)
![Bootstrap 5](https://img.shields.io/badge/Bootstrap-7952B3?style=flat-square&logo=bootstrap&logoColor=white&logoWidth=20)
![jQuery](https://img.shields.io/badge/jQuery-0769AD?style=flat-square&logo=jquery&logoColor=white&logoWidth=20)
![Bootstrap Icons](https://img.shields.io/badge/Bootstrap%20Icons-7952B3?style=flat-square&logo=bootstrap&logoColor=white&logoWidth=20)
## Yêu Cầu Hệ Thống

- .NET 6.0 SDK trở lên
- Visual Studio 2022 hoặc VS Code
- SQL Server (LocalDB đã có sẵn trong Visual Studio)

## Các Bước Cài Đặt

### 1. Tạo Dự Án Mới

```bash
dotnet new mvc -n ECommerceApp
cd ECommerceApp
```

### 2. Cài Đặt Các Package Cần Thiết

```bash
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package Microsoft.EntityFrameworkCore.Tools
dotnet add package Microsoft.EntityFrameworkCore.Design
```

### 3. Cấu Trúc Thư Mục

Tạo các thư mục sau trong dự án:

```
ECommerceApp/
├── Controllers/
├── Models/
│   └── ViewModels/
├── Views/
│   ├── Home/
│   ├── Products/
│   ├── Cart/
│   ├── Checkout/
│   └── Shared/
├── Data/
└── wwwroot/
```

### 4. Thêm Code

- Copy tất cả code từ các artifacts vào đúng thư mục
- Models → Models/
- Controllers → Controllers/
- Views → Views/
- DbContext → Data/

### 5. Cập Nhật appsettings.json

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=ECommerceDb;Trusted_Connection=True;MultipleActiveResultSets=true"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```

### 6. Tạo Migration và Database

```bash
# Tạo migration
dotnet ef migrations add InitialCreate

# Cập nhật database
dotnet ef database update
```

### 7. Chạy Ứng Dụng

```bash
dotnet run
```

Hoặc nhấn F5 trong Visual Studio.

Ứng dụng sẽ chạy tại: `https://localhost:5001` hoặc `http://localhost:5000`

## Các Chức Năng Chính

### 1. Trang Chủ (/)
- Hiển thị sản phẩm nổi bật
- Banner chào mừng
- Nút thêm vào giỏ hàng nhanh

### 2. Danh Sách Sản Phẩm (/Products)
- Hiển thị tất cả sản phẩm
- Lọc theo danh mục
- Thêm vào giỏ hàng

### 3. Chi Tiết Sản Phẩm (/Products/Details/{id})
- Xem thông tin chi tiết
- Chọn số lượng
- Thêm vào giỏ hàng

### 4. Giỏ Hàng (/Cart)
- Xem các sản phẩm đã thêm
- Cập nhật số lượng
- Xóa sản phẩm
- Tính tổng tiền

### 5. Thanh Toán (/Checkout)
- Nhập thông tin khách hàng
- Xác nhận đơn hàng
- Trang thành công sau khi đặt hàng

## Cấu Trúc Database

### Tables:
- **Categories**: Danh mục sản phẩm
- **Products**: Sản phẩm
- **CartItems**: Giỏ hàng (dùng Session)
- **Orders**: Đơn hàng
- **OrderItems**: Chi tiết đơn hàng

## Dữ Liệu Mẫu

Database đã được seed với:
- 3 danh mục: Điện thoại, Laptop, Phụ kiện
- 5 sản phẩm mẫu với giá và hình ảnh

## Các Lệnh EF Core Hữu Ích

```bash
# Tạo migration mới
dotnet ef migrations add TenMigration

# Cập nhật database
dotnet ef database update

# Xóa migration chưa apply
dotnet ef migrations remove

# Xóa database
dotnet ef database drop
```

## Tính Năng Bổ Sung Có Thể Phát Triển

1. **Xác thực người dùng**: Thêm ASP.NET Identity
2. **Tìm kiếm**: Thêm chức năng tìm kiếm sản phẩm
3. **Đánh giá**: Cho phép khách hàng đánh giá sản phẩm
4. **Quản trị**: Trang admin quản lý sản phẩm, đơn hàng
5. **Thanh toán online**: Tích hợp cổng thanh toán
6. **Email**: Gửi email xác nhận đơn hàng
7. **Upload ảnh**: Cho phép upload ảnh sản phẩm thật
8. **Phân trang**: Thêm phân trang cho danh sách sản phẩm
9. **Lọc nâng cao**: Lọc theo giá, sắp xếp

## Xử Lý Lỗi Thường Gặp

### Lỗi Migration
```bash
# Xóa thư mục Migrations và tạo lại
dotnet ef database drop -f
dotnet ef migrations add InitialCreate
dotnet ef database update
```

### Lỗi Connection String
- Kiểm tra SQL Server đã chạy chưa
- Thay đổi connection string phù hợp với môi trường

### Lỗi Port đã được sử dụng
```bash
# Thay đổi port trong Properties/launchSettings.json
```

## Công Nghệ Sử Dụng

- **ASP.NET Core 8.0+**: Framework web
- **Entity Framework Core**: ORM
- **SQL Server**: Database
- **Bootstrap 5**: UI Framework
- **jQuery**: JavaScript library
- **Bootstrap Icons**: Icons

## License

Dự án demo cho mục đích học tập.
