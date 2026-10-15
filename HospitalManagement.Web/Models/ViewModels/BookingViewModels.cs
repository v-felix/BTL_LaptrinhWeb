using System.ComponentModel.DataAnnotations;
using HospitalManagement.Web.Services;

namespace HospitalManagement.Web.Models.ViewModels;

public class BookAppointmentVM
{
    [Range(1, int.MaxValue), Display(Name = "Bác sĩ")] public int DoctorId { get; set; }
    [DataType(DataType.Date), Display(Name = "Ngày khám")] public DateOnly AppointmentDate { get; set; } = HospitalClock.Today.AddDays(1);
    [Required, Display(Name = "Giờ khám")] public string AppointmentTime { get; set; } = "";
    [Required, StringLength(150), Display(Name = "Họ tên bệnh nhân")] public string PatientName { get; set; } = "";
    [Required, Phone, StringLength(30), Display(Name = "Số điện thoại")] public string PatientPhone { get; set; } = "";
    [Required, EmailAddress, StringLength(256), Display(Name = "Email")] public string PatientEmail { get; set; } = "";
    [Range(0, 120), Display(Name = "Tuổi")] public int PatientAge { get; set; }
    [Required, RegularExpression("Male|Female|Other"), Display(Name = "Giới tính")] public string PatientGender { get; set; } = "Other";
    [StringLength(2000), Display(Name = "Triệu chứng / Lý do khám")] public string? Problem { get; set; }
}
public class BookDiagnosticVM
{
    public int ServiceId { get; set; }
    [DataType(DataType.Date), Display(Name = "Ngày xét nghiệm")] public DateOnly BookingDate { get; set; } = HospitalClock.Today.AddDays(1);
    [Required, Display(Name = "Giờ hẹn")] public string BookingTime { get; set; } = "08:00";
    [Required, StringLength(150), Display(Name = "Họ tên bệnh nhân")] public string PatientName { get; set; } = "";
    [Required, Phone, StringLength(30), Display(Name = "Số điện thoại")] public string PatientPhone { get; set; } = "";
    [StringLength(500), Display(Name = "Địa chỉ")] public string? Address { get; set; }
    [StringLength(2000), Display(Name = "Ghi chú")] public string? Notes { get; set; }
    [StringLength(30), Display(Name = "Mã lịch khám liên quan (nếu có)")] public string? AppointmentReference { get; set; }
}
public class BookPackageVM
{
    public int PackageId { get; set; }
    [DataType(DataType.Date), Display(Name = "Ngày khám")] public DateOnly BookingDate { get; set; } = HospitalClock.Today.AddDays(1);
    [AllowedValues(50, 100), Display(Name = "Mức đăng ký thanh toán")] public int DepositPercent { get; set; } = 50;
}
public class PrescriptionVM
{
    public int AppointmentId { get; set; }
    public string RowVersion { get; set; } = "";
    [Required, StringLength(2000), Display(Name = "Chẩn đoán")] public string Diagnosis { get; set; } = "";
    [Required, StringLength(6000), Display(Name = "Thuốc và cách dùng")] public string MedicineDetails { get; set; } = "";
    [StringLength(4000), Display(Name = "Lời dặn")] public string? Advice { get; set; }
}
public class ResultVM
{
    public int BookingId { get; set; }
    public string RowVersion { get; set; } = "";
    [Required, StringLength(6000), Display(Name = "Kết quả xét nghiệm")] public string Content { get; set; } = "";
    [StringLength(2000), Display(Name = "Kết luận")] public string? Conclusion { get; set; }
    [Display(Name = "Tệp PDF (tối đa 5 MB)")] public IFormFile? File { get; set; }
}
public class PatientDashboardVM
{
    public List<Appointment> Appointments { get; set; } = [];
    public List<BookingDiagnostic> Diagnostics { get; set; } = [];
    public List<PackageBooking> Packages { get; set; } = [];
}
public class DashboardVM
{
    public int Doctors { get; set; }
    public int Patients { get; set; }
    public int Pending { get; set; }
    public int Today { get; set; }
    public Dictionary<string, decimal> RecordedPackagePayments { get; set; } = [];
    public List<Appointment> Appointments { get; set; } = [];
}
public class HomeVM
{
    public List<Doctor> Doctors { get; set; } = [];
    public List<HealthCheck> Packages { get; set; } = [];
    public List<News> News { get; set; } = [];
    public int DoctorCount { get; set; }
    public int ServiceCount { get; set; }
}
