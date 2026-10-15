using HospitalManagement.Web.Data;
using HospitalManagement.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagement.Web.Areas.Staff.Controllers;

[Area("Staff"), Authorize(Roles = "Staff,Admin")]
public class DashboardController(ApplicationDbContext db) : Controller
{
    public async Task<IActionResult> Index() => View(await db.Appointments.AsNoTracking().Include(x => x.Doctor.User)
        .Where(x => x.AppointmentDate == HospitalClock.Today).OrderBy(x => x.AppointmentTime).ToListAsync());
}
