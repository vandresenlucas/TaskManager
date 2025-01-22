using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace TaskManager.CrossCutting.Extensions
{
    public static class StringExtensions
    {
        public static string CalculateSHA256Hash(this string value)
        {
            using var sha256 = SHA256.Create();

            return ComputeHash(value, sha256);
        }

        private static string ComputeHash(string input, SHA256 algorithm)
        {
            var inputBytes = Encoding.UTF8.GetBytes(input);
            var hashBytes = algorithm.ComputeHash(inputBytes);
            return Convert.ToBase64String(hashBytes);
        }

        public static bool ValidatePassword(this string password)
        {
            if (password.Length < 8)
            {
                return false;
            }

            string pattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*()_+={}\[\]:;'"",.<>?/\\|`~]).{8,}$";
            Regex regex = new Regex(pattern);

            return regex.IsMatch(password);
        }
    }
}
