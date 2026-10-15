using HospitalManagement.Web.Data;
using HospitalManagement.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagement.Web.Areas.Admin.Controllers;

// Conventional MVC CRUD shared by three independent catalog controllers and their Razor views.
[Area("Admin"), Authorize]
public abstract class CatalogController<T>(ApplicationDbContext db) : Controller where T : class, new()
{
    public async Task<IActionResult> Index() => View(await db.Set<T>().AsNoTracking().OrderBy(x => EF.Property<int>(x, "Id")).ToListAsync());
    [HttpGet] public IActionResult Create() => View("Edit", new T());
    [HttpGet] public async Task<IActionResult> Edit(int id)
    {
        var item = await db.Set<T>().FindAsync(id); return item == null ? NotFound() : View(item);
    }
    [HttpPost] public async Task<IActionResult> Create(T model)
    {
        if (!ModelState.IsValid) return View("Edit", model);
        db.Entry(model).Property("Id").CurrentValue = 0;
        db.Set<T>().Add(model); await db.SaveChangesAsync(); TempData["Success"] = "Đã thêm dữ liệu."; return RedirectToAction(nameof(Index));
    }
    [HttpPost] public async Task<IActionResult> Edit(int id, T model)
    {
        if (!ModelState.IsValid) return View(model);
        var item = await db.Set<T>().FindAsync(id); if (item == null) return NotFound();
        if ((int)db.Entry(model).Property("Id").CurrentValue! != id) return BadRequest();
        db.Entry(item).CurrentValues.SetValues(model);
        await db.SaveChangesAsync(); TempData["Success"] = "Đã lưu thay đổi."; return RedirectToAction(nameof(Index));
    }
    [HttpPost] public async Task<IActionResult> Delete(int id)
    {
        var item = await db.Set<T>().FindAsync(id); if (item == null) return NotFound();
        db.Remove(item);
        try { await db.SaveChangesAsync(); TempData["Success"] = "Đã xóa dữ liệu."; }
        catch (DbUpdateException) { TempData["Error"] = "Dữ liệu đã có phiếu đặt. Hãy bỏ chọn Đang cung cấp thay vì xóa."; }
        return RedirectToAction(nameof(Index));
    }
}
[Authorize(Roles = "Admin,Diagnostic")]
public class ServiceController(ApplicationDbContext db) : CatalogController<DiagnosticService>(db);
[Authorize(Roles = Roles.Admin)]
public class HealthCheckController(ApplicationDbContext db) : CatalogController<HealthCheck>(db);
[Authorize(Roles = Roles.Admin)]
public class NewsController(ApplicationDbContext db) : CatalogController<News>(db);
