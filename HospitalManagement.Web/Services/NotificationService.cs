using System.Net;
using System.Net.Mail;
using System.Text;

namespace HospitalManagement.Web.Services;

public class NotificationService(IConfiguration configuration, IWebHostEnvironment env, ILogger<NotificationService> logger)
{
    public async Task SendAsync(string to, string subject, string body)
    {
        var mode = configuration["Email:Mode"] ?? "Disabled";
        if (mode == "Disabled") return;
        try
        {
            using var message = new MailMessage(configuration["Email:From"] ?? "hospital@example.com", to, subject, body)
            { BodyEncoding = Encoding.UTF8, SubjectEncoding = Encoding.UTF8 };
            using var client = new SmtpClient();
            if (mode == "File" && env.IsDevelopment())
            {
                var directory = Path.Combine(env.ContentRootPath, "App_Data", "MailOutbox");
                Directory.CreateDirectory(directory);
                client.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
                client.PickupDirectoryLocation = directory;
            }
            else if (mode == "Smtp")
            {
                client.Host = configuration["Email:Host"] ?? throw new InvalidOperationException("Thiếu Email:Host");
                client.Port = configuration.GetValue("Email:Port", 587);
                client.EnableSsl = true;
                client.Credentials = new NetworkCredential(configuration["Email:User"], configuration["Email:Password"]);
            }
            else return;
            await client.SendMailAsync(message);
        }
        catch (Exception exception) { logger.LogError(exception, "Không thể gửi thông báo email."); }
    }
    public string AbsoluteUrl(string relative) => (configuration["Email:PublicBaseUrl"] ?? "http://localhost:5080").TrimEnd('/') + relative;
}
