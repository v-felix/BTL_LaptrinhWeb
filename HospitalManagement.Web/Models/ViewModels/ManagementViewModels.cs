using System.ComponentModel.DataAnnotations;

namespace HospitalManagement.Web.Models.ViewModels;

public class UserEditVM
{
    public string? Id { get; set; }
    [Required, StringLength(150), Display(Name = "Họ và tên")] public string FullName { get; set; } = "";
    [Required, EmailAddress, StringLength(256)] public string Email { get; set; } = "";
    [Phone, StringLength(30), Display(Name = "Điện thoại")] public string? Phone { get; set; }
    [Required, AllowedValues(Roles.Admin, Roles.Doctor, Roles.Staff, Roles.Diagnostic, Roles.Patient), Display(Name = "Vai trò")] public string Role { get; set; } = Roles.Patient;
    [StringLength(100, MinimumLength = 8), DataType(DataType.Password), Display(Name = "Mật khẩu (để trống nếu không đổi)")] public string? Password { get; set; }
    [Display(Name = "Cho phép đăng nhập")] public bool IsActive { get; set; } = true;
    [StringLength(200), Display(Name = "Chức danh")] public string? Designation { get; set; }
    [StringLength(150), Display(Name = "Chuyên khoa / Phòng ban")] public string? Department { get; set; }
    [StringLength(4000), Display(Name = "Giới thiệu")] public string? About { get; set; }
}
public class ScheduleVM : IValidatableObject
{
    public int Id { get; set; }
    [Range(1, int.MaxValue), Display(Name = "Bác sĩ")] public int DoctorId { get; set; }
    [Range(0, 6), Display(Name = "Thứ trong tuần")] public int DayOfWeek { get; set; } = 1;
    [DataType(DataType.Time), Display(Name = "Bắt đầu")] public TimeOnly StartTime { get; set; } = new(8, 0);
    [DataType(DataType.Time), Display(Name = "Kết thúc")] public TimeOnly EndTime { get; set; } = new(12, 0);
    [Range(5, 120), Display(Name = "Số phút / lượt")] public int SlotMinutes { get; set; } = 30;
    [Range(1, 200), Display(Name = "Số bệnh nhân tối đa")] public int MaxPatientsPerDay { get; set; } = 8;
    [Range(0, 100000000), Display(Name = "Phí khám (VND)")] public decimal Fee { get; set; } = 250000;
    public IEnumerable<ValidationResult> Validate(ValidationContext context)
    {
        if (EndTime <= StartTime) yield return new("Giờ kết thúc phải sau giờ bắt đầu.", [nameof(EndTime)]);
        else if ((EndTime - StartTime).TotalMinutes < SlotMinutes) yield return new("Ca khám phải đủ ít nhất một lượt.", [nameof(SlotMinutes)]);
    }
}
