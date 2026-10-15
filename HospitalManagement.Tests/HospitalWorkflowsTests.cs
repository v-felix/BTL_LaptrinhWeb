using System.Net;
using System.Text.RegularExpressions;
using HospitalManagement.Web.Data;
using HospitalManagement.Web.Models;
using HospitalManagement.Web.Models.ViewModels;
using HospitalManagement.Web.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace HospitalManagement.Tests;

public class HospitalWorkflowsTests(HospitalFactory factory) : IClassFixture<HospitalFactory>
{
    private async Task<T> Query<T>(Func<ApplicationDbContext, Task<T>> action)
    {
        using var scope = factory.Services.CreateScope();
        return await action(scope.ServiceProvider.GetRequiredService<ApplicationDbContext>());
    }
    private static async Task<string> Token(HttpClient client, string path)
    {
        var page = await client.GetAsync(path); Assert.Equal(HttpStatusCode.OK, page.StatusCode);
        var html = await page.Content.ReadAsStringAsync();
        var match = Regex.Match(html, "name=\"__RequestVerificationToken\"[^>]*value=\"([^\"]+)\"");
        Assert.True(match.Success, "No antiforgery token on " + path);
        return WebUtility.HtmlDecode(match.Groups[1].Value);
    }
    private async Task<HttpResponseMessage> Post(HttpClient client, string page, string action, Dictionary<string, string> values)
    {
        values["__RequestVerificationToken"] = await Token(client, page);
        return await client.PostAsync(action, new FormUrlEncodedContent(values));
    }
    private async Task<HttpClient> Login(string email, string password)
    {
        var client = factory.Client();
        var response = await Post(client, "/Account/Login", "/Account/Login", new() { ["Email"] = email, ["Password"] = password });
        Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
        return client;
    }
    private static DateOnly FutureDay(int offset = 1)
    {
        var date = HospitalClock.Today.AddDays(offset); while (date.DayOfWeek == DayOfWeek.Sunday) date = date.AddDays(1); return date;
    }
    private async Task<Appointment> Appointment(int offset = 1, string time = "08:00", string? patient = null, int? doctorId = null)
    {
        using var scope = factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        doctorId ??= await db.Doctors.Where(x => x.User.Email == "doctor@example.com").Select(x => x.Id).SingleAsync();
        patient ??= await db.Users.Where(x => x.Email == "patient@example.com").Select(x => x.Id).SingleAsync();
        return await scope.ServiceProvider.GetRequiredService<AppointmentService>().BookAsync(new BookAppointmentVM
        {
            DoctorId = doctorId.Value, AppointmentDate = FutureDay(offset), AppointmentTime = time,
            PatientName = "Bệnh nhân kiểm thử", PatientPhone = "0901234567", PatientEmail = "patient@example.com", PatientAge = 25
        }, patient);
    }

    [Fact] public async Task Public_pages_render_and_source_data_is_preserved()
    {
        using var client = factory.Client();
        foreach (var path in new[] { "/", "/Home/About", "/Home/Doctors", "/Home/Doctor/2", "/Home/News", "/Home/Article/1", "/Diagnostic", "/HealthPackage", "/Appointment/Book", "/Account/Register" })
            Assert.Equal(HttpStatusCode.OK, (await client.GetAsync(path)).StatusCode);
        Assert.Equal(21, await Query(db => db.Doctors.CountAsync()));
        Assert.Equal(43, await Query(db => db.DiagnosticServices.CountAsync()));
        Assert.Equal(20, await Query(db => db.Staff.CountAsync()));
        Assert.Equal(7, await Query(db => db.News.CountAsync()));
    }

