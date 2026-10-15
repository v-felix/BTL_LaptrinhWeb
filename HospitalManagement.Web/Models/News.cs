using System.ComponentModel.DataAnnotations;

namespace HospitalManagement.Web.Models;

public class News
{
    public int Id { get; set; }
    [Required, StringLength(250), Display(Name = "Tiêu đề")] public string Title { get; set; } = "";
    [Required, StringLength(1000), Display(Name = "Tóm tắt")] public string Excerpt { get; set; } = "";
    [Required, StringLength(20000), Display(Name = "Nội dung")] public string Content { get; set; } = "";
    [Required, StringLength(100), Display(Name = "Chuyên mục")] public string Category { get; set; } = "Tin bệnh viện";
    [Display(Name = "Ngày đăng"), DataType(DataType.Date)] public DateOnly PublishedDate { get; set; } = DateOnly.FromDateTime(DateTime.UtcNow.AddHours(7));
    [Display(Name = "Công khai")] public bool IsPublished { get; set; } = true;
}

public class DoctorMessage
{
    public int Id { get; set; }
    public int DoctorId { get; set; }
    public Doctor Doctor { get; set; } = null!;
    public string AdminId { get; set; } = "";
    public ApplicationUser Admin { get; set; } = null!;
    [Required, MaxLength(2000)] public string Message { get; set; } = "";
    public bool IsRead { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
