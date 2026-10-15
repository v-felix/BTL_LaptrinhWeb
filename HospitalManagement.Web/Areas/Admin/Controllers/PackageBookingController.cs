using HospitalManagement.Web.Data;
using HospitalManagement.Web.Models;
using HospitalManagement.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagement.Web.Areas.Admin.Controllers;

[Area("Admin"), Authorize(Roles = "Admin,Staff")]
public class PackageBookingController(ApplicationDbContext db) : Controller
{
    public async Task<IActionResult> Index() => View(await db.PackageBookings.Include(x => x.Patient).Include(x => x.HealthCheck).OrderByDescending(x => x.CreatedAt).ToListAsync());
    [HttpPost] public async Task<IActionResult> Update(int id, BookingStatus status, decimal amountPaid, string? reason, string rowVersion)
    {
        var booking = await db.PackageBookings.FindAsync(id); if (booking == null) return NotFound();
        if (!ModelState.IsValid || amountPaid < 0 || amountPaid > booking.TotalAmount || !Enum.IsDefined(status)) return BadRequest("Số tiền hoặc trạng thái không hợp lệ.");
        if (!AccessService.VersionMatches(booking.RowVersion, rowVersion)) return Conflict("Phiếu đã thay đổi. Vui lòng tải lại trang.");
        if (booking.Status is BookingStatus.Completed or BookingStatus.Cancelled) return BadRequest("Phiếu đã kết thúc, không thể sửa.");
        if (status != booking.Status && !AppointmentService.CanTransition(booking.Status, status)) return BadRequest("Chuyển trạng thái không hợp lệ.");
        if (status == BookingStatus.Cancelled && (string.IsNullOrWhiteSpace(reason) || reason.Length > 1000)) return BadRequest("Nhập lý do hủy.");
        booking.Status = status; booking.AmountPaid = amountPaid; booking.CancellationReason = status == BookingStatus.Cancelled ? reason : null;
        try { await db.SaveChangesAsync(); } catch (DbUpdateConcurrencyException) { return Conflict("Phiếu đã thay đổi."); }
        TempData["Success"] = "Đã cập nhật phiếu và số tiền đã thu."; return RedirectToAction(nameof(Index));
    }
}
