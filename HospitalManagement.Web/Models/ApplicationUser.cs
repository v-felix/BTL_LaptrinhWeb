using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace HospitalManagement.Web.Models;

public class ApplicationUser : IdentityUser
{
    [Required, StringLength(150)] public string FullName { get; set; } = "";
    [StringLength(500)] public string? Photo { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public Doctor? DoctorProfile { get; set; }
    public Staff? StaffProfile { get; set; }
}

public static class Roles
{
    public const string Admin = "Admin", Doctor = "Doctor", Staff = "Staff", Diagnostic = "Diagnostic", Patient = "Patient";
    public static readonly string[] All = [Admin, Doctor, Staff, Diagnostic, Patient];
}