    [Fact] public async Task Roles_and_all_management_views_work_with_legacy_passwords()
    {
        foreach (var (email, password, paths) in new[]
        {
            ("admin@example.com", "admin123", new[] { "/Admin/Dashboard", "/Admin/User", "/Admin/User/Create?role=Doctor", "/Admin/User/Edit/legacy-2", "/Admin/Schedule", "/Admin/Schedule/Edit/1", "/Admin/Service", "/Admin/Service/Create", "/Admin/Service/Edit/1", "/Admin/HealthCheck", "/Admin/HealthCheck/Edit/1", "/Admin/News", "/Admin/News/Edit/1", "/Admin/Appointment", "/Admin/PackageBooking", "/Diagnostic/Booking" }),
            ("doctor@example.com", "doctor123", new[] { "/Doctor/Appointment", "/Doctor/Schedule", "/Doctor/Schedule/Edit", "/Doctor/Message" }),
            ("diagnostic@example.com", "diagnostic123", new[] { "/Diagnostic/Booking", "/Admin/Service", "/Admin/Service/Create" }),
            ("staff@example.com", "staff123", new[] { "/Staff/Dashboard", "/Admin/Appointment", "/Admin/PackageBooking" }),
            ("patient@example.com", "patient123", new[] { "/Patient", "/Account/Profile", "/Account/Password", "/Diagnostic/Book/1", "/HealthPackage/Book/1" })
        })
        {
            using var client = await Login(email, password);
            foreach (var path in paths)
                Assert.True((await client.GetAsync(path)).StatusCode == HttpStatusCode.OK, email + " could not render " + path);
        }
        var hash = await Query(db => db.Users.Where(x => x.Email == "doctor@example.com").Select(x => x.PasswordHash).SingleAsync());
        Assert.False(hash!.StartsWith("$2")); // Upgraded from bcrypt after a successful login.
        using var patientClient = await Login("patient@example.com", "patient123");
        var denied = await patientClient.GetAsync("/Admin/User");
        Assert.Equal(HttpStatusCode.Redirect, denied.StatusCode); Assert.Contains("AccessDenied", denied.Headers.Location!.ToString());
        foreach (var path in new[] { "/Admin/Service", "/Admin/News", "/Admin/HealthCheck", "/Doctor/Appointment", "/Diagnostic/Booking" })
            Assert.Contains("AccessDenied", (await patientClient.GetAsync(path)).Headers.Location!.ToString());
    }

    [Fact] public async Task Guest_booking_uses_schedule_fee_and_private_capability_link()
    {
        using var guest = factory.Client();
        var date = FutureDay(3);
        var response = await Post(guest, "/Appointment/Book", "/Appointment/Book", new()
        {
            ["DoctorId"] = "1", ["AppointmentDate"] = date.ToString("yyyy-MM-dd"), ["AppointmentTime"] = "08:30",
            ["PatientName"] = "Khách kiểm thử", ["PatientPhone"] = "0901234567", ["PatientEmail"] = "guest@example.com",
            ["PatientAge"] = "20", ["PatientGender"] = "Other", ["Fee"] = "1", ["Status"] = "Completed"
        });
        Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
        var link = response.Headers.Location!.ToString(); Assert.Contains("token=", link);
        Assert.Equal(HttpStatusCode.OK, (await guest.GetAsync(link)).StatusCode);
        var item = await Query(db => db.Appointments.SingleAsync(x => x.PatientEmail == "guest@example.com"));
        Assert.Equal(250000, item.Fee); Assert.Equal(BookingStatus.Pending, item.Status); Assert.Null(item.PatientId);
        Assert.Equal(HttpStatusCode.NotFound, (await guest.GetAsync("/Appointment/Details/" + item.Id)).StatusCode);
        Assert.Equal(HttpStatusCode.NotFound, (await guest.GetAsync("/Appointment/Details/" + item.Id + "?token=wrong")).StatusCode);
        var pdf = await guest.GetAsync($"/Appointment/Download/{item.Id}?token={item.AccessToken}");
        Assert.Equal("application/pdf", pdf.Content.Headers.ContentType!.MediaType);
        Assert.True((await pdf.Content.ReadAsByteArrayAsync()).AsSpan().StartsWith("%PDF-"u8));
    }

    [Fact] public async Task Concurrent_bookings_allow_only_one_patient_in_a_slot()
    {
        async Task<bool> Attempt()
        {
            try { await Appointment(5, "09:00"); return true; } catch (BookingException) { return false; }
        }
        var results = await Task.WhenAll(Attempt(), Attempt());
        Assert.Single(results, x => x);
        var invalid = await Assert.ThrowsAsync<BookingException>(() => Appointment(5, "03:00"));
        Assert.Contains("Khung giờ", invalid.Message);
    }

