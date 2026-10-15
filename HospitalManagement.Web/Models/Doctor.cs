using System.ComponentModel.DataAnnotations;

namespace HospitalManagement.Web.Models;

public class Doctor
{
    public int Id { get; set; }
    [Required] public string UserId { get; set; } = "";
    public ApplicationUser User { get; set; } = null!;
    [Required, StringLength(200)] public string Designation { get; set; } = "";
    [Required, StringLength(150)] public string Speciality { get; set; } = "";
    [StringLength(30)] public string? Phone { get; set; }
    [StringLength(4000)] public string? About { get; set; }
    public bool IsActive { get; set; } = true;
    public ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();
}

public class Staff
{
    public int Id { get; set; }
    [Required] public string UserId { get; set; } = "";
    public ApplicationUser User { get; set; } = null!;
    [StringLength(200)] public string? Designation { get; set; }
    [StringLength(150)] public string? Department { get; set; }
    [StringLength(30)] public string? Phone { get; set; }
    [StringLength(4000)] public string? About { get; set; }
}
