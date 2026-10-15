using System.Security.Claims;
using HospitalManagement.Web.Data;
using HospitalManagement.Web.Models;
using HospitalManagement.Web.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagement.Web.Areas.Doctor.Controllers;

[Area("Doctor"), Authorize(Roles = Roles.Doctor)]
public class ScheduleController(ApplicationDbContext db) : Controller
{
    public async Task<IActionResult> Index()
    {
        var doctorId = await DoctorId(); if (doctorId == 0) return Forbid();
        ViewBag.DoctorId = doctorId;
        return View(await db.Schedules.Where(x => x.DoctorId == doctorId).OrderBy(x => x.DayOfWeek).ToListAsync());
    }
    [HttpGet] public async Task<IActionResult> Edit(int? id)
    {
        var doctorId = await DoctorId(); if (doctorId == 0) return Forbid();
        if (id == null) return View(new ScheduleVM { DoctorId = doctorId });
        var s = await db.Schedules.FirstOrDefaultAsync(x => x.Id == id && x.DoctorId == doctorId); if (s == null) return NotFound();
        return View(new ScheduleVM { Id = s.Id, DoctorId = s.DoctorId, DayOfWeek = s.DayOfWeek, StartTime = s.StartTime, EndTime = s.EndTime, Fee = s.Fee, SlotMinutes = s.SlotMinutes, MaxPatientsPerDay = s.MaxPatientsPerDay });
    }
    [HttpPost] public async Task<IActionResult> Edit(ScheduleVM model)
    {
        var doctorId = await DoctorId(); if (doctorId == 0 || model.DoctorId != doctorId) return Forbid();
        if (await db.Schedules.AnyAsync(x => x.DoctorId == doctorId && x.DayOfWeek == model.DayOfWeek && x.Id != model.Id)) ModelState.AddModelError("", "Bạn đã có ca trong ngày này.");
        if (!ModelState.IsValid) return View(model);
        var s = model.Id == 0 ? new Schedule { DoctorId = doctorId } : await db.Schedules.FirstOrDefaultAsync(x => x.Id == model.Id && x.DoctorId == doctorId);
        if (s == null) return NotFound();
        s.DayOfWeek = model.DayOfWeek; s.StartTime = model.StartTime; s.EndTime = model.EndTime; s.Fee = model.Fee; s.SlotMinutes = model.SlotMinutes; s.MaxPatientsPerDay = model.MaxPatientsPerDay;
        if (s.Id == 0) db.Schedules.Add(s);
        try { await db.SaveChangesAsync(); } catch (DbUpdateException) { ModelState.AddModelError("", "Ca làm việc đã thay đổi. Vui lòng kiểm tra lại."); return View(model); }
        TempData["Success"] = "Đã lưu lịch làm việc. Những lịch hẹn đã đặt vẫn được giữ."; return RedirectToAction(nameof(Index));
    }
    [HttpPost] public async Task<IActionResult> Delete(int id)
    {
        var doctorId = await DoctorId();
        var schedule = await db.Schedules.FirstOrDefaultAsync(x => x.Id == id && x.DoctorId == doctorId); if (schedule == null) return NotFound();
        db.Schedules.Remove(schedule); await db.SaveChangesAsync(); return RedirectToAction(nameof(Index));
    }
    private async Task<int> DoctorId() => await db.Doctors.Where(x => x.UserId == User.FindFirstValue(ClaimTypes.NameIdentifier) && x.IsActive).Select(x => x.Id).SingleOrDefaultAsync();
}
