using System.ComponentModel.DataAnnotations;

namespace HospitalManagement.Web.Models;

public class Schedule
{
    public int Id { get; set; }
    public int DoctorId { get; set; }
    public Doctor Doctor { get; set; } = null!;
    [Range(0, 6)] public int DayOfWeek { get; set; }
    public TimeOnly StartTime { get; set; } = new(8, 0);
    public TimeOnly EndTime { get; set; } = new(12, 0);
    [Range(5, 120)] public int SlotMinutes { get; set; } = 30;
    [Range(1, 200)] public int MaxPatientsPerDay { get; set; } = 8;
    [Range(0, 100000000)] public decimal Fee { get; set; } = 250000;
}
