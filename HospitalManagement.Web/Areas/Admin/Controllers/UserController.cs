using System.Security.Claims;
using HospitalManagement.Web.Data;
using HospitalManagement.Web.Models;
using HospitalManagement.Web.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagement.Web.Areas.Admin.Controllers;

[Area("Admin"), Authorize(Roles = Roles.Admin)]
public class UserController(ApplicationDbContext db, UserManager<ApplicationUser> users) : Controller
{
    public async Task<IActionResult> Index(string? q, string? role)
    {
        var query = db.Users.AsNoTracking().Include(x => x.DoctorProfile).Include(x => x.StaffProfile).AsQueryable();
        if (!string.IsNullOrWhiteSpace(q)) query = query.Where(x => x.FullName.Contains(q) || x.Email!.Contains(q));
        if (Roles.All.Contains(role))
            query = query.Where(x => db.UserRoles.Any(ur => ur.UserId == x.Id && db.Roles.Any(r => r.Id == ur.RoleId && r.Name == role)));
        ViewBag.Roles = await (from ur in db.UserRoles join r in db.Roles on ur.RoleId equals r.Id select new { ur.UserId, Name = r.Name! }).ToDictionaryAsync(x => x.UserId, x => x.Name);
        ViewBag.Q = q; ViewBag.Role = role;
        return View(await query.OrderBy(x => x.FullName).ToListAsync());
    }
    [HttpGet] public IActionResult Create(string? role) => View("Edit", new UserEditVM { Role = Roles.All.Contains(role) ? role! : Roles.Patient });
    [HttpGet] public async Task<IActionResult> Edit(string id)
    {
        var user = await db.Users.Include(x => x.DoctorProfile).Include(x => x.StaffProfile).FirstOrDefaultAsync(x => x.Id == id);
        if (user == null) return NotFound();
        return View(new UserEditVM { Id = user.Id, FullName = user.FullName, Email = user.Email!, Phone = user.PhoneNumber ?? user.DoctorProfile?.Phone ?? user.StaffProfile?.Phone,
            IsActive = user.IsActive, Role = (await users.GetRolesAsync(user)).FirstOrDefault() ?? Roles.Patient,
            Designation = user.DoctorProfile?.Designation ?? user.StaffProfile?.Designation, Department = user.DoctorProfile?.Speciality ?? user.StaffProfile?.Department,
            About = user.DoctorProfile?.About ?? user.StaffProfile?.About });
    }
    [HttpPost] public Task<IActionResult> Create(UserEditVM model) { model.Id = null; return Save(model); }
    [HttpPost] public Task<IActionResult> Edit(string id, UserEditVM model)
    {
        if (id != model.Id) return Task.FromResult<IActionResult>(BadRequest());
        return Save(model);
    }
    private async Task<IActionResult> Save(UserEditVM model)
    {
        if (model.Id == null && string.IsNullOrWhiteSpace(model.Password)) ModelState.AddModelError(nameof(model.Password), "Nhập mật khẩu cho tài khoản mới.");
        if (model.Role == Roles.Doctor && (string.IsNullOrWhiteSpace(model.Designation) || string.IsNullOrWhiteSpace(model.Department)))
            ModelState.AddModelError("", "Bác sĩ cần chức danh và chuyên khoa.");
        if (model.Id == User.FindFirstValue(ClaimTypes.NameIdentifier) && (!model.IsActive || model.Role != Roles.Admin))
            ModelState.AddModelError("", "Không thể tự khóa hoặc bỏ quyền quản trị của chính mình.");
        if (!ModelState.IsValid) return View("Edit", model);
        await using var transaction = await db.Database.BeginTransactionAsync();
        ApplicationUser user;
        if (model.Id == null) user = new ApplicationUser();
        else
        {
            var existing = await db.Users.Include(x => x.DoctorProfile).Include(x => x.StaffProfile).FirstOrDefaultAsync(x => x.Id == model.Id);
            if (existing == null) return NotFound(); user = existing;
        }
        user.FullName = model.FullName.Trim(); user.Email = model.Email.Trim(); user.UserName = model.Email.Trim();
        user.PhoneNumber = model.Phone; user.IsActive = model.IsActive;
        var result = model.Id == null ? await users.CreateAsync(user, model.Password!) : await users.UpdateAsync(user);
        if (!result.Succeeded) return Errors(result, model);
        var oldRoles = await users.GetRolesAsync(user);
        if (oldRoles.Count != 1 || oldRoles[0] != model.Role)
        {
            result = await users.RemoveFromRolesAsync(user, oldRoles); if (!result.Succeeded) return Errors(result, model);
            result = await users.AddToRoleAsync(user, model.Role); if (!result.Succeeded) return Errors(result, model);
        }
        if (model.Id != null && !string.IsNullOrEmpty(model.Password))
        {
            result = await users.ResetPasswordAsync(user, await users.GeneratePasswordResetTokenAsync(user), model.Password);
            if (!result.Succeeded) return Errors(result, model);
        }
        if (model.Role == Roles.Doctor)
        {
            user.DoctorProfile ??= new Models.Doctor { UserId = user.Id };
            user.DoctorProfile.Designation = model.Designation!; user.DoctorProfile.Speciality = model.Department!;
            user.DoctorProfile.Phone = model.Phone; user.DoctorProfile.About = model.About; user.DoctorProfile.IsActive = model.IsActive;
        }
        else if (user.DoctorProfile != null) user.DoctorProfile.IsActive = false;
        if (model.Role == Roles.Staff)
        {
            user.StaffProfile ??= new Models.Staff { UserId = user.Id };
            user.StaffProfile.Designation = model.Designation; user.StaffProfile.Department = model.Department;
            user.StaffProfile.Phone = model.Phone; user.StaffProfile.About = model.About;
        }
        await db.SaveChangesAsync();
        DbInitializer.Check(await users.UpdateSecurityStampAsync(user));
        await transaction.CommitAsync(); TempData["Success"] = "Đã lưu tài khoản và hồ sơ.";
        return RedirectToAction(nameof(Index), new { role = model.Role });
    }
    [HttpPost] public async Task<IActionResult> Delete(string id)
    {
        if (id == User.FindFirstValue(ClaimTypes.NameIdentifier)) return BadRequest("Không thể xóa chính mình.");
        var user = await db.Users.Include(x => x.DoctorProfile).Include(x => x.StaffProfile).FirstOrDefaultAsync(x => x.Id == id);
        if (user == null) return NotFound();
        await using var transaction = await db.Database.BeginTransactionAsync();
        if (user.DoctorProfile != null)
        {
            db.Schedules.RemoveRange(db.Schedules.Where(x => x.DoctorId == user.DoctorProfile.Id));
            db.DoctorMessages.RemoveRange(db.DoctorMessages.Where(x => x.DoctorId == user.DoctorProfile.Id));
            db.Doctors.Remove(user.DoctorProfile);
        }
        if (user.StaffProfile != null) db.Staff.Remove(user.StaffProfile);
        try
        {
            await db.SaveChangesAsync(); var result = await users.DeleteAsync(user);
            if (!result.Succeeded) { TempData["Error"] = string.Join("; ", result.Errors.Select(x => x.Description)); return RedirectToAction(nameof(Index)); }
            await transaction.CommitAsync(); TempData["Success"] = "Đã xóa tài khoản.";
        }
        catch (DbUpdateException) { TempData["Error"] = "Tài khoản có hồ sơ nghiệp vụ. Hãy tắt Cho phép đăng nhập thay vì xóa."; }
        return RedirectToAction(nameof(Index));
    }
    private IActionResult Errors(IdentityResult result, UserEditVM model)
    { foreach (var e in result.Errors) ModelState.AddModelError("", e.Description); return View("Edit", model); }
}
