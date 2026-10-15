# Ứng dụng web quản lý bệnh viện

Project ASP.NET Core MVC (.NET 8) phục vụ giao diện, tài khoản và nghiệp vụ bệnh viện. Điểm khởi động là `Program.cs`; dữ liệu được truy cập qua Entity Framework Core và SQL Server.

| Thư mục | Nội dung |
|---|---|
| `Controllers`, `Views` | Trang giới thiệu, bác sĩ, tin tức, tài khoản, đặt dịch vụ và hồ sơ bệnh nhân |
| `Areas/Admin` | Tài khoản, bác sĩ, nhân viên, ca khám, danh mục và điều phối |
| `Areas/Doctor` | Lịch làm việc, lịch khám được phân công, kê đơn, thông báo |
| `Areas/Diagnostic` | Tiếp nhận phiếu và trả kết quả xét nghiệm |
| `Areas/Staff` | Tiếp nhận và theo dõi lịch khám trong ngày |
| `Models` | Entity tài khoản, bác sĩ, ca làm việc, lịch hẹn, dịch vụ, phiếu và kết quả |
| `Models/ViewModels` | Dữ liệu nhập/xuất của màn hình và quy tắc validation |
| `Data` | DbContext, cấu hình quan hệ và khởi tạo dữ liệu mẫu |
| `Migrations` | Các phiên bản schema EF Core |
| `Services` | Đặt lịch, kiểm tra truy cập, PDF, email và thời gian nghiệp vụ |
| `wwwroot` | Tài nguyên công khai: CSS, JavaScript, thư viện và hình ảnh |
| `App_Data` | Font PDF, thư và tệp kết quả được lưu ngoài thư mục công khai |

Các vai trò `Admin`, `Doctor`, `Staff`, `Diagnostic`, `Patient` được quản lý bằng ASP.NET Core Identity. Chức năng ghi dữ liệu kiểm tra quyền, biểu mẫu và phiên bản phiếu tại máy chủ.

`Data/Legacy/snapshot.example.json` chứa danh mục mẫu để khởi tạo hệ thống. Bản snapshot riêng của từng máy không đưa vào Git. Tệp cấu hình kết nối mặc định là `appsettings.json`; cấu hình riêng và mật khẩu dịch vụ nên đặt bằng User Secrets hoặc biến môi trường.

Xem [thiết kế hệ thống](../docs/THIET_KE_HE_THONG.md) để theo dõi các quan hệ và quy tắc nghiệp vụ; [hướng dẫn sử dụng](../README.md) dành cho người dùng cuối.
