using System.Globalization;
using System.Text.Json;
using HospitalManagement.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagement.Web.Data;

public static class DbInitializer
{
    public static async Task InitializeAsync(IServiceProvider services, bool demo)
    {
        var db = services.GetRequiredService<ApplicationDbContext>();
        await db.Database.MigrateAsync();
        if (!demo) return;
        var users = services.GetRequiredService<UserManager<ApplicationUser>>();
        var roles = services.GetRequiredService<RoleManager<IdentityRole>>();
        foreach (var role in Roles.All)
            if (!await roles.RoleExistsAsync(role)) Check(await roles.CreateAsync(new(role)));

        if (!await db.Users.AnyAsync())
        {
            var env = services.GetRequiredService<IWebHostEnvironment>();
            var seedDirectory = Path.Combine(env.ContentRootPath, "Data", "Legacy");
            var seedPath = Path.Combine(seedDirectory, "snapshot.json");
            if (!File.Exists(seedPath)) seedPath = Path.Combine(seedDirectory, "snapshot.example.json");
            using var json = JsonDocument.Parse(await File.ReadAllTextAsync(seedPath));
            var root = json.RootElement;
            // Atomic import: a failure cannot leave half an imported hospital.
            await using var transaction = await db.Database.BeginTransactionAsync();
            foreach (var row in root.GetProperty("users").EnumerateArray())
            {
                var role = Roles.All.FirstOrDefault(x => x.Equals(S(row, "role"), StringComparison.OrdinalIgnoreCase)) ?? Roles.Patient;
                var legacyHash = S(row, "password");
                var user = new ApplicationUser
                {
                    Id = "legacy-" + row.GetProperty("id").GetInt32(), FullName = S(row, "name").Trim(),
                    Email = S(row, "email"), UserName = S(row, "email"), EmailConfirmed = true,
                    PasswordHash = string.IsNullOrEmpty(legacyHash) ? null : legacyHash, SecurityStamp = Guid.NewGuid().ToString(), Photo = null
                };
                Check(string.IsNullOrEmpty(legacyHash)
                    ? await users.CreateAsync(user, role.ToLowerInvariant() + "123")
                    : await users.CreateAsync(user));
                Check(await users.AddToRoleAsync(user, role));
            }
            foreach (var row in root.GetProperty("doctors").EnumerateArray())
                db.Doctors.Add(new Doctor { UserId = "legacy-" + row.GetProperty("user_id").GetInt32(), Designation = S(row, "designation"), Speciality = S(row, "speciality"), Phone = S(row, "phone"), About = S(row, "about") });
            foreach (var row in root.GetProperty("staff").EnumerateArray())
                db.Staff.Add(new Staff { UserId = "legacy-" + row.GetProperty("user_id").GetInt32(), Designation = S(row, "designation"), Department = S(row, "department"), Phone = S(row, "phone"), About = S(row, "about") });
            foreach (var row in root.GetProperty("health_checks").EnumerateArray())
            {
                var features = JsonSerializer.Deserialize<string[]>(S(row, "features")) ?? [];
                db.HealthChecks.Add(new HealthCheck { Name = S(row, "name"), Price = Price(S(row, "price")), CurrencyCode = "VND", Features = string.Join('\n', features), Popular = S(row, "popular") == "1" });
            }
            foreach (var row in root.GetProperty("diagnostic_services").EnumerateArray())
                db.DiagnosticServices.Add(new DiagnosticService { Name = S(row, "name"), Category = S(row, "category"), Description = S(row, "description"), SampleType = S(row, "sample_type"), Price = Price(S(row, "price")), CurrencyCode = "VND", Duration = int.Parse(S(row, "duration")), ReportHours = int.Parse(S(row, "report_time")), HomeCollectionAvailable = S(row, "home_collection_available") == "1" });
            foreach (var row in root.GetProperty("news").EnumerateArray())
                db.News.Add(new News { Title = S(row, "title"), Excerpt = S(row, "excerpt"), Content = S(row, "content"), Category = S(row, "category"), PublishedDate = DateOnly.Parse(S(row, "date"), CultureInfo.InvariantCulture) });
            await db.SaveChangesAsync();
            // The source has no schedules. Add sample schedules only on the initial demo import.
            foreach (var doctor in await db.Doctors.ToListAsync())
                foreach (var day in new[] { 1, 2, 3, 4, 5, 6 })
                    db.Schedules.Add(new Schedule { DoctorId = doctor.Id, DayOfWeek = day });
            await db.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        await DemoUser(users, "patient@example.com", "Bệnh nhân mẫu", Roles.Patient, "patient123");
        await DemoUser(users, "staff@example.com", "Nhân viên tiếp nhận", Roles.Staff, "staff123");
    }

    private static async Task DemoUser(UserManager<ApplicationUser> users, string email, string name, string role, string password)
    {
        if (await users.FindByEmailAsync(email) != null) return;
        var user = new ApplicationUser { FullName = name, Email = email, UserName = email, PhoneNumber = "0901234567", EmailConfirmed = true };
        Check(await users.CreateAsync(user, password));
        Check(await users.AddToRoleAsync(user, role));
    }
    private static string S(JsonElement element, string name) => element.TryGetProperty(name, out var value) && value.ValueKind != JsonValueKind.Null ? value.ToString() : "";
    private static decimal Price(string raw) => decimal.Parse(raw, CultureInfo.InvariantCulture);
    public static void Check(IdentityResult result)
    {
        if (!result.Succeeded) throw new InvalidOperationException(string.Join("; ", result.Errors.Select(e => e.Description)));
    }
}
