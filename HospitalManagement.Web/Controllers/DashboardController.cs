using HospitalManagement.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HospitalManagement.Web.Controllers;

[Authorize]
public class DashboardController : Controller
{
    public IActionResult Index()
    {
        if (User.IsInRole(Roles.Admin)) return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
        if (User.IsInRole(Roles.Doctor)) return RedirectToAction("Index", "Appointment", new { area = "Doctor" });
        if (User.IsInRole(Roles.Diagnostic)) return RedirectToAction("Index", "Booking", new { area = "Diagnostic" });
        if (User.IsInRole(Roles.Staff)) return RedirectToAction("Index", "Dashboard", new { area = "Staff" });
        return RedirectToAction("Index", "Patient");
    }
}
