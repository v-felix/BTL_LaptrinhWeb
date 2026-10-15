using System.Security.Claims;
using HospitalManagement.Web.Models;

namespace HospitalManagement.Web.Services;

public class AccessService
{
    public bool CanRead(Appointment appointment, ClaimsPrincipal user, string? token = null) =>
        user.IsInRole(Roles.Admin) || user.IsInRole(Roles.Staff) ||
        (user.Identity?.IsAuthenticated == true && appointment.PatientId == user.FindFirstValue(ClaimTypes.NameIdentifier)) ||
        (appointment.PatientId == null && token?.Length == 64 && string.Equals(appointment.AccessToken, token, StringComparison.Ordinal));
    public bool CanRead(BookingDiagnostic booking, ClaimsPrincipal user) =>
        user.IsInRole(Roles.Admin) || user.IsInRole(Roles.Diagnostic) || booking.PatientId == user.FindFirstValue(ClaimTypes.NameIdentifier);
    public bool CanRead(PackageBooking booking, ClaimsPrincipal user) =>
        user.IsInRole(Roles.Admin) || user.IsInRole(Roles.Staff) || booking.PatientId == user.FindFirstValue(ClaimTypes.NameIdentifier);
    public static bool VersionMatches(byte[] version, string? encoded) => !string.IsNullOrEmpty(encoded) && Convert.ToBase64String(version) == encoded;
}
