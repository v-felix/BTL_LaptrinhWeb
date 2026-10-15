# Tiện ích quản lý dự án

Chạy các script PowerShell từ thư mục chứa `HospitalManagement.sln`.

| Script | Chức năng |
|---|---|
| `Initialize-Database.ps1` | Khôi phục công cụ và package, chạy chế độ chỉ khởi tạo database rồi thoát |
| `Verify.ps1` | Build Release, chạy kiểm thử tích hợp và kiểm tra thay đổi model chưa có migration |
| `Open-VisualStudio.ps1` | Mở solution bằng Visual Studio; nhận tham số `-VisualStudioPath` khi cần chỉ rõ IDE |
| `Use-LocalSdk.ps1` | Chọn SDK tại `artifacts/dotnet` nếu có, chỉ áp dụng cho tiến trình hiện tại |

## Chuẩn bị và chạy

Máy cần .NET 8 SDK, SQL Server và Visual Studio có workload **ASP.NET and web development**. Mặc định ứng dụng kết nối `.\SQLEXPRESS`, database `HospitalManagementMvc`, xác thực Windows; kiểm tra `HospitalManagement.Web/appsettings.json` nếu sử dụng instance khác.

```powershell
.\scripts\Initialize-Database.ps1
.\scripts\Open-VisualStudio.ps1
```

Trong Visual Studio, chọn `HospitalManagement.Web` làm startup project và chạy profile cùng tên. Địa chỉ mặc định là `http://localhost:5080`. Cũng có thể mở trực tiếp `HospitalManagement.sln` bằng Visual Studio 2022 từ 17.8 trở lên hoặc Visual Studio 2026.

Để kiểm tra solution:

```powershell
.\scripts\Verify.ps1
```

Kiểm thử cần SQL Server và quyền tạo/xóa database riêng. Xem [hướng dẫn kiểm thử](../HospitalManagement.Tests/README.md).

## Migration và dữ liệu mẫu

Khôi phục công cụ bằng `dotnet tool restore`. Để cập nhật schema từ migration:

```powershell
dotnet ef database update --project HospitalManagement.Web
```

Trong Package Manager Console của Visual Studio, chọn default project `HospitalManagement.Web` rồi chạy `Update-Database`. `database-schema.sql` phục vụ tạo schema bằng SSMS trên database đã chọn; script này không nhập dữ liệu mẫu.

Ứng dụng dùng `Data/Legacy/snapshot.example.json` để tạo danh mục mẫu khi bật khởi tạo và seed dữ liệu. Nếu máy có snapshot riêng tại `snapshot.json`, initializer ưu tiên tệp đó. Snapshot riêng, thư xuất ra và tệp kết quả không đưa vào Git.

## Thông báo email

Mặc định khi chạy Development, email được lưu thành tệp `.eml` tại `HospitalManagement.Web/App_Data/MailOutbox`. Có thể mở thư để thử liên kết đặt lại mật khẩu. Muốn gửi SMTP, cấu hình `Email__Mode=Smtp`, `Email__Host`, `Email__Port`, `Email__User`, `Email__Password` và `Email__PublicBaseUrl` bằng User Secrets hoặc biến môi trường.
