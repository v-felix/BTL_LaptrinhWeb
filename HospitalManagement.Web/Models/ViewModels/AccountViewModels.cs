using System.ComponentModel.DataAnnotations;

namespace HospitalManagement.Web.Models.ViewModels;

public class LoginVM
{
    [Required, EmailAddress, Display(Name = "Email")] public string Email { get; set; } = "";
    [Required, DataType(DataType.Password), Display(Name = "Mật khẩu")] public string Password { get; set; } = "";
    [Display(Name = "Ghi nhớ đăng nhập")] public bool RememberMe { get; set; }
    public string? ReturnUrl { get; set; }
}
public class RegisterVM
{
    [Required, StringLength(150), Display(Name = "Họ và tên")] public string FullName { get; set; } = "";
    [Required, EmailAddress, StringLength(256)] public string Email { get; set; } = "";
    [Required, Phone, StringLength(30), Display(Name = "Số điện thoại")] public string Phone { get; set; } = "";
    [Required, StringLength(100, MinimumLength = 8), DataType(DataType.Password), Display(Name = "Mật khẩu")] public string Password { get; set; } = "";
    [Compare(nameof(Password)), DataType(DataType.Password), Display(Name = "Nhập lại mật khẩu")] public string ConfirmPassword { get; set; } = "";
}
public class ProfileVM
{
    [Required, StringLength(150), Display(Name = "Họ và tên")] public string FullName { get; set; } = "";
    [Required, Phone, StringLength(30), Display(Name = "Số điện thoại")] public string Phone { get; set; } = "";
    public string Email { get; set; } = "";
}
public class PasswordVM
{
    [Required, DataType(DataType.Password), Display(Name = "Mật khẩu hiện tại")] public string CurrentPassword { get; set; } = "";
    [Required, StringLength(100, MinimumLength = 8), DataType(DataType.Password), Display(Name = "Mật khẩu mới")] public string NewPassword { get; set; } = "";
    [Compare(nameof(NewPassword)), DataType(DataType.Password), Display(Name = "Nhập lại mật khẩu mới")] public string ConfirmPassword { get; set; } = "";
}
public class ForgotPasswordVM
{
    [Required, EmailAddress] public string Email { get; set; } = "";
}
public class ResetPasswordVM : ForgotPasswordVM
{
    [Required] public string Token { get; set; } = "";
    [Required, StringLength(100, MinimumLength = 8), DataType(DataType.Password), Display(Name = "Mật khẩu mới")] public string Password { get; set; } = "";
    [Compare(nameof(Password)), DataType(DataType.Password), Display(Name = "Nhập lại mật khẩu")] public string ConfirmPassword { get; set; } = "";
}
