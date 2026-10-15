using System.Security.Claims;
using HospitalManagement.Web.Data;
using HospitalManagement.Web.Models;
using HospitalManagement.Web.Models.ViewModels;
using HospitalManagement.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagement.Web.Controllers;

public class HealthPackageController(ApplicationDbContext db, AccessService access, ReportService reports, NotificationService mail) : Controller
{
    public async Task<IActionResult> Index() => View(await db.HealthChecks.AsNoTracking().Where(x => x.IsActive).OrderBy(x => x.Price).ToListAsync());
    [Authorize, HttpGet] public async Task<IActionResult> Book(int id)
    {
        var package = await db.HealthChecks.FirstOrDefaultAsync(x => x.Id == id && x.IsActive);
        if (package == null) return NotFound();
        ViewBag.Package = package; return View(new BookPackageVM { PackageId = id });
    }
    [Authorize, HttpPost] public async Task<IActionResult> Book(BookPackageVM model)
    {
        var package = await db.HealthChecks.FirstOrDefaultAsync(x => x.Id == model.PackageId && x.IsActive);
        if (package == null) return NotFound();
        if (model.BookingDate <= HospitalClock.Today || model.BookingDate > HospitalClock.Today.AddDays(90))
            ModelState.AddModelError(nameof(model.BookingDate), "Chọn ngày từ ngày mai, tối đa 90 ngày.");
        if (!ModelState.IsValid) { ViewBag.Package = package; return View(model); }
        var booking = new PackageBooking { PatientId = User.FindFirstValue(ClaimTypes.NameIdentifier)!, HealthCheckId = package.Id,
            BookingDate = model.BookingDate, DepositPercent = model.DepositPercent, TotalAmount = package.Price, CurrencyCode = package.CurrencyCode, AmountPaid = 0 };
        db.PackageBookings.Add(booking); await db.SaveChangesAsync();
        await mail.SendAsync(User.Identity!.Name!, "Tiếp nhận gói khám " + booking.Reference, mail.AbsoluteUrl(Url.Action(nameof(Details), new { id = booking.Id })!));
        return RedirectToAction(nameof(Details), new { id = booking.Id });
    }
    [Authorize, ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public async Task<IActionResult> Details(int id)
    {
        var booking = await Load(id);
        return booking == null || !access.CanRead(booking, User) ? NotFound() : View(booking);
    }
    [Authorize, HttpPost] public async Task<IActionResult> Cancel(int id, string reason, string rowVersion)
    {
        var booking = await db.PackageBookings.FindAsync(id);
        if (booking == null || booking.PatientId != User.FindFirstValue(ClaimTypes.NameIdentifier)) return NotFound();
        if (!AccessService.VersionMatches(booking.RowVersion, rowVersion)) return Conflict("Phiếu đã thay đổi.");
        if (!AppointmentService.CanTransition(booking.Status, BookingStatus.Cancelled) || booking.BookingDate <= HospitalClock.Today)
            return BadRequest("Không thể hủy phiếu này.");
        if (string.IsNullOrWhiteSpace(reason) || reason.Length > 1000) return BadRequest("Nhập lý do hủy (tối đa 1000 ký tự).");
        booking.Status = BookingStatus.Cancelled; booking.CancellationReason = reason.Trim();
        try { await db.SaveChangesAsync(); } catch (DbUpdateConcurrencyException) { return Conflict("Phiếu đã thay đổi."); }
        return RedirectToAction(nameof(Details), new { id });
    }
    [Authorize] public async Task<IActionResult> Download(int id)
    {
        var booking = await Load(id);
        return booking == null || !access.CanRead(booking, User) ? NotFound() : File(reports.Package(booking), "application/pdf", booking.Reference + ".pdf");
    }
    private Task<PackageBooking?> Load(int id) => db.PackageBookings.Include(x => x.HealthCheck).Include(x => x.Patient).FirstOrDefaultAsync(x => x.Id == id);
}
