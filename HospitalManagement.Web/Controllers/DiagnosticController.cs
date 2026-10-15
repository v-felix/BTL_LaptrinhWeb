using System.Security.Claims;
using HospitalManagement.Web.Data;
using HospitalManagement.Web.Models;
using HospitalManagement.Web.Models.ViewModels;
using HospitalManagement.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagement.Web.Controllers;

public class DiagnosticController(ApplicationDbContext db, UserManager<ApplicationUser> users, AccessService access,
    IWebHostEnvironment env, ReportService reports, NotificationService mail) : Controller
{
    public async Task<IActionResult> Index(string? q, string? category)
    {
        var query = db.DiagnosticServices.AsNoTracking().Where(x => x.IsActive);
        if (!string.IsNullOrWhiteSpace(q)) query = query.Where(x => x.Name.Contains(q));
        if (!string.IsNullOrWhiteSpace(category)) query = query.Where(x => x.Category == category);
        ViewBag.Categories = await db.DiagnosticServices.Where(x => x.IsActive).Select(x => x.Category).Distinct().OrderBy(x => x).ToListAsync();
        ViewBag.Q = q; ViewBag.Category = category; return View(await query.OrderBy(x => x.Name).ToListAsync());
    }
    [Authorize, HttpGet] public async Task<IActionResult> Book(int id)
    {
        var service = await db.DiagnosticServices.FirstOrDefaultAsync(x => x.Id == id && x.IsActive);
        if (service == null) return NotFound();
        var user = (await users.GetUserAsync(User))!;
        ViewBag.Service = service;
        return View(new BookDiagnosticVM { ServiceId = id, PatientName = user.FullName, PatientPhone = user.PhoneNumber ?? "" });
    }
    [Authorize, HttpPost] public async Task<IActionResult> Book(BookDiagnosticVM model)
    {
        var service = await db.DiagnosticServices.FirstOrDefaultAsync(x => x.Id == model.ServiceId && x.IsActive);
        if (service == null) return NotFound();
        var patientId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        if (!TimeOnly.TryParseExact(model.BookingTime, "HH:mm", out var time) || time < new TimeOnly(7, 0) || time > new TimeOnly(17, 0))
            ModelState.AddModelError(nameof(model.BookingTime), "Chọn giờ từ 07:00 đến 17:00.");
        if (model.BookingDate < HospitalClock.Today || model.BookingDate > HospitalClock.Today.AddDays(90) || model.BookingDate.ToDateTime(time) <= HospitalClock.Now)
            ModelState.AddModelError(nameof(model.BookingDate), "Chọn giờ hẹn trong tương lai, tối đa 90 ngày.");
        Appointment? appointment = null;
        if (!string.IsNullOrWhiteSpace(model.AppointmentReference))
        {
            appointment = await db.Appointments.FirstOrDefaultAsync(x => x.Reference == model.AppointmentReference && x.PatientId == patientId && x.Status != BookingStatus.Cancelled);
            if (appointment == null) ModelState.AddModelError(nameof(model.AppointmentReference), "Không tìm thấy mã lịch khám thuộc tài khoản của bạn.");
        }
        if (!ModelState.IsValid) { ViewBag.Service = service; return View(model); }
        var booking = new BookingDiagnostic { DiagnosticServiceId = service.Id, PatientId = patientId, AppointmentId = appointment?.Id,
            PatientName = model.PatientName, PatientPhone = model.PatientPhone, BookingDate = model.BookingDate, BookingTime = time,
            Address = model.Address, Notes = model.Notes, Price = service.Price, CurrencyCode = service.CurrencyCode };
        db.DiagnosticBookings.Add(booking); await db.SaveChangesAsync();
        await mail.SendAsync((await users.GetUserAsync(User))!.Email!, "Tiếp nhận xét nghiệm " + booking.Reference,
            mail.AbsoluteUrl(Url.Action(nameof(Details), new { id = booking.Id })!));
        return RedirectToAction(nameof(Details), new { id = booking.Id });
    }
    [Authorize, ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public async Task<IActionResult> Details(int id)
    {
        var booking = await Load(id);
        return booking == null || !access.CanRead(booking, User) ? NotFound() : View(booking);
    }
    [Authorize, HttpPost] public async Task<IActionResult> Cancel(int id, string rowVersion)
    {
        var booking = await db.DiagnosticBookings.FindAsync(id);
        if (booking == null || booking.PatientId != User.FindFirstValue(ClaimTypes.NameIdentifier)) return NotFound();
        if (!AccessService.VersionMatches(booking.RowVersion, rowVersion)) return Conflict("Phiếu đã thay đổi.");
        if (!AppointmentService.CanTransition(booking.Status, BookingStatus.Cancelled) || booking.BookingDate.ToDateTime(booking.BookingTime) <= HospitalClock.Now)
            return BadRequest("Không thể hủy phiếu này.");
        booking.Status = BookingStatus.Cancelled;
        try { await db.SaveChangesAsync(); } catch (DbUpdateConcurrencyException) { return Conflict("Phiếu đã thay đổi."); }
        return RedirectToAction(nameof(Details), new { id });
    }
    [Authorize] public async Task<IActionResult> Download(int id)
    {
        var booking = await Load(id);
        return booking == null || !access.CanRead(booking, User) ? NotFound() : File(reports.Diagnostic(booking), "application/pdf", booking.Reference + ".pdf");
    }
    [Authorize] public async Task<IActionResult> ResultFile(int id)
    {
        var result = await db.TestResults.Include(x => x.BookingDiagnostic).FirstOrDefaultAsync(x => x.Id == id);
        if (result?.FileName == null || !access.CanRead(result.BookingDiagnostic, User)) return NotFound();
        var path = Path.Combine(env.ContentRootPath, "App_Data", "Results", Path.GetFileName(result.FileName));
        return System.IO.File.Exists(path) ? PhysicalFile(path, "application/pdf", "ket-qua.pdf") : NotFound();
    }
    private Task<BookingDiagnostic?> Load(int id) => db.DiagnosticBookings.Include(x => x.DiagnosticService).Include(x => x.TestResults).FirstOrDefaultAsync(x => x.Id == id);
}
