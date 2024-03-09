using System.Security.Cryptography;
using System.Text.Json;
using System.Text;
using HashBack.Models;

namespace HashBack.Services;

public class CryptoService : ICryptoService
{
    public string GenerateRandomString()
    {
        byte[] random = new byte[32];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(random);
        }
        return Convert.ToBase64String(random);
    }

    public bool ValidateRandomString(string random)
    {
        try 
        {
            var bytes = Convert.FromBase64String(random);
            return bytes.Length == 32;
        }
        catch
        {
            return false;
        }
    }

    public string GetHash(Request request)
    {
        // Canonicalize JSON request body here
        // todo: implement the actual canonicalization based on RFC 8785
        string canonicalJson = JsonSerializer.Serialize(request);

        // Convert the canonical JSON into bytes
        byte[] passwordBytes = Encoding.UTF8.GetBytes(canonicalJson);

        // Get the salt as bytes
        byte[] salt = Convert.FromBase64String(request.Unus);

        // Use PBKDF2 with SHA256 to derive the key
        using var pbkdf2 = new Rfc2898DeriveBytes(passwordBytes, salt, request.Rounds, HashAlgorithmName.SHA256);

        byte[] key = pbkdf2.GetBytes(32); // 256 bits / 32 bytes

        // Encode the hash result using BASE-64
        string base64Hash = Convert.ToBase64String(key);

        return base64Hash;
    }
}
