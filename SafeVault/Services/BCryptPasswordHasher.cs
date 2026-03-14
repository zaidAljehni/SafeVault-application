using Microsoft.AspNetCore.Identity;

namespace SafeVault.Services;

public class BCryptPasswordHasher<TUser> : IPasswordHasher<TUser>
    where TUser : class
{
    public string HashPassword(TUser user, string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    public PasswordVerificationResult VerifyHashedPassword(
        TUser user,
        string hashedPassword,
        string providedPassword)
    {
        var valid = BCrypt.Net.BCrypt.Verify(providedPassword, hashedPassword);

        return valid
            ? PasswordVerificationResult.Success
            : PasswordVerificationResult.Failed;
    }
}