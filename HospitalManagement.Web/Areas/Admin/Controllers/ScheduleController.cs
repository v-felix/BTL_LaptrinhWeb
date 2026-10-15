using System.Security.Claims;
using HospitalManagement.Web.Data;
using HospitalManagement.Web.Models;
using HospitalManagement.Web.Models.ViewModels;
using HospitalManagement.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagement.Web.Areas.Admin.Controllers;

[Area("Admin"), Authorize(Roles = Roles.Admin)]
public class ScheduleController(ApplicationDbContext db) : Controller
{
    public async Task<IActionResult> Index(int? doctorId)
    {
        ViewBag.Doctors = await db.Doctors.Include(x => x.User).OrderBy(x => x.User.FullName).ToListAsync();
        ViewBag.DoctorId = doctorId;
        return View(await db.Schedules.Include(x => x.Doctor.User).Where(x => doctorId == null || x.DoctorId == doctorId).OrderBy(x => x.DoctorId).ThenBy(x => x.DayOfWeek).ToListAsync());
    }
    [HttpGet] public async Task<IActionResult> Edit(int? id, int? doctorId)
    {
        await Populate();
        if (id == null) return View(new ScheduleVM { DoctorId = doctorId ?? 0 });
        var s = await db.Schedules.FindAsync(id); if (s == null) return NotFound();
        return View(new ScheduleVM { Id = s.Id, DoctorId = s.DoctorId, DayOfWeek = s.DayOfWeek, StartTime = s.StartTime, EndTime = s.EndTime, Fee = s.Fee, SlotMinutes = s.SlotMinutes, MaxPatientsPerDay = s.MaxPatientsPerDay });
    }
    [HttpPost] public async Task<IActionResult> Edit(ScheduleVM model)
    {
        if (!await db.Doctors.AnyAsync(x => x.Id == model.DoctorId && x.IsActive)) ModelState.AddModelError("", "Chọn bác sĩ đang hoạt động.");
        if (await db.Schedules.AnyAsync(x => x.DoctorId == model.DoctorId && x.DayOfWeek == model.DayOfWeek && x.Id != model.Id)) ModelState.AddModelError("", "Bác sĩ đã có ca trong ngày này.");
        if (!ModelState.IsValid) { await Populate(); return View(model); }
        var schedule = model.Id == 0 ? new Schedule() : await db.Schedules.FindAsync(model.Id);
        if (schedule == null) return NotFound();
        if (schedule.Id != 0 && schedule.DoctorId != model.DoctorId) return BadRequest("Không thể chuyển ca sang bác sĩ khác.");
        schedule.DoctorId = model.DoctorId; schedule.DayOfWeek = model.DayOfWeek; schedule.StartTime = model.StartTime; schedule.EndTime = model.EndTime;
        schedule.SlotMinutes = model.SlotMinutes; schedule.MaxPatientsPerDay = model.MaxPatientsPerDay; schedule.Fee = model.Fee;
        if (schedule.Id == 0) db.Schedules.Add(schedule);
        try { await db.SaveChangesAsync(); } catch (DbUpdateException) { ModelState.AddModelError("", "Ca làm việc vừa được cập nhật. Vui lòng kiểm tra lại."); await Populate(); return View(model); }
        TempData["Success"] = "Đã lưu ca làm việc. Lịch hẹn đã đặt vẫn giữ nguyên.";
        return RedirectToAction(nameof(Index), new { doctorId = model.DoctorId });
    }
    [HttpPost] public async Task<IActionResult> Delete(int id)
    {
        var schedule = await db.Schedules.FindAsync(id); if (schedule == null) return NotFound();
        db.Schedules.Remove(schedule); await db.SaveChangesAsync();
        TempData["Success"] = "Đã xóa ca. Những lịch hẹn đã đặt vẫn được giữ để xử lý."; return RedirectToAction(nameof(Index));
    }
    [HttpPost] public async Task<IActionResult> Notify(int doctorId, string message)
    {
        if (string.IsNullOrWhiteSpace(message) || message.Length > 2000 || !await db.Doctors.AnyAsync(x => x.Id == doctorId)) return BadRequest("Thông báo không hợp lệ.");
        db.DoctorMessages.Add(new DoctorMessage { DoctorId = doctorId, AdminId = User.FindFirstValue(ClaimTypes.NameIdentifier)!, Message = message.Trim() });
        await db.SaveChangesAsync(); TempData["Success"] = "Đã gửi thông báo nội bộ cho bác sĩ.";
        return RedirectToAction(nameof(Index), new { doctorId });
    }
    private async Task Populate() => ViewBag.Doctors = await db.Doctors.Include(x => x.User).Where(x => x.IsActive && x.User.IsActive).OrderBy(x => x.User.FullName).ToListAsync();
}
