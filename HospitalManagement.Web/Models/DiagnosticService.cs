using System.ComponentModel.DataAnnotations;

namespace HospitalManagement.Web.Models;

public class DiagnosticService
{
    public int Id { get; set; }
    [Required, StringLength(200), Display(Name = "Tên dịch vụ")] public string Name { get; set; } = "";
    [Required, StringLength(150), Display(Name = "Nhóm dịch vụ")] public string Category { get; set; } = "";
    [StringLength(4000), Display(Name = "Mô tả")] public string? Description { get; set; }
    [StringLength(100), Display(Name = "Loại mẫu")] public string? SampleType { get; set; }
    [Range(0, 100000000), Display(Name = "Giá")] public decimal Price { get; set; }
    [Required, StringLength(3), AllowedValues("VND", "BDT"), Display(Name = "Đơn vị tiền")] public string CurrencyCode { get; set; } = "VND";
    [Range(1, 1440), Display(Name = "Thời gian thực hiện (phút)")] public int Duration { get; set; } = 30;
    [Range(1, 720), Display(Name = "Trả kết quả sau (giờ)")] public int ReportHours { get; set; } = 24;
    [Display(Name = "Lấy mẫu tại nhà")] public bool HomeCollectionAvailable { get; set; }
    [Display(Name = "Đang cung cấp")] public bool IsActive { get; set; } = true;
}