    [Fact] public async Task Doctor_can_only_prescribe_own_confirmed_appointments_and_owner_can_download()
    {
        var appointment = await Appointment(7);
        using var doctor = await Login("doctor@example.com", "doctor123");
        var response = await Post(doctor, "/Doctor/Appointment", "/Doctor/Appointment/Confirm", new()
        { ["id"] = appointment.Id.ToString(), ["rowVersion"] = Convert.ToBase64String(appointment.RowVersion) });
        Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
        appointment = await Query(db => db.Appointments.SingleAsync(x => x.Id == appointment.Id));
        var form = "/Doctor/Appointment/Prescribe/" + appointment.Id;
        var completed = await Post(doctor, form, "/Doctor/Appointment/Prescribe", new()
        {
            ["AppointmentId"] = appointment.Id.ToString(), ["RowVersion"] = Convert.ToBase64String(appointment.RowVersion),
            ["Diagnosis"] = "Chẩn đoán kiểm thử", ["MedicineDetails"] = "Thuốc mẫu, chỉ dùng để kiểm thử", ["Advice"] = "Tái khám theo hẹn"
        });
        Assert.Equal(HttpStatusCode.Redirect, completed.StatusCode);
        Assert.Equal(BookingStatus.Completed, await Query(db => db.Appointments.Where(x => x.Id == appointment.Id).Select(x => x.Status).SingleAsync()));
        Assert.Equal(1, await Query(db => db.Prescriptions.CountAsync(x => x.AppointmentId == appointment.Id)));
        using var patient = await Login("patient@example.com", "patient123");
        Assert.Equal(HttpStatusCode.OK, (await patient.GetAsync("/Appointment/Details/" + appointment.Id)).StatusCode);
        Assert.Equal("application/pdf", (await patient.GetAsync("/Appointment/Download/" + appointment.Id)).Content.Headers.ContentType!.MediaType);
        using var other = await Login("farzana.akter@example.com", "doctor123");
        Assert.Equal(HttpStatusCode.NotFound, (await other.GetAsync("/Appointment/Details/" + appointment.Id)).StatusCode);
        Assert.Equal(HttpStatusCode.NotFound, (await other.GetAsync(form)).StatusCode);
    }

    [Fact] public async Task Patient_cannot_read_another_patient_and_post_requires_antiforgery()
    {
        var owner = await Query(db => db.Users.Where(x => x.Email == "admin@example.com").Select(x => x.Id).SingleAsync());
        var appointment = await Appointment(9, patient: owner);
        using var patient = await Login("patient@example.com", "patient123");
        Assert.Equal(HttpStatusCode.NotFound, (await patient.GetAsync($"/Appointment/Details/{appointment.Id}?token={appointment.AccessToken}")).StatusCode);
        var response = await patient.PostAsync("/Appointment/Cancel", new FormUrlEncodedContent(new Dictionary<string, string> { ["id"] = appointment.Id.ToString() }));
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact] public async Task Diagnostic_booking_reception_result_and_pdf_work_end_to_end()
    {
        using var patient = await Login("patient@example.com", "patient123");
        var response = await Post(patient, "/Diagnostic/Book/1", "/Diagnostic/Book", new()
        {
            ["ServiceId"] = "1", ["BookingDate"] = FutureDay(11).ToString("yyyy-MM-dd"), ["BookingTime"] = "09:00",
            ["PatientName"] = "Xét nghiệm kiểm thử", ["PatientPhone"] = "0901234567", ["Price"] = "1"
        });
        Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
        var booking = await Query(db => db.DiagnosticBookings.SingleAsync(x => x.PatientName == "Xét nghiệm kiểm thử"));
        Assert.Equal(50, booking.Price);
        Assert.Equal("BDT", booking.CurrencyCode);
        using var technician = await Login("diagnostic@example.com", "diagnostic123");
        var confirmed = await Post(technician, "/Diagnostic/Booking", "/Diagnostic/Booking/Status", new()
        { ["id"] = booking.Id.ToString(), ["status"] = "Confirmed", ["paymentStatus"] = "Paid", ["rowVersion"] = Convert.ToBase64String(booking.RowVersion) });
        Assert.Equal(HttpStatusCode.Redirect, confirmed.StatusCode);
        booking = await Query(db => db.DiagnosticBookings.SingleAsync(x => x.Id == booking.Id));
        var result = await Post(technician, "/Diagnostic/Booking/Result/" + booking.Id, "/Diagnostic/Booking/Result", new()
        { ["BookingId"] = booking.Id.ToString(), ["RowVersion"] = Convert.ToBase64String(booking.RowVersion), ["Content"] = "Kết quả kiểm thử tiếng Việt", ["Conclusion"] = "Mẫu kiểm thử" });
        Assert.Equal(HttpStatusCode.Redirect, result.StatusCode);
        Assert.Equal(BookingStatus.Completed, await Query(db => db.DiagnosticBookings.Where(x => x.Id == booking.Id).Select(x => x.Status).SingleAsync()));
        Assert.Equal(HttpStatusCode.OK, (await patient.GetAsync("/Diagnostic/Details/" + booking.Id)).StatusCode);
        Assert.Equal("application/pdf", (await patient.GetAsync("/Diagnostic/Download/" + booking.Id)).Content.Headers.ContentType!.MediaType);
        using var outsider = await Login("staff@example.com", "staff123");
        Assert.Equal(HttpStatusCode.NotFound, (await outsider.GetAsync("/Diagnostic/Details/" + booking.Id)).StatusCode);
    }

