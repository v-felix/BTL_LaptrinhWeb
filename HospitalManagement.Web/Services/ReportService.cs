using HospitalManagement.Web.Models;
using PdfSharp.Drawing;
using PdfSharp.Fonts;
using PdfSharp.Pdf;

namespace HospitalManagement.Web.Services;

public class ReportService
{
    private static readonly object FontLock = new();
    public ReportService(IWebHostEnvironment environment)
    {
        lock (FontLock)
            if (GlobalFontSettings.FontResolver is not HospitalFontResolver)
                GlobalFontSettings.FontResolver = new HospitalFontResolver(Path.Combine(environment.ContentRootPath, "App_Data", "Fonts"));
    }
    public byte[] Appointment(Appointment a) => Create("PHIẾU KHÁM BỆNH", [
        $"Mã lịch: {a.Reference}  |  Số thứ tự: {a.SerialNo}", $"Bệnh nhân: {a.PatientName}",
        $"Điện thoại: {a.PatientPhone}  |  Tuổi: {a.PatientAge}",
        $"Bác sĩ: {a.Doctor.User.FullName}  |  {a.Doctor.Speciality}",
        $"Ngày khám: {a.AppointmentDate:dd/MM/yyyy}  -  {a.AppointmentTime:HH:mm}",
        $"Phí khám: {Display.Money(a.Fee)}  |  {Display.Status(a.Status)}",
        $"Lý do khám: {a.Problem}", "", .. a.Prescriptions.OrderBy(x => x.CreatedAt).SelectMany(p => new[] {
            $"ĐƠN THUỐC - {p.CreatedAt.AddHours(7):dd/MM/yyyy HH:mm}", $"Chẩn đoán: {p.Diagnosis}",
            $"Thuốc và cách dùng: {p.MedicineDetails}", $"Lời dặn: {p.Advice}", "" })]);
    public byte[] Diagnostic(BookingDiagnostic b) => Create("PHIẾU XÉT NGHIỆM", [
        $"Mã phiếu: {b.Reference}", $"Bệnh nhân: {b.PatientName}  |  {b.PatientPhone}",
        $"Dịch vụ: {b.DiagnosticService.Name}", $"Ngày hẹn: {b.BookingDate:dd/MM/yyyy}  -  {b.BookingTime:HH:mm}",
        $"Giá: {Display.Money(b.Price, b.CurrencyCode)}  |  {Display.Status(b.Status)}", "",
        .. b.TestResults.OrderBy(x => x.DeliveredAt).SelectMany(x => new[] { $"KẾT QUẢ - {x.DeliveredAt.AddHours(7):dd/MM/yyyy HH:mm}", x.Content, $"Kết luận: {x.Conclusion}", "" })]);
    public byte[] Package(PackageBooking b) => Create("PHIẾU ĐĂNG KÝ GÓI KHÁM", [
        $"Mã phiếu: {b.Reference}", $"Bệnh nhân: {b.Patient.FullName}", $"Gói khám: {b.HealthCheck.Name}",
        $"Ngày khám: {b.BookingDate:dd/MM/yyyy}", $"Tổng phí: {Display.Money(b.TotalAmount, b.CurrencyCode)}",
        $"Số tiền đã ghi nhận: {Display.Money(b.AmountPaid, b.CurrencyCode)}", $"Còn lại: {Display.Money(b.TotalAmount - b.AmountPaid, b.CurrencyCode)}",
        $"Trạng thái: {Display.Status(b.Status)}", $"Lý do hủy: {b.CancellationReason}" ]);

    private static byte[] Create(string title, IEnumerable<string> content)
    {
        using var document = new PdfDocument();
        document.Info.Title = title;
        var font = new XFont("Hospital", 11, XFontStyleEx.Regular);
        var heading = new XFont("Hospital", 18, XFontStyleEx.Bold);
        XGraphics? graphics = null;
        double y = 0, width = 0, height = 0;
        void Page()
        {
            graphics?.Dispose();
            var page = document.AddPage(); page.Size = PdfSharp.PageSize.A4;
            width = page.Width.Point - 80; height = page.Height.Point;
            graphics = XGraphics.FromPdfPage(page); y = 44;
            graphics.DrawString("XET SPECIALIZED HOSPITAL", font, XBrushes.Teal, 40, y); y += 30;
        }
        Page();
        graphics!.DrawString(title, heading, XBrushes.DarkSlateGray, 40, y); y += 32;
        foreach (var paragraph in content.Append($"Xuất lúc: {HospitalClock.Now:dd/MM/yyyy HH:mm} (UTC+7)"))
        {
            foreach (var line in paragraph.Replace("\r", "").Split('\n'))
            {
                var current = "";
                foreach (var word in line.Split(' '))
                {
                    // Split oversized tokens as well as ordinary words so nothing leaves the page.
                    foreach (var ch in word + " ")
                    {
                        if (graphics!.MeasureString(current + ch, font).Width > width)
                        {
                            if (y > height - 50) Page();
                            graphics.DrawString(current, font, XBrushes.Black, 40, y); y += 17; current = "";
                        }
                        current += ch;
                    }
                }
                if (y > height - 50) Page();
                graphics!.DrawString(current, font, XBrushes.Black, 40, y); y += 19;
            }
            y += 6;
        }
        graphics?.Dispose();
        using var stream = new MemoryStream(); document.Save(stream, false); return stream.ToArray();
    }
    private class HospitalFontResolver(string directory) : IFontResolver
    {
        public string DefaultFontName => "Hospital";
        public byte[] GetFont(string faceName) => File.ReadAllBytes(Path.Combine(directory, faceName == "bold" ? "DejaVuSans-Bold.ttf" : "DejaVuSans.ttf"));
        public FontResolverInfo ResolveTypeface(string familyName, bool isBold, bool isItalic) => new(isBold ? "bold" : "regular");
    }
}
