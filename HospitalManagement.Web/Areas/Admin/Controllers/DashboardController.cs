using HospitalManagement.Web.Data;
using HospitalManagement.Web.Models;
using HospitalManagement.Web.Models.ViewModels;
using HospitalManagement.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagement.Web.Areas.Admin.Controllers;

[Area("Admin"), Authorize(Roles = Roles.Admin)]
public class DashboardController(ApplicationDbContext db) : Controller
{
    public async Task<IActionResult> Index() => View(new DashboardVM
    {
        Doctors = await db.Doctors.CountAsync(x => x.IsActive && x.User.IsActive),
        Patients = await (from u in db.UserRoles join r in db.Roles on u.RoleId equals r.Id where r.Name == Roles.Patient select u).CountAsync(),
        Pending = await db.Appointments.CountAsync(x => x.Status == BookingStatus.Pending),
        Today = await db.Appointments.CountAsync(x => x.AppointmentDate == HospitalClock.Today && x.Status != BookingStatus.Cancelled),
        RecordedPackagePayments = await db.PackageBookings.SumAsync(x => (decimal?)x.AmountPaid) ?? 0,
        Appointments = await db.Appointments.AsNoTracking().Include(x => x.Doctor.User).OrderByDescending(x => x.CreatedAt).Take(10).ToListAsync()
    });
}
