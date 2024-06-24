using System.Security.Cryptography;
using System.Text;

namespace Services.Auth;

public static class AuthHelper
{
    public static (string hash, string salt) CreatePasswordHash(string password)
    {
        using var hmac = new HMACSHA512();

        var salt = Convert.ToBase64String(hmac.Key);
        var hash = Convert.ToBase64String(hmac.ComputeHash(Encoding.ASCII.GetBytes(password)));
        return (hash, salt);
    }

    public static bool VerifyPasswordHash(string password, string storedHash, string storedSalt)
    {
        using var hmac = new HMACSHA512(Convert.FromBase64String(storedSalt));

        var computedHash = Convert.ToBase64String(hmac.ComputeHash(Encoding.ASCII.GetBytes(password)));
        return computedHash == storedHash;
    }
}
