using HospitalManagement.Web.Models;
using Microsoft.AspNetCore.Identity;

namespace HospitalManagement.Web.Services;

// Existing Laravel bcrypt hashes remain usable. Successful login upgrades the hash to Identity PBKDF2.
public class LegacyPasswordHasher : PasswordHasher<ApplicationUser>
{
    public override PasswordVerificationResult VerifyHashedPassword(ApplicationUser user, string hashedPassword, string providedPassword)
    {
        if (!hashedPassword.StartsWith("$2")) return base.VerifyHashedPassword(user, hashedPassword, providedPassword);
        try
        {
            return BCrypt.Net.BCrypt.Verify(providedPassword, hashedPassword)
                ? PasswordVerificationResult.SuccessRehashNeeded : PasswordVerificationResult.Failed;
        }
        catch (Exception ex) when (ex is ArgumentException or BCrypt.Net.SaltParseException)
        {
            return PasswordVerificationResult.Failed;
        }
    }
}
