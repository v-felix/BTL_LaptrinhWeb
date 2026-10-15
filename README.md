# Hospital Management — ASP.NET Core MVC

Đã chuyển Laravel 12 + Vue 3 sang **ASP.NET Core MVC (.NET 8) + Entity Framework Core 8 + Microsoft SQL Server**. Giao diện Razor Views, Bootstrap 5 và JavaScript thuần; tài nguyên được lưu cục bộ. Chạy bằng Visual Studio, không cần PHP, Composer, Node.js, npm hay Vite.

## Chạy bằng Visual Studio

1. Mở **HospitalManagement.sln** bằng Visual Studio 2022 (17.8 trở lên) hoặc Visual Studio 2026, có workload **ASP.NET and web development** và .NET 8 SDK.
2. Nhấn chuột phải **HospitalManagement.Web → Set as Startup Project**.
3. Kiểm tra dịch vụ **SQL Server (SQLEXPRESS)** đang chạy. Connection string nằm trong `HospitalManagement.Web/appsettings.json`: server `.\SQLEXPRESS`, database **HospitalManagementMvc**, xác thực Windows. Nếu dùng instance khác, sửa `Server` cho phù hợp. Tài khoản Windows chạy Visual Studio cần quyền tạo/truy cập database.
4. Chờ NuGet restore, chọn profile **HospitalManagement.Web**, nhấn **F5** hoặc **Ctrl+F5**.
5. Trình duyệt mở **http://localhost:5080**. Development tự áp dụng migration và nhập dữ liệu mẫu ở lần đầu; những lần sau không nhập trùng.

Profile `https` chạy tại `https://localhost:7080`; có thể cần `dotnet dev-certs https --trust`. Profile HTTP mặc định không cần chứng chỉ. IIS Express cũng được cấu hình.

Trên máy bàn giao đã chuẩn bị thêm SDK .NET 8 ổn định tại `artifacts/dotnet` vì SDK hệ thống đang được bộ cài thay đổi. Nếu mở solution trực tiếp gặp lỗi SDK hoặc thiếu runtime, chạy `scripts/Open-VisualStudio.ps1` để mở IDE với SDK này rồi F5 như bình thường. Script chỉ đặt môi trường cho tiến trình IDE, không đổi cấu hình hệ thống. Thư mục SDK này không cần đưa vào mã nguồn khi chia sẻ; máy khác chỉ cần cài .NET 8 SDK chuẩn.

## Tài khoản thử

| Vai trò | Email | Mật khẩu |
|---|---|---|
| Quản trị | admin@example.com | admin123 |
| Bác sĩ | doctor@example.com | doctor123 |
| Kỹ thuật viên | diagnostic@example.com | diagnostic123 |
| Tiếp nhận | staff@example.com | staff123 |
| Bệnh nhân | patient@example.com | patient123 |

Tài khoản cũ giữ nguyên email và mật khẩu. Hash bcrypt Laravel được kiểm tra và tự nâng cấp sang Identity khi đăng nhập thành công. Tài khoản mẫu chỉ được bổ sung trong Development.

## Chức năng

- Khách: tìm bác sĩ/chuyên khoa, xem lịch làm việc, tin tức, gói khám/dịch vụ; đặt lịch khám và tải phiếu qua liên kết riêng.
- Bệnh nhân: đăng ký/đăng nhập, sửa thông tin, đổi/đặt lại mật khẩu; đặt khám, xét nghiệm, gói khám; hủy lịch phù hợp; xem đơn thuốc, kết quả và tải PDF.
- Bác sĩ: quản lý ca trong tuần, xem đúng bệnh nhân của mình, xác nhận/hủy lịch, khám/kê đơn và nhận thông báo điều phối.
- Quản trị: dashboard, CRUD tài khoản/bác sĩ/nhân viên, phân quyền, khóa đăng nhập, lịch làm việc, thông báo nội bộ, CRUD dịch vụ/gói khám/tin tức, điều phối lịch hẹn và phiếu gói khám.
- Kỹ thuật viên: danh mục xét nghiệm, tiếp nhận phiếu, ghi nhận thanh toán, nhập kết quả và đính kèm PDF.
- Nhân viên tiếp nhận: lịch khám hôm nay, xác nhận/hủy lịch, tiếp nhận gói khám và ghi nhận số tiền thực thu.

## Dữ liệu đã chuyển

