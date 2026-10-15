using System.Data;
using HospitalManagement.Web.Data;
using HospitalManagement.Web.Models;
using HospitalManagement.Web.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagement.Web.Services;

public record AvailableSlot(string Time, decimal Fee);
public class BookingException(string message) : Exception(message);

public class AppointmentService(ApplicationDbContext db)
{
    public async Task<List<AvailableSlot>> GetAvailableAsync(int doctorId, DateOnly date)
    {
        if (date < HospitalClock.Today || date > HospitalClock.Today.AddDays(90)) return [];
        if (!await db.Doctors.AnyAsync(x => x.Id == doctorId && x.IsActive && x.User.IsActive)) return [];
        var schedule = await db.Schedules.AsNoTracking().SingleOrDefaultAsync(x => x.DoctorId == doctorId && x.DayOfWeek == (int)date.DayOfWeek);
        if (schedule == null) return [];
        var taken = await db.Appointments.Where(x => x.DoctorId == doctorId && x.AppointmentDate == date && x.Status != BookingStatus.Cancelled)
            .Select(x => x.AppointmentTime).ToListAsync();
        if (taken.Count >= schedule.MaxPatientsPerDay) return [];
        var slots = new List<AvailableSlot>();
        var total = (int)(schedule.EndTime - schedule.StartTime).TotalMinutes;
        for (var minute = 0; minute + schedule.SlotMinutes <= total; minute += schedule.SlotMinutes)
        {
            var time = schedule.StartTime.AddMinutes(minute);
            if (!taken.Contains(time) && date.ToDateTime(time) > HospitalClock.Now)
                slots.Add(new(time.ToString("HH:mm"), schedule.Fee));
        }
        return slots;
    }

    public async Task<Appointment> BookAsync(BookAppointmentVM model, string? patientId)
    {
        await using var transaction = await db.Database.BeginTransactionAsync(IsolationLevel.Serializable);
        // A database lock also protects against bookings arriving at another server instance.
        var doctor = await db.Doctors.FromSqlInterpolated($"SELECT * FROM [Doctors] WITH (UPDLOCK, HOLDLOCK) WHERE [Id] = {model.DoctorId}").SingleOrDefaultAsync();
        if (doctor == null) throw new BookingException("Bác sĩ không tồn tại.");
        var slot = (await GetAvailableAsync(model.DoctorId, model.AppointmentDate)).FirstOrDefault(x => x.Time == model.AppointmentTime);
        if (slot == null) throw new BookingException("Khung giờ không còn trống hoặc nằm ngoài lịch làm việc. Vui lòng chọn lại.");
        var serial = await db.Appointments.Where(x => x.DoctorId == model.DoctorId && x.AppointmentDate == model.AppointmentDate)
            .MaxAsync(x => (int?)x.SerialNo) ?? 0;
        var appointment = new Appointment
        {
            DoctorId = model.DoctorId, PatientId = patientId, AppointmentDate = model.AppointmentDate,
            AppointmentTime = TimeOnly.ParseExact(slot.Time, "HH:mm"), SerialNo = serial + 1,
            PatientName = model.PatientName.Trim(), PatientPhone = model.PatientPhone.Trim(), PatientEmail = model.PatientEmail.Trim(),
            PatientAge = model.PatientAge, PatientGender = model.PatientGender, Problem = model.Problem,
            Fee = slot.Fee, Status = BookingStatus.Pending
        };
        db.Appointments.Add(appointment);
        await db.SaveChangesAsync();
        await transaction.CommitAsync();
        return appointment;
    }
    public static bool CanTransition(BookingStatus from, BookingStatus to) => (from, to) switch
    {
        (BookingStatus.Pending, BookingStatus.Confirmed or BookingStatus.Cancelled) => true,
        (BookingStatus.Confirmed, BookingStatus.Completed or BookingStatus.Cancelled) => true,
        _ => false
    };
}
