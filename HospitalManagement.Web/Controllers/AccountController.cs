using HospitalManagement.Web.Models;
using HospitalManagement.Web.Models.ViewModels;
using HospitalManagement.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HospitalManagement.Web.Controllers;

public class AccountController(UserManager<ApplicationUser> users, SignInManager<ApplicationUser> signIn, NotificationService mail) : Controller
{
    [HttpGet] public IActionResult Login(string? returnUrl) => View(new LoginVM { ReturnUrl = returnUrl });
    [HttpPost] public async Task<IActionResult> Login(LoginVM model)
    {
        if (!ModelState.IsValid) return View(model);
        var user = await users.FindByEmailAsync(model.Email);
        if (user?.IsActive == true)
        {
            var result = await signIn.PasswordSignInAsync(user, model.Password, model.RememberMe, lockoutOnFailure: true);
            if (result.Succeeded) return Url.IsLocalUrl(model.ReturnUrl) ? LocalRedirect(model.ReturnUrl!) : RedirectToAction("Index", "Dashboard");
        }
        ModelState.AddModelError("", "Email hoặc mật khẩu không đúng, tài khoản bị khóa hoặc tạm khóa sau nhiều lần thử.");
        return View(model);
    }
    [HttpGet] public IActionResult Register() => View(new RegisterVM());
    [HttpPost] public async Task<IActionResult> Register(RegisterVM model)
    {
        if (!ModelState.IsValid) return View(model);
        var user = new ApplicationUser { FullName = model.FullName.Trim(), Email = model.Email, UserName = model.Email, PhoneNumber = model.Phone };
        var result = await users.CreateAsync(user, model.Password);
        if (result.Succeeded)
        {
            var role = await users.AddToRoleAsync(user, Roles.Patient);
            if (!role.Succeeded) { await users.DeleteAsync(user); AddErrors(role); return View(model); }
            await signIn.SignInAsync(user, false);
            return RedirectToAction("Index", "Patient");
        }
        AddErrors(result); return View(model);
    }
    [Authorize, HttpPost] public async Task<IActionResult> Logout() { await signIn.SignOutAsync(); return RedirectToAction("Index", "Home"); }
    public IActionResult AccessDenied() { Response.StatusCode = 403; return View(); }
    [Authorize, HttpGet] public async Task<IActionResult> Profile()
    {
        var user = (await users.GetUserAsync(User))!;
        return View(new ProfileVM { FullName = user.FullName, Phone = user.PhoneNumber ?? "", Email = user.Email! });
    }
    [Authorize, HttpPost] public async Task<IActionResult> Profile(ProfileVM model)
    {
        var user = (await users.GetUserAsync(User))!;
        model.Email = user.Email!;
        if (!ModelState.IsValid) return View(model);
        user.FullName = model.FullName.Trim(); user.PhoneNumber = model.Phone;
        var result = await users.UpdateAsync(user);
        if (!result.Succeeded) { AddErrors(result); return View(model); }
        TempData["Success"] = "Đã cập nhật hồ sơ."; return RedirectToAction(nameof(Profile));
    }
    [Authorize, HttpGet] public IActionResult Password() => View(new PasswordVM());
    [Authorize, HttpPost] public async Task<IActionResult> Password(PasswordVM model)
    {
        if (!ModelState.IsValid) return View(model);
        var user = (await users.GetUserAsync(User))!;
        var result = await users.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
        if (!result.Succeeded) { AddErrors(result); return View(model); }
        await signIn.RefreshSignInAsync(user); TempData["Success"] = "Đã đổi mật khẩu.";
        return RedirectToAction(nameof(Profile));
    }
    [HttpGet] public IActionResult ForgotPassword() => View(new ForgotPasswordVM());
    [HttpPost] public async Task<IActionResult> ForgotPassword(ForgotPasswordVM model)
    {
        if (!ModelState.IsValid) return View(model);
        var user = await users.FindByEmailAsync(model.Email);
        if (user?.IsActive == true)
        {
            var token = await users.GeneratePasswordResetTokenAsync(user);
            var link = mail.AbsoluteUrl(Url.Action(nameof(ResetPassword), new { email = user.Email, token })!);
            await mail.SendAsync(user.Email!, "Đặt lại mật khẩu", $"Mở liên kết sau để đặt lại mật khẩu (hiệu lực 24 giờ):\n{link}");
        }
        TempData["Success"] = "Nếu email đã được đăng ký, hướng dẫn đặt lại mật khẩu sẽ được gửi qua kênh email đã cấu hình.";
        return RedirectToAction(nameof(Login));
    }
    [HttpGet] public IActionResult ResetPassword(string email, string token) => View(new ResetPasswordVM { Email = email, Token = token });
    [HttpPost] public async Task<IActionResult> ResetPassword(ResetPasswordVM model)
    {
        if (!ModelState.IsValid) return View(model);
        var user = await users.FindByEmailAsync(model.Email);
        if (user != null)
        {
            var result = await users.ResetPasswordAsync(user, model.Token, model.Password);
            if (result.Succeeded) { TempData["Success"] = "Đã đặt lại mật khẩu."; return RedirectToAction(nameof(Login)); }
        }
        ModelState.AddModelError("", "Liên kết không hợp lệ hoặc đã hết hạn."); return View(model);
    }
    private void AddErrors(IdentityResult result) { foreach (var error in result.Errors) ModelState.AddModelError("", error.Description); }
}
