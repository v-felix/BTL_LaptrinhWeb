using HospitalManagement.Web.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagement.Web.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
{
    public DbSet<Doctor> Doctors => Set<Doctor>();
    public DbSet<Staff> Staff => Set<Staff>();
    public DbSet<Schedule> Schedules => Set<Schedule>();
    public DbSet<Appointment> Appointments => Set<Appointment>();
    public DbSet<Prescription> Prescriptions => Set<Prescription>();
    public DbSet<DiagnosticService> DiagnosticServices => Set<DiagnosticService>();
    public DbSet<BookingDiagnostic> DiagnosticBookings => Set<BookingDiagnostic>();
    public DbSet<TestResult> TestResults => Set<TestResult>();
    public DbSet<HealthCheck> HealthChecks => Set<HealthCheck>();
    public DbSet<PackageBooking> PackageBookings => Set<PackageBooking>();
    public DbSet<News> News => Set<News>();
    public DbSet<DoctorMessage> DoctorMessages => Set<DoctorMessage>();

    protected override void OnModelCreating(ModelBuilder b)
    {
        base.OnModelCreating(b);
        b.Entity<DiagnosticService>().ToTable(t => t.HasCheckConstraint("CK_DiagnosticServices_CurrencyVnd", "[CurrencyCode] = N'VND'"));
        b.Entity<HealthCheck>().ToTable(t => t.HasCheckConstraint("CK_HealthChecks_CurrencyVnd", "[CurrencyCode] = N'VND'"));
        b.Entity<BookingDiagnostic>().ToTable(t => t.HasCheckConstraint("CK_DiagnosticBookings_CurrencyVnd", "[CurrencyCode] = N'VND'"));
        b.Entity<PackageBooking>().ToTable(t => t.HasCheckConstraint("CK_PackageBookings_CurrencyVnd", "[CurrencyCode] = N'VND'"));
        b.Entity<Doctor>().HasOne(x => x.User).WithOne(x => x.DoctorProfile).HasForeignKey<Doctor>(x => x.UserId);
        b.Entity<Staff>().HasOne(x => x.User).WithOne(x => x.StaffProfile).HasForeignKey<Staff>(x => x.UserId);
        b.Entity<Schedule>().HasIndex(x => new { x.DoctorId, x.DayOfWeek }).IsUnique();
        b.Entity<Appointment>().HasIndex(x => x.Reference).IsUnique();
        // SQL Server enforces this even if two booking requests arrive together.
        b.Entity<Appointment>().HasIndex(x => new { x.DoctorId, x.AppointmentDate, x.AppointmentTime })
            .IsUnique().HasFilter("[Status] <> N'Cancelled'");
        b.Entity<Appointment>().HasIndex(x => new { x.DoctorId, x.AppointmentDate, x.SerialNo }).IsUnique();
        b.Entity<Appointment>().HasOne(x => x.Patient).WithMany().HasForeignKey(x => x.PatientId);
        b.Entity<Appointment>().HasOne(x => x.Doctor).WithMany().HasForeignKey(x => x.DoctorId);
        b.Entity<Prescription>().HasOne(x => x.Appointment).WithMany(x => x.Prescriptions).HasForeignKey(x => x.AppointmentId);
        b.Entity<BookingDiagnostic>().HasIndex(x => x.Reference).IsUnique();
        b.Entity<PackageBooking>().HasIndex(x => x.Reference).IsUnique();
        b.Entity<TestResult>().HasOne(x => x.BookingDiagnostic).WithMany(x => x.TestResults).HasForeignKey(x => x.BookingDiagnosticId);
        foreach (var entity in b.Model.GetEntityTypes())
        {
            // Avoid SQL Server's multiple cascading paths, and preserve medical history.
            if (entity.ClrType.Namespace == typeof(Appointment).Namespace)
                foreach (var foreignKey in entity.GetForeignKeys()) foreignKey.DeleteBehavior = DeleteBehavior.Restrict;
            foreach (var property in entity.GetProperties())
            {
                if (property.ClrType == typeof(decimal)) { property.SetPrecision(18); property.SetScale(2); }
                if (property.ClrType == typeof(BookingStatus)) b.Entity(entity.ClrType).Property(property.Name).HasConversion<string>().HasMaxLength(20);
                if (property.ClrType == typeof(PaymentStatus)) b.Entity(entity.ClrType).Property(property.Name).HasConversion<string>().HasMaxLength(20);
            }
        }
    }
}
