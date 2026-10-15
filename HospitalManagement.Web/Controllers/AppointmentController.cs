using System.Security.Claims;
using HospitalManagement.Web.Data;
using HospitalManagement.Web.Models;
using HospitalManagement.Web.Models.ViewModels;
using HospitalManagement.Web.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagement.Web.Controllers;

public class AppointmentController(ApplicationDbContext db, AppointmentService bookings, AccessService access,
    UserManager<ApplicationUser> users, NotificationService mail, ReportService reports) : Controller
{
    [HttpGet] public async Task<IActionResult> Book(int? doctorId)
    {
        var user = await users.GetUserAsync(User);
        var model = new BookAppointmentVM { DoctorId = doctorId ?? 0, PatientName = user?.FullName ?? "", PatientPhone = user?.PhoneNumber ?? "", PatientEmail = user?.Email ?? "" };
        await Populate(model); return View(model);
    }
    [HttpGet] public async Task<IActionResult> Slots(int doctorId, DateOnly date) => Json(await bookings.GetAvailableAsync(doctorId, date));
    [HttpPost] public async Task<IActionResult> Book(BookAppointmentVM model)
    {
        if (ModelState.IsValid)
        {
            try
            {
                var appointment = await bookings.BookAsync(model, User.FindFirstValue(ClaimTypes.NameIdentifier));
                var token = appointment.PatientId == null ? appointment.AccessToken : null;
                await mail.SendAsync(appointment.PatientEmail, "Tiếp nhận lịch khám " + appointment.Reference,
                    "Lịch khám đang chờ xác nhận. Xem phiếu: " + mail.AbsoluteUrl(Url.Action(nameof(Details), new { id = appointment.Id, token })!));
                return RedirectToAction(nameof(Details), new { id = appointment.Id, token });
            }
            catch (BookingException ex) { ModelState.AddModelError("", ex.Message); }
            catch (DbUpdateException ex) when (ex.InnerException is SqlException { Number: 2601 or 2627 or 1205 })
            { ModelState.AddModelError("", "Khung giờ vừa được đặt. Vui lòng chọn lại."); }
        }
        await Populate(model); return View(model);
    }
    [HttpGet, ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public async Task<IActionResult> Details(int id, string? token)
    {
        var appointment = await Load(id);
        if (appointment == null) return NotFound();
        if (!access.CanRead(appointment, User, token) && !await OwnsDoctor(appointment)) return NotFound();
        ViewBag.Token = token;
        ViewBag.CanCancel = (appointment.Status is BookingStatus.Pending or BookingStatus.Confirmed) &&
            appointment.AppointmentDate.ToDateTime(appointment.AppointmentTime) > HospitalClock.Now &&
            (appointment.PatientId == User.FindFirstValue(ClaimTypes.NameIdentifier) || (appointment.PatientId == null && access.CanRead(appointment, User, token)));
        return View(appointment);
    }
    [HttpPost] public async Task<IActionResult> Cancel(int id, string? token, string reason, string rowVersion)
    {
        var appointment = await db.Appointments.FindAsync(id);
        if (appointment == null || !access.CanRead(appointment, User, token)) return NotFound();
        if (!AccessService.VersionMatches(appointment.RowVersion, rowVersion)) return Conflict("Lịch khám đã thay đổi. Vui lòng tải lại trang.");
        if (!AppointmentService.CanTransition(appointment.Status, BookingStatus.Cancelled) || appointment.AppointmentDate.ToDateTime(appointment.AppointmentTime) <= HospitalClock.Now)
            return BadRequest("Không thể hủy lịch đã khám, đã hủy hoặc đã qua giờ hẹn.");
        if (string.IsNullOrWhiteSpace(reason) || reason.Length > 1000) return BadRequest("Vui lòng nhập lý do hủy (tối đa 1000 ký tự).");
        appointment.Status = BookingStatus.Cancelled; appointment.CancellationReason = reason.Trim();
        try { await db.SaveChangesAsync(); }
        catch (DbUpdateConcurrencyException) { return Conflict("Lịch khám đã thay đổi. Vui lòng tải lại trang."); }
        await mail.SendAsync(appointment.PatientEmail, "Hủy lịch " + appointment.Reference, "Lịch khám đã được hủy. Lý do: " + reason);
        TempData["Success"] = "Đã hủy lịch khám."; return RedirectToAction(nameof(Details), new { id, token });
    }
    public async Task<IActionResult> Download(int id, string? token)
    {
        var appointment = await Load(id);
        if (appointment == null || (!access.CanRead(appointment, User, token) && !await OwnsDoctor(appointment))) return NotFound();
        return File(reports.Appointment(appointment), "application/pdf", appointment.Reference + ".pdf");
    }
    private Task<Appointment?> Load(int id) => db.Appointments.Include(x => x.Doctor.User).Include(x => x.Prescriptions).FirstOrDefaultAsync(x => x.Id == id);
    private Task<bool> OwnsDoctor(Appointment appointment) => User.IsInRole(Roles.Doctor)
        ? db.Doctors.AnyAsync(x => x.Id == appointment.DoctorId && x.UserId == User.FindFirstValue(ClaimTypes.NameIdentifier)) : Task.FromResult(false);
    private async Task Populate(BookAppointmentVM model)
    {
        ViewBag.Doctors = await db.Doctors.AsNoTracking().Include(x => x.User).Where(x => x.IsActive && x.User.IsActive).OrderBy(x => x.User.FullName).ToListAsync();
        ViewBag.Slots = await bookings.GetAvailableAsync(model.DoctorId, model.AppointmentDate);
    }
}
