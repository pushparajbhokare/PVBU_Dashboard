
using System.Text;

namespace dotnetWebService.Authentication
{
    internal static class Settings
    {
        internal static string SecretKey = "8egeef9327gh6d22d4852221dg780ehc";
        internal static byte[] GenerateSecretByte() =>
            Encoding.ASCII.GetBytes(SecretKey);
    }
}
