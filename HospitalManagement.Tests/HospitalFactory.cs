using HospitalManagement.Web.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace HospitalManagement.Tests;

public class HospitalFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    public string DatabaseName { get; } = "HospitalManagementTests_" + Guid.NewGuid().ToString("N");
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        var server = Environment.GetEnvironmentVariable("HOSPITAL_TEST_SQLSERVER") ?? @".\SQLEXPRESS";
        var connection = $"Server={server};Database={DatabaseName};Trusted_Connection=True;TrustServerCertificate=True";
        builder.UseEnvironment("Development");
        builder.ConfigureAppConfiguration((_, configuration) => configuration.AddInMemoryCollection(new Dictionary<string, string?>
        {
            ["ConnectionStrings:DefaultConnection"] = connection, ["Database:Initialize"] = "true",
            ["Database:SeedDemoData"] = "true", ["Email:Mode"] = "Disabled", ["Logging:LogLevel:Default"] = "Error"
        }));
        builder.ConfigureServices(services =>
        {
            services.RemoveAll<ApplicationDbContext>(); services.RemoveAll<DbContextOptions<ApplicationDbContext>>();
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connection));
        });
    }
    public HttpClient Client() => CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false, HandleCookies = true });
    public async Task InitializeAsync()
    {
        using var client = Client();
        using var scope = Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        Assert.Equal(DatabaseName, db.Database.GetDbConnection().Database);
        await db.Database.CanConnectAsync();
    }
    async Task IAsyncLifetime.DisposeAsync()
    {
        using var scope = Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        if (db.Database.GetDbConnection().Database != DatabaseName || !DatabaseName.StartsWith("HospitalManagementTests_"))
            throw new InvalidOperationException("Refusing to remove a database outside this test run.");
        await db.Database.EnsureDeletedAsync();
        await base.DisposeAsync();
    }
}
