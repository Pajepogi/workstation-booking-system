using Microsoft.AspNetCore.Identity;
using wbs_api.Models;

namespace wbs_api.Services;

public class PasswordService
{
    private readonly PasswordHasher<User> _passwordHasher = new();

    public string HashPassword(User user, string password)
    {
        return _passwordHasher.HashPassword(user, password);
    }

    public bool VerifyPassword(User user, string password, string hashedPassword)
    {
        try
        {
            var result = _passwordHasher.VerifyHashedPassword(
                user,
                hashedPassword,
                password);

            return result == PasswordVerificationResult.Success;
        }
        catch (FormatException)
        {
            return false;
        }
    }
}