    [Fact] public async Task Package_price_is_trusted_and_only_reception_can_record_payment()
    {
        using var patient = await Login("patient@example.com", "patient123");
        var response = await Post(patient, "/HealthPackage/Book/1", "/HealthPackage/Book", new()
        { ["PackageId"] = "1", ["BookingDate"] = FutureDay(13).ToString("yyyy-MM-dd"), ["DepositPercent"] = "50", ["AmountPaid"] = "9999", ["TotalAmount"] = "1" });
        Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
        var booking = await Query(db => db.PackageBookings.OrderByDescending(x => x.Id).FirstAsync());
        Assert.Equal(2500, booking.TotalAmount); Assert.Equal(0, booking.AmountPaid);
        Assert.Equal("BDT", booking.CurrencyCode);
        using var staff = await Login("staff@example.com", "staff123");
        var paid = await Post(staff, "/Admin/PackageBooking", "/Admin/PackageBooking/Update", new()
        { ["id"] = booking.Id.ToString(), ["status"] = "Confirmed", ["amountPaid"] = "1250", ["rowVersion"] = Convert.ToBase64String(booking.RowVersion) });
        Assert.Equal(HttpStatusCode.Redirect, paid.StatusCode);
        Assert.Equal(1250, await Query(db => db.PackageBookings.Where(x => x.Id == booking.Id).Select(x => x.AmountPaid).SingleAsync()));
        Assert.Equal("application/pdf", (await patient.GetAsync("/HealthPackage/Download/" + booking.Id)).Content.Headers.ContentType!.MediaType);
    }

    [Fact] public async Task Admin_can_create_edit_delete_service_without_binding_navigation_models()
    {
        using var admin = await Login("admin@example.com", "admin123");
        var values = new Dictionary<string, string> { ["Name"] = "Dịch vụ kiểm thử CRUD", ["Category"] = "Test", ["Price"] = "123.45", ["Duration"] = "30", ["ReportHours"] = "24", ["IsActive"] = "true" };
        var created = await Post(admin, "/Admin/Service/Create", "/Admin/Service/Create", values);
        Assert.Equal(HttpStatusCode.Redirect, created.StatusCode);
        var service = await Query(db => db.DiagnosticServices.SingleAsync(x => x.Name == "Dịch vụ kiểm thử CRUD"));
        values["Id"] = service.Id.ToString(); values["Price"] = "456.78";
        var edited = await Post(admin, "/Admin/Service/Edit/" + service.Id, "/Admin/Service/Edit/" + service.Id, values);
        Assert.Equal(HttpStatusCode.Redirect, edited.StatusCode);
        Assert.Equal(456.78m, await Query(db => db.DiagnosticServices.Where(x => x.Id == service.Id).Select(x => x.Price).SingleAsync()));
        var deleted = await Post(admin, "/Admin/Service", "/Admin/Service/Delete/" + service.Id, new());
        Assert.Equal(HttpStatusCode.Redirect, deleted.StatusCode);
        Assert.False(await Query(db => db.DiagnosticServices.AnyAsync(x => x.Id == service.Id)));
    }

