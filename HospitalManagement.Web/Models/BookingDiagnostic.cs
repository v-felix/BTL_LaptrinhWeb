using System.ComponentModel.DataAnnotations;

namespace HospitalManagement.Web.Models;

public class BookingDiagnostic
{
    public int Id { get; set; }
    [MaxLength(30)] public string Reference { get; set; } = "XN-" + Guid.NewGuid().ToString("N")[..12].ToUpperInvariant();
    public string PatientId { get; set; } = "";
    public ApplicationUser Patient { get; set; } = null!;
    public int DiagnosticServiceId { get; set; }
    public DiagnosticService DiagnosticService { get; set; } = null!;
    public int? AppointmentId { get; set; }
    public Appointment? Appointment { get; set; }
    public DateOnly BookingDate { get; set; }
    public TimeOnly BookingTime { get; set; }
    [MaxLength(150)] public string PatientName { get; set; } = "";
    [MaxLength(30)] public string PatientPhone { get; set; } = "";
    [MaxLength(500)] public string? Address { get; set; }
    [MaxLength(2000)] public string? Notes { get; set; }
    public decimal Price { get; set; }
    [MaxLength(3)] public string CurrencyCode { get; set; } = "VND";
    public BookingStatus Status { get; set; }
    public PaymentStatus PaymentStatus { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    [Timestamp] public byte[] RowVersion { get; set; } = [];
    public ICollection<TestResult> TestResults { get; set; } = new List<TestResult>();
}

public class TestResult
{
    public int Id { get; set; }
    public int BookingDiagnosticId { get; set; }
    public BookingDiagnostic BookingDiagnostic { get; set; } = null!;
    [Required, MaxLength(6000)] public string Content { get; set; } = "";
    [MaxLength(2000)] public string? Conclusion { get; set; }
    [MaxLength(100)] public string? FileName { get; set; }
    public string CreatedById { get; set; } = "";
    public ApplicationUser CreatedBy { get; set; } = null!;
    public DateTime DeliveredAt { get; set; } = DateTime.UtcNow;
}
