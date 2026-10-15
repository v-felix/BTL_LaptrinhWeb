using System.Security.Claims;
using HospitalManagement.Web.Data;
using HospitalManagement.Web.Models;
using HospitalManagement.Web.Models.ViewModels;
using HospitalManagement.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagement.Web.Areas.Doctor.Controllers;

[Area("Doctor"), Authorize(Roles = Roles.Doctor)]
public class AppointmentController(ApplicationDbContext db, NotificationService mail) : Controller
{
    public async Task<IActionResult> Index(DateOnly? date)
    {
        var id = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var query = db.Appointments.AsNoTracking().Include(x => x.Doctor.User).Where(x => x.Doctor.UserId == id);
        if (date != null) query = query.Where(x => x.AppointmentDate == date);
        ViewBag.Date = date;
        return View(await query.OrderByDescending(x => x.AppointmentDate).ThenBy(x => x.SerialNo).ToListAsync());
    }
    [HttpPost] public async Task<IActionResult> Confirm(int id, string rowVersion)
    {
        var appointment = await Own(id); if (appointment == null) return NotFound();
        if (appointment.Status != BookingStatus.Pending) return BadRequest("Chỉ xác nhận lịch đang chờ.");
        if (!AccessService.VersionMatches(appointment.RowVersion, rowVersion)) return Conflict("Lịch đã thay đổi.");
        appointment.Status = BookingStatus.Confirmed;
        try { await db.SaveChangesAsync(); } catch (DbUpdateConcurrencyException) { return Conflict("Lịch đã thay đổi."); }
        await mail.SendAsync(appointment.PatientEmail, "Xác nhận lịch " + appointment.Reference, "Bác sĩ đã xác nhận lịch khám.");
        return RedirectToAction(nameof(Index));
    }
    [HttpGet] public async Task<IActionResult> Prescribe(int id)
    {
        var appointment = await Own(id); if (appointment == null) return NotFound();
        if (appointment.Status != BookingStatus.Confirmed) return BadRequest("Cần xác nhận lịch trước khi kê đơn.");
        ViewBag.Appointment = appointment;
        return View(new PrescriptionVM { AppointmentId = id, RowVersion = Convert.ToBase64String(appointment.RowVersion) });
    }
    [HttpPost] public async Task<IActionResult> Cancel(int id, string reason, string rowVersion)
    {
        var appointment = await Own(id); if (appointment == null) return NotFound();
        if (!AppointmentService.CanTransition(appointment.Status, BookingStatus.Cancelled)) return BadRequest("Không thể hủy lịch đã kết thúc.");
        if (!AccessService.VersionMatches(appointment.RowVersion, rowVersion)) return Conflict("Lịch đã thay đổi.");
        if (string.IsNullOrWhiteSpace(reason) || reason.Length > 1000) return BadRequest("Nhập lý do hủy, tối đa 1000 ký tự.");
        appointment.Status = BookingStatus.Cancelled; appointment.CancellationReason = reason.Trim();
        try { await db.SaveChangesAsync(); } catch (DbUpdateConcurrencyException) { return Conflict("Lịch đã thay đổi."); }
        await mail.SendAsync(appointment.PatientEmail, "Hủy lịch " + appointment.Reference, "Lý do: " + reason);
        TempData["Success"] = "Đã hủy lịch và tạo thông báo cho bệnh nhân."; return RedirectToAction(nameof(Index));
    }
    [HttpPost] public async Task<IActionResult> Prescribe(PrescriptionVM model)
    {
        var appointment = await Own(model.AppointmentId); if (appointment == null) return NotFound();
        if (appointment.Status != BookingStatus.Confirmed) return BadRequest("Lịch khám không ở trạng thái đã xác nhận.");
        if (!AccessService.VersionMatches(appointment.RowVersion, model.RowVersion)) return Conflict("Lịch đã thay đổi.");
        if (!ModelState.IsValid) { ViewBag.Appointment = appointment; return View(model); }
        db.Prescriptions.Add(new Prescription { AppointmentId = appointment.Id, Diagnosis = model.Diagnosis, MedicineDetails = model.MedicineDetails, Advice = model.Advice });
        appointment.Status = BookingStatus.Completed;
        try { await db.SaveChangesAsync(); } catch (DbUpdateConcurrencyException) { return Conflict("Lịch đã thay đổi, đơn thuốc chưa được lưu."); }
        await mail.SendAsync(appointment.PatientEmail, "Đơn thuốc " + appointment.Reference,
            "Bác sĩ đã hoàn tất khám. Xem và tải đơn thuốc tại: " + mail.AbsoluteUrl(Url.Action("Details", "Appointment", new { area = "", id = appointment.Id, token = appointment.PatientId == null ? appointment.AccessToken : null })!));
        TempData["Success"] = "Đã lưu đơn thuốc và hoàn tất khám.";
        return RedirectToAction("Details", "Appointment", new { area = "", id = appointment.Id });
    }
    private Task<Appointment?> Own(int id) => db.Appointments.Include(x => x.Doctor.User).FirstOrDefaultAsync(x => x.Id == id && x.Doctor.UserId == User.FindFirstValue(ClaimTypes.NameIdentifier));
}