Đã nhập từ SQLite nguồn: **47 tài khoản, 21 bác sĩ, 20 nhân viên, 43 dịch vụ, 3 gói khám, 7 bài tin**. Bổ sung 2 tài khoản mẫu và 126 ca khám mẫu (thứ 2–thứ 7, 08:00–12:00). Nguồn chưa có lịch làm việc, lịch hẹn, phiếu gói khám, xét nghiệm hoặc đơn thuốc. Banner có sẵn được tái sử dụng; nguồn không có thư mục upload ảnh bác sĩ.

**Tiền tệ:** danh mục nguồn dùng Bangladesh taka (`৳`), được giữ nguyên giá và ghi rõ **BDT**. Ca khám mẫu và danh mục mới mặc định **VND**. Quản trị có thể sửa giá cùng đơn vị tiền. Không tự quy đổi tỷ giá hoặc cộng tổng các đơn vị tiền khác nhau. Phiếu lưu giá và đơn vị tiền tại thời điểm đặt.

`HospitalManagement.Web/Data/Legacy/snapshot.json` chỉ phục vụ khởi tạo; mọi truy vấn/ghi khi chạy đều qua EF Core và SQL Server. Snapshot chứa hash mật khẩu cũ và nằm ngoài `wwwroot`. Session, cache và token reset cũ không chuyển sang hệ thống xác thực mới.

Bản trên GitHub không chứa `snapshot.json` riêng của máy nguồn. Khi clone mới, ứng dụng dùng `snapshot.example.json` đã bỏ toàn bộ hash mật khẩu và dùng địa chỉ email mẫu dưới `example.com`. Các tài khoản demo được tạo bằng Identity với mật khẩu trong bảng trên; bác sĩ/nhân viên/kỹ thuật viên mẫu khác dùng lần lượt `doctor123`/`staff123`/`diagnostic123`. Nếu có `snapshot.json` ở máy hiện tại, luồng nhập dữ liệu cũ vẫn được ưu tiên.

## Email, PDF và thanh toán

Development mặc định `Email:Mode=File`: thư được lưu thành `.eml` tại `HospitalManagement.Web/App_Data/MailOutbox`, **không gửi ra Internet**. Luồng quên mật khẩu cũng dùng kênh này; mở thư mới nhất để lấy liên kết thử.

Muốn gửi SMTP, dùng User Secrets hoặc biến môi trường `Email__Host`, `Email__Port`, `Email__User`, `Email__Password`; đặt `Email__Mode=Smtp` và chỉnh `Email__PublicBaseUrl`. Email gửi liên kết tới phiếu/PDF có kiểm tra quyền truy cập.

PDF dùng PDFsharp với font DejaVu nhúng, hỗ trợ tiếng Việt. PDF kết quả được upload lưu ngoài `wwwroot` tại `App_Data/Results` và tải qua controller có phân quyền. Font/giấy phép đi kèm dự án.

Thanh toán là **ghi nhận số tiền thực thu tại bệnh viện**. Đăng ký cọc 50%/100% không tự ghi nhận đã trả tiền. Chưa tích hợp ngân hàng hoặc SMS. Phiếu bị hủy sau khi thu tiền không tự được coi là đã hoàn tiền.

## Lệnh tùy chọn

Chạy ở thư mục chứa solution:

```powershell
dotnet restore
dotnet tool restore
dotnet run --project HospitalManagement.Web
```

Khởi tạo DB mà không mở web:

```powershell
.\scripts\Initialize-Database.ps1
```

Migration bằng CLI:

```powershell
dotnet ef database update --project HospitalManagement.Web
dotnet ef migrations add TenThayDoi --project HospitalManagement.Web
```

Trong Visual Studio Package Manager Console, chọn Default project `HospitalManagement.Web`, chạy `Update-Database`. `database-schema.sql` là script schema idempotent cho SSMS, chạy trên database rỗng đã tạo; script không nhập dữ liệu mẫu. Dữ liệu mẫu được nhập khi chạy Development.

## Kiểm thử

```powershell
dotnet test HospitalManagement.sln -c Release
```

13 test tích hợp với SQL Server thực: mật khẩu cũ, phân quyền, CRUD, tài khoản, ca khám, chống đặt trùng/công suất, chống giả mạo giá, cập nhật đồng thời, khám/kê đơn, trả kết quả, thu phí và PDF. Mỗi lần chạy tạo database riêng `HospitalManagementTests_<GUID>` rồi chỉ xóa database đó; không dùng database ứng dụng. Biến `HOSPITAL_TEST_SQLSERVER` cho phép chọn instance test khác.

Chi tiết kiến trúc và kết quả kiểm chứng: [docs/BAO_CAO_CHUYEN_DOI.md](docs/BAO_CAO_CHUYEN_DOI.md).
