using System.Security.Cryptography;
using System.Text;

namespace Bookstore.Utils
{
    public class PasswordUtils
    {
        public static void CreatePasswordHash(string password, out string passwordHash)
        {
            using var hmac = new HMACSHA512([1, 0]);
            passwordHash = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(password)));
        }

        public static bool VerifyPasswordHash(string password, string passwordHash)
        {
            using var hmac = new HMACSHA512([1, 0]);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

            var passwordString = Convert.FromBase64String(passwordHash);

            return computedHash.SequenceEqual(passwordString);
        }
    }
}
