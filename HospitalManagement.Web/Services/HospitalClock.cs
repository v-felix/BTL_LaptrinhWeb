using System.Globalization;
using HospitalManagement.Web.Models;

namespace HospitalManagement.Web.Services;

public static class HospitalClock
{
    public static DateTime Now => DateTime.UtcNow.AddHours(7);
    public static DateOnly Today => DateOnly.FromDateTime(Now);
}

public static class Display
{
    public static string Money(decimal value, string currency = "VND") => value.ToString("N0", CultureInfo.GetCultureInfo("vi-VN")) + (currency == "BDT" ? " BDT" : " ₫");
    public static string Day(int value) => value == 0 ? "Chủ nhật" : "Thứ " + (value + 1);
    public static string Status(BookingStatus value) => value switch
    {
        BookingStatus.Pending => "Chờ xác nhận", BookingStatus.Confirmed => "Đã xác nhận",
        BookingStatus.Completed => "Hoàn tất", BookingStatus.Cancelled => "Đã hủy", _ => ""
    };
    public static string Role(string value) => value switch
    {
        Roles.Admin => "Quản trị viên", Roles.Doctor => "Bác sĩ", Roles.Staff => "Nhân viên",
        Roles.Diagnostic => "Kỹ thuật viên", _ => "Bệnh nhân"
    };
}
