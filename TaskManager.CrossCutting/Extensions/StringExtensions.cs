using System.Security.Cryptography;
using System.Text;

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
    }
}
