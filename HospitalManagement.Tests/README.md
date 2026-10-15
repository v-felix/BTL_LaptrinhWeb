# Kiểm thử nghiệp vụ bệnh viện

Project kiểm thử tích hợp dùng xUnit, ASP.NET Core MVC TestServer và **SQL Server thực**. `HospitalFactory` khởi tạo ứng dụng trong môi trường kiểm thử; `HospitalWorkflowsTests` kiểm tra các luồng qua HTTP.

## Phạm vi

- Trang công khai và dữ liệu mẫu; đăng nhập theo năm vai trò.
- Đặt khám, quyền truy cập phiếu, giá lấy từ máy chủ và xuất PDF.
- Đặt cùng khung giờ đồng thời, giới hạn công suất, ngày hợp lệ và số thứ tự.
- Bác sĩ xác nhận, khám và kê đơn đúng bệnh nhân.
- Phân quyền hồ sơ, bảo vệ biểu mẫu và xung đột phiên bản phiếu.
- Đăng ký xét nghiệm, tiếp nhận, trả kết quả và tải PDF.
- Gói khám và ghi nhận số tiền thực thu.
- Quản lý danh mục, tài khoản, bác sĩ, ca khám và thông báo.
- Đăng ký, đổi thông tin, đổi mật khẩu, khóa phiên và bảo vệ hồ sơ có liên quan.

## Chạy kiểm thử

Tại thư mục chứa solution, với .NET 8 SDK và SQL Server đang chạy:

```powershell
dotnet test HospitalManagement.sln -c Release
```

Mặc định kết nối `.\SQLEXPRESS` bằng xác thực Windows. Để chọn SQL Server khác, đặt biến `HOSPITAL_TEST_SQLSERVER` trước khi chạy, ví dụ:

```powershell
$env:HOSPITAL_TEST_SQLSERVER = '.\SQLEXPRESS'
dotnet test HospitalManagement.sln -c Release
```

Mỗi lần khởi tạo dùng database riêng có tên `HospitalManagementTests_<GUID>`. Tài khoản chạy test cần quyền tạo và xóa database kiểm thử. Factory kiểm tra đúng tên database trước khi dọn dữ liệu, không sử dụng database ứng dụng.

Script [Verify.ps1](../scripts/Verify.ps1) chạy thêm build, kiểm thử với log TRX và kiểm tra model EF Core khớp migration. Kết quả TRX nằm trong `HospitalManagement.Tests/TestResults`.
