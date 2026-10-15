# Công cụ Entity Framework Core

`dotnet-tools.json` khai báo công cụ cục bộ `dotnet-ef` phiên bản **8.0.28**, thống nhất với phiên bản EF Core của ứng dụng. Khôi phục từ thư mục solution:

```powershell
dotnet tool restore
```

Các lệnh thường dùng:

```powershell
dotnet ef migrations list --project HospitalManagement.Web
dotnet ef database update --project HospitalManagement.Web
dotnet ef migrations has-pending-model-changes --project HospitalManagement.Web
```

Khi thay đổi model, tạo migration mới và kiểm tra nội dung trước khi cập nhật database:

```powershell
dotnet ef migrations add TenThayDoi --project HospitalManagement.Web
```

Xem thêm [các tiện ích dự án](../scripts/README.md).
