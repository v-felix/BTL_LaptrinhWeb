using System.ComponentModel.DataAnnotations;

namespace HospitalManagement.Web.Models;

public class HealthCheck
{
    public int Id { get; set; }
    [Required, StringLength(200), Display(Name = "Tên gói khám")] public string Name { get; set; } = "";
    [Range(0, 100000000), Display(Name = "Giá (VND)")] public decimal Price { get; set; }
    [Required, StringLength(3), AllowedValues("VND", ErrorMessage = "Hệ thống chỉ sử dụng Việt Nam đồng (VND)."), Display(Name = "Đơn vị tiền")] public string CurrencyCode { get; set; } = "VND";
    [Required, StringLength(4000), Display(Name = "Quyền lợi (mỗi dòng một mục)")] public string Features { get; set; } = "";
    [Display(Name = "Gói nổi bật")] public bool Popular { get; set; }
    [Display(Name = "Đang cung cấp")] public bool IsActive { get; set; } = true;
}

public class PackageBooking
{
    public int Id { get; set; }
    [MaxLength(30)] public string Reference { get; set; } = "GK-" + Guid.NewGuid().ToString("N")[..12].ToUpperInvariant();
    public string PatientId { get; set; } = "";
    public ApplicationUser Patient { get; set; } = null!;
    public int HealthCheckId { get; set; }
    public HealthCheck HealthCheck { get; set; } = null!;
    public DateOnly BookingDate { get; set; }
    public int DepositPercent { get; set; } = 50;
    public decimal TotalAmount { get; set; }
    [MaxLength(3)] public string CurrencyCode { get; set; } = "VND";
    public decimal AmountPaid { get; set; }
    public BookingStatus Status { get; set; }
    [MaxLength(1000)] public string? CancellationReason { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    [Timestamp] public byte[] RowVersion { get; set; } = [];
}