    [Fact] public async Task Stale_status_update_is_rejected_and_cancelled_slot_becomes_available()
    {
        var appointment = await Appointment(15);
        using var staff = await Login("staff@example.com", "staff123");
        var values = new Dictionary<string, string> { ["id"] = appointment.Id.ToString(), ["status"] = "Confirmed", ["rowVersion"] = Convert.ToBase64String(appointment.RowVersion) };
        Assert.Equal(HttpStatusCode.Redirect, (await Post(staff, "/Admin/Appointment", "/Admin/Appointment/Status", values)).StatusCode);
        values["status"] = "Cancelled"; values["reason"] = "Kiểm thử hủy";
        Assert.Equal(HttpStatusCode.Conflict, (await Post(staff, "/Admin/Appointment", "/Admin/Appointment/Status", values)).StatusCode);
        appointment = await Query(db => db.Appointments.SingleAsync(x => x.Id == appointment.Id));
        values["rowVersion"] = Convert.ToBase64String(appointment.RowVersion);
        Assert.Equal(HttpStatusCode.Redirect, (await Post(staff, "/Admin/Appointment", "/Admin/Appointment/Status", values)).StatusCode);
        var replacement = await Appointment(15);
        Assert.True(replacement.SerialNo > appointment.SerialNo);
    }

    [Fact] public async Task Admin_can_manage_doctor_profile_schedule_and_internal_notification()
    {
        using var admin = await Login("admin@example.com", "admin123");
        var email = "doctor-crud@example.com";
        var values = new Dictionary<string, string>
        {
            ["FullName"] = "Bác sĩ kiểm thử CRUD", ["Email"] = email, ["Password"] = "testing123", ["Phone"] = "0901234567",
            ["Role"] = "Doctor", ["IsActive"] = "true", ["Designation"] = "Thạc sĩ Bác sĩ", ["Department"] = "Nội tổng hợp"
        };
        Assert.Equal(HttpStatusCode.Redirect, (await Post(admin, "/Admin/User/Create", "/Admin/User/Create", values)).StatusCode);
        var user = await Query(db => db.Users.Include(x => x.DoctorProfile).SingleAsync(x => x.Email == email));
        Assert.NotNull(user.DoctorProfile);
        values["Id"] = user.Id; values["Password"] = ""; values["Department"] = "Tim mạch kiểm thử";
        Assert.Equal(HttpStatusCode.Redirect, (await Post(admin, "/Admin/User/Edit/" + user.Id, "/Admin/User/Edit/" + user.Id, values)).StatusCode);
        Assert.Equal("Tim mạch kiểm thử", await Query(db => db.Doctors.Where(x => x.UserId == user.Id).Select(x => x.Speciality).SingleAsync()));
        using var doctor = await Login(email, "testing123");
        Assert.Equal(HttpStatusCode.Redirect, (await Post(doctor, "/Doctor/Schedule/Edit", "/Doctor/Schedule/Edit", new()
        {
            ["DoctorId"] = user.DoctorProfile!.Id.ToString(), ["DayOfWeek"] = "1", ["StartTime"] = "08:00", ["EndTime"] = "10:00",
            ["SlotMinutes"] = "30", ["MaxPatientsPerDay"] = "4", ["Fee"] = "350000"
        })).StatusCode);
        var schedule = await Query(db => db.Schedules.SingleAsync(x => x.DoctorId == user.DoctorProfile!.Id));
        Assert.Equal(350000, schedule.Fee);
        var forbidden = await Post(doctor, "/Doctor/Schedule/Edit", "/Doctor/Schedule/Delete/1", new());
        Assert.Equal(HttpStatusCode.NotFound, forbidden.StatusCode);
        Assert.Equal(HttpStatusCode.Redirect, (await Post(admin, "/Admin/Schedule", "/Admin/Schedule/Notify", new()
        { ["doctorId"] = user.DoctorProfile!.Id.ToString(), ["message"] = "Thông báo phân ca kiểm thử" })).StatusCode);
        var messages = WebUtility.HtmlDecode(await doctor.GetStringAsync("/Doctor/Message"));
        Assert.Contains("Thông báo phân ca kiểm thử", messages);
        Assert.Equal(HttpStatusCode.Redirect, (await Post(admin, "/Admin/User", "/Admin/User/Delete/" + user.Id, new())).StatusCode);
        Assert.False(await Query(db => db.Users.AnyAsync(x => x.Id == user.Id)));
        Assert.False(await Query(db => db.Schedules.AnyAsync(x => x.Id == schedule.Id)));
    }

