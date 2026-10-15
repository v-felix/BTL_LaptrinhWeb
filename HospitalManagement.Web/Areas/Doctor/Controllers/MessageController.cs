using System.Security.Claims;
using HospitalManagement.Web.Data;
using HospitalManagement.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagement.Web.Areas.Doctor.Controllers;

[Area("Doctor"), Authorize(Roles = Roles.Doctor)]
public class MessageController(ApplicationDbContext db) : Controller
{
    public async Task<IActionResult> Index() => View(await db.DoctorMessages.Include(x => x.Admin).Where(x => x.Doctor.UserId == User.FindFirstValue(ClaimTypes.NameIdentifier)).OrderByDescending(x => x.CreatedAt).ToListAsync());
    [HttpPost] public async Task<IActionResult> Read(int id)
    {
        var message = await db.DoctorMessages.FirstOrDefaultAsync(x => x.Id == id && x.Doctor.UserId == User.FindFirstValue(ClaimTypes.NameIdentifier));
        if (message == null) return NotFound();
        message.IsRead = true; await db.SaveChangesAsync(); return RedirectToAction(nameof(Index));
    }
}
