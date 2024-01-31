using System.Security.Cryptography;
using System.Text;

namespace PizzaMauiApp.Helpers.Crypto;

public class Sha256
{
    public static string? GetHashString(string value)
    {
        return string.IsNullOrEmpty(value) ? null : Convert.ToBase64String(SHA256.HashData(Encoding.UTF8.GetBytes(value)));
    }
}