    [Fact] public async Task Registration_cannot_grant_admin_and_disabled_accounts_lose_existing_sessions()
    {
        using var client = factory.Client();
        var response = await Post(client, "/Account/Register", "/Account/Register", new()
        {
            ["FullName"] = "Bệnh nhân đăng ký", ["Email"] = "registration-test@example.com", ["Phone"] = "0901234567",
            ["Password"] = "testing123", ["ConfirmPassword"] = "testing123", ["Role"] = "Admin"
        });
        Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
        var user = await Query(db => db.Users.SingleAsync(x => x.Email == "registration-test@example.com"));
        using (var scope = factory.Services.CreateScope())
            Assert.Equal(new[] { Roles.Patient }, await scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>().GetRolesAsync(user));
        Assert.Equal(HttpStatusCode.OK, (await client.GetAsync("/Patient")).StatusCode);
        var profile = await Post(client, "/Account/Profile", "/Account/Profile", new() { ["FullName"] = "Bệnh nhân đổi tên", ["Phone"] = "0901234568" });
        Assert.Equal(HttpStatusCode.Redirect, profile.StatusCode);
        var password = await Post(client, "/Account/Password", "/Account/Password", new()
        { ["CurrentPassword"] = "testing123", ["NewPassword"] = "changed123", ["ConfirmPassword"] = "changed123" });
        Assert.Equal(HttpStatusCode.Redirect, password.StatusCode);
        using var signedInAgain = await Login(user.Email!, "changed123");
        using var admin = await Login("admin@example.com", "admin123");
        var disabled = await Post(admin, "/Admin/User/Edit/" + user.Id, "/Admin/User/Edit/" + user.Id, new()
        { ["Id"] = user.Id, ["FullName"] = user.FullName, ["Email"] = user.Email!, ["Role"] = "Patient", ["IsActive"] = "false" });
        Assert.Equal(HttpStatusCode.Redirect, disabled.StatusCode);
        var blocked = await signedInAgain.GetAsync("/Patient");
        Assert.Equal(HttpStatusCode.Redirect, blocked.StatusCode); Assert.Contains("Login", blocked.Headers.Location!.ToString());
        Assert.Equal(HttpStatusCode.Redirect, (await Post(admin, "/Admin/User", "/Admin/User/Delete/" + user.Id, new())).StatusCode);
    }

    [Fact] public async Task Booking_respects_capacity_past_dates_and_database_restricts_medical_history_deletion()
    {
        using var scope = factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var service = scope.ServiceProvider.GetRequiredService<AppointmentService>();
        var day = FutureDay(20);
        var doctor = await db.Doctors.Include(x => x.User).SingleAsync(x => x.User.Email == "doctor@example.com");
        var schedule = await db.Schedules.SingleAsync(x => x.DoctorId == doctor.Id && x.DayOfWeek == (int)day.DayOfWeek);
        var originalCapacity = schedule.MaxPatientsPerDay; schedule.MaxPatientsPerDay = 1; await db.SaveChangesAsync();
        try
        {
            await Appointment(20, "10:00");
            Assert.Empty(await service.GetAvailableAsync(doctor.Id, day));
            await Assert.ThrowsAsync<BookingException>(() => Appointment(20, "10:30"));
        }
        finally { schedule.MaxPatientsPerDay = originalCapacity; await db.SaveChangesAsync(); }
        Assert.Empty(await service.GetAvailableAsync(doctor.Id, HospitalClock.Today.AddDays(-1)));
        Assert.Empty(await service.GetAvailableAsync(doctor.Id, HospitalClock.Today.AddDays(91)));
        using var admin = await Login("admin@example.com", "admin123");
        var deletion = await Post(admin, "/Admin/User", "/Admin/User/Delete/" + doctor.UserId, new());
        Assert.Equal(HttpStatusCode.Redirect, deletion.StatusCode);
        Assert.True(await Query(context => context.Doctors.AnyAsync(x => x.Id == doctor.Id)));
    }
}
