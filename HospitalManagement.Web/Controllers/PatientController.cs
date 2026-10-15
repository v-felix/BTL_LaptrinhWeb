using System.Security.Claims;
using HospitalManagement.Web.Data;
using HospitalManagement.Web.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagement.Web.Controllers;

[Authorize]
public class PatientController(ApplicationDbContext db) : Controller
{
    public async Task<IActionResult> Index()
    {
        var id = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return View(new PatientDashboardVM
        {
            Appointments = await db.Appointments.AsNoTracking().Include(x => x.Doctor.User).Include(x => x.Prescriptions).Where(x => x.PatientId == id).OrderByDescending(x => x.AppointmentDate).ToListAsync(),
            Diagnostics = await db.DiagnosticBookings.AsNoTracking().Include(x => x.DiagnosticService).Include(x => x.TestResults).Where(x => x.PatientId == id).OrderByDescending(x => x.CreatedAt).ToListAsync(),
            Packages = await db.PackageBookings.AsNoTracking().Include(x => x.HealthCheck).Where(x => x.PatientId == id).OrderByDescending(x => x.CreatedAt).ToListAsync()
        });
    }
}
