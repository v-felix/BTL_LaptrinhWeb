using HospitalManagement.Web.Data;
using HospitalManagement.Web.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HospitalManagement.Tests;

public class VietnameseCatalogMigrationTests
{
    [Fact]
    public async Task Upgrade_localizes_existing_records_once_and_enforces_vnd()
    {
        var database = "HospitalManagementTests_Vietnamese_" + Guid.NewGuid().ToString("N");
        var connection = new SqlConnectionStringBuilder
        {
            DataSource = Environment.GetEnvironmentVariable("HOSPITAL_TEST_SQLSERVER") ?? @".\SQLEXPRESS",
            InitialCatalog = database, IntegratedSecurity = true, TrustServerCertificate = true
        };
        await using var db = new ApplicationDbContext(new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlServer(connection.ConnectionString).Options);
        try
        {
            var migrator = db.GetService<IMigrator>();
            await migrator.MigrateAsync("20261015145152_IdentityCleanupAndCurrency");

            var doctor = new Doctor
            {
                User = new ApplicationUser { Id = "legacy-2", FullName = "Dr. Xihad Ansari", UserName = "upgrade@example.com", Email = "upgrade@example.com" },
                Speciality = "General Physician", Designation = "MBBS, FCPS (Medicine)", About = "Mental health & counseling."
            };
            var service = new DiagnosticService { Name = "Blood Test", Category = "Laboratory", SampleType = "Blood", Price = 50, CurrencyCode = "BDT" };
            var package = new HealthCheck { Name = "Basic Health Check", Features = "Blood Tests", Price = 2500, CurrencyCode = "BDT" };
            db.AddRange(doctor, service, package);
            await db.SaveChangesAsync();
            db.DiagnosticBookings.Add(new BookingDiagnostic
            {
                PatientId = doctor.UserId, DiagnosticServiceId = service.Id, PatientName = "Người bệnh mẫu",
                BookingDate = new DateOnly(2027, 1, 1), Price = 75, CurrencyCode = "BDT"
            });
            db.PackageBookings.Add(new PackageBooking
            {
                PatientId = doctor.UserId, HealthCheckId = package.Id, BookingDate = new DateOnly(2027, 1, 1),
                TotalAmount = 2500, AmountPaid = 500, CurrencyCode = "BDT"
            });
            db.Schedules.Add(new Schedule { DoctorId = doctor.Id, DayOfWeek = 1, Fee = 250000 });
            await db.SaveChangesAsync();

            await migrator.MigrateAsync();
            db.ChangeTracker.Clear();
            var updatedDoctor = await db.Doctors.Include(x => x.User).SingleAsync();
            Assert.Equal("Nguyễn Minh Đức", updatedDoctor.User.FullName);
            Assert.Equal("Nội tổng quát", updatedDoctor.Speciality);
            Assert.Equal("Bác sĩ chuyên khoa II - Nội tổng quát", updatedDoctor.Designation);
            Assert.Equal("Xét nghiệm máu tổng quát", (await db.DiagnosticServices.SingleAsync()).Name);
            Assert.Equal("Gói khám sức khỏe cơ bản", (await db.HealthChecks.SingleAsync()).Name);
            Assert.Equal(50000, (await db.DiagnosticServices.SingleAsync()).Price);
            // The booked price is a snapshot and may differ from the current catalog.
            Assert.Equal(75000, (await db.DiagnosticBookings.SingleAsync()).Price);
            var paid = await db.PackageBookings.SingleAsync();
            Assert.Equal(2500000, paid.TotalAmount);
            Assert.Equal(500000, paid.AmountPaid);
            Assert.Equal("VND", paid.CurrencyCode);
            Assert.Equal(250000, (await db.Schedules.SingleAsync()).Fee);

            await migrator.MigrateAsync();
            db.ChangeTracker.Clear();
            Assert.Equal(75000, (await db.DiagnosticBookings.SingleAsync()).Price);
            Assert.Equal(500000, (await db.PackageBookings.SingleAsync()).AmountPaid);
            foreach (var command in new[] { "UPDATE [DiagnosticServices] SET [CurrencyCode] = N'USD'", "UPDATE [HealthChecks] SET [CurrencyCode] = N'USD'", "UPDATE [DiagnosticBookings] SET [CurrencyCode] = N'USD'", "UPDATE [PackageBookings] SET [CurrencyCode] = N'USD'" })
                await Assert.ThrowsAsync<SqlException>(async () =>
                {
                    await db.Database.ExecuteSqlRawAsync(command);
                });
        }
        finally
        {
            if (db.Database.GetDbConnection().Database != database || !database.StartsWith("HospitalManagementTests_Vietnamese_"))
                throw new InvalidOperationException("Refusing to remove a database outside this test run.");
            await db.Database.EnsureDeletedAsync();
        }
    }
}
