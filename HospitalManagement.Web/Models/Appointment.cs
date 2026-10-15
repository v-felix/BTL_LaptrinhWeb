using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;

namespace HospitalManagement.Web.Models;

public enum BookingStatus { Pending, Confirmed, Completed, Cancelled }
public enum PaymentStatus { Unpaid, Paid, Refunded }

public class Appointment
{
    public int Id { get; set; }
    [MaxLength(30)] public string Reference { get; set; } = "KH-" + Guid.NewGuid().ToString("N")[..12].ToUpperInvariant();
    [MaxLength(64)] public string AccessToken { get; set; } = Convert.ToHexString(RandomNumberGenerator.GetBytes(32));
    public string? PatientId { get; set; }
    public ApplicationUser? Patient { get; set; }
    public int DoctorId { get; set; }
    public Doctor Doctor { get; set; } = null!;
    public DateOnly AppointmentDate { get; set; }
    public TimeOnly AppointmentTime { get; set; }
    public int SerialNo { get; set; }
    [Required, MaxLength(150)] public string PatientName { get; set; } = "";
    [Required, MaxLength(30)] public string PatientPhone { get; set; } = "";
    [Required, MaxLength(256)] public string PatientEmail { get; set; } = "";
    public int PatientAge { get; set; }
    [MaxLength(20)] public string PatientGender { get; set; } = "Other";
    [MaxLength(2000)] public string? Problem { get; set; }
    public BookingStatus Status { get; set; }
    [MaxLength(1000)] public string? CancellationReason { get; set; }
    public decimal Fee { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    [Timestamp] public byte[] RowVersion { get; set; } = [];
    public ICollection<Prescription> Prescriptions { get; set; } = new List<Prescription>();
}

public class Prescription
{
    public int Id { get; set; }
    public int AppointmentId { get; set; }
    public Appointment Appointment { get; set; } = null!;
    [Required, MaxLength(2000)] public string Diagnosis { get; set; } = "";
    [Required, MaxLength(6000)] public string MedicineDetails { get; set; } = "";
    [MaxLength(4000)] public string? Advice { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
