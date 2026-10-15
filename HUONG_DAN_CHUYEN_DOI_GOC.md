# 🏥 HƯỚNG DẪN CHI TIẾT CHUYỂN ĐỔI DỰ ÁN QUẢN LÝ BỆNH VIỆN SANG ASP.NET CORE MVC

> **Dự án gốc:** Xet Specialized Hospital (Laravel 12 + Vue 3 + Tailwind CSS)  
> **Mục tiêu:** Chuyển đổi hoàn chỉnh sang **ASP.NET Core MVC (.NET) + Entity Framework Core + Microsoft SQL Server**, cấu trúc chuẩn đồ án BTL môn Lập trình Web tại trường UTC.

---

## 📑 MỤC LỤC
1. [Tổng quan hệ thống & Kiến trúc mục tiêu](#1-tổng-quan-hệ-thống--kiến-trúc-mục-tiêu)
2. [Yêu cầu môi trường & Công cụ](#2-yêu-cầu-môi-trường--công-cụ)
3. [Cấu trúc thư mục dự án đề xuất](#3-cấu-trúc-thư-mục-dự-án-đề-xuất)
4. [Bước 1: Khởi tạo Project & Cài NuGet Packages](#bước-1-khởi-tạo-project--cài-nuget-packages)
5. [Bước 2: Xây dựng các Model (Entities)](#bước-2-xây-dựng-các-model-entities)
6. [Bước 3: Cấu hình ApplicationDbContext & SQL Server](#bước-3-cấu-hình-applicationdbcontext--sql-server)
7. [Bước 4: Cấu hình Program.cs & Phân quyền Identity](#bước-4-cấu-hình-programcs--phân-quyền-identity)
8. [Bước 5: Tạo Seed Data mẫu (DbInitializer)](#bước-5-tạo-seed-data-mẫu-dbinitializer)
9. [Bước 6: Thực thi Migrations tạo Database](#bước-6-thực-thi-migrations-tạo-database)
10. [Bước 7: Thiết kế Giao diện Layout & Trang chủ](#bước-7-thiết-kế-giao-diện-layout--trang-chủ)
11. [Bước 8: Xây dựng Chức năng Đặt lịch khám bệnh](#bước-8-xây-dựng-chức-năng-đặt-lịch-khám-bệnh)
12. [Bước 9: Xây dựng Phân hệ Bác sĩ (Doctor Area) & Quản trị (Admin Area)](#bước-9-xây-dựng-phân-hệ-bác-sĩ-doctor-area--quản-trị-admin-area)
13. [Checklist kiểm tra trước khi báo cáo BTL](#10-checklist-kiểm-tra-trước-khi-báo-cáo-btl)

---

## 1. TỔNG QUAN HỆ THỐNG & KIẾN TRÚC MỤC TIÊU

### 1.1. Các phân hệ người dùng (Roles)
- **Bệnh nhân (Patient / Khách):** Xem thông tin viện, tra cứu chuyên khoa, tìm bác sĩ, đặt lịch hẹn khám theo ca/giờ, đặt gói khám tổng quát, đặt dịch vụ xét nghiệm, tra cứu kết quả.
- **Bác sĩ (Doctor):** Đăng ký/sửa lịch làm việc trong tuần, xem danh sách bệnh nhân hẹn khám theo ngày, cập nhật trạng thái khám, kê đơn thuốc điện tử.
- **Quản trị viên (Admin):** Quản lý tài khoản, danh mục bác sĩ, điều phối ca trực, quản lý danh mục gói khám/dịch vụ, xem thống kê dashboard.
- **Kỹ thuật viên xét nghiệm (Diagnostic/Staff):** Tiếp nhận ca xét nghiệm, nhập và trả kết quả xét nghiệm cho bệnh nhân.

### 1.2. Công nghệ áp dụng
- **Nền tảng:** ASP.NET Core MVC (.NET 8 / .NET 9 / .NET 10)
- **Cơ sở dữ liệu:** Microsoft SQL Server (SQLEXPRESS)
- **ORM:** Entity Framework Core (Code-First)
- **Xác thực & Phân quyền:** ASP.NET Core Identity (Role-based Authorization)
- **Giao diện:** Razor Views (`.cshtml`), Bootstrap 5 / Tailwind CSS, jQuery / Vanilla JS, Toastr / SweetAlert2

---

## 2. YÊU CẦU MÔI TRƯỜNG & CÔNG CỤ
- **.NET SDK:** .NET 8.0 trở lên (máy bạn đã có sẵn .NET SDK)
- **Database:** Microsoft SQL Server (dịch vụ `MSSQL$SQLEXPRESS` đang chạy sẵn)
- **IDE:** Visual Studio 2022/2026 hoặc Visual Studio Code (kèm C# Dev Kit extension)

---

## 3. CẤU TRÚC THƯ MỤC DỰ ÁN ĐỀ XUẤT

```
HospitalManagement.sln
└── HospitalManagement.Web/
    ├── Areas/
    │   ├── Admin/
    │   │   ├── Controllers/
    │   │   │   ├── DashboardController.cs
    │   │   │   ├── DoctorController.cs
    │   │   │   ├── ServiceController.cs
    │   │   │   └── ScheduleController.cs
    │   │   └── Views/
    │   │       ├── Dashboard/Index.cshtml
    │   │       ├── Doctor/Index.cshtml, Create.cshtml, Edit.cshtml
    │   │       └── Shared/_AdminLayout.cshtml
    │   │
    │   └── Doctor/
    │       ├── Controllers/
    │       │   ├── ScheduleController.cs
    │       │   └── AppointmentController.cs
    │       └── Views/
    │           ├── Appointment/Index.cshtml, Prescribe.cshtml
    │           └── Schedule/Index.cshtml
    │
    ├── Controllers/
    │   ├── HomeController.cs
    │   ├── AppointmentController.cs
    │   ├── DiagnosticController.cs
    │   ├── HealthPackageController.cs
    │   └── AccountController.cs
    │
    ├── Data/
    │   ├── ApplicationDbContext.cs
    │   └── DbInitializer.cs
    │
    ├── Models/
    │   ├── ApplicationUser.cs
    │   ├── Doctor.cs
    │   ├── Schedule.cs
    │   ├── Appointment.cs
    │   ├── Prescription.cs
    │   ├── DiagnosticService.cs
    │   ├── BookingDiagnostic.cs
    │   ├── HealthCheck.cs
    │   ├── News.cs
    │   └── ViewModels/
    │       ├── LoginVM.cs
    │       ├── RegisterVM.cs
    │       ├── BookAppointmentVM.cs
    │       └── DashboardVM.cs
    │
    ├── Views/
    │   ├── Home/
    │   │   ├── Index.cshtml
    │   │   └── About.cshtml
    │   ├── Appointment/
    │   │   ├── Book.cshtml
    │   │   └── Confirmation.cshtml
    │   ├── Account/
    │   │   ├── Login.cshtml
    │   │   └── Register.cshtml
    │   └── Shared/
    │       ├── _Layout.cshtml
    │       └── _LoginPartial.cshtml
    │
    ├── wwwroot/
    │   ├── css/
    │   ├── js/
    │   ├── images/
    │   └── uploads/
    ├── appsettings.json
    └── Program.cs
```

---

## BƯỚC 1: KHỞI TẠO PROJECT & CÀI NUGET PACKAGES

Mở PowerShell tại thư mục bạn muốn lưu dự án:

```powershell
# 1. Tạo solution và project ASP.NET Core MVC
dotnet new sln -n HospitalManagement
dotnet new mvc -n HospitalManagement.Web
dotnet sln HospitalManagement.sln add HospitalManagement.Web/HospitalManagement.Web.csproj

cd HospitalManagement.Web

# 2. Cài đặt các thư viện NuGet cần thiết
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package Microsoft.EntityFrameworkCore.Tools
dotnet add package Microsoft.AspNetCore.Identity.EntityFrameworkCore
dotnet add package Microsoft.AspNetCore.Identity.UI
dotnet add package Microsoft.VisualStudio.Web.CodeGeneration.Design
```

---

## BƯỚC 2: XÂY DỰNG CÁC MODEL (ENTITIES)

Tạo các file sau trong thư mục `Models/`:

### 1. `Models/ApplicationUser.cs`
```csharp
using Microsoft.AspNetCore.Identity;

namespace HospitalManagement.Web.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; } = string.Empty;
        public string? Photo { get; set; }
        public string Role { get; set; } = "Patient"; // Admin, Doctor, Staff, Diagnostic, Patient
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public virtual Doctor? DoctorProfile { get; set; }
    }
}
```

### 2. `Models/Doctor.cs` & `Models/Schedule.cs`
```csharp
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalManagement.Web.Models
{
    public class Doctor
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;
        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; } = null!;

        public string? Designation { get; set; } // Học hàm / Chức danh (MBBS, MD, ThS.BS...)
        public string? Speciality { get; set; }  // Chuyên khoa (Cardiology, Neurology...)
        public string? Phone { get; set; }
        public string? About { get; set; }

        public virtual ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();
        public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
    }

    public class Schedule
    {
        [Key]
        public int Id { get; set; }
        public int DoctorId { get; set; }
        [ForeignKey("DoctorId")]
        public virtual Doctor Doctor { get; set; } = null!;

        public string DayOfWeek { get; set; } = "mon"; // mon, tue, wed, thu, fri, sat, sun
        public string StartTime { get; set; } = "08:00";
        public string EndTime { get; set; } = "17:00";
        public int SlotMinutes { get; set; } = 30; // Số phút 1 ca
        public int MaxPatientsPerDay { get; set; } = 20;
        public decimal Fee { get; set; } = 200000; // Giá khám (VND)
    }
}
```

### 3. `Models/Appointment.cs` & `Models/Prescription.cs`
```csharp
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalManagement.Web.Models
{
    public class Appointment
    {
        [Key]
        public int Id { get; set; }

        public string? PatientId { get; set; }
        [ForeignKey("PatientId")]
        public virtual ApplicationUser? Patient { get; set; }

        public int? DoctorId { get; set; }
        [ForeignKey("DoctorId")]
        public virtual Doctor? Doctor { get; set; }

        [Required]
        public DateTime AppointmentDate { get; set; }
        [Required]
        public string AppointmentTime { get; set; } = string.Empty;
        public int SerialNo { get; set; } // STT khám trong ngày

        [Required(ErrorMessage = "Vui lòng nhập họ tên bệnh nhân")]
        public string PatientName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng nhập số điện thoại")]
        public string PatientPhone { get; set; } = string.Empty;

        public int PatientAge { get; set; }
        public string PatientGender { get; set; } = "Male"; // Male, Female, Other
        public string? Problem { get; set; } // Triệu chứng bệnh

        public string Status { get; set; } = "pending"; // pending, confirmed, completed, cancelled
        public decimal Fee { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public virtual Prescription? Prescription { get; set; }
    }

    public class Prescription
    {
        [Key]
        public int Id { get; set; }
        public int AppointmentId { get; set; }
        [ForeignKey("AppointmentId")]
        public virtual Appointment Appointment { get; set; } = null!;

        public string Diagnosis { get; set; } = string.Empty; // Chẩn đoán
        public string MedicineDetails { get; set; } = string.Empty; // Tên thuốc & cách dùng
        public string? Advice { get; set; } // Dặn dò
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
```

### 4. `Models/DiagnosticService.cs` & `Models/HealthCheck.cs`
```csharp
using System.ComponentModel.DataAnnotations;

namespace HospitalManagement.Web.Models
{
    // Dịch vụ xét nghiệm / Chẩn đoán hình ảnh (X-Quang, MRI, Xét nghiệm máu...)
    public class DiagnosticService
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        public string? Category { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
    }

    // Gói khám sức khỏe tổng quát
    public class HealthCheck
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; } = string.Empty;
        public string? Subtitle { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public string? Features { get; set; } // Danh sách quyền lợi dạng text hoặc JSON
    }
}
```

---

## BƯỚC 3: CẤU HÌNH APPLICATIONDBCONTEXT & SQL SERVER

### 1. Tạo file `Data/ApplicationDbContext.cs`
```csharp
using HospitalManagement.Web.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagement.Web.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Doctor> Doctors { get; set; } = null!;
        public DbSet<Schedule> Schedules { get; set; } = null!;
        public DbSet<Appointment> Appointments { get; set; } = null!;
        public DbSet<Prescription> Prescriptions { get; set; } = null!;
        public DbSet<DiagnosticService> DiagnosticServices { get; set; } = null!;
        public DbSet<HealthCheck> HealthChecks { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Cấu hình định dạng tiền tệ tránh cảnh báo EF Core
            builder.Entity<Schedule>().Property(s => s.Fee).HasColumnType("decimal(18,2)");
            builder.Entity<Appointment>().Property(a => a.Fee).HasColumnType("decimal(18,2)");
            builder.Entity<DiagnosticService>().Property(d => d.Price).HasColumnType("decimal(18,2)");
            builder.Entity<HealthCheck>().Property(h => h.Price).HasColumnType("decimal(18,2)");
        }
    }
}
```

### 2. Cấu hình Connection String trong `appsettings.json`
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=HospitalDb;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
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

---

## BƯỚC 4: CẤU HÌNH PROGRAM.CS & PHÂN QUYỀN IDENTITY

Mở file `Program.cs` và cập nhật toàn bộ:

```csharp
using HospitalManagement.Web.Data;
using HospitalManagement.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 1. Kết nối Database SQL Server
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2. Cấu hình ASP.NET Core Identity & Roles
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 6;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// 3. Đường dẫn Cookie xác thực
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.LogoutPath = "/Account/Logout";
    options.AccessDeniedPath = "/Account/AccessDenied";
});

builder.Services.AddControllersWithViews();

var app = builder.Build();

// 4. Tự động áp dụng Migration & Seed dữ liệu mẫu khi ứng dụng khởi động
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await DbInitializer.SeedAsync(services);
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// 5. Cấu hình định tuyến (Routing)
// Route cho Areas (Admin / Doctor)
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");

// Default Route
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
```

---

## BƯỚC 5: TẠO SEED DATA MẪU (DBINITIALIZER)

Tạo file `Data/DbInitializer.cs`:

```csharp
using HospitalManagement.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagement.Web.Data
{
    public static class DbInitializer
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<ApplicationDbContext>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            await context.Database.MigrateAsync();

            // 1. Tạo các Roles
            string[] roles = { "Admin", "Doctor", "Staff", "Diagnostic", "Patient" };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole(role));
            }

            // 2. Tạo tài khoản Admin mặc định
            if (await userManager.FindByEmailAsync("admin@example.com") == null)
            {
                var admin = new ApplicationUser
                {
                    UserName = "admin@example.com",
                    Email = "admin@example.com",
                    FullName = "Quản Trị Viên Hệ Thống",
                    Role = "Admin",
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(admin, "admin123");
                await userManager.AddToRoleAsync(admin, "Admin");
            }

            // 3. Tạo tài khoản Bác sĩ mẫu & Lịch làm việc
            if (await userManager.FindByEmailAsync("doctor@example.com") == null)
            {
                var docUser = new ApplicationUser
                {
                    UserName = "doctor@example.com",
                    Email = "doctor@example.com",
                    FullName = "BS. CKII. Nguyễn Văn A",
                    Role = "Doctor",
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(docUser, "doctor123");
                await userManager.AddToRoleAsync(docUser, "Doctor");

                var doctor = new Doctor
                {
                    UserId = docUser.Id,
                    Designation = "Bác sĩ Chuyên khoa II (Nội Tim Mạch)",
                    Speciality = "Tim Mạch",
                    Phone = "0912345678",
                    About = "Hơn 15 năm kinh nghiệm điều trị bệnh lý tim mạch, cao huyết áp và can thiệp mạch vành."
                };
                context.Doctors.Add(doctor);
                await context.SaveChangesAsync();

                // Lịch làm việc trong tuần của bác sĩ
                context.Schedules.AddRange(
                    new Schedule { DoctorId = doctor.Id, DayOfWeek = "mon", StartTime = "08:00", EndTime = "12:00", SlotMinutes = 30, MaxPatientsPerDay = 15, Fee = 250000 },
                    new Schedule { DoctorId = doctor.Id, DayOfWeek = "wed", StartTime = "08:00", EndTime = "12:00", SlotMinutes = 30, MaxPatientsPerDay = 15, Fee = 250000 },
                    new Schedule { DoctorId = doctor.Id, DayOfWeek = "fri", StartTime = "13:30", EndTime = "17:00", SlotMinutes = 30, MaxPatientsPerDay = 15, Fee = 250000 }
                );
                await context.SaveChangesAsync();
            }

            // 4. Tạo các Dịch vụ xét nghiệm mẫu
            if (!context.DiagnosticServices.Any())
            {
                context.DiagnosticServices.AddRange(
                    new DiagnosticService { Name = "Xét nghiệm Công thức máu toàn phần", Category = "Huyết học", Price = 120000, Description = "Đánh giá tình trạng nhiễm trùng, thiếu máu" },
                    new DiagnosticService { Name = "Chụp X-Quang Tim Phổi thẳng", Category = "Chẩn đoán hình ảnh", Price = 150000, Description = "Kiểm tra tổn thương phổi và bóng tim" },
                    new DiagnosticService { Name = "Siêu âm Tim Doppler màu", Category = "Thăm dò chức năng", Price = 350000, Description = "Đánh giá hình thái và chức năng co bóp cơ tim" }
                );
                await context.SaveChangesAsync();
            }
        }
    }
}
```

---

## BƯỚC 6: THỰC THI MIGRATIONS TẠO DATABASE

Chạy lệnh trong terminal:

```powershell
dotnet ef migrations add InitialHospitalDb
dotnet ef database update
```
*Kết quả:* Database `HospitalDb` sẽ tự động được tạo trên SQL Server `localhost\SQLEXPRESS` với đầy đủ các bảng và dữ liệu mẫu đã seed.

---

## BƯỚC 7: THIẾT KẾ GIAO DIỆN LAYOUT & TRANG CHỦ

### 1. `Views/Shared/_Layout.cshtml`
Sử dụng Bootstrap 5 kèm FontAwesome và tone màu xanh y tế thanh lịch:

```html
<!DOCTYPE html>
<html lang="vi">
<head>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>@ViewData["Title"] - Bệnh Viện Đa Khoa Quốc Tế</title>
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.5.1/css/all.min.css" />
    <style>
        :root { --primary-color: #0284c7; --primary-hover: #0369a1; }
        .bg-medical { background: linear-gradient(135deg, #0284c7 0%, #0369a1 100%); color: white; }
        .btn-medical { background-color: var(--primary-color); color: white; font-weight: 500; }
        .btn-medical:hover { background-color: var(--primary-hover); color: white; }
        .nav-link { font-weight: 500; }
    </style>
</head>
<body>
    <header>
        <nav class="navbar navbar-expand-lg navbar-light bg-white border-bottom shadow-sm py-3">
            <div class="container">
                <a class="navbar-brand d-flex align-items-center gap-2 fw-bold text-primary fs-4" asp-controller="Home" asp-action="Index">
                    <i class="fa-solid fa-hospital-user text-danger"></i>
                    <span>XET HOSPITAL</span>
                </a>
                <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#mainNav">
                    <span class="navbar-toggler-icon"></span>
                </button>
                <div class="collapse navbar-collapse" id="mainNav">
                    <ul class="navbar-nav me-auto mb-2 mb-lg-0 ms-lg-4">
                        <li class="nav-item"><a class="nav-link text-dark" asp-controller="Home" asp-action="Index">Trang chủ</a></li>
                        <li class="nav-item"><a class="nav-link text-dark" asp-controller="Appointment" asp-action="Book">Đặt lịch khám</a></li>
                        <li class="nav-item"><a class="nav-link text-dark" asp-controller="Home" asp-action="Doctors">Đội ngũ bác sĩ</a></li>
                        <li class="nav-item"><a class="nav-link text-dark" asp-controller="Home" asp-action="Services">Dịch vụ xét nghiệm</a></li>
                    </ul>
                    <div class="d-flex align-items-center gap-2">
                        @if (User.Identity?.IsAuthenticated == true)
                        {
                            <span class="text-secondary small">Xin chào, <strong>@User.Identity.Name</strong></span>
                            @if (User.IsInRole("Admin"))
                            {
                                <a class="btn btn-outline-danger btn-sm" asp-area="Admin" asp-controller="Dashboard" asp-action="Index">Vào Quản Trị</a>
                            }
                            @if (User.IsInRole("Doctor"))
                            {
                                <a class="btn btn-outline-success btn-sm" asp-area="Doctor" asp-controller="Appointment" asp-action="Index">Lịch Khám Của Tôi</a>
                            }
                            <form asp-controller="Account" asp-action="Logout" method="post" class="d-inline">
                                <button type="submit" class="btn btn-sm btn-light border">Đăng xuất</button>
                            </form>
                        }
                        else
                        {
                            <a class="btn btn-outline-primary btn-sm px-3" asp-controller="Account" asp-action="Login">Đăng nhập</a>
                            <a class="btn btn-medical btn-sm px-3" asp-controller="Appointment" asp-action="Book">Đặt lịch ngay</a>
                        }
                    </div>
                </div>
            </div>
        </nav>
    </header>

    <main role="main" class="pb-5">
        @RenderBody()
    </main>

    <footer class="bg-dark text-white pt-5 pb-3">
        <div class="container text-center text-md-start">
            <div class="row">
                <div class="col-md-4 mb-4">
                    <h5 class="fw-bold text-info"><i class="fa-solid fa-hospital-user"></i> Xet Specialized Hospital</h5>
                    <p class="text-secondary small">Hệ thống chăm sóc sức khỏe toàn diện chuẩn quốc tế, trang thiết bị hiện đại cùng đội ngũ chuyên gia đầu ngành.</p>
                </div>
                <div class="col-md-4 mb-4">
                    <h6 class="fw-bold">Liên kết nhanh</h6>
                    <ul class="list-unstyled small text-secondary">
                        <li><a href="#" class="text-secondary text-decoration-none">Đặt lịch trực tuyến</a></li>
                        <li><a href="#" class="text-secondary text-decoration-none">Tra cứu kết quả xét nghiệm</a></li>
                        <li><a href="#" class="text-secondary text-decoration-none">Bảng giá viện phí</a></li>
                    </ul>
                </div>
                <div class="col-md-4 mb-4">
                    <h6 class="fw-bold">Hotline & Cấp cứu 24/7</h6>
                    <p class="fs-5 text-warning fw-bold mb-1"><i class="fa-solid fa-phone"></i> 1900 1234</p>
                    <p class="text-secondary small">Địa chỉ: Số 3 Cầu Giấy, Láng Thượng, Đống Đa, Hà Nội</p>
                </div>
            </div>
            <hr class="border-secondary" />
            <div class="text-center text-secondary small">
                &copy; 2026 - BTL Lập trình Web UTC - Hospital Management System.
            </div>
        </div>
    </footer>

    <script src="https://code.jquery.com/jquery-3.7.1.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/js/bootstrap.bundle.min.js"></script>
    @await RenderSectionAsync("Scripts", required: false)
</body>
</html>
```

---

## BƯỚC 8: XÂY DỰNG CHỨC NĂNG ĐẶT LỊCH KHÁM BỆNH

### 1. `Controllers/AppointmentController.cs`
```csharp
using HospitalManagement.Web.Data;
using HospitalManagement.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagement.Web.Controllers
{
    public class AppointmentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AppointmentController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Appointment/Book
        public async Task<IActionResult> Book(int? doctorId)
        {
            ViewBag.Doctors = await _context.Doctors
                .Include(d => d.User)
                .ToListAsync();

            ViewBag.Specialities = await _context.Doctors
                .Where(d => !string.IsNullOrEmpty(d.Speciality))
                .Select(d => d.Speciality!)
                .Distinct()
                .ToListAsync();

            var model = new Appointment
            {
                DoctorId = doctorId,
                AppointmentDate = DateTime.Today.AddDays(1)
            };

            return View(model);
        }

        // AJAX lấy ca làm việc của bác sĩ
        [HttpGet]
        public async Task<IActionResult> GetDoctorSchedules(int doctorId)
        {
            var schedules = await _context.Schedules
                .Where(s => s.DoctorId == doctorId)
                .Select(s => new
                {
                    s.DayOfWeek,
                    s.StartTime,
                    s.EndTime,
                    s.SlotMinutes,
                    s.Fee
                })
                .ToListAsync();

            return Json(schedules);
        }

        // POST: /Appointment/Book
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Book(Appointment appointment)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra số thứ tự khám trong ngày của bác sĩ
                var countToday = await _context.Appointments
                    .Where(a => a.DoctorId == appointment.DoctorId && a.AppointmentDate.Date == appointment.AppointmentDate.Date)
                    .CountAsync();

                appointment.SerialNo = countToday + 1;
                appointment.Status = "pending";
                appointment.CreatedAt = DateTime.UtcNow;

                _context.Appointments.Add(appointment);
                await _context.SaveChangesAsync();

                return RedirectToAction("Confirmation", new { id = appointment.Id });
            }

            ViewBag.Doctors = await _context.Doctors.Include(d => d.User).ToListAsync();
            return View(appointment);
        }

        // GET: /Appointment/Confirmation
        public async Task<IActionResult> Confirmation(int id)
        {
            var app = await _context.Appointments
                .Include(a => a.Doctor)
                .ThenInclude(d => d!.User)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (app == null) return NotFound();
            return View(app);
        }
    }
}
```

### 2. `Views/Appointment/Book.cshtml`
Form đặt lịch khám trực quan, hỗ trợ chọn bác sĩ và tự động tính toán khung giờ:

```html
@model HospitalManagement.Web.Models.Appointment
@{
    ViewData["Title"] = "Đặt Lịch Khám Bệnh Trực Tuyến";
    var doctors = ViewBag.Doctors as List<HospitalManagement.Web.Models.Doctor>;
}

<div class="container py-5">
    <div class="row justify-content-center">
        <div class="col-lg-8">
            <div class="card shadow-sm border-0 rounded-4 overflow-hidden">
                <div class="bg-medical p-4 text-center">
                    <h3 class="fw-bold mb-1"><i class="fa-solid fa-calendar-check me-2"></i>ĐẶT LỊCH HẸN BÁC SĨ</h3>
                    <p class="small text-white-50 mb-0">Nhanh chóng, tiện lợi, không phải xếp hàng chờ đợi lấy số</p>
                </div>
                <div class="card-body p-4 p-md-5">
                    <form asp-action="Book" method="post">
                        <div asp-validation-summary="ModelOnly" class="text-danger mb-3"></div>

                        <h5 class="fw-bold text-primary mb-3"><i class="fa-solid fa-user-doctor me-2"></i>1. Chọn Bác Sĩ & Lịch Khám</h5>
                        <div class="row g-3 mb-4">
                            <div class="col-md-6">
                                <label class="form-label fw-semibold">Bác sĩ chuyên khoa</label>
                                <select asp-for="DoctorId" class="form-select form-select-lg" id="selectDoctor" required>
                                    <option value="">-- Chọn bác sĩ --</option>
                                    @if (doctors != null)
                                    {
                                        foreach (var doc in doctors)
                                        {
                                            <option value="@doc.Id">@doc.User?.FullName - @doc.Speciality</option>
                                        }
                                    }
                                </select>
                            </div>
                            <div class="col-md-6">
                                <label class="form-label fw-semibold">Ngày dự kiến khám</label>
                                <input asp-for="AppointmentDate" type="date" class="form-control form-control-lg" min="@DateTime.Today.AddDays(1).ToString("yyyy-MM-dd")" required />
                            </div>
                            <div class="col-12">
                                <label class="form-label fw-semibold">Khung giờ khám</label>
                                <select asp-for="AppointmentTime" class="form-select form-select-lg" required>
                                    <option value="08:00 - 08:30">08:00 - 08:30</option>
                                    <option value="08:30 - 09:00">08:30 - 09:00</option>
                                    <option value="09:00 - 09:30">09:00 - 09:30</option>
                                    <option value="09:30 - 10:00">09:30 - 10:00</option>
                                    <option value="14:00 - 14:30">14:00 - 14:30</option>
                                    <option value="14:30 - 15:00">14:30 - 15:00</option>
                                </select>
                            </div>
                        </div>

                        <h5 class="fw-bold text-primary mb-3"><i class="fa-solid fa-id-card me-2"></i>2. Thông Tin Bệnh Nhân</h5>
                        <div class="row g-3 mb-4">
                            <div class="col-md-6">
                                <label class="form-label fw-semibold">Họ và tên bệnh nhân</label>
                                <input asp-for="PatientName" class="form-control" placeholder="Vd: Nguyễn Văn Bình" required />
                                <span asp-validation-for="PatientName" class="text-danger small"></span>
                            </div>
                            <div class="col-md-6">
                                <label class="form-label fw-semibold">Số điện thoại liên hệ</label>
                                <input asp-for="PatientPhone" class="form-control" placeholder="09xxxxxxxx" required />
                                <span asp-validation-for="PatientPhone" class="text-danger small"></span>
                            </div>
                            <div class="col-md-6">
                                <label class="form-label fw-semibold">Tuổi</label>
                                <input asp-for="PatientAge" type="number" class="form-control" min="1" max="120" value="25" />
                            </div>
                            <div class="col-md-6">
                                <label class="form-label fw-semibold">Giới tính</label>
                                <select asp-for="PatientGender" class="form-select">
                                    <option value="Nam">Nam</option>
                                    <option value="Nữ">Nữ</option>
                                    <option value="Khác">Khác</option>
                                </select>
                            </div>
                            <div class="col-12">
                                <label class="form-label fw-semibold">Mô tả triệu chứng / Lý do khám</label>
                                <textarea asp-for="Problem" class="form-control" rows="3" placeholder="Mô tả cảm giác khó chịu, đau nhức hoặc tiền sử bệnh lý..."></textarea>
                            </div>
                        </div>

                        <div class="d-grid mt-4">
                            <button type="submit" class="btn btn-medical btn-lg py-3 rounded-3 shadow">
                                <i class="fa-solid fa-check-circle me-2"></i>XÁC NHẬN ĐẶT LỊCH KHÁM
                            </button>
                        </div>
                    </form>
                </div>
            </div>
        </div>
    </div>
</div>
```

---

## BƯỚC 9: XÂY DỰNG PHÂN HỆ BÁC SĨ & QUẢN TRỊ (AREAS)

Tạo thư mục `Areas/Doctor/Controllers/AppointmentController.cs`:

```csharp
using HospitalManagement.Web.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagement.Web.Areas.Doctor.Controllers
{
    [Area("Doctor")]
    [Authorize(Roles = "Doctor,Admin")]
    public class AppointmentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AppointmentController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Danh sách bệnh nhân hẹn khám
        public async Task<IActionResult> Index(DateTime? date)
        {
            var targetDate = date ?? DateTime.Today;
            var appointments = await _context.Appointments
                .Where(a => a.AppointmentDate.Date == targetDate.Date)
                .OrderBy(a => a.SerialNo)
                .ToListAsync();

            ViewBag.SelectedDate = targetDate;
            return View(appointments);
        }

        // Xác nhận đã khám & kê đơn thuốc
        [HttpPost]
        public async Task<IActionResult> Complete(int id, string diagnosis, string medicineDetails, string? advice)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment != null)
            {
                appointment.Status = "completed";
                var prescription = new Models.Prescription
                {
                    AppointmentId = appointment.Id,
                    Diagnosis = diagnosis,
                    MedicineDetails = medicineDetails,
                    Advice = advice
                };
                _context.Prescriptions.Add(prescription);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
```

---

## 10. CHECKLIST KIỂM TRA TRƯỚC KHI BÁO CÁO BTL

| STT | Hạng mục kiểm tra | Tiêu chuẩn đạt |
| :---: | :--- | :--- |
| 1 | **Database & Migration** | Đã tạo đầy đủ các bảng trên SQL Server, chạy `dotnet ef database update` không có lỗi. |
| 2 | **Phân quyền Identity** | Đăng nhập bằng `admin@example.com` vào được `/Admin/Dashboard`. Đăng nhập bằng `doctor@example.com` vào được `/Doctor/Appointment`. |
| 3 | **Trang chủ & Giới thiệu** | Giao diện chuẩn y tế, hiển thị danh sách bác sĩ, tin tức, số điện thoại khẩn cấp. |
| 4 | **Luồng Đặt lịch khám** | Bệnh nhân chọn Bác sĩ -> Chọn ngày -> Chọn giờ -> Điền thông tin -> Lưu thành công vào bảng `Appointments`. |
| 5 | **Bác sĩ xem lịch & Kê đơn** | Bác sĩ thấy danh sách bệnh nhân theo ngày đã chọn, ấn hoàn tất và tạo đơn thuốc `Prescription`. |
| 6 | **Admin CRUD** | Quản trị viên thêm/sửa/xóa được danh mục bác sĩ và danh mục dịch vụ xét nghiệm. |

---

*File tài liệu này được tạo tự động để bạn lưu trữ, tham khảo và triển khai từng bước cho bài tập lớn!*
